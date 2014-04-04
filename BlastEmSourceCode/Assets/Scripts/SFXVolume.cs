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

public class SFXVolume : MonoBehaviour 
{
	private Sprite _Sprite;
	private Transform _Transform;

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
			_Sprite.AddFrame("V10.png");
			_Sprite.AddFrame("V9.png");
			_Sprite.AddFrame("V8.png");
			_Sprite.AddFrame("V7.png");
			_Sprite.AddFrame("V6.png");
			_Sprite.AddFrame("V5.png");
			_Sprite.AddFrame("V4.png");
			_Sprite.AddFrame("V3.png");
			_Sprite.AddFrame("V2.png");
			_Sprite.AddFrame("V1.png");
			_Sprite.AddFrame("V0.png");
			_Sprite._X = 1000.0f;
			_Sprite._Z = 1000.0f;
			_Sprite._Y = 0.1f;
		}

		_Sprite._X = _Transform.position.x;
		_Sprite._Z = _Transform.position.z;
		_Sprite._Width = _Transform.localScale.x;
		_Sprite._Height = _Transform.localScale.y;

		if(Director._Instance._SFXVolume > 0.9f)
		{
			_Sprite.SetFrame(0);
		}
		else if(Director._Instance._SFXVolume > 0.8f)
		{
			_Sprite.SetFrame(1);
		}
		else if(Director._Instance._SFXVolume > 0.7f)
		{
			_Sprite.SetFrame(2);
		}
		else if(Director._Instance._SFXVolume > 0.6f)
		{
			_Sprite.SetFrame(3);
		}
		else if(Director._Instance._SFXVolume > 0.5f)
		{
			_Sprite.SetFrame(4);
		}
		else if(Director._Instance._SFXVolume > 0.4f)
		{
			_Sprite.SetFrame(5);
		}
		else if(Director._Instance._SFXVolume > 0.3f)
		{
			_Sprite.SetFrame(6);
		}
		else if(Director._Instance._SFXVolume > 0.2f)
		{
			_Sprite.SetFrame(7);
		}
		else if(Director._Instance._SFXVolume > 0.1f)
		{
			_Sprite.SetFrame(8);
		}
		else if(Director._Instance._SFXVolume > 0.0f)
		{
			_Sprite.SetFrame(9);
		}
		else
		{
			_Sprite.SetFrame(10);
		}
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
