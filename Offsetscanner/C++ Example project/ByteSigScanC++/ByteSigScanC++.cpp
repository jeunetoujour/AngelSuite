// ByteSigScanC++.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>

__declspec(dllimport) DWORD SigScan(const char* szPattern, int offset = 0);
__declspec(dllimport) void InitializeSigScan(DWORD ProcessID, const char* Module);
__declspec(dllimport) void FinalizeSigScan();
#pragma comment(lib,"ByteSigScan.lib")

int _tmain(int argc, _TCHAR* argv[])
{

	//alter to whatever pid aion is running for this to work
	DWORD dwPID = 2428;
	HANDLE hProc = OpenProcess(PROCESS_VM_READ,FALSE, dwPID);
	BYTE data = NULL;
	InitializeSigScan(dwPID, "Game.dll");

	//this sig has lasted since it was found using version .08 
	DWORD memloc = SigScan("8b5720535068XXXXXXXX8d4f3651",0);
	FinalizeSigScan();

	//reading the playerinfo address + 24 is the players level
	ReadProcessMemory(hProc,(LPCVOID) (memloc + 24) ,&data,sizeof(data),NULL);


	printf("Player Lvl: %u\n", data);
	CloseHandle(hProc);
	system("pause");
	return 0;
}

