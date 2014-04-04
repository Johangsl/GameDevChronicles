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


public class Particle
{
	static private LinkedList<Particle>	_Pool = null;
	static private LinkedList<Particle>	_Active = null;
	static private List<Particle> _ClearList;

	public float _X;
	public float _Y;
	public float _Z;
	public float _Scale;
	public float _VX;
	public float _VZ;
	public float _Speed;
	public float _ScaleSpeed;
	public float _Red;
	public float _Green;
	public float _Blue;
	public float _Alpha;

	private static QuadRender _QR;

	public Particle()
	{

	}

	public bool UpdateAndRender(float fTimeDelta)
	{
		_X += _VX * _Speed * fTimeDelta;
		_Z += _VZ * _Speed * fTimeDelta;
		_Scale -= _ScaleSpeed * fTimeDelta;

		if(_Scale <= 0.0f)
		{
			_Scale = 0.0f;
			return false;
		}

		_QR.AddQuad(_X,_Y,_Z,_Scale,_Scale,_Red,_Green,_Blue,_Alpha,0.0f,0.0f,0.0f,0.0f,0.0f);
		return true;
	}

	public static void AddExplosion(float fX, float fZ, float iCount, int iType = 0)
	{
		for(int i = 0; i < iCount; ++i)
		{
			float fVX = Mathf.Sin(Random.Range(0.0f,360.0f));
			float fVZ = Mathf.Cos(Random.Range(0.0f,360.0f));
			float fDist = Mathf.Sqrt(fVX * fVX + fVZ * fVZ);
			fVX /= fDist;
			fVZ /= fDist;

			if(iType == 0)
			{
				SpawnRed(fX,fZ,Random.Range(0.3f,0.5f),fVX,fVZ,Random.Range(2.0f,10.0f),Random.Range(0.5f,1.0f));
			}
			else if(iType == 1)
			{
				SpawnBlue(fX,fZ,Random.Range(0.3f,0.5f),fVX,fVZ,Random.Range(2.0f,10.0f),Random.Range(0.5f,1.0f));
			}
		}
	}

	public static void AddFirework(float fX, float fZ, float iCount)
	{
		for(int i = 0; i < iCount; ++i)
		{
			float fVX = Mathf.Sin(Random.Range(0.0f,360.0f));
			float fVZ = Mathf.Cos(Random.Range(0.0f,360.0f));
			float fDist = Mathf.Sqrt(fVX * fVX + fVZ * fVZ);
			fVX /= fDist;
			fVZ /= fDist;

			SpawnFirework(fX,fZ,Random.Range(0.3f,0.5f),fVX,fVZ,Random.Range(2.0f,10.0f),Random.Range(0.5f,1.0f));
		}
	}

	public static void SpawnRed(float fX, float fZ, float fScale, float fVX, float fVZ, float fSpeed, float fScaleSpeed)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return;
		}

		Particle pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			//Debug.Log("Particle spawn");

			pObject._X = fX;
			pObject._Y = 0.1f;
			pObject._Z = fZ;
			pObject._Scale = fScale;
			pObject._VX = fVX;
			pObject._VZ = fVZ;
			pObject._Speed = fSpeed;
			pObject._ScaleSpeed = fScaleSpeed;
			pObject._Red = Random.Range(0.1f,1.0f);
			pObject._Green = 0.0f;
			pObject._Blue = 0.0f;
		}
	}


	public static void SpawnGold(float fX, float fZ, float fScale, float fVX, float fVZ, float fSpeed, float fScaleSpeed)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return;
		}

		Particle pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			//Debug.Log("Particle spawn");

			pObject._X = fX;
			pObject._Y = 0.1f;
			pObject._Z = fZ;
			pObject._Scale = fScale;
			pObject._VX = fVX;
			pObject._VZ = fVZ;
			pObject._Speed = fSpeed;
			pObject._ScaleSpeed = fScaleSpeed;
			pObject._Red = Random.Range(0.1f,1.0f);
			pObject._Green = pObject._Red;
			pObject._Blue = 0.0f;
		}
	}	

	public static void SpawnFirework(float fX, float fZ, float fScale, float fVX, float fVZ, float fSpeed, float fScaleSpeed)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return;
		}

		Particle pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			//Debug.Log("Particle spawn");

			pObject._X = fX;
			pObject._Y = 0.1f;
			pObject._Z = fZ;
			pObject._Scale = fScale;
			pObject._VX = fVX;
			pObject._VZ = fVZ;
			pObject._Speed = fSpeed;
			pObject._ScaleSpeed = fScaleSpeed;
			pObject._Red = Random.Range(0.0f,1.0f);
			pObject._Green = Random.Range(0.0f,1.0f);;
			pObject._Blue = Random.Range(0.0f,1.0f);;
		}
	}	

	public static void SpawnBlue(float fX, float fZ, float fScale, float fVX, float fVZ, float fSpeed, float fScaleSpeed)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled bullets");
			return;
		}

		Particle pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			//Debug.Log("Particle spawn");

			pObject._X = fX;
			pObject._Y = 0.1f;
			pObject._Z = fZ;
			pObject._Scale = fScale;
			pObject._VX = fVX;
			pObject._VZ = fVZ;
			pObject._Speed = fSpeed;
			pObject._ScaleSpeed = fScaleSpeed;
			pObject._Red = 0.0f;
			pObject._Green = 0.0f;
			pObject._Blue = Random.Range(0.1f,1.0f);
		}
	}	

	public static void Initialise(int iCount)
	{
		_QR = GameObject.Find("Explosions").GetComponent<Explosions>()._QR;

		Debug.Log("Particle initialise QR = " + _QR);

		if(_Pool == null)
		{
			_Pool = new LinkedList<Particle>();
			_Active = new LinkedList<Particle>();
			_ClearList = new List<Particle>();
		}

		for(int i = 0; i < iCount; ++i)
		{
			Particle pObject = new Particle();
			_Pool.AddLast(pObject);
		}

	}

	public static void UpdateAndRenderAll(float fTimeDelta)
	{
    	Particle pObject;

		for(LinkedListNode<Particle> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;

			if(!pObject.UpdateAndRender(fTimeDelta))
			{
				_ClearList.Add(pObject);
			}
		}

		for(int i = 0; i < _ClearList.Count; ++i)
		{
			_Active.Remove(_ClearList[i]);
			_Pool.AddLast(_ClearList[i]);
		}

		_ClearList.Clear();
	}

	public static void ClearAll()
	{
	    Particle pObject;

		for(LinkedListNode<Particle> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;
		}

		_Active.Clear();
	}		

}
