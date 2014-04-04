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

public class QuadEntry
{
	public Vector3 _Position;
	public float _Width;
	public float _Height;
	public float _HalfWidth;
	public float _HalfHeight;
	public Color _Colour;
	public float _U;
	public float _V;
	public float _U1;
	public float _V1;
	public string _Sprite;
	public float _Angle;
	public float _Depth;


	public QuadEntry()
	{
		_Position = new Vector3(0.0f,0.0f,0.0f);
		_Colour = new Color(0.0f,0.0f,0.0f,0.0f);
	}
}
