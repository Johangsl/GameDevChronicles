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

public class Atlas 
{
	private Dictionary<string,AtlasItem> _AtlasItems;

	private Font _Font;

	public Atlas()
	{
		_AtlasItems = new Dictionary<string,AtlasItem>();
	}

	public void LoadAtlas(string pAtlasName)
	{		
		Debug.Log("LoadAtlas called - " + pAtlasName);
		
		// Load atlas XML
		TextAsset pXMLText = Resources.Load(pAtlasName) as TextAsset;

		XmlDocument pXML = new XmlDocument();
		pXML.Load(new StringReader(pXMLText.text));

		XmlNodeList pTextureAtlas = pXML.SelectNodes("TextureAtlas");

		//Debug.Log("Node count = " + pTextureAtlas.Count);

		XmlNode pSprites = pTextureAtlas[0];

		//Debug.Log("Child count = " + pSprites.ChildNodes.Count);

		float fTextureWidth = float.Parse(pSprites.Attributes["width"].Value);
		float fTextureHeight = float.Parse(pSprites.Attributes["height"].Value);

		//Debug.Log("Width = " + fTextureWidth + " height = " + fTextureHeight);

		for(int i = 0; i < pSprites.ChildNodes.Count; ++i)
		{
			XmlNode pSprite = pSprites.ChildNodes[i];

			string pPNGName =  pSprite.Attributes["n"].Value + ".png";
			float fX = float.Parse(pSprite.Attributes["x"].Value);
			float fY = float.Parse(pSprite.Attributes["y"].Value);
			float fWidth = float.Parse(pSprite.Attributes["w"].Value);
			float fHeight = float.Parse(pSprite.Attributes["h"].Value);

			_AtlasItems.Add(pPNGName,new AtlasItem(pPNGName,"",fX,fY,fWidth,fHeight, fTextureWidth, fTextureHeight));
			//Debug.Log("name = " + pPNGName + " x = " + fX + " y = " + fY + " w = " + fWidth + " h = " + fHeight);
		}

		_Font = new Font("test");
	}
	
	public AtlasItem GetAtlasItem(string pName)
	{
		if(_AtlasItems.ContainsKey(pName))
		{
			return _AtlasItems[pName];
		}

		Debug.Log("Could not find item: " + pName);
		
		return null;
	}	


}
