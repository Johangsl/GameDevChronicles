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

public class QuadRender
{
	private QuadEntry[]			_Quads;
	private QuadEntry[]			_UI;
	private int 				_QuadCount;
	private int 				_UIQuadCount;

	private Mesh				_Mesh;
	private Vector3				_MeshPosition;
	private Quaternion			_MeshRotation;
	private Matrix4x4			_Matrix;
	private Vector3[]			_Vertices;
	private Vector2[] 			_UV;
	private Color32[]			_Colours;	
	private Material 			_Material;
	private int 				_TotalQuadCount;
	private bool				_UseA;
	private int 				_LastSize;


	public QuadRender(Material pMaterial, int iQuadCount, Mesh pMesh)
	{
		//_IndexCache = new IndexCache();

		_TotalQuadCount = iQuadCount;
		_Quads = new QuadEntry[_TotalQuadCount];
		_QuadCount = 0;

		_UI = new QuadEntry[_TotalQuadCount];
		_UIQuadCount = 0;


		for(int i = 0; i < _TotalQuadCount; ++i)
		{
			_Quads[i] = new QuadEntry();
			_UI[i] = new QuadEntry();
		}

		_Mesh = pMesh;

		//_Mesh.bounds.SetMinMax(new Vector3(-1000.0f,-1000.0f,-1000.0f),new Vector3(1000.0f,1000.0f,1000.0f));

		_Mesh.MarkDynamic();

		_MeshPosition = new Vector3(0.0f,0.0f,0.0f);
		_MeshRotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
		_Matrix = Matrix4x4.identity;
		_Vertices = new Vector3[_TotalQuadCount * 4 * 2];
		_UV = new Vector2[_TotalQuadCount * 4 * 2];
		_Colours = new Color32[_TotalQuadCount * 4 * 2];

		_Material = pMaterial; //Resources.Load("Quads") as Material;
		_LastSize = 0;
	}

	public void AddQuad(float fX, float fY, float fZ, float fWidth, float fHeight, float fR, float fG, float fB, float fA, float fU, float fV, float fU1, float fV1, float fAngle)
	{
		if(_QuadCount == 3000)
		{
			return;
		}
		
		_Quads[_QuadCount]._Position.x = fX;
		_Quads[_QuadCount]._Position.y = fY;
		_Quads[_QuadCount]._Position.z = fZ;
		_Quads[_QuadCount]._Width = fWidth;
		_Quads[_QuadCount]._Height = fHeight;
		_Quads[_QuadCount]._Colour.r = fR;
		_Quads[_QuadCount]._Colour.g = fG;
		_Quads[_QuadCount]._Colour.b = fB;
		_Quads[_QuadCount]._Colour.a = fA;
		_Quads[_QuadCount]._HalfWidth = fWidth * 0.5f;
		_Quads[_QuadCount]._HalfHeight = fHeight * 0.5f;
		_Quads[_QuadCount]._U = fU;
		_Quads[_QuadCount]._V = fV;
		_Quads[_QuadCount]._U1 = fU1;
		_Quads[_QuadCount]._V1 = fV1;
		_Quads[_QuadCount]._Angle = fAngle;

		++_QuadCount;

	}

	public void Render()
	{
		if(_QuadCount == 0 && _UIQuadCount == 0)
		{
			return;
		}

		int iVertexCount = 0;

		float fTX;
		float fTZ;
		float fCos;
		float fSin;
		float fX;
		float fZ;

		//Profiler.BeginSample("QuadRender BuildVertices");
		for(int i = 0; i < _QuadCount; ++i)
		{
			fSin = Mathf.Sin(_Quads[i]._Angle);
			fCos = Mathf.Cos(_Quads[i]._Angle);

			fX = -_Quads[i]._HalfWidth;
			fZ = -_Quads[i]._HalfHeight;
			fTX = fX * fCos - fZ * fSin;
			fTZ = fX * fSin + fZ * fCos;
			fTX += _Quads[i]._Position.x;
			fTZ += _Quads[i]._Position.z;
			_Vertices[iVertexCount].x = fTX;
			_Vertices[iVertexCount].z = fTZ;

			//_Vertices[iVertexCount].x = _Quads[i]._Position.x - _Quads[i]._HalfWidth;
			//_Vertices[iVertexCount].z = _Quads[i]._Position.z - _Quads[i]._HalfHeight;
			_Vertices[iVertexCount].y = _Quads[i]._Position.y;
			_Colours[iVertexCount] = _Quads[i]._Colour;
			_UV[iVertexCount].x = _Quads[i]._U;
			_UV[iVertexCount].y = _Quads[i]._V;
			++iVertexCount;

			fX = _Quads[i]._HalfWidth;
			fZ = -_Quads[i]._HalfHeight;
			fTX = fX * fCos - fZ * fSin;
			fTZ = fX * fSin + fZ * fCos;
			fTX += _Quads[i]._Position.x;
			fTZ += _Quads[i]._Position.z;
			_Vertices[iVertexCount].x = fTX;
			_Vertices[iVertexCount].z = fTZ;			
			
			//_Vertices[iVertexCount].x = _Quads[i]._Position.x + _Quads[i]._HalfWidth;
			//_Vertices[iVertexCount].z = _Quads[i]._Position.z - _Quads[i]._HalfHeight;
			_Vertices[iVertexCount].y = _Quads[i]._Position.y;
			_Colours[iVertexCount] = _Quads[i]._Colour;
			_UV[iVertexCount].x = _Quads[i]._U1;
			_UV[iVertexCount].y = _Quads[i]._V;
			++iVertexCount;
			
			fX = _Quads[i]._HalfWidth;
			fZ = _Quads[i]._HalfHeight;
			fTX = fX * fCos - fZ * fSin;
			fTZ = fX * fSin + fZ * fCos;
			fTX += _Quads[i]._Position.x;
			fTZ += _Quads[i]._Position.z;
			_Vertices[iVertexCount].x = fTX;
			_Vertices[iVertexCount].z = fTZ;

			//_Vertices[iVertexCount].x = _Quads[i]._Position.x + _Quads[i]._HalfWidth;
			//_Vertices[iVertexCount].z = _Quads[i]._Position.z + _Quads[i]._HalfHeight;
			_Vertices[iVertexCount].y = _Quads[i]._Position.y;
			_Colours[iVertexCount] = _Quads[i]._Colour;
			_UV[iVertexCount].x = _Quads[i]._U1;
			_UV[iVertexCount].y = _Quads[i]._V1;
			++iVertexCount;

			fX = -_Quads[i]._HalfWidth;
			fZ = _Quads[i]._HalfHeight;
			fTX = fX * fCos - fZ * fSin;
			fTZ = fX * fSin + fZ * fCos;
			fTX += _Quads[i]._Position.x;
			fTZ += _Quads[i]._Position.z;
			_Vertices[iVertexCount].x = fTX;
			_Vertices[iVertexCount].z = fTZ;

			//_Vertices[iVertexCount].x = _Quads[i]._Position.x - _Quads[i]._HalfWidth;
			//_Vertices[iVertexCount].z = _Quads[i]._Position.z + _Quads[i]._HalfHeight;
			_Vertices[iVertexCount].y = _Quads[i]._Position.y;
			_Colours[iVertexCount] = _Quads[i]._Colour;
			_UV[iVertexCount].x = _Quads[i]._U;
			_UV[iVertexCount].y = _Quads[i]._V1;
			++iVertexCount;
		}


		if(_LastSize == (_QuadCount * 6))
		{
			_Mesh.vertices = _Vertices;
			_Mesh.uv = _UV;
			_Mesh.colors32 = _Colours;

		}
		else
		{
			_Mesh.Clear();
			_Mesh.vertices = _Vertices;
			_Mesh.uv = _UV;
			_Mesh.colors32 = _Colours;
			int[] pIndices = IndexCache.GetIndexTable(_QuadCount * 6);
			_Mesh.triangles = pIndices;
		}

		_Mesh.RecalculateBounds();

		_LastSize = _QuadCount * 6;
		//Debug.Log("Vertex size = " + iVertexCount + " Quad list size = " + _QuadCount + " Indices size = " + pIndices.Length);

		//Graphics.DrawMesh(_Mesh,_Matrix,_Material,0);
		_QuadCount = 0;
		_UIQuadCount = 0;
	
	}
}
