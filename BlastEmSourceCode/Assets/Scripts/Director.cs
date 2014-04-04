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
using System.IO;
using System.Xml;

public class Director : MonoBehaviour 
{
	public int _MajorVersion;
	public int _MinorVersion;
	public bool _ThereIsANewVersion;
	public string _MacURL;
	public string _WinURL;
	private float _NewVersionTime;

	public Camera _Camera;
	public float _YAdjust;
	public float _Aspect;
	private Transform _CameraTrans;
	public static Director _Instance;
	public bool _Debug;

	public Generator _Generator;

	// Game state
	public enum eGameState
	{
		GAMESTATE_SETUP_COMPANY,
		GAMESTATE_COMPANY,
		GAMESTATE_SETUP_TITLE,
		GAMESTATE_TITLE,
		GAMESTATE_SETUP_STAGES,
		GAMESTATE_STAGES,
		GAMESTATE_SETUP_GAME,
		GAMESTATE_GAME,
		GAMESTATE_SETUP_CREDITS,
		GAMESTATE_CREDITS,
		GAMESTATE_HISCORES,
		GAMESTATE_SETUP_HOW_TO_PLAY,
		GAMESTATE_HOW_TO_PLAY,
		GAMESTATE_HIGH_SCORE_SETUP,
		GAMESTATE_HIGH_SCORE,
		GAMESTATE_NAME_SETUP,
		GAMESTATE_NAME,
		GAMESTATE_GET_NEW_VERSION_SETUP,
		GAMESTATE_GET_NEW_VERSION,
	};

	private eGameState _CurrentState;
	private eGameState _LastState;

	// Player movement stuff
	private GameObject _PlayerObject;
	public Player _Player;
	private Transform _CameraTransform;
	private Vector3 _CameraPos;
	public bool _PlayerDead;
	public float _PlayerDeadCountdown;

	// Score data
	public Number _Number;
	public int _Score;
	public int _LocalHiScore;

	// Debug data
	private Number _SpriteCount;
	private Number _SpriteCount1;
	private Number _AICount;
	private Number _AICount1;
	private Number _ExplosionCount;

	// UI controls
	private Button _PlayButton;
	private Button _CreditsButton;
	private Button _CreditsBackButton;	
	private Image _Title;
	private Image _Credits;
	public Image _LevelInfo;

	// AI notification
	public delegate void Pause();
	public static Pause _Pause = null;
	public delegate void Terminate();
	public static Terminate _Terminate = null;

	// Sequencer data
	public int _SequencerSemaphore;
	private float _SequencerDelay;
	private float _SequencerLifetime;

	// Mouse tracking
	private bool _MouseDown;
	private Vector3 _MousePosition;

	// Camera shake
	private float _ShakeIntensity;
	private float _ShakeTime;
	private float _ShakeStartTime;
	private float _ShakePerc;

	// Camera rotate
	private float _XAngle;
	private float _ZAngle;
	private Vector3 _CameraAngles;

	// Screen stuff
	private Dictionary<string,ScreenContainer> _Screens;
	private ScreenContainer _CurrentScreen;

	// Coin scores
	public int _GoldCoinScore;
	public int _SilverCoinScore;
	public int _BronzeCoinScore;

	public float _PowerUpDelay;


	public int _SpeedUpCost;
	public int _TripleFireCost;
	public int _ReverseFireCost;
	public int _ShieldCost;
	public int _SmartBombCost;
	public int _MagnetCost;


	public float _NormalFireDelay;
	public float _SpeedUpDelay;	
	public float _SpeedUpFireTime;
	public float _SpeedUpFireCurrentTime;
	public float _TriFireTime;
	public float _TriFireCurrentTime;

	public float _MagnetTime;
	public float _MagnetCurrentTime;

	public float _ShieldTime;
	public float _ShieldCurrentTime;

	public float _MinMineDistance;


	public float _MinAIFireDistance;

	// Samples
	public Dictionary<string,AudioClip> _Samples;
	public Dictionary<string,float> _SamplesTimeStamp;
	public Dictionary<string,bool> _SamplePlayAlways;

	public AudioSource _AudioSource;

	// Stage and life information
	public int _CurrentStage;
	public int _CurrentLockState;
	public int _Lives;
	public List<Sprite> _LivesSprites;

	private float _Time;


	public Material _Starfield;
	public float _Warp;
	public float _Speed;

	private bool _PatheEditor;
	private WayPoints _WPCopyBuffer;

	// WayPoints
	public WayPointDatabase _WayPointDatabase;
	private WayPoints _A;
	private Sprite _PF;
	private Vector3 _PFP;
	private int _PFI;

	private WayPointWalker _Walker;

	// Line drawing - Unity Pro only
	private List<Vector3> _GLLines;
	public Material _LineMaterial;

	public Scores _Scores;
	public int _TimePlayed;
	public float _TimeCount;

	// Music
	
	public AudioClip[] _Music;
	private int _MusicIndex;
	private AudioSource _Audio;
	private float _TimeMusicPlaying;

	// Hud indicators
	private Sprite _BulletSpeedUp;


	private float _TriggerSoundDelay;
	private bool _TriggerSound;
	private string _TriggerSoundName;
	private float _TriggerSoundVolume;

	public float _MusicVolume;
	public float _SFXVolume;

	public AudioSource _SFX;

	private float _CompanyDelay;

	void Awake()
	{
		_TriggerSound = false;

		_SFX = GameObject.Find("SFXPlayer").GetComponent<AudioSource>();


#if UNITY_IPHONE				
		Application.targetFrameRate = 60;
#endif
		_Instance = this;
		_Camera = this.GetComponent<Camera>();
		_CameraTrans = _Camera.transform;
		_CameraPos = _CameraTrans.position;
		Sprite.Initialise(1000);
		Bullet.Initialise(400);
		Particle.Initialise(1000);
		AI.Initialise(500);

		_MouseDown = false;
		_MousePosition = new Vector3(0.0f,0.0f,0.0f);

		_Screens = new Dictionary<string,ScreenContainer>();
		_CurrentScreen = null;
		_CameraAngles = new Vector3(0.0f,0.0f,0.0f);
		_XAngle = 40.0f;
		_ZAngle = 30.0f;
		_CurrentStage = 0;

		_LocalHiScore = PlayerPrefs.GetInt("HISCORE");
		_CurrentLockState = PlayerPrefs.GetInt("LOCK");
		Scores._Name = PlayerPrefs.GetString("NAME");

	
		_SFXVolume = 1.0f;

		_MusicVolume = 1.0f;

		if(_CurrentLockState == 0)
		{
			_CurrentLockState = 1;
		}

		Debug.Log("Aspect ratio = " + _Camera.aspect);

		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
		

 #if UNITY_IPHONE			
			if(iPhone.generation == iPhoneGeneration.iPhone5 || iPhone.generation == iPhoneGeneration.iPodTouch5Gen)
			{
				if(_Camera.aspect > 1.0f)
				{
					_Aspect = _Camera.aspect;
				}

				_CameraPos = new Vector3(0.0f,25.0f,0.0f);
			}
			else
			{
				_Aspect = 1.0f;
				_CameraPos = new Vector3(0.0f,20.0f,0.0f);
			}
#endif
		}
		else
		{
			_Aspect = 1.0f;
			_CameraPos = new Vector3(0.0f,20.0f,0.0f);
		}

		_WayPointDatabase = GameObject.Find("WayPointDatabase").GetComponent<WayPointDatabase>();
		_YAdjust = 0.0f;

		_GLLines = new List<Vector3>();

		
		_MusicIndex = Random.Range(0,5);

		_Audio = this.GetComponent<AudioSource>();
		_Audio.clip = _Music[_MusicIndex];
		_Audio.Play();
		_TimeMusicPlaying = 0.0f;
		
	}


	// Use this for initialization
	void Start () 
	{

		// Get all the stuff necessary for player movement
		_PlayerObject = GameObject.Find("Player");
		_Player = _PlayerObject.GetComponent<Player>();
		_CameraTransform = this.transform;
		_CameraTransform.position = _CameraPos;

		// Find the UI
		_PlayButton = GameObject.Find("PlayButton").GetComponent<Button>();
		_CreditsBackButton = GameObject.Find("CreditsBackButton").GetComponent<Button>();
		_Title = GameObject.Find("Title").GetComponent<Image>();
		_Credits = GameObject.Find("Credits").GetComponent<Image>();
		_LevelInfo = GameObject.Find("LevelInfo").GetComponent<Image>();

		// Build the screens
		ScreenContainer pTitleScreen = new ScreenContainer();

		
		pTitleScreen.AddButton("PlayButton");
		pTitleScreen.AddButton("CloseButton");
		pTitleScreen.AddButton("WhoMadeThisButton");
		pTitleScreen.AddButton("HighScoreTableButton");
		pTitleScreen.AddButton("TwitterButton");
		pTitleScreen.AddButton("SettingsButton");
		

		pTitleScreen.AddCompound("LogoContainer");


		_Screens.Add("TITLE",pTitleScreen);

		ScreenContainer pCredits = new ScreenContainer();
		pCredits.AddButton("CreditsBackButton");

		pCredits.AddUIGO("CreditsByron");
		pCredits.AddUIGO("CreditsGavin");		
		pCredits.AddUIGO("CreditsNatalie");
		pCredits.AddUIGO("CreditsSusan");
		pCredits.AddUIGO("CreditsWill");
		pCredits.AddUIGO("CreditsDes");
		pCredits.AddUIGO("CreditsDugan");
		pCredits.AddUIGO("CreditsDan");
		pCredits.AddUIGO("CreditsThanks");

		_Screens.Add("CREDITS",pCredits);

		ScreenContainer pHowTo = new ScreenContainer();
		pHowTo.AddButton("CreditsBackButton");
		pHowTo.AddImage("HowToPlay");
		_Screens.Add("HOW_TO_PLAY",pHowTo);

		ScreenContainer pPlay = new ScreenContainer();
		pPlay.AddUIGO("Land");
		pPlay.AddUIGO("Land2");		
		_Screens.Add("PLAY",pPlay);

		ScreenContainer pPathEditor = new ScreenContainer();
		_Screens.Add("PATH",pPathEditor);

		ScreenContainer pScores = new ScreenContainer();
		pScores.AddButton("HighScoreBackButton");
		pScores.AddButton("HSUpButton");
		pScores.AddButton("HSDownButton");
		_Screens.Add("SCORES",pScores);

		ScreenContainer pName = new ScreenContainer();
		pName.AddButton("CreditsBackButton");
		pName.AddButton("OneUpButton");
		pName.AddButton("OneDownButton");
		pName.AddButton("TwoUpButton");
		pName.AddButton("TwoDownButton");
		pName.AddButton("ThreeUpButton");
		pName.AddButton("ThreeDownButton");
		pName.AddButton("FourUpButton");
		pName.AddButton("FourDownButton");
		pName.AddButton("FiveUpButton");
		pName.AddButton("FiveDownButton");
		pName.AddButton("SixUpButton");
		pName.AddButton("SixDownButton");
		pName.AddButton("SevenUpButton");
		pName.AddButton("SevenDownButton");
		pName.AddButton("EightUpButton");
		pName.AddButton("EightDownButton");
		pName.AddButton("NineUpButton");
		pName.AddButton("NineDownButton");
		pName.AddButton("TenUpButton");
		pName.AddButton("TenDownButton");

		pName.AddButton("MusicVolumeUpButton");
		pName.AddButton("MusicVolumeDownButton");
		pName.AddButton("SFXVolumeUpButton");
		pName.AddButton("SFXVolumeDownButton");
		pName.AddCompound("MusicVolumeContainer");
		pName.AddCompound("SFXVolumeContainer");
		pName.AddCompound("MusicVolumeNameContainer");
		pName.AddCompound("SFXNameContainer");
		pName.AddCompound("NameContainer");

		_Screens.Add("NAME",pName);

		ScreenContainer pNewVersion = new ScreenContainer();
		pNewVersion.AddButton("GoGetButton");
		pNewVersion.AddButton("GoGetBackButton");	
		pNewVersion.AddUIGO("NewVersion");	
		_Screens.Add("NEW_VERSION",pNewVersion);

		ScreenContainer pCompany = new ScreenContainer();	
		pCompany.AddUIGO("Company");	
		_Screens.Add("COMPANY",pCompany);


		_Number = new Number();
		_Number._Z = 10.0f;	
		_Number.Disable();
	
		// Setup the initial game state
		//_CurrentState = eGameState.GAMESTATE_SETUP_COMPANY;
		_CurrentState = eGameState.GAMESTATE_SETUP_TITLE;

		_Camera = GetComponent<Camera>();

		// Disable components to start
		_Player.Disable();
	
		_Samples = new Dictionary<string,AudioClip>();
		_SamplesTimeStamp = new Dictionary<string, float>();
		_SamplePlayAlways = new Dictionary<string, bool>();
		_AudioSource = this.GetComponent<AudioSource>();

		// Add samples
		AddSample("ALIEN_SHOT","SFX/Enemy");
		AddSample("PLAYER_SHOT","SFX/PlayerShot2");
		AddSample("EXPLOSION","SFX/Explosion");
		AddSample("COIN","SFX/Coin");
		AddSample("POWERUP","SFX/PowerUp");
		AddSample("PLAYERDEATH","SFX/PlayerDeath",true);
		AddSample("RAPIDFIRE","SFX/RapidFire",true);
		AddSample("GAMEON","SFX/GameOn",true);		
		AddSample("WIPEOUT","SFX/WipeOut",true);	
		AddSample("CLICK","SFX/Click",true);	
		AddSample("PRESS","SFX/Press",true);
		AddSample("THREEWAY","SFX/ThreeWay",true);		
		AddSample("MAGNET","SFX/Magnatism",true);		

		// Create lives sprites
		_LivesSprites = new List<Sprite>();

		float fLX = Ground._Instance._HigherX + 2.0f;
		float fLZ = Ground._Instance._LowerZ - 2.0f;

		for(int i = 0; i < 20; ++i)
		{
			Sprite pSprite = Sprite.Spawn(1);

			pSprite.AddFrame("Life.png");
			pSprite._Width = 0.5f;
			pSprite._Height = 0.5f;
			pSprite._Animate = false;;
			pSprite._Y = 1.0f;
			pSprite._X = fLX;
			pSprite._Z = fLZ;
			pSprite._Alive = false;
			_LivesSprites.Add(pSprite);

			fLX += 0.6f;
		}

		_Starfield = GameObject.Find("Starfield").GetComponent<Renderer>().sharedMaterial;
		_Warp = 0.0f;
		_Speed = 1.0f;

		_PatheEditor = false;
		_Generator = new Generator();

		_Scores = GameObject.Find("Scores").GetComponent<Scores>();


		// Load HUD indicators
		_BulletSpeedUp = Sprite.Spawn(1);
		_BulletSpeedUp.AddFrame("FasterShot1.png");
		_BulletSpeedUp.AddFrame("FasterShot2.png");
		_BulletSpeedUp.AddFrame("FasterShot3.png");
		_BulletSpeedUp.AddFrame("FasterShot4.png");
		_BulletSpeedUp.AddFrame("FasterShot5.png");
		_BulletSpeedUp.AddFrame("FasterShot6.png");
		_BulletSpeedUp.AddFrame("FasterShot7.png");
		_BulletSpeedUp.AddFrame("FasterShot8.png");
		_BulletSpeedUp.AddFrame("FasterShot9.png");
		_BulletSpeedUp.AddFrame("FasterShot10.png");
		_BulletSpeedUp._X = -10.0f;
		_BulletSpeedUp._Z = 10.0f;
		_BulletSpeedUp._Height = 1.2f;
		_BulletSpeedUp._Width = 1.2f;
		_BulletSpeedUp._Animate = false;
		_BulletSpeedUp._Y = 0.1f;
		_BulletSpeedUp._Alive = false;



	}

	void OnPostRender()
	{
		GL.PushMatrix();
		_LineMaterial.SetPass(0);

		GL.Begin( GL.LINES );

		int iIndex = 0;

		Color pColor = Color.red;

		if(_GLLines.Count > 0)
		{
			for(int i = 0; i < _GLLines.Count / 3; ++i)
			{
				pColor.r = _GLLines[iIndex].x;
				pColor.g = _GLLines[iIndex].y;
				pColor.b = _GLLines[iIndex].z;
				pColor.a = 1.0f;

				GL.Color(pColor);
				GL.Vertex3(_GLLines[iIndex+1].x, _GLLines[iIndex+1].y, _GLLines[iIndex+1].z);
				GL.Vertex3(_GLLines[iIndex+2].x, _GLLines[iIndex+2].y, _GLLines[iIndex+2].z);
				iIndex+=3;
			}
		}

		GL.End();	

	 	GL.PopMatrix();

	 	_GLLines.Clear();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(_ThereIsANewVersion)
		{
			_CurrentState = eGameState.GAMESTATE_GET_NEW_VERSION_SETUP;
			_ThereIsANewVersion = false;
		}


		float fTimeDelta = Time.deltaTime;
		_Time += fTimeDelta;

		Bullet.UpdateAndRenderAll(fTimeDelta);
		Particle.UpdateAndRenderAll(fTimeDelta);
		AI.UpdateAndRenderAll(fTimeDelta);
		Sprite.UpdateAndRenderAll(fTimeDelta);

		switch(_CurrentState)
		{
			case eGameState.GAMESTATE_SETUP_COMPANY:
				DoSetupCompany();
				break;

			case eGameState.GAMESTATE_COMPANY:
				DoCompany(fTimeDelta);
				break;

			case eGameState.GAMESTATE_SETUP_TITLE:
				DoSetupTitle();
				break;
	
			case eGameState.GAMESTATE_TITLE:
				DoTitle(fTimeDelta);
				break;

			case eGameState.GAMESTATE_SETUP_STAGES:
				DoSetupStages();
				break;

			case eGameState.GAMESTATE_STAGES:
				DoStages(fTimeDelta);
				break;


			case eGameState.GAMESTATE_SETUP_GAME:
				DoSetupGame();
				break;

			case eGameState.GAMESTATE_GAME:
				DoGame(fTimeDelta);
				break;

			case eGameState.GAMESTATE_SETUP_CREDITS:
				DoSetupCredits();
				break;

			case eGameState.GAMESTATE_CREDITS:
				DoCredits(fTimeDelta);
				break;

			case eGameState.GAMESTATE_HISCORES:
				//DoHiScore(fTimeDelta);
				break;

			case eGameState.GAMESTATE_SETUP_HOW_TO_PLAY:
				DoSetupHowToPlay(fTimeDelta);
				break;

			case eGameState.GAMESTATE_HOW_TO_PLAY:
				DoHowToPlay(fTimeDelta);
				break;

			case eGameState.GAMESTATE_HIGH_SCORE_SETUP:
				DoHighScoreSetup();
				break;

			case eGameState.GAMESTATE_HIGH_SCORE:
				DoHiScore();
				break;

			case eGameState.GAMESTATE_NAME_SETUP:
				DoNameSetup();
				break;

			case eGameState.GAMESTATE_NAME:
				DoName();
				break;

			case eGameState.GAMESTATE_GET_NEW_VERSION_SETUP:
				DoGetNewVersionSetup();
				break;

			case eGameState.GAMESTATE_GET_NEW_VERSION:
				DoGetNewVersion();
				break;

		}

		/*
		_SpriteCount.SetNumber(Sprite.GetPoolCount());
		_SpriteCount1.SetNumber(Sprite.GetActiveCount());

		_AICount.SetNumber(AI.GetPoolCount());
		_AICount1.SetNumber(AI.GetActiveCount());
		*/

		DoShake(fTimeDelta);

		//DoCameraRotate(fTimeDelta);

		//_Starfield.SetFloat("_Warp",_Warp);
		//_Starfield.SetFloat("_Speed", _Speed);



		// Handle giving input to the path editor

		PathEditor();

		
		_TimeMusicPlaying += fTimeDelta;

		_Audio.volume = _MusicVolume;
		_SFX.volume = _SFXVolume;

		if(!_Audio.isPlaying)
		{
			++_MusicIndex;

			if(_MusicIndex == 5)
			{
				_MusicIndex = 0;
			}

			_Audio.clip = _Music[_MusicIndex];
			_Audio.Play();
			_TimeMusicPlaying = 0.0f;
		}
		

		DisplayHUD();

		if(_TriggerSound)
		{
			_TriggerSoundDelay -= fTimeDelta;

			if(_TriggerSoundDelay <= 0.0f)
			{
				_TriggerSound = false;
				PlaySample(_TriggerSoundName,_TriggerSoundVolume);
			}
		}
	}

	private void DisplayHUD()
	{


	}

	private void ClearHUD()
	{
		_BulletSpeedUp._Alive = false;
		_SpeedUpFireCurrentTime = 0.0f;
	}

	private void PathEditor()
	{
		if(_PatheEditor)
		{
			Ray ray;
			RaycastHit hit;

			Vector3 vPos = _CameraPos;
			vPos.y += 15.0f;
			_CameraTrans.position = vPos;

			Vector3 vPoint = _Walker.Walk(Time.deltaTime);
			_PF._X = vPoint.x;
			_PF._Z = vPoint.z;
		
			if(Input.GetMouseButton(0))
			{
				_WayPointDatabase.MouseDown();
			}
			else
			{
				_WayPointDatabase.MouseUp();
			}

			ray = _Camera.ScreenPointToRay(Input.mousePosition);

       		if(Physics.Raycast(ray, out hit, 200))
			{
				if(hit.transform.gameObject.name == "Ground")
				{
					_WayPointDatabase.Mouse(hit.point.x,hit.point.z);

				}
			}

			if(Input.GetKeyDown(KeyCode.A))
			{
				_WayPointDatabase.AddPoint();
			}

			if(Input.GetKeyDown(KeyCode.D))
			{
				_WayPointDatabase.DeletePoint();
			}

			// Exit the path editor
			if(Input.GetKeyDown(KeyCode.P))
			{
				_PatheEditor = false;

				Sprite.Kill(_PF);
				_Walker = null;

				_Number.SetNumber(_LocalHiScore);
				SetScreen("TITLE");	
				return;
			}

			/*
			if(Input.GetKeyDown(KeyCode.S))
			{
				_WayPointDatabase.Save();
			}		

			if(Input.GetKeyDown(KeyCode.L))
			{
				//_WayPointDatabase.Load();
			}	
			*/	

			if(Input.GetKeyDown(KeyCode.Z))
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					for(int i = 0; i < 10; ++i)
					{
						_WayPointDatabase.Previous();
					}
				}
				else
				{
					_WayPointDatabase.Previous();
				}

				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
			}		

			if(Input.GetKeyDown(KeyCode.X))
			{

				if(Input.GetKey(KeyCode.LeftShift))
				{
					for(int i = 0; i < 10; ++i)
					{
						_WayPointDatabase.Next();
					}
				}
				else
				{
					_WayPointDatabase.Next();
				}

				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
			}		
			
			// Add a new path onto the end of the list
			if(Input.GetKeyDown(KeyCode.N))
			{
				_WayPointDatabase.New();
				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
			}	

			if(Input.GetKeyDown(KeyCode.I))
			{
				_WayPointDatabase.NewInsert();
				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
			}	

			if(Input.GetKeyDown(KeyCode.E))
			{
				_WayPointDatabase.ErasePath();
				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
			}					
				
			if(Input.GetKeyDown(KeyCode.C))
			{
				_WPCopyBuffer = _Walker._WayPoints;
			}	

			if(Input.GetKeyDown(KeyCode.V))
			{
				if(_WPCopyBuffer == null)
				{
					return;
				}

				_WayPointDatabase.NewInsert();
				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
				_Walker._WayPoints.Copy(_WPCopyBuffer);
			}								

			if(Input.GetKeyDown(KeyCode.B))
			{
				if(_WPCopyBuffer == null)
				{
					return;
				}

				_WayPointDatabase.New();
				_Walker = new WayPointWalker(0.3f,_WayPointDatabase._CurrentIndex);
				_Walker._WayPoints.Copy(_WPCopyBuffer);
			}	

			if(Input.GetKey(KeyCode.UpArrow))
			{
				if(_Walker != null)
				{
					if(Input.GetKey(KeyCode.LeftShift))
					{
						_Walker._WayPoints.MarchUp(1.0f);
					}
					else
					{
						_Walker._WayPoints.MarchUp(10.0f);
					}
					
				}
			}

			if(Input.GetKey(KeyCode.DownArrow))
			{
				if(_Walker != null)
				{
					if(Input.GetKey(KeyCode.LeftShift))
					{
						_Walker._WayPoints.MarchDown(1.0f);
					}
					else
					{
						_Walker._WayPoints.MarchDown(10.0f);
					}
				}
			}

			if(Input.GetKey(KeyCode.LeftArrow))
			{
				if(_Walker != null)
				{
					if(Input.GetKey(KeyCode.LeftShift))
					{
						_Walker._WayPoints.MarchLeft(1.0f);
					}
					else
					{
						_Walker._WayPoints.MarchLeft(10.0f);
					}
					
				}
			}

			if(Input.GetKey(KeyCode.RightArrow))
			{
				if(_Walker != null)
				{
					if(Input.GetKey(KeyCode.LeftShift))
					{
						_Walker._WayPoints.MarchRight(1.0f);
					}
					else
					{
						_Walker._WayPoints.MarchRight(10.0f);
					}
				}
			}

			if(Input.GetKeyDown(KeyCode.R))
			{
				_Walker._WayPoints.Reverse();
			}	

			if(Input.GetKeyDown(KeyCode.F))
			{
				_Walker._WayPoints.Reverse();
				_Walker._WayPoints.MirrorX();
			}	

			if(Input.GetKeyDown(KeyCode.G))
			{
				_Walker._WayPoints.MirrorZ();
			}	

			_WayPointDatabase.DebugDraw();

			_Number.SetNumber(_WayPointDatabase._CurrentIndex);
		}
	}

	private void DoGame(float fTimeDelta)
	{
		Ray ray;
		RaycastHit hit;

		if(!_PlayerDead)
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
					ray = _Camera.ScreenPointToRay(Input.mousePosition);
		    
		       		if(Physics.Raycast(ray, out hit, 200))
					{

						if(hit.transform.gameObject.name == "Ground")
						{

							_Player.SetPosition(hit.point.x,hit.point.z);
						}
					}
			}
			else
			{
				// Handle touches
	      		if(Input.touchCount > 0)
		        {
		        	Touch pTouch = Input.touches[0];

		        	switch(pTouch.phase)
		        	{
		        		case TouchPhase.Began:
							ray = _Camera.ScreenPointToRay(pTouch.position);

							//Debug.Log("Touch began x = " + pTouch.position.x + " z = " + pTouch.position.y);
		    
		       				if(Physics.Raycast(ray, out hit, 200))
							{
								if(hit.transform.gameObject.name == "Ground")
								{
									//Debug.Log("Hit ground");
									_Player.SetOffset(hit.point.x,hit.point.z);
									_MouseDown = true;
								}

							}
		         			break;

		        		case TouchPhase.Moved:
							ray = _Camera.ScreenPointToRay(pTouch.position);

							//Debug.Log("Touch moved x = " + pTouch.position.x + " z = " + pTouch.position.y);
		    
		       				if(Physics.Raycast(ray, out hit, 200))
							{
								if(hit.transform.gameObject.name == "Ground")
								{
									_Player.SetPosition(hit.point.x,hit.point.z);
									//Debug.Log("Hit ground");
								}
							}
		        			break;

		        		 case TouchPhase.Ended:
		        			 //Debug.Log("Touch ended");
							_MouseDown = false;
		        		 	break;
		        	}
		        }
	    	}
	
			_TimeCount += fTimeDelta;

			if(_TimeCount >= 1.0f)
			{
				_TimeCount -= 1.0f;
				++_TimePlayed;
			}

			_Generator.Update(fTimeDelta);
		}
		else
		{
			_PlayerDeadCountdown -= fTimeDelta;

			if(_PlayerDeadCountdown <= 0.0f)
			{
				if(_Lives != 0)
				{
					--_Lives;
					_CurrentState = eGameState.GAMESTATE_SETUP_GAME;
				}
				else
				{
					_CurrentState = eGameState.GAMESTATE_SETUP_TITLE;
				}
				
				Particle.AddExplosion(_Player._X,_Player._Z-1.0f,300);
				ShakeCamera(0.3f,0.6f);
				PlaySample("PLAYERDEATH",1.0f);

				//if(_Score > _LocalHiScore)
				//{
					_Scores.AddScore(Scores._Name,_Score,_TimePlayed);
				//}
			}
		}


		_Number.SetNumber(_Score);

		if(_Score > _LocalHiScore)
		{
			_LocalHiScore = _Score;
		}

		_SpeedUpFireCurrentTime -= fTimeDelta;

		if(_SpeedUpFireCurrentTime <= 0.0f)
		{
			_SpeedUpFireCurrentTime = 0.0f;
		}

		_TriFireCurrentTime -= fTimeDelta;

		if(_TriFireCurrentTime <= 0.0f)
		{
			_TriFireCurrentTime = 0.0f;
		}

		_MagnetCurrentTime -= fTimeDelta;

		if(_MagnetCurrentTime <= 0.0f)
		{
			_MagnetCurrentTime = 0.0f;
		}
		
		_ShieldCurrentTime -= fTimeDelta;

		if(_ShieldCurrentTime <= 0.0f)
		{
			_ShieldCurrentTime = 0.0f;
		}
	}

	private void DoSetupTitle()
	{
		_CurrentState = eGameState.GAMESTATE_TITLE;
		//_Number.SetNumber(_LocalHiScore);
		_Player.Disable();
		Scores.ShutdownNameScreen();
		_Score = 0;
		SetScreen("TITLE");
		_Scores.GetScoresTable();

		Screen.showCursor = true;
		ClearHUD();
	}

	private void DoTitle(float fTimeDelta)
	{

	}

	private void DoSetupHowToPlay(float fTimeDelta)
	{
		Debug.Log("Setup how to play");
		_CurrentState = eGameState.GAMESTATE_HOW_TO_PLAY;
		SetScreen("HOW_TO_PLAY");
		_Number.Disable();
	}

	private void DoHowToPlay(float fTimeDelta)
	{

	}

	private void DoSetupStages()
	{
		SetScreen("STAGES");
		_CurrentState = eGameState.GAMESTATE_STAGES;

	}

	private void DoStages(float fTimeDelta)
	{

	}

	private void DoSetupGame()
	{
		_CurrentState = eGameState.GAMESTATE_GAME;
		_Player.Enable();
		_Number.SetNumber(0);

		_SequencerSemaphore = 0;
		_SequencerLifetime = 0.0f;
		_SequencerDelay = 0.0f;
		_PlayerDead = false;
	
		_SpeedUpFireCurrentTime = 0.0f;
		_TriFireCurrentTime = 0.0f;
		_MagnetCurrentTime = 0.0f;
		_ShieldCurrentTime = 0.0f;

		SetScreen("PLAY");

		for(int i = 0; i < 20; ++i)
		{
			Sprite pSprite = _LivesSprites[i];
			pSprite._Alive = false;
		}

		for(int i = 0; i < _Lives; ++i)
		{
			Sprite pSprite = _LivesSprites[i];
			pSprite._Alive = true;
		}

		Screen.showCursor = false;

		Debug.Log("Playing GAMEON");
		PlaySample("GAMEON",1.0f);

	}

	private void DoSetupCredits()
	{
		_CurrentState = eGameState.GAMESTATE_CREDITS;
		_LastState = eGameState.GAMESTATE_TITLE;
		SetScreen("CREDITS");
		_Number.Disable();
	}

	private void DoCredits(float fTimeDelta)
	{

	}

	private void DoHiScore()
	{

	}

	private void DoHighScoreSetup()
	{
		_CurrentState = eGameState.GAMESTATE_HIGH_SCORE;
		SetScreen("SCORES");
	}

	private void DoNameSetup()
	{
		_CurrentState = eGameState.GAMESTATE_NAME;
		Scores.SetupNameScreen();
		SetScreen("NAME");

	}

	private void DoName()
	{

	}

	private void DoGetNewVersionSetup()
	{
		_CurrentState = eGameState.GAMESTATE_GET_NEW_VERSION;
		_NewVersionTime = 0.0f;
		SetScreen("NEW_VERSION");
	}

	private void DoGetNewVersion()
	{

		_NewVersionTime -= Time.deltaTime;

		if(_NewVersionTime <= 0.0f)
		{
			_NewVersionTime = 0.1f;

			Particle.AddFirework(Random.Range(-20.0f,20.0f),Random.Range(-14.0f,14.0f),40);
		}
	}

	private void DoSetupCompany()
	{
		_CurrentState = eGameState.GAMESTATE_COMPANY;
		_CompanyDelay = 2.5f;
		SetScreen("COMPANY");
	}

	private void DoCompany(float fTimeDelta)
	{
		_CompanyDelay -= fTimeDelta;

		if(_CompanyDelay <= 0.0f)
		{
			_CompanyDelay = 0.0f;
			_CurrentState = eGameState.GAMESTATE_SETUP_TITLE;
		}
	}



	public void FireEvent(int iEvent)
	{
		switch(iEvent)
		{
			case 0:
				//_CurrentState = eGameState.GAMESTATE_SETUP_STAGES;
				_Lives = 0;
				_CurrentStage = 1;
				_Time = 0.0f;
				_Generator.Reset();
				_CurrentState = eGameState.GAMESTATE_SETUP_GAME;
				_TimePlayed = 0;
				_TimeCount = 0.0f;
				break;

			case 1:
				_CurrentState = eGameState.GAMESTATE_SETUP_CREDITS;
				break;

			case 2:
				_CurrentState = eGameState.GAMESTATE_SETUP_HOW_TO_PLAY;
				break;	

			case 3:
				_CurrentState = eGameState.GAMESTATE_SETUP_TITLE;
				break;

			case 4:
				_Lives = 0;
				_CurrentStage = 1;
				_Time = 0.0f;
				_CurrentState = eGameState.GAMESTATE_SETUP_GAME;
				break;

			case 5:
				_Lives = 0;
				_CurrentStage = 2;
				_Time = 0.0f;
				_CurrentState = eGameState.GAMESTATE_SETUP_GAME;
				break;

			case 6:
				Scores.SetupScoreScreen();
				_CurrentState = eGameState.GAMESTATE_HIGH_SCORE_SETUP;
				break;

			case 7:
				Scores.ShutdownScoreScreen();
				_CurrentState = eGameState.GAMESTATE_SETUP_TITLE;
				break;

			case 8:
				Scores.ScoreUp();
				break;

			case 9:
				Scores.ScoreDown();
				break;

			case 10:
				_CurrentState = eGameState.GAMESTATE_NAME_SETUP;
				break;

			// Name events

			case 11:
				Scores.CharUp(0);
				break;

			case 12:
				Scores.CharDown(0);
				break;

			case 13:
				Scores.CharUp(1);
				break;

			case 14:
				Scores.CharDown(1);
				break;

			case 15:
				Scores.CharUp(2);
				break;

			case 16:
				Scores.CharDown(2);
				break;

			case 17:
				Scores.CharUp(3);
				break;

			case 18:
				Scores.CharDown(3);
				break;

			case 19:
				Scores.CharUp(4);
				break;

			case 20:
				Scores.CharDown(4);
				break;

			case 21:
				Scores.CharUp(5);
				break;

			case 22:
				Scores.CharDown(5);
				break;

			case 23:
				Scores.CharUp(6);
				break;

			case 24:
				Scores.CharDown(6);
				break;

			case 25:
				Scores.CharUp(7);
				break;

			case 26:
				Scores.CharDown(7);
				break;

			case 27:
				Scores.CharUp(8);
				break;

			case 28:
				Scores.CharDown(8);
				break;

			case 29:
				Scores.CharUp(9);
				break;

			case 30:
				Scores.CharDown(9);
				break;

			case 31:
				Application.Quit();
				break;

			case 32:
				 Application.OpenURL("https://twitter.com/xiotex");
				 Application.Quit();
				break;

			case 33:
				
				// Get new version of the game

				if(Application.platform == RuntimePlatform.WindowsPlayer)
				{
					 Application.OpenURL(_WinURL);
				}
		
				if(Application.platform == RuntimePlatform.OSXPlayer)
				{
					 Application.OpenURL(_MacURL);
				}

				Application.Quit();
				break;		

			case 201:
				if(_MusicVolume > 0.0f)
				{
					_MusicVolume -= 0.1f;
				}
				else
				{
					_MusicVolume = 0.0f;
				}
				break;

			case 200:
				if(_MusicVolume < 1.0f)
				{
					_MusicVolume += 0.1f;
				}
				else
				{
					_MusicVolume = 1.0f;
				}
				break;	

			case 203:
				if(_SFXVolume > 0.0f)
				{
					_SFXVolume -= 0.1f;
				}
				else
				{
					_SFXVolume = 0.0f;
				}
				break;

			case 202:
				if(_SFXVolume < 1.0f)
				{
					_SFXVolume += 0.1f;
				}
				else
				{
					_SFXVolume = 1.0f;
				}
				break;	


		}
	}

	public void KillPlayer()
	{
		if(_Pause != null)
		{
			_Pause();
		}

		_LevelInfo.Disable();
		_PlayerDead = true;
		_PlayerDeadCountdown = 3.0f;

		ShakeCamera(1.3f,1.6f);

		ClearHUD();
		PlaySample("PLAYERDEATH",1.0f);
		TriggerSound("WIPEOUT",0.5f,1.0f);
	}

	public bool IsPlayerDead()
	{
		return _PlayerDead;
	}

	public void AddToScore(int iScore)
	{
		_Score += iScore;
	}


	public void MoveToState(eGameState eState)
	{
		// shutdown old state...

		if(_CurrentState == eGameState.GAMESTATE_GAME)
		{
			// Shutdown game
			_Player.Disable();
		}

		if(_CurrentState == eGameState.GAMESTATE_TITLE)
		{
			// Shutdown title
		}

		if(_CurrentState == eGameState.GAMESTATE_CREDITS)
		{
			// Shutdown credits
		}

		if(_CurrentState == eGameState.GAMESTATE_HISCORES)
		{
			// Shutdown hiscores
		}

		// Initialise the new state

		if(_CurrentState == eGameState.GAMESTATE_GAME)
		{
			// Setup game
			_Player.Enable();
		}

		if(_CurrentState == eGameState.GAMESTATE_TITLE)
		{
			// Setup title
		}

		if(_CurrentState == eGameState.GAMESTATE_CREDITS)
		{
			// Setup credits
		}

		if(_CurrentState == eGameState.GAMESTATE_HISCORES)
		{
			// Setup hiscores
		}

	}	

	public void ShakeCamera(float fIntensity, float fTime)
	{
		_ShakeTime = fTime;
		_ShakeIntensity = fIntensity;
		_ShakeStartTime = fTime;
	}

	public void DoCameraRotate(float fTimeDelta)
	{
		_XAngle *= 0.98f; // * fTimeDelta;
		_ZAngle *= 0.98f; // * fTimeDelta;
		_CameraAngles.x = 90.0f + _XAngle;
		_CameraAngles.z = _ZAngle;
		_CameraTrans.localEulerAngles = new Vector3(90.0f,0.0f,10.0f);

	}

	private void DoShake(float fTimeDelta)
	{
		_ShakeTime -= fTimeDelta;

		if(_ShakeTime <= 0.0f)
		{
			_CameraTrans.position = _CameraPos;
			_ShakeTime = 0.0f;
			return;
		}

		_ShakePerc = _ShakeTime / _ShakeIntensity;
		float fX = Random.Range(0.0f,_ShakeIntensity * _ShakePerc);
		float fZ = Random.Range(0.0f,_ShakeIntensity * _ShakePerc);

		Vector3 vShake = _CameraPos;
		vShake.x += fX;
		vShake.y += fZ;
		_CameraTrans.position = vShake;
	}

	public void SetScreen(string pName)
	{
		if(_CurrentScreen != null)
		{
			_CurrentScreen.TransitionOut();
		}

		if(_Screens.ContainsKey(pName))
		{
			_CurrentScreen = _Screens[pName];
			_CurrentScreen.TransitionIn();
		}
	}

	public void AddScreen(string pName, ScreenContainer pScreen)
	{
		_Screens.Add(pName,pScreen);
	}

	public void AddSample(string pName, string pSample, bool bPlayAlways = false)
	{
		AudioClip pAudioClip = Resources.Load(pSample) as AudioClip;

		if(pAudioClip != null)
		{
			_Samples.Add(pName,pAudioClip);
			_SamplesTimeStamp.Add(pName,0.0f);
			_SamplePlayAlways.Add(pName,bPlayAlways);
		}
	}

	public void PlaySample(string pName, float fVolume)
	{
		if(_Samples.ContainsKey(pName))
		{
			AudioClip pAudioClip = _Samples[pName];

			if(!_SamplePlayAlways[pName])
			{
				float fTime = _SamplesTimeStamp[pName];
				float fDiff = _Time - fTime;

				if(fDiff < 0.0f)
				{
					fDiff = 1.0f;
				}

				if(fDiff < 0.1f)
				{
					//Debug.Log("Time = " + _Time + " fTime = " + fTime + " Diff = " + fDiff);
					return;
				}

				_SFX.PlayOneShot(pAudioClip,fVolume * _SFXVolume);
				_SamplesTimeStamp[pName] = _Time;

			}
			else
			{
				_SFX.PlayOneShot(pAudioClip,fVolume * _SFXVolume);
			}
		}
	}

	public void TriggerSound(string pName, float fDelay, float fVolume)
	{
		_TriggerSound = true;
		_TriggerSoundName = pName;
		_TriggerSoundDelay = fDelay;
		_TriggerSoundVolume = fVolume;
	}


	public void AddLine(Vector3 vColour, Vector3 vA, Vector3 vB)
	{
		_GLLines.Add(vColour);
		_GLLines.Add(vA);
		_GLLines.Add(vB);

	}
}
