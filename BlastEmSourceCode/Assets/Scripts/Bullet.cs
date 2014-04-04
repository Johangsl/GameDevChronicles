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
using System.Collections.Generic;


public class Bullet
{
	static private LinkedList<Bullet>	_Pool = null;
	static private LinkedList<Bullet>	_Active = null;
	static private List<Bullet> _ClearList;

	public float _X;
	public float _Y;
	public float _Z;
	public bool _Alive;
	public float _Radius;
	public float _VX;
	public float _VZ;
	public int _Type;

	public delegate void Collide(Bullet pBullet);
	public static Collide _Collide = null;

	private Sprite _Sprite = null;
	private static float _Speed;
	private static float _MaxX;

	public Bullet()
	{

	}

	public bool UpdateAndRender(float fTimeDelta)
	{
		if(_Sprite == null)
		{
			_Sprite = Sprite.Spawn(1);


			if(_Type == 0)
			{
				_Sprite.AddFrame("Bullet.png");
			}
			else if(_Type == 1)
			{
				_Sprite.AddFrame("BulletUp.png");

			}
			else
			{
				_Sprite.AddFrame("BulletDown.png");
			}

			_Sprite._Width = 1.0f;
			_Sprite._Height = 1.0f;		

			_Sprite._AI = null;
			_Sprite._Bullet = this;	
		}

		_X += _VX * _Speed * fTimeDelta;
		_Z += _VZ * _Speed * fTimeDelta;

		if(_Collide != null)
		{
			_Collide(this);

			if(!_Alive)
			{
				return false;
			}
		}

		if(_X > _MaxX)
		{
			return false;
		}

		if(_Z > 12.0f)
		{
			return false;
		}

		if(_Z < -12.0f)
		{
			return false;
		}

		_Sprite._X = _X;
		_Sprite._Y = 0.1f;
		_Sprite._Z = _Z;

		return true;

	}

	public void Kill()
	{
		_Alive = false;
	}

	public static Bullet Spawn(float fX, float fZ)
	{
		if(_Pool.Count == 0)
		{
			//Debug.Log("ERROR: No pooled bullets");
			return null;
		}

		Bullet pObject = _Pool.First.Value;
		pObject._Alive = true;
		_Pool.RemoveFirst();
		_Active.AddLast(pObject);

		pObject._X = fX;
		pObject._Y = 0.1f;
		pObject._Z = fZ;
		pObject._VX = 1.0f;
		pObject._VZ = 0.0f;
		pObject._Radius = 0.5f;
		pObject._Type = 0;
		return pObject;
	}

	public static Bullet SpawnUp(float fX, float fZ)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return null;
		}

		Bullet pObject = _Pool.First.Value;
		pObject._Alive = true;
		_Pool.RemoveFirst();
		_Active.AddLast(pObject);

		pObject._X = fX;
		pObject._Y = 0.1f;
		pObject._Z = fZ;
		pObject._VX = 0.0f;
		pObject._VZ = 1.0f;
		pObject._Radius = 0.5f;
		pObject._Type = 1;
		return pObject;
	}

	public static Bullet SpawnDown(float fX, float fZ)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return null;
		}

		Bullet pObject = _Pool.First.Value;
		pObject._Alive = true;
		_Pool.RemoveFirst();
		_Active.AddLast(pObject);

		pObject._X = fX;
		pObject._Y = 0.1f;
		pObject._Z = fZ;
		pObject._VX = 0.0f;
		pObject._VZ = -1.0f;
		pObject._Radius = 0.5f;
		pObject._Type = 2;
		return pObject;
	}



	public static void Initialise(int iCount)
	{
		Debug.Log("Bullet initialise");

		if(_Pool == null)
		{
			_Pool = new LinkedList<Bullet>();
			_Active = new LinkedList<Bullet>();
			_ClearList = new List<Bullet>();
		}

		for(int i = 0; i < iCount; ++i)
		{
			Bullet pObject = new Bullet();
			_Pool.AddLast(pObject);
		}

		_Speed = 20.0f;
		_MaxX = Ground._Instance._LowerX;

		//Debug.Log("MaxX = " + _MaxX);
	}

	public static void UpdateAndRenderAll(float fTimeDelta)
	{
    	Bullet pObject;

		for(LinkedListNode<Bullet> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;

			if(!pObject.UpdateAndRender(fTimeDelta))
			{
				_ClearList.Add(pObject);
			}
		}

		for(int i = 0; i < _ClearList.Count; ++i)
		{
			pObject = _ClearList[i];
			_Pool.AddLast(pObject);
			Sprite.Kill(pObject._Sprite);
			pObject._Sprite = null;
			_Active.Remove(pObject);
		}

		_ClearList.Clear();
	}

	public static void ClearAll()
	{
	    Bullet pObject;

		for(LinkedListNode<Bullet> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;
		}

		_Active.Clear();
	}		

}
