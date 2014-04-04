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

public class Player : MonoBehaviour 
{
	private Transform _Transform;

	public float _FireRepeatDelay;
	public float _FireRepeatDelayReset;

	private float _TimeDelta;

	public Sprite _Sprite;

	public float _X;
	public float _Z;

	private bool _Alive;

	// Collision stuff
	public delegate void Collide(Player pPlayer);
	public static Collide _Collide = null;
	public float _Radius;

	// Mouse tracking
	private float _XOffset;
	private float _ZOffset;
	private float _XPosition;
	private float _ZPosition;
	private Vector3 _Pos;

	// Ground stuff
	private float _LowerX;
	private float _HigherX;
	private float _LowerZ;
	private float _HigherZ;	

	// AABB
	public float _X1;
	public float _Z1;
	public float _X2;
	public float _Z2;
	private float _HalfWidth;
	private float _HalfHeight;

	private Image _LevelInfo;


	// Use this for initialization
	void Start () 
	{
		_Transform = this.transform;


		_FireRepeatDelayReset = 0.25f;
		_FireRepeatDelay = 0.0f;

		// Setup the sprite
		_Sprite = Sprite.Spawn(1);
		_Sprite.AddFrame("PlayerF1.png");
		_Sprite.AddFrame("PlayerF2.png");
		_Sprite.AddFrame("PlayerF3.png");
		_Sprite._X = 0.0f;
		_Sprite._Y = 0.1f;
		_Sprite._Z = 0.0f;
		_Sprite._Width = 1.8f;
		_Sprite._Height = 1.8f;
		_Sprite._Animate = true;
		_Alive = false;

		_Radius = 0.75f;
		_XOffset = 0.0f;
		_ZOffset = 0.0f;
		_XPosition = 0.0f;
		_ZPosition = 0.0f;
		_X = 0.0f;
		_Z = 0.0f;

		_Pos = new Vector3(0.0f,0.0f,0.0f);

		_LowerX = Ground._Instance._HigherX;
		_LowerZ = Ground._Instance._HigherZ;
		_HigherX = Ground._Instance._LowerX;
		_HigherZ = Ground._Instance._LowerZ;

		Debug.Log("LX = " + _LowerX + " HX = " + _HigherX + " LZ = " + _LowerZ + " HZ = " + _HigherZ);

		_HalfWidth = 1.0f * 0.5f;
		_HalfHeight = (1.0f * 0.5f) * Director._Instance._Aspect;

	}

	public void SetOffset(float fPosX, float fPosZ)
	{
		//Debug.Log("New offset");

		if(fPosX > _X)
		{
			_XOffset = -(fPosX - _X);
		}
		else
		{
			_XOffset = _X - fPosX;
		}

		if(fPosZ > _Z)
		{
			_ZOffset = -(fPosZ - _Z);
		}
		else
		{
			_ZOffset = _Z - fPosZ;
		}
	}

	public void SetPosition(float fPosX, float fPosZ)
	{
		//Debug.Log("Setting position");

		if(Application.platform != RuntimePlatform.IPhonePlayer)
		{
			_X = fPosX; //+ _XOffset;
			_Z = fPosZ; // + _ZOffset;
		}
		else
		{
			_X = fPosX + _XOffset;
			_Z = fPosZ + _ZOffset;
		}

		// Clamp to screen ranges
		if(_X < (_LowerX + _Radius))
		{
			_X = (_LowerX + _Radius);
		}

		if(_X > (_HigherX - _Radius))
		{
			_X = (_HigherX - _Radius);
		}

		if(_Z < (_LowerZ + (_Radius * 2.0f)))
		{
			_Z = (_LowerZ + (_Radius * 2.0f));
		}

		if(_Z > (_HigherZ))
		{
			_Z = (_HigherZ);
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		_TimeDelta = Time.deltaTime;

		if(!Director._Instance.IsPlayerDead())
		{
		
			// Fire the weapon
			_FireRepeatDelay -= _TimeDelta;

			if(_FireRepeatDelay <= 0.0f)
			{
				if(Director._Instance._SpeedUpFireCurrentTime > 0.0f)
				{
					_FireRepeatDelay = Director._Instance._SpeedUpDelay;
				}
				else
				{
					_FireRepeatDelay = Director._Instance._NormalFireDelay;
				}

				Director._Instance.PlaySample("PLAYER_SHOT",0.2f);


				Bullet.Spawn(_Sprite._X + 1.0f,_Sprite._Z);

				if(Director._Instance._TriFireCurrentTime > 0.0f)
				{
					Bullet.SpawnUp(_Sprite._X + 1.0f,_Sprite._Z);
					Bullet.SpawnDown(_Sprite._X + 1.0f,_Sprite._Z);
				}
			}

			if(_Collide != null)
			{
				_Collide(this);
			}
		}
		else
		{
			float fSin = Mathf.Sin(Director._Instance._PlayerDeadCountdown * 20.0f);

			if(fSin > 0.0f)
			{
				_Sprite._Alive = false;
			}
			else
			{
				_Sprite._Alive = true;
			}
		}


		_Pos.x = _X;
		_Pos.z = _Z;

		// Set the position of the sprite
		Vector3 vPos = _Pos;
		_Sprite._X = vPos.x;
		_Sprite._Y = 0.1f;
		_Sprite._Z = vPos.z - 1.0f;

		// Build the AABB
		_X1 = _X - _HalfWidth;
		_Z1 = _Z - _HalfHeight - 1.0f;
		_X2 = _X + _HalfWidth;
		_Z2 = _Z + _HalfHeight - 1.0f;

	}

	public void Enable()
	{
		this.active = true;
		_Sprite._Alive = true;
		_Sprite._X = 0.0f;
		_Sprite._Y = 0.1f;
		_Sprite._Z = 0.0f;
		_XOffset = 0.0f;
		_ZOffset = 0.0f;
		_X = 0.0f;
		_Z = 0.0f;
		_Pos = new Vector3(0.0f,0.0f,0.0f);
	}

	public void Disable()
	{
		this.active = false;
		_Sprite._Alive = false;
	}
}
