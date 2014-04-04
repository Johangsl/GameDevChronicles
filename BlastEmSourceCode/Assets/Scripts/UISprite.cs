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

public class UISprite : MonoBehaviour 
{
	private Sprite _Sprite;
	private Transform _Transform;
	public string _Name;

	void Start () 
	{
		_Transform = transform;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_Sprite == null)
		{
			_Sprite = Sprite.Spawn(1);
			_Sprite._Width = _Transform.localScale.x;
			_Sprite._Height = _Transform.localScale.y;
			_Sprite._Animate = false;
			_Sprite.AddFrame(_Name);
			_Sprite._X = 1000.0f;
			_Sprite._Z = 1000.0f;
			_Sprite._Y = 0.1f;

		}

		_Sprite._X = _Transform.position.x;
		_Sprite._Z = _Transform.position.z;
		_Sprite._Width = _Transform.localScale.x;
		_Sprite._Height = _Transform.localScale.y;
	}

	void OnDisable()
	{
		if(_Sprite != null)
		{
			Sprite.Kill(_Sprite);
			_Sprite = null;
		}
	}
}
