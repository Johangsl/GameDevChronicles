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

public class QR : MonoBehaviour 
{
	public QuadRender _QR;
	public Atlas _Atlas;
	public string _AtlasScript;

	void Awake()
	{
		_Atlas = new Atlas();
		_Atlas.LoadAtlas(_AtlasScript);
		_QR = new QuadRender(null,300,this.GetComponent<MeshFilter>().mesh);
	}

	void LateUpdate () 
	{
		_QR.Render();
	}
}
