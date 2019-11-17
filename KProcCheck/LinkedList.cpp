//************************************************************************************
// KProcCheck (Proof-of-Concept) 
// Version 0.2-beta1
// by Chew Keong TAN
//
// LICENSE TERMS
//
// The free distribution and use of this software in both source and binary 
// form is allowed (with or without changes) provided that:
//
//   1. distributions of this source code include the above copyright 
//      notice, this list of conditions and the following disclaimer;
//
//   2. distributions in binary form include the above copyright
//      notice, this list of conditions and the following disclaimer
//      in the documentation and/or other associated materials;
//
//   3. the copyright holder's name is not used to endorse products 
//      built using this software without specific written permission. 
//
// DISCLAIMER
//
// This software is provided 'as is' with no explicit or implied warranties
// in respect of its properties, including, but not limited to, correctness 
// and fitness for purpose.
//
//************************************************************************************

//************************************************************************************
// This file implements a double linked-list data structure.  Process items on list
// uses callback functions supplied by developer.
//
//************************************************************************************

#include <stdio.h>
#include <windows.h>
#include "LinkedList.h"


void initList(LINKED_LIST *list)
{
	list->head = NULL;
	list->tail = NULL;
}


void freeList(LINKED_LIST *list)
{
	LINKED_LIST_NODE *curNode = list->head;
	LINKED_LIST_NODE *nextNode;

	while(curNode)
	{
		free(curNode->data);
		nextNode = curNode->next;
		free(curNode);
		curNode = nextNode;
	}	
	initList(list);
}

BOOL insertItem(LINKED_LIST *list, LPVOID data)
{
	LINKED_LIST_NODE *llNode = (LINKED_LIST_NODE *)malloc(sizeof(LINKED_LIST_NODE));
	
	if(llNode)
	{
		llNode->data = data;
		llNode->next = NULL;		

		if(!list->head && !list->tail)
		{
			llNode->prev = NULL;
			list->head = llNode;
			list->tail = llNode;
		}
		else
		{
			llNode->prev = list->tail;
			list->tail->next = llNode;
			list->tail = llNode;
		}
		return TRUE;
	}
	return FALSE;
}



LPVOID findItem(LINKED_LIST *list, FIND_ITEM_FUNC fi, LPVOID toFind)
{
	LINKED_LIST_NODE *curNode = list->head;

	while(curNode)
	{
		if(fi(curNode->data, toFind))
		{
			return curNode->data;
		}
		curNode = curNode->next;
	}
	return NULL;
}


// addr of nodedata will be returned, so caller can free it
LPVOID deleteSingleItem(LINKED_LIST *list, FIND_ITEM_FUNC fi, LPVOID toDelete)
{
	LINKED_LIST_NODE *curNode = list->head;

	while(curNode)
	{
		if(fi(curNode->data, toDelete))
		{
			if(curNode == list->head && curNode == list->tail)
			{
				list->head = list->tail = NULL;
			}
			else if(curNode == list->head)
			{
				list->head = curNode->next;
				if(list->head)
					list->head->prev = NULL;
			}
			else if(curNode == list->tail)
			{
				list->tail = curNode->prev;
				if(list->tail)
					list->tail->next = NULL;
			}
			else
			{
				curNode->prev->next = curNode->next;
				curNode->next->prev = curNode->prev;
			}
			LPVOID nodeData = curNode->data;
			free(curNode);

			return nodeData;
		}
		curNode = curNode->next;
	}
	return NULL;
}


// caller must free node data in callback func
int deleteMultipleItems(LINKED_LIST *list, FIND_ITEM_FUNC fi, LPVOID toDelete)
{
	LINKED_LIST_NODE *curNode = list->head;
	int count = 0;

	while(curNode)
	{
		if(fi(curNode->data, toDelete))
		{
			count++;
			if(curNode == list->head && curNode == list->tail)
			{
				list->head = list->tail = NULL;
				free(curNode);
				return count;
			}
			else if(curNode == list->head)
			{
				list->head = curNode->next;
				if(list->head)
					list->head->prev = NULL;
				free(curNode);
				curNode = list->head;
			}
			else if(curNode == list->tail)
			{
				list->tail = curNode->prev;
				if(list->tail)
					list->tail->next = NULL;
				free(curNode);
				return count;
			}
			else
			{
				curNode->prev->next = curNode->next;
				curNode->next->prev = curNode->prev;
				LINKED_LIST_NODE *tempNode = curNode->next;

				free(curNode);
				curNode = tempNode;
			}
		}
		else
			curNode = curNode->next;
	}
	return count;
}



void printList(LINKED_LIST *list, PRINT_ITEM_FUNC pi)
{
	LINKED_LIST_NODE *curNode = list->head;

	while(curNode)
	{
		pi(curNode->data);
		curNode = curNode->next;
	}	
}


void printListBackwards(LINKED_LIST *list, PRINT_ITEM_FUNC pi)
{
	LINKED_LIST_NODE *curNode = list->tail;

	while(curNode)
	{
		pi(curNode->data);
		curNode = curNode->prev;
	}	
}


void processMultipleItems(LINKED_LIST *list, PROCESS_ITEM_FUNC pi, LPVOID procData)
{
	LINKED_LIST_NODE *curNode = list->head;

	while(curNode)
	{
		pi(curNode->data, procData);
		curNode = curNode->next;
	}	
}


BOOL processSingleItem(LINKED_LIST *list, PROCESS_ITEM_FUNC pi, LPVOID procData)
{
	LINKED_LIST_NODE *curNode = list->head;

	while(curNode)
	{
		if(pi(curNode->data, procData))
			return TRUE;
		curNode = curNode->next;
	}
	return FALSE;
}
