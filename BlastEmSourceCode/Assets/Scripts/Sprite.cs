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
using System.Linq;


public class Sprite
{
	static private LinkedList<Sprite>	_Pool = null;
	static private LinkedList<Sprite>	_Active = null;
	static QR _Background;
	static QR _Foreground;
	static QR _UI;

	public float _X;
	public float _Y;
	public float _Z;
	public float _Width;
	public float _Height;
	public bool _Animate;
	public bool _Alive;
	public float _Alpha;
	public float _Angle;
	public bool _KillOnLastFrame;
	public AI _AI;
	public Bullet _Bullet;
	public float _Scale;

	private List<AtlasItem> _Frames;
	private int _CurrentFrame;
	private float _FrameDelay;
	private float _FrameDelayReset;

	private QR _Renderer;

	public Sprite()
	{
		_Frames = new List<AtlasItem>();
		_CurrentFrame = 0;
		_FrameDelay = 0.1f;
		_FrameDelayReset = 0.1f;
		_Y = 0.0f;
		_Alpha = 1.0f;
		_Scale = 1.0f;
	}

	public void UpdateAndRender(float fTimeDelta)
	{
		if(!_Alive)
		{
			return;
		}

		if(_Animate)
		{
			_FrameDelay -= fTimeDelta;

			if(_FrameDelay <= 0.0f)
			{
				_FrameDelay = _FrameDelayReset;
				++_CurrentFrame;

				if(_CurrentFrame >= _Frames.Count)
				{
					_CurrentFrame = 0;

					if(_KillOnLastFrame)
					{
						Kill(this);
					}
				}
			}
		}

		AtlasItem pAtlasItem = null;

		try
		{
			pAtlasItem = _Frames[_CurrentFrame];
		}
		catch(System.Exception e)
		{	
			Debug.Log("Failed to get atlas item for index : " + _CurrentFrame + " AI = " + _AI + " Bullet = " + _Bullet);

			if(_AI != null)
			{
				Debug.Log("AI Type = " + _AI._Type);
			}
		}

		_Renderer._QR.AddQuad(_X,0.2f,_Z,_Width * _Scale,_Height * Director._Instance._Aspect * _Scale,1.0f,1.0f,1.0f,_Alpha,pAtlasItem._U,pAtlasItem._V,pAtlasItem._U1,pAtlasItem._V1,_Angle);
	}

	public void SetLayer(int iLayer)
	{
		switch(iLayer)
		{
			case 0:
				_Renderer = _Background;
				break;

			case 1:
				_Renderer = _Foreground;
				break;

			case 2:
				_Renderer = _UI;
				break;
		}
	}

	public void AddFrame(string pName)
	{
		AtlasItem pAtlasItem = _Renderer._Atlas.GetAtlasItem(pName);

		_Frames.Add(pAtlasItem);
	}

	public void SetFrame(int iFrame)
	{
		_CurrentFrame = iFrame;
	}

	public int GetFrameCount()
	{
		return _Frames.Count;
	}

	public static int GetPoolCount()
	{
		return _Pool.Count;
	}

	public static int GetActiveCount()
	{
		return _Active.Count;
	}

	public static Sprite Spawn(int iLayer, bool bAnimate = true)
	{
		if(_Pool.Count == 0)
		{
			Debug.Log("ERROR: No pooled sprites");
			return null;
		}

		Sprite pObject = _Pool.First.Value;
		pObject._Animate = bAnimate;
		pObject.SetLayer(iLayer);
		_Pool.RemoveFirst();
		_Active.AddLast(pObject);
		pObject._Alive = true;

		// Reset our sprite
		pObject._Frames.Clear();
		pObject._CurrentFrame = 0;
		pObject._FrameDelay = 0.1f;
		pObject._FrameDelayReset = 0.1f;
		pObject._Y = 0.0f;
		pObject._Alpha = 1.0f;
		pObject._KillOnLastFrame = false;
		pObject._Scale = 1.0f;
		return pObject;
	}

	public static void Kill(Sprite pSprite)
	{
		pSprite._Alive = false;
		_Active.Remove(pSprite);
		_Pool.AddLast(pSprite);
		pSprite._Frames.Clear();
	}

	public static void Initialise(int iCount)
	{
		Debug.Log("Sprite initialise");

		if(_Pool == null)
		{
			_Pool = new LinkedList<Sprite>();
			_Active = new LinkedList<Sprite>();

			//GameObject pObject = GameObject.Find("BackgroundQuads");
			//_Background = pObject.GetComponent<QR>();

			GameObject pObject = GameObject.Find("ForegroundQuads");
			_Foreground = pObject.GetComponent<QR>();

			//pObject = GameObject.Find("UIQuads");
			//_UI = pObject.GetComponent<QR>();			
		}

		for(int i = 0; i < iCount; ++i)
		{
			Sprite pSprite = new Sprite();
			_Pool.AddLast(pSprite);
		}
	}

	public static void UpdateAndRenderAll(float fTimeDelta)
	{
    	//Sprite pObject;

		//for(LinkedListNode<Sprite> it = _Active.First; it != null; it = it.Next)
		//{
		//	pObject = it.Value;
		//	pObject.UpdateAndRender(fTimeDelta);
		//}

		Sprite[] pSprites = _Active.OrderBy(Sprite => Sprite._Y).ToArray();

		for(int i = 0; i < pSprites.Length; ++i)
		{
			pSprites[i].UpdateAndRender(fTimeDelta);
		}
	}

	public static void ClearAll()
	{
	    Sprite pObject;

		for(LinkedListNode<Sprite> it = _Active.First; it != null; it = it.Next)
		{
			pObject = it.Value;
		}

		_Active.Clear();
	}		

}
