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

public class Title : MonoBehaviour 
{
	public Vector3 _A;
	public Vector3 _B;
	private Vector3 _C;
	public float _Speed;
	private float _Time;
	public string _Image;
	public float _Width;
	public float _Height;

	public enum eTitleEase
	{
		TITLE_EASE_IN,
		TITLE_EASE_OUT,
		TITLE_EASE_NONE,
		TITLE_EASE_KILL,
	};

	private eTitleEase _Ease;
	private Sprite _Sprite = null;

	void Start () 
	{
		_Time = 0.0f;
		_Ease = eTitleEase.TITLE_EASE_IN;
		_C = _A;
	}
	
	void Update () 
	{
		if(_Sprite == null)
		{
			_Sprite = Sprite.Spawn(1);
			_Sprite._Width = _Width;
			_Sprite._Height = _Height;
			_Sprite._Animate = false;
			_Sprite.AddFrame(_Image);
			_Sprite._X = 1000.0f;
			_Sprite._Z = 1000.0f;
		}

		switch(_Ease)
		{
			case eTitleEase.TITLE_EASE_IN:
				_Time += Time.deltaTime;

				if(_Time > 1.0f)
				{
					_Time = 1.0f;
					_C = _B;
					_Ease = eTitleEase.TITLE_EASE_NONE;
				}

				float fRealTime = 1.0f - _Time;
				_C = _B + (_A - _B) * (fRealTime * fRealTime);
				break;

			case eTitleEase.TITLE_EASE_OUT:

				_Time -= Time.deltaTime;

				if(_Time < 0.0f)
				{
					_Time = 0.0f;
					_C = _A;
					_Ease = eTitleEase.TITLE_EASE_KILL;
				}

				_C = _A + (_B - _A) * (_Time * _Time);
				break;

			case eTitleEase.TITLE_EASE_KILL:
				this.active = false;
				_Sprite._Alive = false;
				break;
		}

		this.transform.position = _C;	
		_Sprite._X = _C.x;
		_Sprite._Z = _C.z;
	}

	public void Enable()
	{
		this.active = true;
		_Sprite._Alive = true;
		_C = _A;
		_Ease = eTitleEase.TITLE_EASE_IN;
		_Time = 0.0f;
	}

	public void Disable()
	{
		_Ease = eTitleEase.TITLE_EASE_OUT;
		_Time = 1.0f;
		_C = _B;
	}
}
