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

[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class Starfield : MonoBehaviour 
{
	
	// World dimension
	public float _LowerWorldX;
	public float _UpperWorldX;
	public float _UpperY;
	public float _LowerY;


	// World Y
	public float _WorldY;
	
	
	public int _Count;
	public float _SizeLowerLimit;
	public float _SizeUpperLimit;

	// Screen data
	public float _LowerZ;
	public float _UpperZ;

	// Speed data
	public float _BaseSpeed;
	public float _SpeedJitter;

	// Colour data
	public float _ColourScale;	
	
	private Mesh _Mesh;
	private Vector3[] _Positions;	
	private Vector3[] _Normals;
	private Color[] _Colour;
	private int[] _Indices;
	private Material _Material;

		
	void Start () 
	{
		
		// Build mesh
		_Mesh = GetComponent<MeshFilter>().mesh;
		_Mesh.Clear();
		_Positions = new Vector3[_Count * 4];
		_Normals = new Vector3[_Count * 4];
		_Colour = new Color[_Count * 4];
		_Indices = new int[_Count * 6];

		_Material = renderer.sharedMaterial;
		_Material.SetFloat("_UpperZ",_UpperZ);
		_Material.SetFloat("_LowerZ",_LowerZ);
		_Material.SetFloat("_Speed",_BaseSpeed);
		_Material.SetFloat("_SpeedJitter",_SpeedJitter);


		float fX;
		float fY = _WorldY;
		float fZ;
		float fSize;
		
		int iVertexIndex = 0;
		int iIndex = 0;
		
		for(int i = 0; i < _Count; ++i)
		{
			fX = 0.0f;
			fZ = Random.Range(_LowerWorldX,_UpperWorldX);; 
			fSize = Random.Range(_SizeLowerLimit,_SizeUpperLimit) * 0.5f;

			fY = Random.Range(_LowerY,_UpperY);
			float fPerc = fY - _LowerY;
			fPerc /= (_UpperY - _LowerY);

			fSize = 0.08f;
			fSize *= fPerc;
						
			_Positions[iVertexIndex].x = fX - fSize;
			_Positions[iVertexIndex].y = fY ;
			_Positions[iVertexIndex].z = fZ - fSize;
			
			_Positions[iVertexIndex + 1].x = fX + fSize;
			_Positions[iVertexIndex + 1].y = fY;
			_Positions[iVertexIndex + 1].z = fZ - fSize;
			
			_Positions[iVertexIndex + 2].x = fX + fSize;
			_Positions[iVertexIndex + 2].y = fY;
			_Positions[iVertexIndex + 2].z = fZ + fSize;
			
			_Positions[iVertexIndex + 3].x = fX - fSize;
			_Positions[iVertexIndex + 3].y = fY;
			_Positions[iVertexIndex + 3].z = fZ + fSize;
			
			_Indices[iIndex] = iVertexIndex + 0;
			_Indices[iIndex + 1] = iVertexIndex + 1;
			_Indices[iIndex + 2] = iVertexIndex + 2;
			
			_Indices[iIndex + 3] = iVertexIndex + 0;
			_Indices[iIndex + 4] = iVertexIndex + 2;
			_Indices[iIndex + 5] = iVertexIndex + 3;
						
			float fTimeOffset = Random.Range(0.0f,1000.0f);
			fTimeOffset /= 1000.0f;

			float fSpeed = Random.Range(0.0f,_SpeedJitter);
			fSpeed /= _SpeedJitter;

			_Normals[iVertexIndex] = new Vector3(fTimeOffset,fSpeed,0.0f);
			_Normals[iVertexIndex + 1] = new Vector3(fTimeOffset,fSpeed,0.0f);
			_Normals[iVertexIndex + 2] = new Vector3(fTimeOffset,fSpeed,0.0f);
			_Normals[iVertexIndex + 3] = new Vector3(fTimeOffset,fSpeed,0.0f);

			float fRed = Random.Range(0.0f,_ColourScale);
			float fGreen = Random.Range(0.0f,_ColourScale);
			float fBlue = Random.Range(0.0f,_ColourScale);

			fPerc *= 0.5f;
			fRed = fPerc;
			fGreen = fPerc;
			fBlue = fPerc;

			_Colour[iVertexIndex] = new Color(fRed,fGreen,fBlue,0.0f);
			_Colour[iVertexIndex + 1] = new Color(fRed,fGreen,fBlue,1.0f);;
			_Colour[iVertexIndex + 2] = new Color(fRed,fGreen,fBlue,1.0f);;
			_Colour[iVertexIndex + 3] = new Color(fRed,fGreen,fBlue,0.0f);;

			
			iVertexIndex += 4;
			iIndex += 6;
		}
		
		_Mesh.vertices = _Positions;
		_Mesh.normals = _Normals;
		_Mesh.colors = _Colour;
		_Mesh.triangles = _Indices;
		_Mesh.RecalculateBounds();
	}
}
