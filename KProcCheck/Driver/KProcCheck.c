//*********************************************************************************************
// KProcCheck Kernel Driver (Proof-of-Concept) 
// Version 0.2-beta1
// by Chew Keong TAN
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, provided that the above
// copyright notice(s) and this permission notice appear in all copies of
// the Software and that both the above copyright notice(s) and this
// permission notice appear in supporting documentation.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
// OF THIRD PARTY RIGHTS. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// HOLDERS INCLUDED IN THIS NOTICE BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL
// INDIRECT OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING
// FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT,
// NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION
// WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
//
//*********************************************************************************************

#include <ntddk.h>
#include <ndis.h>
#include <stdio.h>
#include <windef.h>


//*********************************************************************************************
// Type definitions
//
//*********************************************************************************************
#define VERSION		"0.2-beta1"

typedef struct _MODULE_ENTRY {
	LIST_ENTRY link;		// Flink, Blink
	BYTE unknown1[16];
	DWORD imageBase;
	DWORD entryPoint;
	DWORD imageSize;
	UNICODE_STRING drvPath;
	UNICODE_STRING drvName;
	//...
} MODULE_ENTRY, *PMODULE_ENTRY;


#pragma pack(1)
typedef struct ServiceDescriptorEntry {
	PDWORD ServiceTable;
	PDWORD CounterTableBase;
	DWORD  ServiceLimit;
	PBYTE  ArgumentTable;
} SDT;
#pragma pack()


#define DRV_NAME	KProcCheck

#define _DRV_LINK(_name)   \\DosDevices\\ ##_name
#define _DRV_DEVICE(_name) \\Device\\ ##_name
#define DRV_DEVICE         _DRV_DEVICE(DRV_NAME)
#define DRV_LINK           _DRV_LINK(DRV_NAME)

#define _CSTRING(_text) #_text
#define CSTRING(_text) _CSTRING (_text)

#define _USTRING(_text) L##_text
#define USTRING(_text) _USTRING (_text)


#define PRESET_UNICODE_STRING(_var,_buffer) \
        UNICODE_STRING _var = \
            { \
            sizeof (USTRING (_buffer)) - sizeof (WORD), \
            sizeof (USTRING (_buffer)), \
            USTRING (_buffer) \
            };


PRESET_UNICODE_STRING(gDeviceName,       CSTRING(DRV_DEVICE));
PRESET_UNICODE_STRING(gSymbolicLinkName, CSTRING(DRV_LINK));


//*********************************************************************************************
// Exported symbols that we need.
//
//*********************************************************************************************

NTKERNELAPI NTSTATUS KeSetAffinityThread(long lParam1, long lParam2);

NTKERNELAPI NTSTATUS KeAddSystemServiceTable( 
        PVOID  lpAddressTable, 
        BOOLEAN  bUnknown,
        DWORD  dwNumEntries, 
        PVOID  lpParameterTable,
        DWORD dwTableID );

__declspec(dllimport) SDT KeServiceDescriptorTable;
__declspec(dllimport) DWORD PsInitialSystemProcess;


//*********************************************************************************************
// ioctl codes
//
//*********************************************************************************************

#define IOCTL_CODE1 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x806, METHOD_BUFFERED, FILE_READ_ACCESS + FILE_WRITE_ACCESS)
#define IOCTL_CODE2 CTL_CODE(FILE_DEVICE_UNKNOWN, 0x807, METHOD_BUFFERED, FILE_READ_ACCESS + FILE_WRITE_ACCESS)
#define IOCTL_GETVERSION CTL_CODE(FILE_DEVICE_UNKNOWN, 0x808, METHOD_BUFFERED, FILE_READ_ACCESS + FILE_WRITE_ACCESS)


//*********************************************************************************************
// Globals
//
//*********************************************************************************************
#define NOT_SUPPORTED	0
#define WIN2K			1
#define WINXP			2


PDEVICE_OBJECT gpDeviceObject	= NULL;
DWORD gPsLoadedModuleList		= 0;
DWORD gSystemProcessActiveLink	= 0;
DWORD gProcessNameOffset		= 0;		// offset from ActiveProcessLinks
DWORD gKernelVersion			= NOT_SUPPORTED;
DWORD gKiWaitInListHead			= 0;
DWORD gKiWaitOutListHead		= 0;
DWORD gXPKiWaitListHead			= 0;
DWORD gKiDispatcherReadyListHead = 0;
DWORD gKeServiceDescriptorTableShadow = 0;


//*********************************************************************************************
// FindPsLoadedModuleList - Thanks to fuzen for this technique
//
//*********************************************************************************************

DWORD FindPsLoadedModuleList (IN PDRIVER_OBJECT DriverObject)
{
	PMODULE_ENTRY pm_current, first;

	if (DriverObject == NULL)
		return 0;

	__try
	{
		pm_current = *((PMODULE_ENTRY*)((DWORD)DriverObject + 0x14));
		if (pm_current == NULL)
			return 0;

		first = pm_current;

		do
		{
			if(!MmIsAddressValid((PVOID)pm_current))
				break;

			//DbgPrint("%.8X %.8X %.8X %S\n", pm_current, pm_current->link.Flink, pm_current->link.Blink, pm_current->drvName.Buffer);
			//DbgPrint("%.8X %.8X %.8X %.8X %8X\n", pm_current, pm_current->link.Flink, pm_current->link.Blink, pm_current->entryPoint, pm_current->drvPath.Buffer);
			if (!MmIsAddressValid((PVOID)pm_current->entryPoint) || !MmIsAddressValid(pm_current->drvPath.Buffer) ||
				!MmIsAddressValid(pm_current->drvName.Buffer))
			{
				return (DWORD) pm_current;
			}
			pm_current = (MODULE_ENTRY*)pm_current->link.Flink;		
		}
		while ((PMODULE_ENTRY)pm_current != first);
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		return 0;
	}

	return 0;
}


DWORD FindSystemProcessActiveLink(void)
{
	DWORD ptr = PsInitialSystemProcess;

	if(gKernelVersion == WIN2K)
		ptr += 0xa0;
	else if(gKernelVersion == WINXP)
		ptr += 0x88;
	else
		return 0;

	return ptr;
}


//*********************************************************************************************
// Attempt to locate the non-exported symbols KiWaitInListHead and KiWaitOutListHead by
// scanning in KeWaitForSingleObject API for
//
// mov     ecx,0x80482258   <- KiWaitInListHead
// test    al,al
//
// And,
// mov     ecx, offset dword_0_482808  <- KiWaitOutListHead
//
// Tested on Win2k SP2 and SP4
//*********************************************************************************************

void FindKiWaitInOutListHead(DWORD *waitInListAddr, DWORD *waitOutListAddr)
{
	int i;
	unsigned char *ptr;
	DWORD retVal1 = 0, retVal2 = 0;
	ptr = (unsigned char *)KeWaitForSingleObject;
	
	__try
	{
		for( i = 0; i < PAGE_SIZE; i++ ) 
		{
			if(!MmIsAddressValid((PVOID)ptr) || !MmIsAddressValid((PVOID)(ptr+24)))
				break;

			if(*ptr == 0xb9 && *(ptr+5) == 0x84 && *(ptr+6) == 0xc0 && *(ptr+24) == 0xb9)
			{
				retVal1 = *(DWORD *)(ptr + 1);
				retVal2 = *(DWORD *)(ptr + 25);

				break;
			}
			ptr++;
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		retVal1 = 0;
		retVal2 = 0;
	}

	*waitInListAddr = retVal1;
	*waitOutListAddr = retVal2;

	return;
}


//*********************************************************************************************
// Attempt to locate the non-exported symbol KiDispatcherReadyListHead by
// scanning KeSetAffinityThread for
//
// 8d04cde0224880	lea eax,[nt!KiDispatcherReadyListHead (804822e0)+ecx*8]
// 3900				cmp     [eax],eax
//
// Tested on Win2k SP2 and SP4
//*********************************************************************************************

DWORD FindKiDispatcherReadyListHead(void)
{
	int i;
	unsigned char *ptr;
	DWORD retVal = 0;
	ptr = (unsigned char *)KeSetAffinityThread;

	__try
	{
		for( i = 0; i < PAGE_SIZE; i++ ) 
		{
			if(!MmIsAddressValid((PVOID)ptr) || !MmIsAddressValid((PVOID)(ptr+4)))
				break;

			if(*ptr == 0x8d && *(ptr+1) == 0x04 && *(ptr+2) == 0xcd && *(ptr+7) == 0x39)
			{
				retVal = *(DWORD *)(ptr + 3);
				break;
			}
			ptr++;
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		return 0;
	}

	return retVal;
}


//*********************************************************************************************
// Attempt to locate the non-exported symbols KiWaitListHead by
// scanning in KeDelayExecutionThread API for
//
// mov		dword ptr [ebx], offset _KiWaitListHead
// mov		[ebx+4], eax
//
// Tested on WinXP SP1 SP2
//*********************************************************************************************

void FindXPKiWaitListHead(DWORD *waitListAddr)
{
	int i;
	unsigned char *ptr;
	DWORD retVal = 0;
	ptr = (unsigned char *)KeDelayExecutionThread;

	__try
	{
		for( i = 0; i < PAGE_SIZE; i++ ) 
		{
			if(!MmIsAddressValid((PVOID)ptr) || !MmIsAddressValid((PVOID)(ptr+4)))
				break;

			if(*ptr == 0xc7 && *(ptr+1) == 0x03 && *(ptr+6) == 0x89 && *(ptr+7) == 0x43)
			{
				retVal = *(DWORD *)(ptr + 2);

				break;
			}
			ptr++;
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		retVal = 0;		
	}

	*waitListAddr = retVal;	

	return;
}


//*********************************************************************************************
// Gets the offset to process name, thx to SysInternals for this idea.
// This will not work if driver is loaded using SystemLoadAndCallImage
// 
//*********************************************************************************************

DWORD getProcNameOffset(DWORD startPos)
{
    DWORD curproc;
    int i;
	DWORD procNameOffset = 0;

	curproc = startPos;

	__try
	{
		for(i = 0; i < PAGE_SIZE; i++)
		{
			if(MmIsAddressValid((PCHAR) curproc + i) &&
				strncmp( "System", (PCHAR) curproc + i, strlen("System")) == 0)
			{
				procNameOffset = i;
				break;
			}
		}
	}
	__except (EXCEPTION_EXECUTE_HANDLER)
	{
		return 0;
	}

	return procNameOffset;
}

//*********************************************************************************************
// Find address of KeServiceDescriptorTableShadow by scanning KeAddSystemServiceTable
//
//*********************************************************************************************

DWORD findAddressofShadowTable(void)
{
	int i;
	unsigned char *p;
	DWORD val;

	p = (unsigned char *)KeAddSystemServiceTable;

	for (i = 0; i < PAGE_SIZE; i++, p++)
	{
		__try
		{
			val = *(unsigned int *)p;
		}
		__except (EXCEPTION_EXECUTE_HANDLER)
		{
			return 0;
		}

		if (MmIsAddressValid((PVOID)val))
		{
			if (memcmp((PVOID)val, &KeServiceDescriptorTable, 16) == 0)
			{
				if((PVOID)val != &KeServiceDescriptorTable)
					return val;
			}
		}
	}
	return 0;
}



//*********************************************************************************************
// DeviceDispatcher
//
//*********************************************************************************************

NTSTATUS DeviceDispatcher(PIRP pIrp)
{
    PIO_STACK_LOCATION esp;
    DWORD dInfo = 0;
	char *output;
    NTSTATUS ntS = STATUS_NOT_IMPLEMENTED;

	//DbgPrint("DeviceDispatcher called\n");
    esp = IoGetCurrentIrpStackLocation(pIrp);

    switch (esp->MajorFunction)
	{
		case IRP_MJ_CREATE:
			//DbgPrint("IRP_MJ_CREATE\n");
            ntS = STATUS_SUCCESS;
            break;

		case IRP_MJ_CLEANUP:
			//DbgPrint("IRP_MJ_CLEANUP\n");
            ntS = STATUS_SUCCESS;
            break;

		case IRP_MJ_CLOSE:
			//DbgPrint("IRP_MJ_CLOSE\n");
            ntS = STATUS_SUCCESS;
            break;

		case IRP_MJ_DEVICE_CONTROL:
			{
				DWORD sizeReq;

				// returns address of KeServiceDescriptorTable and KeServiceDescriptorTableShadow
				if(esp->Parameters.DeviceIoControl.IoControlCode == IOCTL_CODE1)
				{
					if(esp->Parameters.DeviceIoControl.OutputBufferLength >= 32)
					{
						DWORD *outDword = (DWORD *)pIrp->AssociatedIrp.SystemBuffer;
						if(outDword)
						{
							outDword[0] = gPsLoadedModuleList;
							outDword[1] = (DWORD)&KeServiceDescriptorTable;
							outDword[2] = (DWORD)gKeServiceDescriptorTableShadow;
							outDword[3] = gSystemProcessActiveLink;
							outDword[4] = gProcessNameOffset;
							if(gKernelVersion == WIN2K)
							{
								outDword[5] = gKiWaitInListHead;
								outDword[6] = gKiWaitOutListHead;								
							}
							else
							{
								outDword[5] = gXPKiWaitListHead;
								outDword[6] = gXPKiWaitListHead;
							}
							outDword[7] = gKiDispatcherReadyListHead;
								

							dInfo = 32;
							ntS = STATUS_SUCCESS;
						}
					}
					else
						ntS = STATUS_INFO_LENGTH_MISMATCH;
				}
				else if(esp->Parameters.DeviceIoControl.IoControlCode == IOCTL_CODE2)
				{
					if(esp->Parameters.DeviceIoControl.OutputBufferLength >= 4)
					{
						DWORD *outDword = (DWORD *)pIrp->AssociatedIrp.SystemBuffer;
						if(outDword)
						{
							PHYSICAL_ADDRESS paddr = MmGetPhysicalAddress((PVOID)outDword[0]);
							outDword[0] = paddr.LowPart;
							dInfo = 4;
							ntS = STATUS_SUCCESS;
						}
					}
					else
						ntS = STATUS_INFO_LENGTH_MISMATCH;
				}
				
				else if(esp->Parameters.DeviceIoControl.IoControlCode == IOCTL_GETVERSION)
				{
					if(esp->Parameters.DeviceIoControl.OutputBufferLength >= 16)
					{
						char *outBuffer = (char *)pIrp->AssociatedIrp.SystemBuffer;
						if(outBuffer)
						{
							memset(outBuffer, 0, 16);
							strncpy(outBuffer, VERSION, 16);
							
							dInfo = 16;
							ntS = STATUS_SUCCESS;
						}
					}
					else
						ntS = STATUS_INFO_LENGTH_MISMATCH;
				}

				break;
			}

		default:
		    break;
	}

    pIrp->IoStatus.Status      = ntS;
    pIrp->IoStatus.Information = dInfo;
    IoCompleteRequest(pIrp, IO_NO_INCREMENT);

    return(ntS);
}


//*********************************************************************************************
// DriverDispatcher
//
//*********************************************************************************************

NTSTATUS DriverDispatcher(PDEVICE_OBJECT pDeviceObject, PIRP pIrp)
{
	NTSTATUS ntS = STATUS_INVALID_PARAMETER_1;

	//DbgPrint("DriverDispatcher called\n");

	if (pDeviceObject == gpDeviceObject)
		ntS = DeviceDispatcher(pIrp);

    return(ntS);
}


//*********************************************************************************************
// Create our device to communicate with user land
//
//*********************************************************************************************

NTSTATUS createOurDevice(PDRIVER_OBJECT driverObject)
{
	PDEVICE_OBJECT pDeviceObject = NULL;
	NTSTATUS NTs;
	int i;

	NTs = IoCreateDevice(driverObject, 0, &gDeviceName,
					FILE_DEVICE_UNKNOWN, 0, FALSE, &pDeviceObject);

	if(NTs == STATUS_SUCCESS)
	{
		NTs = IoCreateSymbolicLink(&gSymbolicLinkName, &gDeviceName);

		if (NTs == STATUS_SUCCESS)
		{
			gpDeviceObject = pDeviceObject;

			//DbgPrint("Device created successfully. Name = %S\n", gDeviceName.Buffer);
		}
		else
		{
			DbgPrint("KProcCheck: Error while creating symbolic link\n");
            IoDeleteDevice (pDeviceObject);
		}

	}
	else
	{
		DbgPrint("KProcCheck: Error while creating device\n");
	}

	return NTs;
}


//*********************************************************************************************
// DriverUnload - removes our device object
//
//*********************************************************************************************

VOID DriverUnload(IN PDRIVER_OBJECT DriverObject)
{
	//DbgPrint("Unloading...\n");

	IoDeleteSymbolicLink(&gSymbolicLinkName);

	if(gpDeviceObject)
		IoDeleteDevice(gpDeviceObject);
}


//*********************************************************************************************
// DriverEntry
//
//*********************************************************************************************

NTSTATUS DriverEntry(PDRIVER_OBJECT driverObject, PUNICODE_STRING RegistryPath)
{
    int i;
	NTSTATUS ns;
	DWORD major, minor, build;
	KIRQL kq;
	
    driverObject->DriverUnload = DriverUnload;

	PsGetVersion(&major, &minor, &build, NULL);
	if(major == 5)
	{
		if(minor == 0)
			gKernelVersion = WIN2K;
		else if(minor == 1)
			gKernelVersion = WINXP;
	}

	if(gKernelVersion == NOT_SUPPORTED)
		return STATUS_NOT_SUPPORTED;

 	for (i = 0; i < IRP_MJ_MAXIMUM_FUNCTION; i++)
	{
   		driverObject->MajorFunction[i] = DriverDispatcher;
	}

	ns = createOurDevice(driverObject);
	if(ns != STATUS_SUCCESS)
	{
		return ns;
	}

	kq = KeRaiseIrqlToDpcLevel();

	gSystemProcessActiveLink = FindSystemProcessActiveLink();
	gProcessNameOffset = getProcNameOffset(gSystemProcessActiveLink);
	gPsLoadedModuleList = FindPsLoadedModuleList(driverObject);
	gKeServiceDescriptorTableShadow	= findAddressofShadowTable();

	if(gKernelVersion == WIN2K)
	{
		FindKiWaitInOutListHead(&gKiWaitInListHead, &gKiWaitOutListHead);
		gKiDispatcherReadyListHead = FindKiDispatcherReadyListHead();
	}
	else if(gKernelVersion == WINXP)
	{
		FindXPKiWaitListHead(&gXPKiWaitListHead);		
	}
	
	KeLowerIrql(kq);

/*
	DbgPrint("At %.8X\n", gPsLoadedModuleList);
	DbgPrint("%.8X\n", IOCTL_CODE1);
	DbgPrint("%.8X\n", IOCTL_CODE2);
	DbgPrint("%.8X\n", PsInitialSystemProcess);
	DbgPrint("%d, %d, %d\n", major, minor, build);
	DbgPrint("Version %d\n", gKernelVersion);
	DbgPrint("Link %.8X\n", gSystemProcessActiveLink);
	DbgPrint("Offset %.8X\n", gProcessNameOffset);
	DbgPrint("%s\n", (char *)gSystemProcessActiveLink + gProcessNameOffset);
	DbgPrint("%.8X, %.8X\n", gKiWaitInListHead, gKiWaitOutListHead);
	DbgPrint("XP -> %.8X\n", gXPKiWaitListHead);
	DbgPrint("Code = %.8X\n", IOCTL_GETVERSION);
*/
	
    return(STATUS_SUCCESS);
}
