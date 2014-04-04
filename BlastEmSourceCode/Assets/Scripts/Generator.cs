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

public class Generator
{

	private enum ePhase
	{
		PHASE_RELAX,
		PHASE_RISE,
		PHASE_PEAK,
		PHASE_FADE,
	};

	private ePhase _Phase;
	private float _PhaseTime;

	// This dictates how intense the spawning is
	private float _MoodWeighting;


	// Overall time
	private float _Time;
	private int _Seconds;
	private int _PowerUpSeconds;

	// Sentinels
	private bool _Sentinels;
	private float _SentinelDelay;
	private float _SentinelDelayReset;
	private bool _SentinelBiDirectional;
	private float _SentinelProbabilityBias;
	private int _SentinelTroopSizeMin;
	private int _SentinelTroopSizeRand;
	private float _SentinelSpeed;
	private float _SentinelDistance;
	private int _SentinelPathStart;
	private int _SentinelPathEnd;
	private int _SentinelRevPathStart;
	private int _SentinelRevPathEnd;

	// Rocks
	private bool _Rocks;
	private float _RocksDelay;
	private float _RocksDelayReset;
	private float _RocksSpeed;
	private int _RocksPathStart;
	private int _RocksPathEnd;

	// flyers
	private bool _Flyers;
	private float _FlyersDelay;
	private float _FlyersSpeed;
	private float _FlyersDelayReset;
	private int _FlyersPathStart;
	private int _FLyersPathEnd;

	// Skulls
	private bool _Skulls;
	private float _SkullsDelay;
	private float _SkullsDelayReset;
	private float _SkullsSpeed;

	// Turrets
	private bool _Turrets;
	private float _TurretsDelay;
	private float _TurretsDelayReset;
	private float _TurretsSpeed;

	private float _Adjust;

	public Generator()
	{

	}

	public void Reset()
	{
		_Time = 0.0f;
		_Seconds = 0;

		/// Reset the channels
		_Sentinels = false;
		_SentinelDelay = 0.0f;
		_SentinelDelayReset = 0.4f;

		_Rocks = false;
		_RocksDelay = 0.0f;
		_RocksDelayReset = 0.5f;
		_PowerUpSeconds = 10;

		_Flyers = false;
		_FlyersDelay = 0.0f;
		_FlyersDelayReset = 0.3f;
		_FlyersSpeed = 0.2f;
		_FlyersPathStart = 106;
		_FLyersPathEnd = 107;

		_Skulls = false;
		_SkullsDelay = 0.0f;
		_SkullsDelayReset = 0.5f;
		_SkullsSpeed = 10.0f;

		_Turrets = false;
		_TurretsDelay = 0.0f;
		_TurretsDelayReset = 0.5f;
		_TurretsSpeed = 10.0f;
	}
	
	public void Update(float fTimeDelta)
	{
		_Time += fTimeDelta;

		if(_Time > 1.0f)
		{
			++_Seconds;
			++_PowerUpSeconds;
			_Time -= 1.0f;

			SetParameters();
		}

		_PhaseTime -= fTimeDelta;

		if(_PhaseTime <= 0.0f)
		{
			TransitionPhase();
		}

		DoSentinels(fTimeDelta);
		DoRocks(fTimeDelta);
		DoFlyers(fTimeDelta);
		DoSkulls(fTimeDelta);
		DoTurrets(fTimeDelta);
		DoPowerups();


	}

	private void TransitionPhase()
	{
		switch(_Phase)
		{
			case ePhase.PHASE_RELAX:
				DoRelax();
				break;

			case ePhase.PHASE_RISE:
				DoRise();
				break;

			case ePhase.PHASE_PEAK:
				DoPeak();
				break;

			case ePhase.PHASE_FADE:
				DoFade();
				break;
		}
	}

	private void DoRelax()
	{

	}

	private void DoRise()
	{

	}

	private void DoPeak()
	{

	}

	private void DoFade()
	{

	}

	private void SetParameters()
	{
		switch(_Seconds)
		{
			case 1:
				_Sentinels = true;
				_SentinelDelay = 0.0f;
				_SentinelDelayReset = 0.8f;
				_SentinelBiDirectional = false;
				_SentinelProbabilityBias = 100.0f;
				_SentinelTroopSizeMin = 1;
				_SentinelTroopSizeRand = 0;
				_SentinelSpeed = 0.2f;
				_SentinelDistance = 0.1f;
				_SentinelPathStart = 0;
				_SentinelPathEnd = 27;
				//_Skulls = true;
				//_SkullsDelayReset = 0.4f;
				//_Turrets = true;
				//_TurretsDelayReset = 2.0f;
				break;

			case 10:
				_SentinelDelayReset = 0.5f;	
				break;

			case 20:
				_SentinelDelayReset = 0.3f;	
				break;


			case 30:
				_SentinelDelayReset = 0.2f;		
				_SentinelPathStart = 0;
				_SentinelPathEnd = 55;
				break;

			case 40:
				_Rocks = true;
				_RocksDelayReset = 0.8f;
				_RocksPathStart = 0;
				_RocksPathEnd = 27;
				_RocksSpeed = 0.2f;
				_SentinelDelayReset = 0.4f;
				_SentinelBiDirectional = false;
				_SentinelProbabilityBias = 90.0f;
				_SentinelRevPathStart = 56;
				_SentinelRevPathEnd = 105;
				break;

			case 50:
				_SentinelDelayReset = 2.0f;
				_Flyers = true;
				_FlyersDelayReset = 0.5f;
				_FlyersSpeed = 0.1f;
				_RocksDelayReset = 1.0f;
				break;

			case 70:
				_Sentinels = false;
				_Rocks = false;
				break;

			case 90:
				_Flyers = false;
				_Sentinels = true;
				_SentinelBiDirectional = false;
				_SentinelDelayReset = 0.2f;
				break;


			case 95:
				_Rocks = true;
				_RocksDelayReset = 0.5f;
				_RocksPathStart = 0;
				_RocksPathEnd = 27;
				_RocksSpeed = 0.2f;
				break;


			case 100:
				_Skulls = true;
				_Sentinels = false;
				break;

			case 120:
				_Sentinels = true;
				break;

			case 140:
				_Skulls = false;
				_Flyers = true;
				_FlyersDelayReset = 0.5f;
				_FlyersSpeed = 0.1f;				
				break;

			case 160:
				_Skulls = true;
				break;
		}
	}

	private void DoSentinels(float fTimeDelta)
	{
		if(!_Sentinels)
		{
			return;
		}

		_SentinelDelay -= fTimeDelta;

		if(_SentinelDelay > 0.0f)
		{
			return;
		}

		_SentinelDelay = _SentinelDelayReset;
		float fStartDistance = 0.0f;
		int iPath = 0;
		int iCount = _SentinelTroopSizeMin + Random.Range(0,_SentinelTroopSizeRand);

		if(!_SentinelBiDirectional)
		{
			iPath = Random.Range(_SentinelPathStart,_SentinelPathEnd);

			for(int i = 0; i < iCount; ++i)
			{
				AI.SpawnOnPath(AI.eAIType.AITYPE_ENEMY1,iPath,_SentinelSpeed,fStartDistance);
				fStartDistance += _SentinelDistance;
			}
		}
		else
		{
			float fPerc = Random.Range(0.0f,100.0f);

			iPath = Random.Range(_SentinelRevPathStart,_SentinelRevPathEnd);

			if(fPerc > _SentinelProbabilityBias)
			{
				AI.SpawnOnPath(AI.eAIType.AITYPE_ENEMY1,iPath,_SentinelSpeed,fStartDistance);
				fStartDistance += _SentinelDistance;
			}
			else
			{
				iPath = Random.Range(_SentinelPathStart,_SentinelPathEnd);

				for(int i = 0; i < iCount; ++i)
				{
					AI.SpawnOnPath(AI.eAIType.AITYPE_ENEMY1,iPath,_SentinelSpeed,fStartDistance);
					fStartDistance += _SentinelDistance;
				}
			}
		}
	}

	private void DoRocks(float fTimeDelta)
	{
		if(!_Rocks)
		{
			return;
		}

		_RocksDelay -= fTimeDelta;

		if(_RocksDelay > 0.0f)
		{
			return;
		}

		_RocksDelay = _RocksDelayReset;
		AI.SpawnOnPath(AI.eAIType.AITYPE_ROCK,Random.Range(_RocksPathStart,_RocksPathEnd),_RocksSpeed,0.0f);
	}

	private void DoPowerups()
	{
		if(_PowerUpSeconds < 10)
		{
			return;
		}

		_PowerUpSeconds = 0;

		// Spawn powerups
		float fX = 28.0f;
		float fZ = Random.Range(-10.0f,10.0f);
		AI.SpawnPowerup(fX,fZ,0);

		fX = 28.0f + Random.Range(0.0f,4.0f);
		fZ = Random.Range(-10.0f,10.0f);
		AI.SpawnPowerup(fX,fZ,1);

		fX = 28.0f + Random.Range(0.0f,4.0f);
		fZ = Random.Range(-10.0f,10.0f);
		AI.SpawnPowerup(fX,fZ,2);

		//fX = 28.0f + Random.Range(0.0f,4.0f);
		//fZ = Random.Range(-10.0f,10.0f);
		//AI.SpawnPowerup(fX,fZ,3);


	}

	private void DoSkulls(float fTimeDelta)
	{
		if(!_Skulls)
		{
			return;
		}

		_SkullsDelay -= fTimeDelta;
		
		if(_SkullsDelay > 0.0f)
		{
			return;
		}

		_SkullsDelay = _SkullsDelayReset;

		float fX = 28.0f;
		float fZ = Random.Range(-10.0f,10.0f);
		AI.Spawn(AI.eAIType.AITYPE_SKULL,fX,fZ);

	}

	private void DoTurrets(float fTimeDelta)
	{
		if(!_Turrets)
		{
			return;
		}

		_TurretsDelay -= fTimeDelta;
		
		if(_TurretsDelay > 0.0f)
		{
			return;
		}

		_TurretsDelay = _TurretsDelayReset;


		int iPerc = Random.Range(0,100);

		if(iPerc > 50)
		{
			AI.Spawn(AI.eAIType.AITYPE_UP_TURRET,28.0f,-10.5f);
		}
		else
		{
			AI.Spawn(AI.eAIType.AITYPE_DOWN_TURRET,28.0f,10.5f);
		}

	}	


	private void DoFlyers(float fTimeDelta)
	{
		if(!_Flyers)
		{
			return;
		}

		_FlyersDelay -= fTimeDelta;

		if(_FlyersDelay > 0.0f)
		{
			return;
		}

		_FlyersDelay = _FlyersDelayReset;

		float fPerc = Random.Range(0.0f,100.0f);

		if(fPerc > 50.0f)
		{
			AI.SpawnOnPath(AI.eAIType.AITYPE_FLYER,107,_FlyersSpeed,0.0f);
		}
		else
		{
			AI.SpawnOnPath(AI.eAIType.AITYPE_FLYER,108,_FlyersSpeed,0.0f);
		}
		


	}
}