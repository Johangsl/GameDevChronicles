﻿/*
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

[RequireComponent (typeof (BoxCollider))]

public class UIGO : MonoBehaviour 
{
	public Vector3 _A;
	public Vector3 _B;
	public Vector3 _C;
	public bool _PosIsLerp;
	public float _LerpSpeed;
	private float _Lerp;
	private bool _DoLerp;

	public Vector3 _SA;
	public Vector3 _SB;
	private Vector3 _SC;

	public float _AAlpha;
	public float _BAlpha;
	public float _Alpha;

	public float _Speed;
	private float _Time;
	public string _Image;
	public float _Depth;
	public bool _FireEventOnFinishTransitionIn;
	public int _Event;
	public bool _IsButton;
	private bool _IsButtonDown;

	private Renderer _Renderer;
	private Transform _Transform;
	private Material _Material;

	public float _StartDelay;
	private float _Delay;

	public enum eEase
	{
		EASE_IN,
		EASE_OUT,
		EASE_NONE,
		EASE_KILL,
	};

	public eEase _Ease;
	
	void Awake()
	{

	}

	void Start () 
	{
		_Time = 0.0f;
		_Ease = eEase.EASE_KILL;
		_C = _A;

		_Renderer = this.GetComponent<Renderer>();
		_Transform = this.transform;
		_Material = _Renderer.material;
		_IsButtonDown = false;			
	}
	
	void Update () 
	{
		_Delay -= Time.deltaTime;

		if(_Delay > 0.0f)
		{
			return;
		}

		_Delay = 0.0f;

		switch(_Ease)
		{
			case eEase.EASE_IN:
				_Time += Time.deltaTime;

				if(_Time > 1.0f)
				{
					_Time = 1.0f;
					_C = _B;
					_Ease = eEase.EASE_NONE;
					_Lerp = 0.0f;
					_DoLerp = true;

					if(_FireEventOnFinishTransitionIn)
					{
						Director._Instance.FireEvent(_Event);
					}
				}

				float fRealTime = 1.0f - _Time;
				_SC = _SB + (_SA - _SB) * (fRealTime * fRealTime);
				_Alpha = _BAlpha + (_AAlpha - _BAlpha) * (fRealTime * fRealTime);
					
				if(!_PosIsLerp)
				{
					_C = _B + (_A - _B) * (fRealTime * fRealTime);
					_Transform.position = _C;
				}

				_Transform.localScale = _SC;
				_Material.SetFloat("_Alpha",_Alpha);

				break;

			case eEase.EASE_OUT:

				_Time -= Time.deltaTime;

				if(_Time < 0.0f)
				{
					_Time = 0.0f;
					_C = _A;
					_Ease = eEase.EASE_KILL;
				}

				_C = _A + (_B - _A) * (_Time * _Time);
				_SC = _SA + (_SB - _SA) * (_Time * _Time);
				_Alpha = _AAlpha + (_BAlpha - _AAlpha) * (_Time * _Time);

				if(!_PosIsLerp)
				{
					_C = _A + (_B - _A) * (_Time * _Time);
					_Transform.position = _C;
				}
			
				_Transform.localScale = _SC;
				_Material.SetFloat("_Alpha",_Alpha);

				break;

			case eEase.EASE_KILL:
				this.active = false;
				_Renderer.enabled = false;
				_Transform.position = _A;
				_Transform.localScale = _SA;
				_Material.SetFloat("_Alpha",_Alpha);
				_DoLerp = false;
				break;
		}


		if(_PosIsLerp && _DoLerp)
		{
			_Lerp += _LerpSpeed * Time.deltaTime;

			if(_Lerp > 1.0f)
			{
				_Lerp = 0.0f;
			}

			_C = Vector3.Lerp(_A,_B,_Lerp);
		}

		this.transform.position = _C;

		if(_IsButton)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
	    
	       		if(Physics.Raycast(ray, out hit, 100))
				{
					if(hit.transform.gameObject.name == this.name)
					{
						_IsButtonDown = true;
						_Material.SetFloat("_Button",1.0f);
						
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
						if(_IsButtonDown)
						{
							Director._Instance.FireEvent(_Event);
						}
					}
				}

				_IsButtonDown = false;

				_Material.SetFloat("_Button",0.0f);
			}	

			if(!_IsButtonDown)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
	    
	       		if(Physics.Raycast(ray, out hit, 100))
				{
					if(hit.transform.gameObject.name == this.name)
					{
						_Material.SetFloat("_Over",1.0f);
					}
					else
					{

						_Material.SetFloat("_Over",0.0f);

					}
				}
			}
			else
			{

				_Material.SetFloat("_Over",0.0f);
	
			}
		}
	}

	public void Enable()
	{
		this.active = true;

		_Renderer.enabled = true;
		_Transform.position = _A;
		_Transform.localScale = _SA;
		_Material.SetFloat("_Alpha",_AAlpha);

		_C = _A;
		_SC = _SA;
		_Ease = eEase.EASE_IN;
		_Time = 0.0f;
		_IsButtonDown = false;	
		_DoLerp = false;
		_Lerp = 0.0f;
		_Delay = _StartDelay;
	}

	public void Disable()
	{
		if(_Ease == eEase.EASE_OUT)
		{
			return;
		}

		_Renderer.enabled = true;
		this.active = true;
		_Transform.position = _B;
		_Material.SetFloat("_Alpha",_BAlpha);

		_Ease = eEase.EASE_OUT;
		_Time = 1.0f;
		_C = _B;
		_SC = _SB;
	}
}
