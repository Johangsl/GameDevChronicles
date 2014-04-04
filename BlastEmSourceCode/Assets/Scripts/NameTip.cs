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

public class NameTip : MonoBehaviour 
{
	private Transform _Transform;
	private Vector3 _Scale;
	private Vector3 _NewScale;
	private float _Time;

	private UIGO _Parent;

	// Use this for initialization
	void Start () 
	{
		_Transform = this.transform;
		_Scale = _Transform.localScale;

		_Parent = this.GetComponent<UIGO>();
		_Scale = _Parent._SB;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Scores._Name != "BLAST EM  ")
		{
			this.active = false;
			this.renderer.enabled = false;
		}

		if(_Parent._Ease == UIGO.eEase.EASE_NONE)
		{

			_Time += Time.deltaTime;
			_NewScale = _Scale;
			float fScale = Mathf.Sin(_Time * 20.0f);
			fScale += 1.0f;
			fScale *= 0.5f;
			
			_NewScale.x += 0.4f * fScale;
			_NewScale.y += 0.4f * fScale;
			_NewScale.z += 0.4f * fScale;


			_Transform.localScale = _NewScale;
		}
	}
}
