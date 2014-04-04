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

public class AtlasItem 
{
	public string _Name;
	public Texture2D _Texture;
	public string _TextureName;
	public float _X;
	public float _Y;
	public float _Width;
	public float _Height;
	public float _TextureWidth;
	public float _TextureHeight;
	
	public float _U;
	public float _V;
	public float _U1;
	public float _V1;
	
	public AtlasItem(string pName, string pTextureName, float fX, float fY, float fWidth, float fHeight, float fTextureSizeX, float fTextureSizeY)
	{
		//Debug.Log("XGDKAtlasItem: Name --> " + pName);
		//Debug.Log("XGDKAtlasItem: TextureName --> " + pTextureName);
		
		_Name = pName;
		_X = fX;
		_Y = fY;
		_Width = fWidth;
		_Height = fHeight;
		

		_TextureWidth = fTextureSizeX;
		_TextureHeight = fTextureSizeY;

		
		_U = _X;
		_V1 = _Y;
		_U1 = _X + _Width;
		_V = _Y + _Height;
		_U /= _TextureWidth;
		_V /= _TextureHeight;
		_U1 /= _TextureWidth;
		_V1 /= _TextureHeight;
		
		_V = 1.0f - _V;
		_V1 = 1.0f - _V1;
	}
}
