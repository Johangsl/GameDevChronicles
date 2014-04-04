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

public class Button : MonoBehaviour 
{
	public Vector3 _A;
	public Vector3 _B;
	private Vector3 _C;
	public float _Speed;
	private float _Time;
	public string _UpImage;
	public string _DownImage;
	public string _OverImage;
	public float _Width;
	public float _Height;
	public int _Event;
	public float _Depth;
	public bool _Shake;
	public float _ShakeIntensity;
	public float _ShakeTime;
	public bool _Locked;
	public string _LockUp;
	public string _LockDown;

	public enum eButtonEase
	{
		BUTTON_EASE_IN,
		BUTTON_EASE_OUT,
		BUTTON_EASE_NONE,
		BUTTON_EASE_KILL,
		BUTTON_EASE_SETUP,
	};

	private eButtonEase _Ease;
	private Sprite _Sprite = null;
	private bool _IsButtonDown;
	private bool _IsOver;

	public enum ePlatform
	{
		PLATFORM_ALL,
		PLATFORM_MOBILE_ONLY,
		PLATFORM_DESKTOP_ONLY,
	};

	public ePlatform _Platform;	

	void Start () 
	{
		_Time = 0.0f;
		_Ease = eButtonEase.BUTTON_EASE_SETUP;
		_IsButtonDown = false;	
		_IsOver = false;
		this.transform.localScale = new Vector3(_Width,1.0f,_Height);
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
			_Sprite.AddFrame(_UpImage);
			_Sprite.AddFrame(_DownImage);
			_Sprite.AddFrame(_OverImage);			
			_Sprite.AddFrame(_LockUp);
			_Sprite.AddFrame(_LockDown);
			_Sprite._X = 10000.0f;
			_Sprite._Z = 10000.0f;
			_Sprite._Y = _Depth;

			if(_Locked)
			{
				_Sprite.SetFrame(2);
			}
		}

		switch(_Ease)
		{
			case eButtonEase.BUTTON_EASE_IN:

				_Time += _Speed * Time.deltaTime;

				if(_Time > 1.0f)
				{
					_Time = 1.0f;
					_C = _B;
					_Ease = eButtonEase.BUTTON_EASE_NONE;

					if(_Shake)
					{
						Director._Instance.ShakeCamera(_ShakeIntensity,_ShakeTime);
					}
				}

				float fRealTime = 1.0f - _Time;
				_C = _B + (_A - _B) * (fRealTime * fRealTime);
				break;

			case eButtonEase.BUTTON_EASE_OUT:

				_Time -= _Speed  * Time.deltaTime;

				if(_Time < 0.0f)
				{
					_Time = 0.0f;
					_C = _A;
					_Ease = eButtonEase.BUTTON_EASE_KILL;
				}

				_C = _A + (_B - _A) * (_Time * _Time);
				break;

			case eButtonEase.BUTTON_EASE_KILL:
				this.active = false;
				_Sprite._Alive = false;
				break;
		}

		this.transform.position = _C;	
		_Sprite._X = _C.x;
		_Sprite._Z = _C.z;

		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
    
       		if(Physics.Raycast(ray, out hit, 100))
			{
				if(hit.transform.gameObject.name == this.name)
				{
					_IsButtonDown = true;

					if(_Locked)
					{
						_Sprite.SetFrame(3);
					}
					else
					{
						_Sprite.SetFrame(1);
					}
				}
			}
		}

		if(Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
    
       		if(Physics.Raycast(ray, out hit, 100))
			{
				if(hit.transform.gameObject.name == this.name)
				{
					if(_IsButtonDown && !_Locked)
					{
						Director._Instance.FireEvent(_Event);
						Director._Instance.PlaySample("PRESS",1.0f);
					}
				}
			}

			_IsButtonDown = false;

			if(_Locked)
			{
				_Sprite.SetFrame(2);
			}
			else
			{
				_Sprite.SetFrame(0);
			}
		}

		if(!_IsButtonDown)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
    
       		if(Physics.Raycast(ray, out hit, 100))
			{
				if(hit.transform.gameObject.name == this.name)
				{
					if(!_IsOver)
					{
						Director._Instance.PlaySample("CLICK",1.0f);
					}

					_IsOver = true;
					_Sprite.SetFrame(2);

				}
				else
				{

					_IsOver = false;
					_Sprite.SetFrame(0);

				}
			}
		}
		else
		{
			_IsOver = false;
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
		_Sprite._Alive = true;
		_C = _A;
		_Ease = eButtonEase.BUTTON_EASE_IN;
		_Time = 0.0f;

		if(_Locked)
		{
			_Sprite.SetFrame(2);
		}
		else
		{
			_Sprite.SetFrame(0);
		}

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
		if(_Ease == eButtonEase.BUTTON_EASE_SETUP)
		{
			return;
		}
		
		_Ease = eButtonEase.BUTTON_EASE_OUT;
		_Time = 1.0f;
		_C = _B;
	}
}
