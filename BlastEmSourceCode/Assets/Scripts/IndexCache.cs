/*
Copyright 2014 Xiotex Studios Ltd.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndexCache
{
	private static Dictionary<int,int[]> _IndexCache = null;
	
	public IndexCache()
	{
		//_IndexCache = new Dictionary<int, int[]>();
	}
	
	public static int[] GetIndexTable(int iSize)
	{
		if(_IndexCache == null)
		{
			_IndexCache = new Dictionary<int, int[]>();
		}

		if(_IndexCache.ContainsKey(iSize))
		{
			return _IndexCache[iSize];
		}
			
		// create the buffer
		int[] pBuffer = new int[iSize];
		
		// Build the index list
		int iIndex = 0;
		int iVertexPointer = 0;
		
		for(int i = 0; i < iSize / 6; ++i)
		{
			iIndex = i * 6;
			pBuffer[iIndex] = iVertexPointer;
			pBuffer[iIndex + 1] = iVertexPointer + 1;
			pBuffer[iIndex + 2] = iVertexPointer + 2;
			
			pBuffer[iIndex + 3] = iVertexPointer + 2;
			pBuffer[iIndex + 4] = iVertexPointer + 3;
			pBuffer[iIndex + 5] = iVertexPointer;
			iVertexPointer += 4;
		}
		
		_IndexCache.Add(iSize,pBuffer);
		return pBuffer;
	}
}
