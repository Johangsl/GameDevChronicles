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

public class QuantumData 
{
	public int _PathIndex;
	public float _OffsetX;
	public float _OffsetZ;
	public int _AIIndex;
	public float _Speed;
	public WayPointWalker _Walker;
	public bool _HasPath;


	private Vector3 _VA;
	private Vector3 _VB;
	private Vector3 _HA;
	private Vector3 _HB;
	private Vector3 _Selected;
	private Vector3 _Colour;
	private bool _BeenSelected;

	public QuantumData(int iPathIndex, float fPathOffsetX, float fPathOffsetZ, int iAIIndex, float fSpeed, bool bhasPath = true)
	{
		_PathIndex = iPathIndex;
		_OffsetX = fPathOffsetX;
		_OffsetZ = fPathOffsetZ;
		_Speed = fSpeed;
		_AIIndex = iAIIndex;

		if(bhasPath)
		{
			_Walker = new WayPointWalker(fSpeed, iPathIndex);
		}

		_HasPath = bhasPath;

		_Colour = new Vector3(1.0f,1.0f,1.0f);
		_Selected = new Vector3(1.0f,0.0f,1.0f);
		_BeenSelected = false;		

		BuildHandle();
	}

	public void Changed()
	{
		_Walker._OffsetX = _OffsetX;
		_Walker._OffsetZ = _OffsetZ;
	}

	public void Execute()
	{

	}

	public void Select()
	{
		_BeenSelected = true;
	}

	public void DeSelect()
	{
		_BeenSelected = false;
	}

	public void BuildHandle()
	{
		_VA = new Vector3(_OffsetX,0.0f,_OffsetZ - 0.3f);
		_VB = new Vector3(_OffsetX,0.0f,_OffsetZ + 0.3f);
		_HA = new Vector3(_OffsetX - 0.3f,0.0f,_OffsetZ);
		_HB = new Vector3(_OffsetX + 0.3f,0.0f,_OffsetZ);		
	}

	public void DebugDraw()
	{
		if(_BeenSelected)
		{
			Director._Instance.AddLine(_Selected,_VA,_VB);
			Director._Instance.AddLine(_Selected,_HA,_HB);
		}
		else
		{
			Director._Instance.AddLine(_Colour,_VA,_VB);
			Director._Instance.AddLine(_Colour,_HA,_HB);
		}
	}
}
