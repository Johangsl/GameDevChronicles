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

public class Pattern 
{
	public float _Time;

	public enum ePatternReturn
	{
		PATTERN_COMPLETE,
		PATTERN_STILL_RUNNING,
	};

	public Pattern()
	{
		_Time = 0.0f;
		Setup();
	}

	virtual public void Setup()
	{

	}

	public ePatternReturn UpdatePattern(float fTimedelta)
	{
		if(Director._Instance._SequencerSemaphore == 0)
		{
			_Time += fTimedelta;
		}

		return Update(fTimedelta);
	}

	virtual public ePatternReturn Update(float fTimedelta)
	{

		return ePatternReturn.PATTERN_COMPLETE;
	}

}
