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

public class Quantum 
{
	public List<QuantumData> _Data;

	private int _ClosestIndex;
	private bool _MouseDown;
	private float _MouseX;
	private float _MouseZ;
	private float _OffsetX;
	private float _OffsetZ;

	public Quantum()
	{
		_Data = new List<QuantumData>();
		_ClosestIndex = 99999999;
		_MouseDown = false;
	}

	public void AddData(QuantumData pData)
	{
		_Data.Add(pData);
	}

	public void Execute(Level pLevel)
	{
		for(int i = 0; i < _Data.Count; ++i)
		{
			if(_Data[i]._HasPath)
			{
				pLevel._Walkers.AddLast(_Data[i]._Walker);
				_Data[i]._Walker.Execute();
			}
			else
			{
				_Data[i].Execute();
			}
		}
	}

	public void DebugDraw()
	{
		for(int i = 0; i < _Data.Count; ++i)
		{
			//Debug.Log("Drawing Quantum");
			_Data[i]._Walker.DebugDrawSpline();
			_Data[i].DebugDraw();
		}
	}

	public void MouseDown()
	{
		if(_MouseDown == false)
		{
			if(_ClosestIndex < 99999999)
			{
				if(_MouseX > _Data[_ClosestIndex]._OffsetX)
				{
					_OffsetX = -(_Data[_ClosestIndex]._OffsetX - _MouseX);
				}
				else
				{
					_OffsetX = _MouseX - _Data[_ClosestIndex]._OffsetX;
				}

				if(_MouseZ > _Data[_ClosestIndex]._OffsetZ)
				{
					_OffsetZ = -(_Data[_ClosestIndex]._OffsetZ - _MouseZ);
				}
				else
				{
					_OffsetZ = _MouseZ - _Data[_ClosestIndex]._OffsetZ;
				}
			}
		}
		_MouseDown = true;
	}

	public void MouseUp()
	{
		_MouseDown = false;
	}

	public void Mouse(float fX, float fZ)
	{
		_MouseX = fX;
		_MouseZ = fZ;

		Vector3 vMouse = new Vector3(fX,0.0f,fZ);
		Vector3 vPoint = Vector3.zero;

		_ClosestIndex = 99999999;

		float fDist = 99999999.0f;
		int iIndex;

		for(int i = 0; i < _Data.Count; ++i)
		{
			vPoint.x = _Data[i]._OffsetX;
			vPoint.z = _Data[i]._OffsetZ;
			_Data[i].DeSelect();

			float fNewDist = Vector3.Distance(vPoint,vMouse);

			//Debug.Log("Dist = " + fNewDist);

			if(fNewDist < fDist)
			{
				if(fNewDist < 2.0f)
				{
					_ClosestIndex = i;
					fDist = fNewDist;
				}
			}
		}


		if(_ClosestIndex < 99999999)
		{
			_Data[_ClosestIndex].Select();
		}

		if(_MouseDown)
		{
			if(_ClosestIndex < 99999999)
			{
				_Data[_ClosestIndex]._OffsetX = _MouseX + _OffsetX;
				_Data[_ClosestIndex]._OffsetZ = _MouseZ + _OffsetZ;
				_Data[_ClosestIndex].Changed();
				_Data[_ClosestIndex].BuildHandle();
			}
		}
	}

	public void AddQuantumDataWithPath()
	{
		// Put down a node where the mouse currently is
		QuantumData pData = new QuantumData(0,0.0f,0.0f,0,1.0f);
		AddData(pData);
	}

	public void NextPath()
	{
		if(_ClosestIndex < 99999999)	
		{
			_Data[_ClosestIndex]._Walker.NextPath();
		}
	}

	public void PreviousPath()
	{
		if(_ClosestIndex < 99999999)	
		{
			_Data[_ClosestIndex]._Walker.PreviousPath();
		}
	}

	public void DecreaseSpeed()
	{
		if(_ClosestIndex < 99999999)	
		{
			_Data[_ClosestIndex]._Walker.DecreaseSpeed();
		}
	}

	public void IncreaseSpeed()
	{
		if(_ClosestIndex < 99999999)	
		{
			_Data[_ClosestIndex]._Walker.IncreaseSpeed();
		}
	}	

	public void Delete()
	{
		if(_ClosestIndex < 99999999)	
		{
			_Data[_ClosestIndex]._Walker.EndDebug();
			_Data.RemoveAt(_ClosestIndex);
		}
	}	

	public void EndDebug()
	{
		for(int i = 0; i < _Data.Count; ++i)
		{
			_Data[i]._Walker.EndDebug();
		}
	}	
}
