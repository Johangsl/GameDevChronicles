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


public class AI
{
	static private LinkedList<AI>	_Pool = null;
	static private LinkedList<AI>	_Active = null;
	static private List<AI> _ClearList;

	public enum eAIType
	{
		AITYPE_ENEMY1,
		AITYPE_BLOCK,
		AITYPE_BLOCK1,
		AITYPE_ROCK,
		AITYPE_BULLET,
		AITYPE_POWERUP,
		AITYPE_DRONE,
		AITYPE_BOSS1,
		AITYPE_GOLD_COIN,
		AITYPE_SILVER_COIN,
		AITYPE_BRONZE_COIN,
		AITYPE_SKULL,
		AITYPE_FLYER,
		AITYPE_RDEFENDER,
		AITYPE_UP_TURRET,
		AITYPE_DOWN_TURRET,
		AITYPE_FEEDBACK,
	};

	// Local data
	public eAIType _Type;
	public float _X;
	public float _Z;
	public float _Radius;
	public float _Speed;
	public float _SpeedBoost;
	public int _Value;
	public bool _AABBCollision;
	public float _Width;
	public float _Height;
	public int _HitPoints;
	public bool _FrameAdvanceOnHit;
	public float _SinRadius;
	public float _SinFrequency;
	public float _RealZ;
	public WayPointWalker _Walker;
	public bool _HasPath;
	public float _StartDelay;

	// private local data
	private Sprite _Sprite;
	private bool _Kill;
	private bool _AddedToDelegates;
	private bool _Pause;
	private float _PauseCounter;
	private int _CurrentFrame;
	private float _VX;
	private float _VZ;
	private float _TimeToFire;
	private float _CullPoint;
	private int _PowerupType;
	private int _FeedbackType;
	private float _Time;
	private int _Block;

	// AABB
	private float _X1;
	private float _Z1;
	private float _X2;
	private float _Z2;

	private float _DroneRadius;
	private float _DroneAngle;
	private float _DroneSpinSpeed;

	private float _MaxX;
	private float _MinX;

	// Boss data
	private List<AI> _Drones;

	public AI()
	{
		_Walker = new WayPointWalker();
	}

	public bool UpdateAndRender(float fTimeDelta)
	{
		_Time += fTimeDelta;
		_StartDelay -= fTimeDelta;

		if(_StartDelay <= 0.0f)
		{
			_StartDelay = 0.0f;
		}
		else
		{
			_X = 100000.0f;
			_Z = 100000.0f;
			_Sprite._X = _X;
			_Sprite._Z = _Z;
			return true;
		}

		switch(_Type)
		{
			case eAIType.AITYPE_ENEMY1:
				DoEnemy1(fTimeDelta);
				break;

			case eAIType.AITYPE_ROCK:
				DoRock1(fTimeDelta);
				break;

			case eAIType.AITYPE_BULLET:
				DoBullet(fTimeDelta);
				break;
			
			case eAIType.AITYPE_POWERUP:
				DoPowerUp(fTimeDelta);
				break;

			case eAIType.AITYPE_BOSS1:
				DoBoss1(fTimeDelta);
				break;

			case eAIType.AITYPE_DRONE:
				DoDrone(fTimeDelta);
				break;

			case eAIType.AITYPE_GOLD_COIN:
				DoGoldCoin(fTimeDelta);
				break;

			case eAIType.AITYPE_SILVER_COIN:
				DoSilverCoin(fTimeDelta);
				break;

			case eAIType.AITYPE_BRONZE_COIN:
				DoBronzeCoin(fTimeDelta);
				break;

			case eAIType.AITYPE_SKULL:
				DoSkull(fTimeDelta);
				break;

			case eAIType.AITYPE_FLYER:
				DoFlyer(fTimeDelta);
				break;

			case eAIType.AITYPE_RDEFENDER:
				DoRockDefender(fTimeDelta);
				break;

			case eAIType.AITYPE_BLOCK:
				DoBlock(fTimeDelta);
				break;

			case eAIType.AITYPE_UP_TURRET:
			case eAIType.AITYPE_DOWN_TURRET:
				DoTurret(fTimeDelta);
				break;

			case eAIType.AITYPE_FEEDBACK:
				DoFeedback(fTimeDelta);
				break;
		}

		_Sprite._X = _X;
		_Sprite._Z = _Z;

		if(_AABBCollision)
		{
			BuildAABB();	
		}


		if(!_HasPath)
		{
			if(_X < -23.0f)
			{
				_Kill = true;
				return false;
			}
		}

		if(_Kill)
		{
			return false;
		}

		if(_Pause)
		{
			_PauseCounter -= fTimeDelta;

			if(_PauseCounter <= 0.0f)
			{
				Particle.AddExplosion(_X,_Z,20);

				//if(_Type != eAIType.AITYPE_BLOCK1)
				//{
			//		Director._Instance.PlaySample("EXPLOSION",0.5f);
			//	}
				return false;
			}
		}

		return true;
	}

	private void BuildAABB()
	{
		_X1 = _X - _Width;
		_X2 = _X + _Width;
		_Z1 = _Z - _Height;
		_Z2 = _Z + _Height;
	}

	public void DoFeedback(float fTimeDelta)
	{
		if(!_Pause)
		{
			_Z += _Speed * fTimeDelta;
			_Sprite._Width -= 1.0f * fTimeDelta;
			_Sprite._Height -= 1.0f * fTimeDelta;

			if(_Sprite._Width <= 0.0f)
			{
				_Kill = true;
			}

			if(_Z > 12.0f)
			{
				_Kill = true;
			}
		}
	}

	public void DoBlock(float fTimeDelta)
	{
		if(!_Pause)
		{
			_X -= _Speed * fTimeDelta;
		}
	}

	public void DoTurret(float fTimeDelta)
	{
		if(!_Pause)
		{
			_X -= _Speed * fTimeDelta;


			_TimeToFire -= fTimeDelta;

			if(_TimeToFire <= 0.0f)
			{
				_TimeToFire = Random.Range(0.5f,1.0f);
				FireAtPlayer();
			}			
		}
	}


	public void DoEnemy1(float fTimeDelta)
	{
		if(!_Pause)
		{
			if(_HasPath)
			{
				Vector3 vPos = _Walker.Walk(fTimeDelta);

				_X = vPos.x;
				_Z = vPos.z;

				if(_Walker.IsAtEnd())
				{
					_Kill = true;	
				}
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}

			if(_X > _MinX)
			{

				_TimeToFire -= fTimeDelta;

				if(_TimeToFire <= 0.0f)
				{
					_TimeToFire = Random.Range(2.0f,3.0f);

					int iPerc = Random.Range(0,100);

					if(iPerc > 60)
					{
						FireAtPlayer();
					}
				}
			}

		}
	}

	public void DoRockDefender(float fTimeDelta)
	{
		if(!_Pause)
		{
			_X -= _Speed * fTimeDelta;

			if(_X > _MinX)
			{

				_TimeToFire -= fTimeDelta;

				if(_TimeToFire <= 0.0f)
				{
					_TimeToFire = Random.Range(0.5f,1.5f);

					int iPerc = Random.Range(0,100);

					if(iPerc > 60)
					{
						FireAtPlayer();
					}
				}
			}
		}
	}	

	public void DoRock1(float fTimeDelta)
	{
		if(!_Pause)
		{
			if(_HasPath)
			{
				Vector3 vPos = _Walker.Walk(fTimeDelta);

				_X = vPos.x;
				_Z = vPos.z;

				if(_Walker.IsAtEnd())
				{
					_Kill = true;	
				}
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}		
		}
	}

	public void DoBullet(float fTimeDelta)
	{	
		if(!_Pause)
		{
			//_X -= _Speed * fTimeDelta;
			_X += _VX * _Speed * fTimeDelta;
			_Z += _VZ * _Speed * fTimeDelta;

			if(_X <= -23.0f || _X >= 23.0f)
			{
				if(_Z <= -12.0f || _Z >= 12.0f)
				{
					_Kill = true;
				}
			}

		}

		//Particle.SpawnRed(_X,_Z,0.3f,0.0f,0.0f,1.0f,0.1f);

		//_Sprite._Scale = Random.Range(0.0f,1.5f);

	}

	public void DoPowerUp(float fTimeDelta)
	{
		if(!_Pause)
		{
			_X -= _Speed * fTimeDelta;

			int iScore = Director._Instance._Score;

			// Check the player has enough points - self destruct if not
			if(iScore < Director._Instance._TripleFireCost && _PowerupType == 1)
			{
				_Kill = true;
				Particle.AddExplosion(_X,_Z,20);
				return;
			}

			if(iScore < Director._Instance._SpeedUpCost && _PowerupType == 0)
			{
				_Kill = true;
				Particle.AddExplosion(_X,_Z,20);
				return;
			}


		}
	}

	public void DoBoss1(float fTimeDelta)
	{
		AI pAI;

		for(int i = 0; i < _Drones.Count; ++i)
		{
			pAI = _Drones[i];
			pAI.UpdateDronePosition(_X,_Z);
		}

	}

	public void DoDrone(float fTimeDelta)
	{
		_DroneAngle += _DroneSpinSpeed * fTimeDelta;

		if(_DroneAngle > 6.28f)
		{
			_DroneAngle -= 6.28f;
		}
	}

	private void DoGoldCoin(float fTimeDelta)
	{
		if(!_Pause)
		{
			if(Director._Instance._MagnetCurrentTime > 0.0f)
			{
				float fX = Director._Instance._Player._Sprite._X - _X;
				float fZ = Director._Instance._Player._Sprite._Z - _Z;
				float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

				fX /= fDist;
				fZ /= fDist;

				_X += fX * (_Speed * 2.0f) * fTimeDelta;
				_Z += fZ * (_Speed * 2.0f) * fTimeDelta;
				
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}
		}
	}

	private void DoSilverCoin(float fTimeDelta)
	{
		if(!_Pause)
		{
			if(Director._Instance._MagnetCurrentTime > 0.0f)
			{
				float fX = Director._Instance._Player._Sprite._X - _X;
				float fZ = Director._Instance._Player._Sprite._Z - _Z;
				float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

				fX /= fDist;
				fZ /= fDist;

				_X += fX * (_Speed * 2.0f) * fTimeDelta;
				_Z += fZ * (_Speed * 2.0f) * fTimeDelta;
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}
		}
	}

	private void DoBronzeCoin(float fTimeDelta)
	{
		if(!_Pause)
		{
			if(Director._Instance._MagnetCurrentTime > 0.0f)
			{
				float fX = Director._Instance._Player._Sprite._X - _X;
				float fZ = Director._Instance._Player._Sprite._Z - _Z;
				float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

				fX /= fDist;
				fZ /= fDist;

				_X += fX * (_Speed * 2.0f) * fTimeDelta;
				_Z += fZ * (_Speed * 2.0f) * fTimeDelta;
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}
		}
	}

	private void DoSkull(float fTimeDelta)
	{
		if(!_Pause)
		{
			float fX = Director._Instance._Player._Sprite._X - _X;
			float fZ = Director._Instance._Player._Sprite._Z - _Z;
			float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

			if(fDist <= Director._Instance._MinMineDistance)
			{
				fX /= fDist;
				fZ /= fDist;

				_X += fX * (_Speed + _SpeedBoost) * fTimeDelta;
				_Z += fZ * (_Speed + _SpeedBoost) * fTimeDelta;
			}
			else
			{
				_X -= _Speed * fTimeDelta;
			}
		}
	}

	private void DoFlyer(float fTimeDelta)
	{
		if(!_Pause)
		{

			if(_HasPath)
			{
				Vector3 vPos = _Walker.Walk(fTimeDelta);

				_X = vPos.x;
				_Z = vPos.z;

				if(_Walker.IsAtEnd())
				{
					_Kill = true;	
				}
			}
			else
			{
				_X -= _Speed * fTimeDelta;
				_Z = _RealZ + Mathf.Sin(_Time * _SinFrequency) * _SinRadius;
			}

			if(_X > _MinX)
			{

				_TimeToFire -= fTimeDelta;

				if(_TimeToFire <= 0.0f)
				{
					_TimeToFire = Random.Range(1.0f,2.0f);

					int iPerc = Random.Range(0,100);

					if(iPerc > 60)
					{
						FireAtPlayer();
					}
				}
			}			
		}
	}


	private void UpdateDronePosition(float fX, float fZ)
	{
		_X = fX + (Mathf.Sin(_DroneAngle) * _DroneRadius);
		_Z = fZ + (Mathf.Cos(_DroneAngle) * _DroneRadius);
	}

	private void FireAtPlayer()
	{
		_VX = Director._Instance._Player._Sprite._X - _X;
		_VZ = Director._Instance._Player._Sprite._Z - _Z;
		float fDist = Mathf.Sqrt(_VX * _VX + _VZ * _VZ);

		if(fDist < Director._Instance._MinAIFireDistance)
		{
			return;
		}

		_VX /= fDist;
		_VZ /= fDist;
		Spawn(AI.eAIType.AITYPE_BULLET,_X,_Z,_VX,_VZ);

		Director._Instance.PlaySample("ALIEN_SHOT",0.3f);


	}

	private void Setup()
	{
		_AddedToDelegates = false;
		_Pause = false;
		_Kill = false;
		_Time = Random.Range(0.0f,100.0f);
		_TimeToFire = 1.0f;

		switch(_Type)
		{
			case eAIType.AITYPE_ENEMY1:
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
				_Radius = 0.5f;
				_Speed = 10.0f + Random.Range(0.0f,3.0f);
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();
				break;

			case eAIType.AITYPE_BLOCK:
				_Sprite = Sprite.Spawn(1);

				switch(_Block)
				{
					case 1:
						_Sprite.AddFrame("Block1.png");
						break;

					case 2:
						_Sprite.AddFrame("Block2.png");
						break;

					case 3:
						_Sprite.AddFrame("Block3.png");
						break;

					case 4:
						_Sprite.AddFrame("Block4.png");
						break;

					case 5:
						_Sprite.AddFrame("Block5.png");
						break;

					case 6:
						_Sprite.AddFrame("Block6.png");
						break;

					case 7:
						_Sprite.AddFrame("Block7.png");
						break;

					case 8:
						_Sprite.AddFrame("Block8.png");
						break;

				}
				
				_Sprite._Width = 2.0f;
				_Sprite._Height = 2.0f;
				_Sprite._Animate = false;
				_Sprite._Y = 0.0f;
				_Radius = 0.8f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();
				break;				

			case eAIType.AITYPE_RDEFENDER:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("RDefenderF1.png");
				_Sprite.AddFrame("RDefenderF2.png");
				_Sprite.AddFrame("RDefenderF3.png");
				_Sprite.AddFrame("RDefenderF4.png");
				_Sprite.AddFrame("RDefenderF5.png");
				_Sprite.AddFrame("RDefenderF6.png");
				_Sprite.AddFrame("RDefenderF7.png");
				_Sprite.AddFrame("RDefenderF8.png");
				_Sprite.AddFrame("RDefenderF9.png");
				_Sprite.AddFrame("RDefenderF10.png");
				_Sprite.AddFrame("RDefenderF11.png");
				_Sprite.AddFrame("RDefenderF12.png");
				_Sprite._Width = 2.5f;
				_Sprite._Height = 2.5f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 1.0f;
				_Speed = 10.0f + Random.Range(0.0f,5.0f);
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(0.3f,1.0f);
				AddToDelegates();
				break;

			case eAIType.AITYPE_UP_TURRET:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("TurretUpF1.png");
				_Sprite.AddFrame("TurretUpF2.png");
				_Sprite.AddFrame("TurretUpF3.png");
				_Sprite.AddFrame("TurretUpF4.png");
				_Sprite.AddFrame("TurretUpF5.png");
				_Sprite.AddFrame("TurretUpF6.png");
				_Sprite.AddFrame("TurretUpF7.png");
				_Sprite.AddFrame("TurretUpF8.png");
				_Sprite.AddFrame("TurretUpF9.png");
				_Sprite.AddFrame("TurretUpF10.png");
				_Sprite._Width = 1.5f;
				_Sprite._Height = 1.5f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Z -= 0.2f;
				_Radius = 1.0f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(0.3f,1.0f);
				AddToDelegates();
				break;

			case eAIType.AITYPE_DOWN_TURRET:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("TurretDownF1.png");
				_Sprite.AddFrame("TurretDownF2.png");
				_Sprite.AddFrame("TurretDownF3.png");
				_Sprite.AddFrame("TurretDownF4.png");
				_Sprite.AddFrame("TurretDownF5.png");
				_Sprite.AddFrame("TurretDownF6.png");
				_Sprite.AddFrame("TurretDownF7.png");
				_Sprite.AddFrame("TurretDownF8.png");
				_Sprite.AddFrame("TurretDownF9.png");
				_Sprite.AddFrame("TurretDownF10.png");
				_Sprite._Width = 1.5f;
				_Sprite._Height = 1.5f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Z += 0.2f;
				_Radius = 1.0f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(0.3f,1.0f);
				AddToDelegates();
				break;				

			case eAIType.AITYPE_SKULL:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("MineF1.png");
				_Sprite.AddFrame("MineF2.png");
				_Sprite.AddFrame("MineF3.png");
				_Sprite.AddFrame("MineF4.png");
				_Sprite.AddFrame("MineF3.png");
				_Sprite.AddFrame("MineF2.png");
				_Sprite._Width = 1.8f;
				_Sprite._Height = 1.8f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 0.7f;
				_Speed = 15.0f + Random.Range(0.0f,5.0f);
				_SpeedBoost = 4.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();
				break;		

			case eAIType.AITYPE_FLYER:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("FlyerF1.png");
				_Sprite.AddFrame("FlyerF2.png");
				_Sprite.AddFrame("FlyerF3.png");
				_Sprite.AddFrame("FlyerF4.png");
				_Sprite.AddFrame("FlyerF5.png");
				_Sprite.AddFrame("FlyerF6.png");
				_Sprite.AddFrame("FlyerF7.png");
				_Sprite.AddFrame("FlyerF8.png");
				_Sprite.AddFrame("FlyerF9.png");
				_Sprite.AddFrame("FlyerF10.png");
				_Sprite._Width = 1.5f;
				_Sprite._Height = 1.5f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 0.5f;
				_Speed = 10.0f + Random.Range(0.0f,5.0f);
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(1.0f,2.0f);
				AddToDelegates();
				_RealZ = _Z;
				_SinRadius = Random.Range(3.0f,5.0f);
				_SinFrequency = _Speed * 0.1f;
				break;


			case eAIType.AITYPE_ROCK:
				_Sprite = Sprite.Spawn(1);

				int iRock = Random.Range(0,100);

				if(iRock > 75)
				{
					_Sprite.AddFrame("Rock1F1.png");
					_Sprite.AddFrame("Rock1F2.png");
					_Sprite.AddFrame("Rock1F3.png");
					_Sprite.AddFrame("Rock1F4.png");
				}
				else if(iRock > 50)
				{
					_Sprite.AddFrame("Rock2F1.png");
					_Sprite.AddFrame("Rock2F2.png");
					_Sprite.AddFrame("Rock2F3.png");
					_Sprite.AddFrame("Rock2F4.png");
				}
				else if(iRock > 25)
				{
					_Sprite.AddFrame("Rock3F1.png");
					_Sprite.AddFrame("Rock3F2.png");
					_Sprite.AddFrame("Rock3F3.png");
					_Sprite.AddFrame("Rock3F4.png");
				}
				else
				{
					_Sprite.AddFrame("Rock4F1.png");
					_Sprite.AddFrame("Rock4F2.png");
					_Sprite.AddFrame("Rock4F3.png");
					_Sprite.AddFrame("Rock4F4.png");
				}

				_Sprite._Width = 4.1f;
				_Sprite._Height = 4.1f;
				_Sprite._Animate = false;
				_Sprite._Y = 0.0f;
				_Sprite.SetFrame(0);
				_Radius = 1.1f;
				_Speed = 10.0f + Random.Range(0.0f,5.0f);
				_Value = 1;		
				_Width = 3.0f * 0.5f;
				_Height = (3.0f * 0.5f) * Director._Instance._Aspect;
				_AABBCollision = true;
				_HitPoints = 4;
				_CurrentFrame = 0;
				_FrameAdvanceOnHit = true;
				AddToDelegates();	
				break;

			case eAIType.AITYPE_BULLET:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("EnemyBullet.png");
				_Sprite._Width = 0.8f;
				_Sprite._Height = 0.8f;
				_Sprite._Animate = false;
				_Sprite._Y = 0.15f;
				_Radius = 0.1f;
				_Speed = 8.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				AddToDelegates();
				break;

			case eAIType.AITYPE_POWERUP:
				_Sprite = Sprite.Spawn(1);

				if(_PowerupType == 0)
				{
					_Sprite.AddFrame("FasterShots.png");
				}
				else if(_PowerupType == 1)
				{
					_Sprite.AddFrame("TriShot.png");					
				}
				else if(_PowerupType == 2)
				{
					_Sprite.AddFrame("Magnet.png");					
				}
				else if(_PowerupType == 3)
				{
					_Sprite.AddFrame("Shield.png");					
				}
				
				_Sprite._Width = 1.5f;
				_Sprite._Height = 1.5f;
				_Sprite._Animate = false;
				_Sprite._Y = 0.15f;
				_Radius = 0.5f;
				_Speed = 8.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				AddToDelegates();
				break;

			case eAIType.AITYPE_DRONE:
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
				_Radius = 0.5f;
				_Speed = 8.0f + Random.Range(0.0f,3.0f);
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 300;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();			
				break;

			case eAIType.AITYPE_BOSS1:
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
				_Radius = 0.5f;
				_Speed = 8.0f + Random.Range(0.0f,3.0f);
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 6;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();
				++Director._Instance._SequencerSemaphore;

				// Create drones for the boss
				_Drones = new List<AI>();

				for(int i = 0; i < 8; ++i)
				{
					AI pAI = Spawn(eAIType.AITYPE_DRONE,0.0f,0.0f);
					_Drones.Add(pAI);
				}

				AI pDrone = _Drones[0];
				pDrone._DroneRadius = 9.0f;
				pDrone._DroneAngle = 0.0f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[1];
				pDrone._DroneRadius = 7.0f;
				pDrone._DroneAngle = 0.0f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[2];
				pDrone._DroneRadius = 5.0f;
				pDrone._DroneAngle = 0.0f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[3];
				pDrone._DroneRadius = 3.0f;
				pDrone._DroneAngle = 0.0f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[4];
				pDrone._DroneRadius = 3.0f;
				pDrone._DroneAngle = 3.14f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[5];
				pDrone._DroneRadius = 5.0f;
				pDrone._DroneAngle = 3.14f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[6];
				pDrone._DroneRadius = 7.0f;
				pDrone._DroneAngle = 3.14f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				pDrone = _Drones[7];
				pDrone._DroneRadius = 9.0f;
				pDrone._DroneAngle = 3.14f;
				pDrone._DroneSpinSpeed = 3.0f;
				pDrone.UpdateDronePosition(_X,_Z);

				break;


			case eAIType.AITYPE_GOLD_COIN:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("GoldF1.png");
				_Sprite.AddFrame("GoldF2.png");
				_Sprite.AddFrame("GoldF3.png");
				_Sprite.AddFrame("GoldF4.png");
				_Sprite.AddFrame("GoldF5.png");
				_Sprite.AddFrame("GoldF6.png");
				_Sprite.AddFrame("GoldF7.png");
				_Sprite.AddFrame("GoldF8.png");
				_Sprite._Width = 1.0f;
				_Sprite._Height = 1.0f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 0.5f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();			
				break;

			case eAIType.AITYPE_SILVER_COIN:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("SilverF1.png");
				_Sprite.AddFrame("SilverF2.png");
				_Sprite.AddFrame("SilverF3.png");
				_Sprite.AddFrame("SilverF4.png");
				_Sprite.AddFrame("SilverF5.png");
				_Sprite.AddFrame("SilverF6.png");
				_Sprite.AddFrame("SilverF7.png");
				_Sprite.AddFrame("SilverF8.png");
				_Sprite._Width = 1.0f;
				_Sprite._Height = 1.0f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 0.5f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();			
				break;

			case eAIType.AITYPE_BRONZE_COIN:
				_Sprite = Sprite.Spawn(1);
				_Sprite.AddFrame("BronzeF1.png");
				_Sprite.AddFrame("BronzeF2.png");
				_Sprite.AddFrame("BronzeF3.png");
				_Sprite.AddFrame("BronzeF4.png");
				_Sprite.AddFrame("BronzeF5.png");
				_Sprite.AddFrame("BronzeF6.png");
				_Sprite.AddFrame("BronzeF7.png");
				_Sprite.AddFrame("BronzeF8.png");
				_Sprite._Width = 1.0f;
				_Sprite._Height = 1.0f;
				_Sprite._Animate = true;
				_Sprite._Y = 0.1f;
				_Radius = 0.5f;
				_Speed = 10.0f;
				_Value = 1;
				_AABBCollision = false;
				_HitPoints = 1;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				_FrameAdvanceOnHit = false;
				_TimeToFire = Random.Range(2.0f,3.0f);
				AddToDelegates();			
				break;

			case eAIType.AITYPE_FEEDBACK:
				_Sprite = Sprite.Spawn(1);

				if(_FeedbackType == 0)
				{
					_Sprite.AddFrame("PlusOne.png");
				}
				else if(_FeedbackType == 1)
				{
					_Sprite.AddFrame("PlusFive.png");
				}
				else if(_FeedbackType == 2)
				{
					_Sprite.AddFrame("PlusTen.png");					
				}
				else if(_FeedbackType == 3)
				{
					_Sprite.AddFrame("MinusTen.png");
				}
				else if(_FeedbackType == 4)
				{
					_Sprite.AddFrame("MinusTwenty.png");
				}

				_Sprite._Width = 3.0f;
				_Sprite._Height = 3.0f;
				_Sprite._Animate = false;
				_Sprite._Y = 0.1f;
				_Speed = 8.0f;
				_Width = 0.5f * 0.5f;
				_Height = (0.5f * 0.5f) * Director._Instance._Aspect;
				break;

		}

		_Sprite._AI = this;
		_Sprite._Bullet = null;

		_Kill = false;
	}

	private void AddToDelegates()
	{
		Bullet._Collide += CollideBullet;
		Player._Collide += CollidePlayer;
		Director._Pause += Pause;
		_AddedToDelegates = true;
	}

	private void RemoveFromDelegates()
	{
		if(_AddedToDelegates)
		{
			Bullet._Collide -= CollideBullet;
			Player._Collide -= CollidePlayer;
			Director._Pause -= Pause;
		}
	}

	private void CollidePlayer(Player pPlayer)
	{

		if(_Kill || _PauseCounter > 0.0f)
		{
			return;
		}

		if(!_AABBCollision)
		{
			float fX = _X - pPlayer._X;
			float fZ = _Z - (pPlayer._Z - 1.0f);
			float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

			if(fDist <= (pPlayer._Radius + _Radius))
			{
				if(_Type == eAIType.AITYPE_GOLD_COIN)
				{
					Particle.AddExplosion(_X,_Z,40,1);
					_Kill = true;
					Director._Instance.AddToScore(Director._Instance._GoldCoinScore);
					Director._Instance.PlaySample("COIN",0.2f);
					SpawnFeedback(_X,_Z,2);
				}
				else if(_Type == eAIType.AITYPE_SILVER_COIN)
				{
					Particle.AddExplosion(_X,_Z,40,1);
					_Kill = true;
					Director._Instance.AddToScore(Director._Instance._SilverCoinScore);
					Director._Instance.PlaySample("COIN",0.2f);
					SpawnFeedback(_X,_Z,1);

				}
				else if(_Type == eAIType.AITYPE_BRONZE_COIN)
				{
					Particle.AddExplosion(_X,_Z,40,1);
					_Kill = true;
					Director._Instance.AddToScore(Director._Instance._BronzeCoinScore);	
					Director._Instance.PlaySample("COIN",0.2f);
					SpawnFeedback(_X,_Z,0);
				
				}
				else if(_Type == eAIType.AITYPE_POWERUP)
				{
					Particle.AddExplosion(_X,_Z,40,1);
					_Kill = true;

					switch(_PowerupType)
					{
						case 0:
							Director._Instance._Score -= Director._Instance._SpeedUpCost;
							Director._Instance._SpeedUpFireCurrentTime = Director._Instance._SpeedUpFireTime;
							SpawnFeedback(_X,_Z,3);
							Director._Instance.TriggerSound("RAPIDFIRE",0.5f,1.0f);
							Director._Instance.PlaySample("POWERUP",1.0f);
							break;

						case 1:
							Director._Instance._Score -= Director._Instance._TripleFireCost;
							Director._Instance._TriFireCurrentTime = Director._Instance._TriFireTime;
							Director._Instance.TriggerSound("THREEWAY",0.5f,1.0f);
							Director._Instance.PlaySample("POWERUP",1.0f);
							SpawnFeedback(_X,_Z,4);
							break;

						case 2:
							Director._Instance._Score -= Director._Instance._MagnetCost;
							Director._Instance._MagnetCurrentTime = Director._Instance._MagnetTime;
							Director._Instance.TriggerSound("MAGNET",0.5f,1.0f);
							Director._Instance.PlaySample("POWERUP",1.0f);
							SpawnFeedback(_X,_Z,4);
							break;

						case 3:
							Director._Instance._Score -= Director._Instance._ShieldCost;
							Director._Instance._ShieldCurrentTime = Director._Instance._ShieldTime;
							//Director._Instance.TriggerSound("THREEWAY",0.5f,1.0f);
							Director._Instance.PlaySample("POWERUP",1.0f);
							SpawnFeedback(_X,_Z,4);
							break;

					}

				}
				else
				{
					if(_Type == eAIType.AITYPE_ROCK || _Type == eAIType.AITYPE_BLOCK)
					{
						Particle.AddExplosion(_X,_Z,40);
						Director._Instance.KillPlayer();
						Director._Instance.PlaySample("EXPLOSION",0.5f);
						return;
					}

					//_Kill = true;
					Particle.AddExplosion(_X,_Z,40);
					Director._Instance.KillPlayer();
					Director._Instance.PlaySample("EXPLOSION",0.5f);
				}

				return;
			}
		}
		else
		{
			bool bX1 = false;
			bool bX2 = false;
			bool bZ1 = false;
			bool bZ2 = false;

			if(pPlayer._X1 >= _X1 && pPlayer._X1 <= _X2)
			{
				bX1 = true;
			}

			if(pPlayer._X2 >= _X1 && pPlayer._X2 <= _X2)
			{
				bX2 = true;
			}

			if(pPlayer._Z1 >= _Z1 && pPlayer._Z1 <= _Z2)
			{
				bZ1 = true;
			}

			if(pPlayer._Z2 >= _Z1 && pPlayer._Z2 <= _Z2)
			{
				bZ2 = true;
			}

			// Check for death
			if((bX1 && bZ1) || (bX2 && bZ1) || (bX1 && bZ2) || (bX2 && bZ2))
			{
				if(_Type == eAIType.AITYPE_ROCK)
				{
					Particle.AddExplosion(_X,_Z,40);
					Director._Instance.KillPlayer();
					return;
				}

				//_Kill = true;
				Particle.AddExplosion(_X,_Z,40);
				Director._Instance.KillPlayer();
				return;
			}
		}

	}

	private void CollideBullet(Bullet pBullet)
	{
		if(!pBullet._Alive)
		{
			return;
		}

		if(_StartDelay > 0.0f)
		{
			return;
		}

		if(Director._Instance._PlayerDead)
		{
			return;
		}

		if(_Kill)
		{
			return;
		}

		if(_Type == eAIType.AITYPE_BULLET || _Type == eAIType.AITYPE_POWERUP || _Type == eAIType.AITYPE_GOLD_COIN || _Type == eAIType.AITYPE_SILVER_COIN  || _Type == eAIType.AITYPE_BRONZE_COIN)
		{
			return;
		}

		float fX = _X - pBullet._X;
		float fZ = _Z - pBullet._Z;
		float fDist = Mathf.Sqrt(fX * fX + fZ * fZ);

		if(fDist <= (pBullet._Radius + _Radius))
		{

			Director._Instance.PlaySample("EXPLOSION",0.5f);
			Particle.AddExplosion(_X,_Z,40);
			pBullet.Kill();

			if(_Type != eAIType.AITYPE_DRONE && _Type != eAIType.AITYPE_BLOCK && _Type != eAIType.AITYPE_DOWN_TURRET && _Type != eAIType.AITYPE_UP_TURRET)
			{
	
				--_HitPoints;

				if(_FrameAdvanceOnHit)
				{
					++_CurrentFrame;
					_Sprite.SetFrame(_CurrentFrame);
				}

				if(_HitPoints == 0)
				{
					_Kill = true;

					if(_Type == eAIType.AITYPE_BOSS1)
					{
						AI pAI;

						for(int i= 0; i < _Drones.Count; ++i)
						{
							pAI = _Drones[i];
							pAI._Pause = true;
							pAI._PauseCounter = Random.Range(0.0f,1.0f);
							//pAI._Kill = true;
							//Particle.AddExplosion(pAI._X,pAI._Z,40);
						}

						Spawn(eAIType.AITYPE_GOLD_COIN,_X,_Z);

						--Director._Instance._SequencerSemaphore;
					}
					else if(_Type == eAIType.AITYPE_ROCK)
					{
						Spawn(eAIType.AITYPE_SILVER_COIN,_X,_Z);
					}
					else if(_Type == eAIType.AITYPE_SKULL)
					{
						Spawn(eAIType.AITYPE_GOLD_COIN,_X,_Z);						
					}
					else
					{
						Spawn(eAIType.AITYPE_BRONZE_COIN,_X,_Z);											
					}

					Director._Instance.ShakeCamera(0.3f,0.6f);
					//Director._Instance.AddExplosion(0,_X,_Z);
				}
				return;
			}
		}
	}

	private void Pause()
	{
		_Pause = true;

		if(_Type == eAIType.AITYPE_BLOCK1)
		{
			_PauseCounter = 1.0f + Random.Range(0.0f,3.0f);
		}
		else
		{
			_PauseCounter = 1.0f + Random.Range(0.0f,1.0f);
		}
	}

	private void Terminate()
	{
		_Kill = true;
	}

	public static int GetPoolCount()
	{
		return _Pool.Count;
	}

	public static int GetActiveCount()
	{
		return _Active.Count;
	}

	public static AI Spawn(eAIType eType, float fX, float fZ, float fVX = 0.0f, float fVZ = 0.0f)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled AI");
			return null;
		}

		AI pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			pObject._Type = eType;
			pObject._X = fX;
			pObject._Z = fZ;
			pObject._VX = fVX;
			pObject._VZ = fVZ;

			pObject.Setup();
			pObject._HasPath = false;

			return pObject;
		}

		return null;
	}

	public static AI SpawnOnPath(eAIType eType, int iPathIndex, float fSpeed, float fStartDelay)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled AI");
			return null;
		}

		AI pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			pObject._Type = eType;
			pObject.Setup();
			pObject._Walker.Set(iPathIndex,fSpeed);
			pObject._HasPath = true;
			Vector3 vPos = pObject._Walker.Walk(0.0f);
			pObject._X = vPos.x;
			pObject._Z = vPos.z;
			pObject._StartDelay = fStartDelay;

			return pObject;
		}

		return null;

	}

	public static AI SpawnBlock(eAIType eType, float fX, float fZ, int iType)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled AI");
			return null;
		}

		AI pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			pObject._Type = eType;
			pObject._X = fX;
			pObject._Z = fZ;
			pObject._Block = iType;

			pObject.Setup();

			return pObject;
		}

		return null;	}

	public static void SpawnPowerup(float fX, float fZ, int iType)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled AI");
			return;
		}

		int iScore = Director._Instance._Score;

		//if(iScore < Director._Instance._ShieldCost && iType == 3)
	//	{
	//		return;
	//	}

		if(iScore < Director._Instance._MagnetCost && iType == 2)
		{
			return;
		}		

		if(iScore < Director._Instance._TripleFireCost && iType == 1)
		{
			return;
		}

		if(iScore < Director._Instance._SpeedUpCost && iType == 0)
		{
			return;
		}


		AI pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			pObject._Type = eAIType.AITYPE_POWERUP;
			pObject._X = fX;
			pObject._Z = fZ;
			pObject._PowerupType = iType;
			pObject.Setup();
		}
	}

	public static void SpawnFeedback(float fX, float fZ, int iType)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled AI");
			return;
		}

		AI pObject = _Pool.First.Value;

		if(pObject != null)
		{
			_Pool.RemoveFirst();
			_Active.AddLast(pObject);

			pObject._Type = eAIType.AITYPE_FEEDBACK;
			pObject._X = fX;
			pObject._Z = fZ;
			pObject._FeedbackType = iType;
			pObject.Setup();
		}
	}

	public static void Initialise(int iCount)
	{
		if(_Pool == null)
		{
			_Pool = new LinkedList<AI>();
			_Active = new LinkedList<AI>();
			_ClearList = new List<AI>();
		}

		for(int i = 0; i < iCount; ++i)
		{
			AI pObject = new AI();
			pObject._MaxX = Ground._Instance._LowerX;
			pObject._MinX = Ground._Instance._HigherX;
			_Pool.AddLast(pObject);
		}
	}

	public static void UpdateAndRenderAll(float fTimeDelta)
	{
    	AI pObject;

		for(LinkedListNode<AI> it = _Active.First; it != null; it = it.Next)
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
			_Active.Remove(pObject);
			_Pool.AddLast(pObject);

			Sprite.Kill(pObject._Sprite);
			pObject.RemoveFromDelegates();

		}

		_ClearList.Clear();

	}

	public static void ClearAll()
	{
	    AI pObject;

		for(LinkedListNode<AI> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;
		}

		_Active.Clear();
	}		

}