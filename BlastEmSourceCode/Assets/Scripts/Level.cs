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

public class Level
{
	public LinkedList<WayPointWalker> _Walkers;
	public List<WayPointWalker>	_DeadWalkers;

	public List<Quantum> _Data;
	public int _DataIndex;

	public Level()
	{
		_Walkers = new LinkedList<WayPointWalker>();
		_DeadWalkers = new List<WayPointWalker>();
		_DataIndex = 0;
		_Data = new List<Quantum>();

		// Add empty quantum
		AddQuantum(new Quantum());
	}

	public void Update(float fTimeDelta)
	{
		for(LinkedListNode<WayPointWalker> it = _Walkers.First; it != null; it = it.Next)
		{
			WayPointWalker pWalker = it.Value;

			pWalker.Walk(fTimeDelta);

			if(pWalker.IsDead())
			{
				_DeadWalkers.Add(pWalker);
			}
		}		

		// Clear dead walkers
		for(int i = 0; i < _DeadWalkers.Count; ++i)
		{
			_Walkers.Remove(_DeadWalkers[i]);
		}

		_DeadWalkers.Clear();
	}

	public bool FixedUpdate()
	{
		if(_DataIndex < _Data.Count-1)
		{
			if(_Data[_DataIndex] != null)
			{
				_Data[_DataIndex].Execute(this);
			}

			++_DataIndex;

			return true;
		}
		else
		{
			return false;
		}
	}

	public void AddQuantum(Quantum pQuantum)
	{
		_Data.Add(pQuantum);
	}

	public void DebugDraw()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				_Data[_DataIndex].DebugDraw();
			}
		}
	}

	public bool IsFinished()
	{
		if((_DataIndex >= _Data.Count-1) && _Walkers.Count == 0)
		{
			return true;
		}

		return false;
	}

	public void Mouse(float fX, float fZ)
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				_Data[_DataIndex].Mouse(fX,fZ);
			}
		}

	}

	public void MouseDown()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				_Data[_DataIndex].MouseDown();
			}
		}	
	}

	public void MouseUp()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				_Data[_DataIndex].MouseUp();
			}
		}	
	}

	public void AddQuantumDataWithPath()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: Adding quantum with path");

				_Data[_DataIndex].AddQuantumDataWithPath();
			}
		}	
	}	

	public void NextPath()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: NextPath");
				_Data[_DataIndex].NextPath();
			}
		}	
	}	

	public void PreviousPath()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: PreviousPath");
				_Data[_DataIndex].PreviousPath();
			}
		}	
	}	

	public void DecreaseSpeed()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: DecreaseSpeed");
				_Data[_DataIndex].DecreaseSpeed();
			}
		}	
	}

	public void IncreaseSpeed()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: IncreaseSpeed");
				_Data[_DataIndex].IncreaseSpeed();
			}
		}	
	}	

	public void Delete()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: Delete");
				_Data[_DataIndex].Delete();
			}
		}	
	}	

	public void EndDebug()
	{
		if(_DataIndex < _Data.Count)
		{
			if(_Data[_DataIndex] != null)
			{
				Debug.Log("Level: EndDebug");
				_Data[_DataIndex].EndDebug();
			}
		}	
	}	
}
