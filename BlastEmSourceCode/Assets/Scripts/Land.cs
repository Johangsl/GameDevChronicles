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

public class Land : MonoBehaviour 
{
	private int _NumCellsPerRow;
	private int _NumCellsPerCol;
	private int _Width;
	private int _Depth;
	private int _NumVertices;
	private int _NumTriangles;
	
	private Mesh _Mesh;
	private Vector3[] _Vertices;
	private Vector2[] _UV;
	private Vector2[] _UV2;
	private int[] _Indices;
	private Color[] _Colours;

	void Start () 
	{
		_Mesh = GetComponent<MeshFilter>().mesh;
		
		_NumCellsPerRow = 10 - 1;
		_NumCellsPerCol = 20 - 1;
		_Width = _NumCellsPerRow * 20;
		_Depth = _NumCellsPerCol * 20;
		_NumVertices = 10 * 20;
		_NumTriangles = _NumCellsPerRow * _NumCellsPerCol * 2;
		
		_Vertices = new Vector3[_NumVertices];
		_Indices = new int[_NumTriangles * 3];
		_UV = new Vector2[_NumVertices];
		_UV2 = new Vector2[_NumVertices];
		_Colours = new Color[_NumVertices];
		
		int StartX = -(_Width / 2);
		int StartZ = -3;
		int EndX = _Width / 2;
		int EndZ = _Depth;
		float fUI = ((float)(_NumCellsPerRow) * 0.5f) / (float) _NumCellsPerRow;
		float fVI = ((float)(_NumCellsPerCol) * 0.5f) / (float) _NumCellsPerCol;
				
		int i = 0;
		float fOffsetU = Random.Range(0.0f,10.0f);
		float fOffsetV = Random.Range(0.0f,10.0f);
		float fWaveStep = Random.Range(0.0f,0.5f);
		int iIndex;

		for(int z = StartZ; z <= EndZ; z += 20)
		{
			int j = 0;
			
			for(int x = StartX; x <= EndX; x += 20)
			{ 
				iIndex = i * 10 + j;					
				fWaveStep += 0.01f + Random.Range(0.01f,0.02f);				
				_Colours[iIndex].g = 1.0f;				
				_Colours[iIndex].r = Mathf.Sin(fWaveStep);
				_Colours[iIndex].b = 1.0f;	
				_Vertices[iIndex].x = (float)x;				
				_Vertices[iIndex].y = 1.0f;				
				_Vertices[iIndex].z = (float)z;								
				_UV[iIndex].x = j * fUI + fOffsetU;
				_UV[iIndex].y = i * fVI + fOffsetV;
				_UV2[iIndex].x = j * fUI + fOffsetU;
				_UV2[iIndex].y = i * fVI + fOffsetV;
				
				++j;
			}
			
			fWaveStep += 0.3f + Random.Range(0.1f,1.4f);
			
			++i;
		}
		
		int iBaseIndex = 0;
		
		for(i = 0; i < _NumCellsPerCol; ++i)
		{
			for(int j = 0; j < _NumCellsPerRow; ++j)
			{
				_Indices[iBaseIndex] = i * 10 + j;
				_Indices[iBaseIndex + 1] = i * 10 + j + 1;
				_Indices[iBaseIndex + 2] = (i + 1) * 10 + j;				
				_Indices[iBaseIndex + 3] = (i + 1) * 10 + j;
				_Indices[iBaseIndex + 4] = i * 10 + j + 1;
				_Indices[iBaseIndex + 5] = (i + 1) * 10 + j + 1;			
 				iBaseIndex += 6;
			}
		}
		
		_Mesh.Clear();
		_Mesh.vertices = _Vertices;
		_Mesh.triangles = _Indices;
		_Mesh.colors = _Colours;
		_Mesh.uv = _UV;
		_Mesh.uv1 = _UV;	
	}	
}
