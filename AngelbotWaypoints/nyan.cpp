// nyan.cpp : main project file.

#include "stdafx.h"
//#compiler: VC 2003 / 2005 max! Release mode. or you'll fail >:0

//################## includes ##################

#include <map>
using namespace std;

//################## structs ##################

class XString;
class XItemInfo
{
public:
	char unknown0[8];
	DWORD Index; //0x0008  
	DWORD Index2; //0x000C  
	DWORD Level; //0x0010  
	char unknown20[8];
	XString* Name; //0x001C  
	char unknown32[28];
	DWORD CooldownTime; //0x003C  
	DWORD CooldownTick; //0x0040  
	char unknown68[12];
	DWORD X6;
	DWORD Remaining; //0x0054  
};

class Map_DataPtr;
class XRelated
{
public:
	char unknown0[312];
	Map_DataPtr* wtf_1; //0x0138  
	char unknown316[12];
	Map_DataPtr* wtf_2; //0x0148  
	char unknown332[12];
	Map_DataPtr* Inventory; //0x0158  
	char unknown348[136];
	Map_DataPtr* Skills; //0x01E4  
	char unknown488[8];
	Map_DataPtr* wtf_3; //0x01F0  
	char unknown500[8];
	Map_DataPtr* Buffs; //0x01FC  
};

class XRelatedPtr
{
public:
	XRelated* Ptr;
};

XRelatedPtr *m_pRelated = 0;

//################## initializer ##################

{
//.text:102E6DA3 A1 34 39 8E 10                       mov     eax, related_108E3934
//.text:102E6DA8 8B 88 38 01 00 00                    mov     ecx, [eax+138h]
//.text:102E6DAE 8B 90 3C 01 00 00                    mov     edx, [eax+13Ch]
//.text:102E6DB4 8B 83 00 05 00 00                    mov     eax, [ebx+500h]
//.text:102E6DBA 8B 30                                mov     esi, [eax]
//.text:102E6DBC BF 1C BF 0D 00                       mov     edi, 0DBF1Ch
//.text:102E6DC1 B8 20 FC 91 10                       mov     eax, offset stringz_1091FC20

	// TODO: check for strings ptr
	pFound = Patcher.FindPattern(pBase, pBaseMax,
		"\xA1\x34\x39\x8E\x10\x8B\x88\x38\x01\x00\x00\x8B\x90\x3C\x01\x00\x00\x8B\x83\x00\x05\x00\x00\x8B\x30\xBF\x1C\xBF\x0D\x00\xB8\x20\xFC\x91\x10",
		"x????xxxxxxxxxxxxxxxxxxxxx????x");

	if(pFound)
		*(PBYTE*)&m_pRelated = *(PBYTE*)&pFound[1]; // 1.5.8 +8E3934h

	LOG("m_pRelated: %X (%X) found at %X", m_pRelated, (ULONG)m_pRelated-(ULONG)pBase, pFound);

}


ULONG DecodeStringW(PVOID ptr, PSTR Buff, ULONG Size)
{
	struct STDSTRINGW
	{
		ULONG Wtf;
		WCHAR Data[8];
		DWORD Offset;
		DWORD Len;
	} *p = (STDSTRINGW *)ptr;

	PWSTR pos = (p->Len < 8) ? p->Data : *(PWSTR *)p->Data;
	PWSTR end = &pos[ p->Offset ];
	PBYTE dst = (PBYTE)Buff;

	while(end > pos && Size)
	{
		if(!(*dst++ = *pos++))
			break;
		Size--;
	}
	*dst = 0;

	return dst - (PBYTE)Buff;
}

//################## main: logger ##################

{
	map<ULONG, XItemInfo *> *Buffs = (map <ULONG, XItemInfo *> *)((PBYTE)&m_pRelated->Ptr->Buffs - 4);

	LOG("\r\nBuffs: %i", Buffs->size());

	for(map<ULONG, XItemInfo *>::iterator i = Buffs->begin(); i != Buffs->end(); i++)
	{
		ULONG id = (*i).first;
		XItemInfo *p = (*i).second;

		char Name[128];
		DecodeStringW( (PBYTE)&p->Name-4, Name, sizeof(Name) );

		LOG("__ %X | %X, %s", p, p->Remaining, Name);
	}

//--------------------------------------

	map<ULONG, XItemInfo *> *Inventory = (map <ULONG, XItemInfo *> *)((PBYTE)&m_pRelated->Ptr->Inventory - 4);

	LOG("\r\Inventory: %i", Inventory->size());

	for(map<ULONG, XItemInfo *>::iterator i = Inventory->begin(); i != Inventory->end(); i++)
	{
		ULONG id = (*i).first;
		XItemInfo *p = (*i).second;

		char Name[128];
		DecodeStringW( (PBYTE)&p->Name-4, Name, sizeof(Name) );

		LOG("__ %X | %s", p, Name);
	}

//--------------------------------------

	typedef struct XSKILL
	{
		XItemInfo *Ptr;
		PVOID wtf;
	} *PSKILL;

	typedef struct XSKILLLIST
	{
		list<XSKILL> List;
	} *PSKILLLIST;

	typedef struct XSKILLMAP
	{
		map<USHORT, XSKILLLIST> Map;
	} *PSKILLMAP;

	map<ULONG, XSKILLMAP> *Skills = (map <ULONG, XSKILLMAP> *)((PBYTE)&m_pRelated->Ptr->Skills - 4);

	LOG("\r\nSkills: %i", Skills->size());

	for(map<ULONG, XSKILLMAP>::iterator i = Skills->begin(); i != Skills->end(); i++)
	{
		ULONG id = (*i).first;
		XSKILLMAP &pm = (*i).second;

		LOG("__ %X %X", id, pm.Map.size());

		for(map<USHORT, XSKILLLIST>::iterator j = pm.Map.begin(); j != pm.Map.end(); j++)
		{
			USHORT id = (*j).first;
			XSKILLLIST &pl = (*j).second;

			LOG("____ %X | %X %X %X", id, pl.List.size());

			for(list<XSKILL>::iterator k = pl.List.begin(); k != pl.List.end(); k++)
			{
				XSKILL &ps = (*k);

				char Name[128];
				DecodeStringW( (PBYTE)&ps.Ptr->Name-4, Name, sizeof(Name) );

				LOG("______ %X %X | %s", ps.Ptr, ps.wtf, Name);
			}
		}

		LOG("");
	}
}


