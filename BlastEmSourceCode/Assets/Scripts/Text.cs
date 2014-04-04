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

public class Text 
{

	public string _Text;
	public float _X;
	public float _Z;

	private string _NumberString;
	private List<Sprite> _Sprites;
	private int _Size;
	private float _Width;
	private float _Cursor;
	private Dictionary<char,int> _Frames;

	public Text()
	{
		_Sprites = null;
	}

	public void SetText(float fX, float fZ, string pText, float fSize = 0.5f)
	{
		Sprite pSprite = null;
		_Text = pText;
		_Size = _Text.Length;

		if(_Size > 10)
		{
			Debug.Log("Text string too large : " + pText);
			return;
		}

		for(int i = 0; i < 10; ++i)
		{
			pSprite = _Sprites[i];
			pSprite._Alive = false;
		}

		_X = fX;
		_Z = fZ;
		_Cursor = fX;

		for(int i = 0; i < _Size; ++i)
		{
			if(_Frames.ContainsKey(_Text[i]))
			{
				pSprite = _Sprites[i];
				pSprite._Alive = true;
				pSprite.SetFrame(_Frames[_Text[i]]);
				pSprite._X = _Cursor;
				pSprite._Z = _Z;
				pSprite._Width = fSize;
				pSprite._Height = fSize;
				_Cursor += fSize;
			}
			else
			{
				Debug.Log("Encountered an unknown character : " + _Text[i]);
				return;
			}
		}
	}

	public void Setup()
	{
		if(_Sprites != null)
		{
			Disable();
		}

		_Sprites = new List<Sprite>();

		for(int i = 0; i < 10; ++i)
		{
			Sprite pSprite = Sprite.Spawn(1);
			pSprite._Animate = false;
			pSprite.AddFrame("0.png");			
			pSprite.AddFrame("1.png");
			pSprite.AddFrame("2.png");
			pSprite.AddFrame("3.png");
			pSprite.AddFrame("4.png");
			pSprite.AddFrame("5.png");
			pSprite.AddFrame("6.png");
			pSprite.AddFrame("7.png");
			pSprite.AddFrame("8.png");
			pSprite.AddFrame("9.png");
			pSprite.AddFrame("@.png");
			pSprite.AddFrame("A.png");
			pSprite.AddFrame("B.png");
			pSprite.AddFrame("C.png");
			pSprite.AddFrame("D.png");
			pSprite.AddFrame("E.png");
			pSprite.AddFrame("F.png");
			pSprite.AddFrame("G.png");
			pSprite.AddFrame("H.png");
			pSprite.AddFrame("I.png");
			pSprite.AddFrame("J.png");
			pSprite.AddFrame("K.png");
			pSprite.AddFrame("L.png");
			pSprite.AddFrame("M.png");
			pSprite.AddFrame("N.png");
			pSprite.AddFrame("O.png");
			pSprite.AddFrame("P.png");
			pSprite.AddFrame("Q.png");
			pSprite.AddFrame("R.png");
			pSprite.AddFrame("S.png");
			pSprite.AddFrame("T.png");
			pSprite.AddFrame("U.png");
			pSprite.AddFrame("V.png");
			pSprite.AddFrame("W.png");
			pSprite.AddFrame("X.png");
			pSprite.AddFrame("Y.png");
			pSprite.AddFrame("Z.png");
			pSprite.AddFrame("space.png");
			pSprite._Z = _Z;
			pSprite._Y = 0.3f;
			pSprite._Width = 0.5f;
			pSprite._Height = 0.5f;
			_Sprites.Add(pSprite);
		}

		// Hacky way of doing a quick lookup
		_Frames = new Dictionary<char,int>();
		_Frames.Add('0',0);
		_Frames.Add('1',1);
		_Frames.Add('2',2);
		_Frames.Add('3',3);
		_Frames.Add('4',4);
		_Frames.Add('5',5);
		_Frames.Add('6',6);
		_Frames.Add('7',7);
		_Frames.Add('8',8);
		_Frames.Add('9',9);
		_Frames.Add('@',10);
		_Frames.Add('A',11);
		_Frames.Add('B',12);
		_Frames.Add('C',13);
		_Frames.Add('D',14);
		_Frames.Add('E',15);
		_Frames.Add('F',16);
		_Frames.Add('G',17);
		_Frames.Add('H',18);
		_Frames.Add('I',19);
		_Frames.Add('J',20);
		_Frames.Add('K',21);
		_Frames.Add('L',22);
		_Frames.Add('M',23);
		_Frames.Add('N',24);
		_Frames.Add('O',25);
		_Frames.Add('P',26);
		_Frames.Add('Q',27);
		_Frames.Add('R',28);
		_Frames.Add('S',29);
		_Frames.Add('T',30);
		_Frames.Add('U',31);
		_Frames.Add('V',32);
		_Frames.Add('W',33);
		_Frames.Add('X',34);
		_Frames.Add('Y',35);
		_Frames.Add('Z',36);
		_Frames.Add(' ',37);		

	}

	public void Disable()
	{

		if(_Sprites == null)
		{
			return;
		}
		
		Sprite pSprite;
		
		for(int i = 0; i < _Sprites.Count; ++i)
		{
			pSprite = _Sprites[i];
			pSprite._Alive = false;
			Sprite.Kill(pSprite);
		}

		_Sprites = null;
	}
}


