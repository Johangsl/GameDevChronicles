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

public class ScreenContainer 
{
	private List<Image> _Images;
	private List<Button> _Buttons;
	private List<CompoundGameObject> _Compounds;
	private List<Camera> _Cameras;
	private List<UIGO> _UIGO;

	public ScreenContainer()
	{
		_Images = new List<Image>();
		_Buttons = new List<Button>();
		_Compounds = new List<CompoundGameObject>();
		_UIGO = new List<UIGO>();
	}

	public void AddButton(string pName)
	{
		Button pButton = GameObject.Find(pName).GetComponent<Button>();

		if(pButton != null)
		{
			_Buttons.Add(pButton);
		}
		else
		{
			Debug.Log("Could not find button named: " + pName);
		}
	}

	public void AddImage(string pName)
	{
		Image pImage = GameObject.Find(pName).GetComponent<Image>();

		if(pImage != null)
		{
			_Images.Add(pImage);
		}
		else
		{
			Debug.Log("Could not find image named: " + pName);
		}
	}

	public void AddCompound(string pName)
	{
		CompoundGameObject pCompound = GameObject.Find(pName).GetComponent<CompoundGameObject>();

		if(pCompound != null)
		{
			_Compounds.Add(pCompound);
		}
		else
		{
			Debug.Log("Could not find compound named: " + pName);
		}
	}

	public void AddUIGO(string pName)
	{
		UIGO pUIGO = GameObject.Find(pName).GetComponent<UIGO>();

		if(pUIGO != null)
		{
			_UIGO.Add(pUIGO);
		}
		else
		{
			Debug.Log("Could not find AddUIGO named: " + pName);
		}
	}	

	public void TransitionIn()
	{
		for(int i = 0; i < _Images.Count; ++i)
		{
			Image pImage = _Images[i];
			pImage.Enable();
		}

		for(int i = 0; i < _Buttons.Count; ++i)
		{
			Button pButton = _Buttons[i];
			pButton.Enable();
		}

		for(int i = 0; i < _Compounds.Count; ++i)
		{
			CompoundGameObject pCompound = _Compounds[i];
			pCompound.Enable();
		}

		for(int i = 0; i < _UIGO.Count; ++i)
		{
			UIGO pUIGO = _UIGO[i];
			pUIGO.Enable();
		}		
	}

	public void TransitionOut()
	{
		for(int i = 0; i < _Images.Count; ++i)
		{
			Image pImage = _Images[i];
			pImage.Disable();
		}

		for(int i = 0; i < _Buttons.Count; ++i)
		{
			Button pButton = _Buttons[i];
			pButton.Disable();
		}

		for(int i = 0; i < _Compounds.Count; ++i)
		{
			CompoundGameObject pCompound = _Compounds[i];
			pCompound.Disable();
		}

		for(int i = 0; i < _UIGO.Count; ++i)
		{
			UIGO pUIGO = _UIGO[i];
			pUIGO.Disable();
		}		



	}
}
