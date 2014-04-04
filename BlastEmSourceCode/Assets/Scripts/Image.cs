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

public class Image : MonoBehaviour 
{
	public Vector3 _A;
	public Vector3 _B;
	private Vector3 _C;

	public Vector2 _SA;
	public Vector2 _SB;
	private Vector2 _SC;

	public float _AAlpha;
	public float _BAlpha;
	public float _Alpha;

	public float _Speed;
	private float _Time;
	public string _Image;
	public float _Depth;
	public bool _FireEventOnFinishTransitionIn;
	public int _Event;

	public float _StartDelay;
	private float _Delay;

	public enum eIMAGEEase
	{
		IMAGE_EASE_IN,
		IMAGE_EASE_OUT,
		IMAGE_EASE_NONE,
		IMAGE_EASE_KILL,
	};

	public enum ePlatform
	{
		PLATFORM_ALL,
		PLATFORM_MOBILE_ONLY,
		PLATFORM_DESKTOP_ONLY,
	};

	public ePlatform _Platform;


	private eIMAGEEase _Ease;
	private Sprite _Sprite = null;

	void Awake()
	{
		/*
		if(_Console)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				this.active = false;
			}
		}
		else
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.active = false;
			}			
		}
		*/
	}

	void Start () 
	{
		_Time = 0.0f;
		_Ease = eIMAGEEase.IMAGE_EASE_KILL;
		_C = _A;
	}
	
	void Update () 
	{
		_Delay -= Time.deltaTime;

		if(_Delay > 0.0f)
		{
			return;
		}

		_Delay = 0.0f;
		
		if(_Sprite == null)
		{
			_Sprite = Sprite.Spawn(1);
			_Sprite._Width = _SA.x;
			_Sprite._Height = _SA.y;
			_Sprite._Animate = false;
			_Sprite.AddFrame(_Image);
			_Sprite._X = 1000.0f;
			_Sprite._Z = 1000.0f;
			_Sprite._Y = _Depth;
		}

		switch(_Ease)
		{
			case eIMAGEEase.IMAGE_EASE_IN:
				_Time += Time.deltaTime;

				if(_Time > 1.0f)
				{
					_Time = 1.0f;
					_C = _B;
					_Ease = eIMAGEEase.IMAGE_EASE_NONE;

					if(_FireEventOnFinishTransitionIn)
					{
						Director._Instance.FireEvent(_Event);
					}
				}

				float fRealTime = 1.0f - _Time;
				_C = _B + (_A - _B) * (fRealTime * fRealTime);
				_SC = _SB + (_SA - _SB) * (fRealTime * fRealTime);
				_Alpha = _BAlpha + (_AAlpha - _BAlpha) * (fRealTime * fRealTime);
				break;

			case eIMAGEEase.IMAGE_EASE_OUT:

				_Time -= Time.deltaTime;

				if(_Time < 0.0f)
				{
					_Time = 0.0f;
					_C = _A;
					_Ease = eIMAGEEase.IMAGE_EASE_KILL;
				}

				_C = _A + (_B - _A) * (_Time * _Time);
				_SC = _SA + (_SB - _SA) * (_Time * _Time);
				_Alpha = _AAlpha + (_BAlpha - _AAlpha) * (_Time * _Time);
				break;

			case eIMAGEEase.IMAGE_EASE_KILL:
				this.active = false;
				_Sprite._Alive = false;
				Sprite.Kill(_Sprite);
				_Sprite = null;
				break;
		}

		this.transform.position = _C;

		if(_Sprite != null)
		{	
			_Sprite._X = _C.x;
			_Sprite._Z = _C.z;
			_Sprite._Width = _SC.x;
			_Sprite._Height = _SC.y;
			_Sprite._Alpha = _Alpha;
		}
	}

	public void Enable()
	{
		if(_Platform != ePlatform.PLATFORM_ALL)
		{
			if(_Platform == ePlatform.PLATFORM_MOBILE_ONLY)
			{
				if(Application.platform != RuntimePlatform.IPhonePlayer)
				{
					if(_Sprite != null)
					{
						Sprite.Kill(_Sprite);
						_Sprite = null;
						
					}
					return;
				}
			}
			else if(_Platform == ePlatform.PLATFORM_DESKTOP_ONLY)
			{
				if(Application.platform == RuntimePlatform.IPhonePlayer)
				{
					if(_Sprite != null)
					{
						Sprite.Kill(_Sprite);
						_Sprite = null;
					}					
					return;
				}
			}
		}		

		this.active = true;
		//_Sprite._Alive = true;
		_C = _A;
		_SC = _SA;
		_Ease = eIMAGEEase.IMAGE_EASE_IN;
		_Time = 0.0f;
		_Delay = _StartDelay;
	}

	public void Disable()
	{

		if(_Platform != ePlatform.PLATFORM_ALL)
		{
			if(_Platform == ePlatform.PLATFORM_MOBILE_ONLY)
			{
				if(Application.platform != RuntimePlatform.IPhonePlayer)
				{
					if(_Sprite != null)
					{
						Sprite.Kill(_Sprite);
						_Sprite = null;
					}

					return;
				}
			}
			else if(_Platform == ePlatform.PLATFORM_DESKTOP_ONLY)
			{
				if(Application.platform == RuntimePlatform.IPhonePlayer)
				{
					if(_Sprite != null)
					{
						Sprite.Kill(_Sprite);
						_Sprite = null;
					}					
					return;
				}
			}
		}		

		if(_Ease == eIMAGEEase.IMAGE_EASE_OUT)
		{
			return;
		}

		_Ease = eIMAGEEase.IMAGE_EASE_OUT;
		_Time = 1.0f;
		_C = _B;
		_SC = _SB;
	}
}
