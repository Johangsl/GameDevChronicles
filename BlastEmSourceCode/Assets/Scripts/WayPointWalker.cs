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

public class WayPointWalker 
{

	public float _Speed;
	public WayPoints _WayPoints;
	public float _T;
	public bool _Dead;
	public int _PathIndex;

	public float _OffsetX;
	public float _OffsetZ;

	private Sprite _Sprite;
	private float _DebugT;

	public WayPointWalker()
	{
		_PathIndex = 999999;
		_WayPoints = null;
		_Dead = false;
		_Sprite = null;
		_DebugT = 0.0f;
		_T = 0.0f;
	}

	public WayPointWalker(float fSpeed, int iIndex)
	{
		_PathIndex = iIndex;
		_WayPoints = Director._Instance._WayPointDatabase.GetWayPoints(_PathIndex);
		_T = 0.0f;
		_Speed = fSpeed;
		_Dead = false;

		_Sprite = null;
		_DebugT = 0.0f;
	}

	public void Set(int iIndex, float fSpeed)
	{
		_PathIndex = iIndex;
		_WayPoints = Director._Instance._WayPointDatabase.GetWayPoints(_PathIndex);
		_T = 0.0f;
		_Speed = fSpeed;
		_Dead = false;

		_Sprite = null;
		_DebugT = 0.0f;		
	}

	public Vector3 Walk(float fTimeDelta)
	{
		if(_WayPoints == null)
		{
			return Vector3.zero;
		}

		_T += _Speed * fTimeDelta;

		if(_T >= 1.0f)
		{
			_T = 1.0f;
			_Dead = true;
		}

		return _WayPoints.Spline(_T);
	}

	public bool IsAtEnd()
	{
		if(_T >= 1.0f || _Dead)
		{
			return true;
		}

		return false;
	}

	// create the AI
	public void Execute()
	{
		_Dead = false;
		_T = 0.0f;
	}

	public bool IsDead()
	{
		return _Dead;
	}

	public void DebugDraw()
	{
		_WayPoints.DebugDraw();
	}

	public void DebugDrawSpline()
	{
		_WayPoints.DebugDrawSpline(_OffsetX,_OffsetZ);

		if(_Sprite == null)
		{
			_Sprite = Sprite.Spawn(1);
			_Sprite.AddFrame("Enemy1F1.png");
			_Sprite.AddFrame("Enemy1F2.png");
			_Sprite.AddFrame("Enemy1F3.png");
			_Sprite.AddFrame("Enemy1F4.png");
			_Sprite.AddFrame("Enemy1F3.png");
			_Sprite.AddFrame("Enemy1F2.png");
			_Sprite._Width = 1.2f;
			_Sprite._Height = 1.2f;
			_Sprite._Animate = true;
			_Sprite._Y = 0.1f;
		}

		_DebugT += _Speed * Time.deltaTime;

		if(_DebugT >= 1.0f)
		{
			_DebugT = 0.0f;

		}

		Vector3 vPos = _WayPoints.Spline(_DebugT);
		_Sprite._X = vPos.x;
		_Sprite._Z = vPos.z;	

		// Draw a circle to represent the speed
		Vector3 vA = Vector3.zero;
		Vector3 vB = Vector3.zero;
		Vector3 vColour = new Vector3(1.0f,1.0f,0.0f);

		float fAngle = 0.0f;
		float fStep = 360.0f / 30.0f;

		vA.x = (Mathf.Sin(fAngle * Mathf.Deg2Rad) * (_Speed * 2.0f)) + _OffsetX;
		vA.z = (Mathf.Cos(fAngle * Mathf.Deg2Rad) * (_Speed * 2.0f)) + _OffsetZ;
		fAngle += fStep;

		for(int i = 0; i < 30; ++i)
		{
			vB.x = (Mathf.Sin(fAngle * Mathf.Deg2Rad) * (_Speed * 2.0f)) + _OffsetX;
			vB.z = (Mathf.Cos(fAngle * Mathf.Deg2Rad) * (_Speed * 2.0f)) + _OffsetZ;

			Director._Instance.AddLine(vColour,vA,vB);
			fAngle += fStep;
			vA = vB;
		}
	}

	public void EndDebug()
	{
		if(_Sprite != null)
		{
			Sprite.Kill(_Sprite);
			_Sprite = null;
		}
	}

	public void NextPath()
	{
		WayPoints pWP = Director._Instance._WayPointDatabase.GetWayPoints(_PathIndex+1);

		if(pWP != null)
		{
			++_PathIndex;
			_WayPoints = pWP;
		}
	}

	public void PreviousPath()
	{
		if(_PathIndex > 0)
		{
			--_PathIndex;
			_WayPoints = Director._Instance._WayPointDatabase.GetWayPoints(_PathIndex);
		}
	}

	public void DecreaseSpeed()
	{

		_Speed -= 0.2f * Time.deltaTime;

		if(_Speed < 0.0f)
		{
			_Speed = 0.0f;
		}

	}

	public void IncreaseSpeed()
	{
		_Speed += 0.2f * Time.deltaTime;
	}	

}
