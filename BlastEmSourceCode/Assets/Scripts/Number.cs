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

public class Number 
{
	public int _Number;
	public float _Z;

	private string _NumberString;
	private ArrayList _NumberArray;
	private int _NumberSize;
	private float _NumberWidth;
	private float _Cursor;

	public Number()
	{
		_NumberArray = new ArrayList();

		for(int i = 0; i < 20; ++i)
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
			pSprite._Z = _Z;
			pSprite._Y = 0.3f;
			pSprite._Width = 1.0f;
			pSprite._Height = 1.0f;
			_NumberArray.Add(pSprite);
		}

	}

	public void SetNumber(int iNumber)
	{
		_Number = iNumber;
		Sprite pSprite = null;

		_NumberString = "" + _Number;
		_Cursor = 0.0f;

		for(int i = 0; i < _NumberArray.Count; ++i)
		{
			pSprite = _NumberArray[i] as Sprite;
			pSprite._Alive = false;
			pSprite._X = 0.0f;
			pSprite._Z = _Z;
		}

		_NumberSize = _NumberString.Length;

		for(int i = 0; i < _NumberSize; ++i)
		{
			pSprite = _NumberArray[i] as Sprite;
			pSprite._Alive = true;
			pSprite._X = _Cursor;

			switch(_NumberString[i])
			{
				case '0':
					pSprite.SetFrame(0);
					_Cursor += 1.0f;
					break;
				
				case '1':
					pSprite.SetFrame(1);
					_Cursor += 1.0f;
					break;
				
				case '2':
					pSprite.SetFrame(2);
					_Cursor += 1.0f;
					break;
				
				case '3':
					pSprite.SetFrame(3);
					_Cursor += 1.0f;
					break;
				
				case '4':
					pSprite.SetFrame(4);
					_Cursor += 1.0f;
					break;
				
				case '5':
					pSprite.SetFrame(5);
					_Cursor += 1.0f;
					break;
				
				case '6':
					pSprite.SetFrame(6);
					_Cursor += 1.0f;
					break;
				
				case '7':
					pSprite.SetFrame(7);
					_Cursor += 1.0f;
					break;
				
				case '8':
					pSprite.SetFrame(8);
					_Cursor += 1.0f;
					break;
				
				case '9':
					pSprite.SetFrame(9);
					_Cursor += 1.0f;
					break;
			}

			pSprite._X = _Cursor;
		}

		_NumberWidth = _Cursor;

		float fHalfWidth = _NumberWidth * 1.0f;

		if(_NumberString.Length > 1 )
		{
			fHalfWidth -= 0.5f;
		}

		for(int i = 0; i < _NumberArray.Count; ++i)
		{
			pSprite = _NumberArray[i] as Sprite;
			pSprite._X -= fHalfWidth;
		}
	}

	public void Disable()
	{
		Sprite pSprite;
		
		for(int i = 0; i < _NumberArray.Count; ++i)
		{
			pSprite = _NumberArray[i] as Sprite;
			pSprite._Alive = false;
		}
	}
}


