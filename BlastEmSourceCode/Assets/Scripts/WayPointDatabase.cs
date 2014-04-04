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

public class WayPointDatabase : MonoBehaviour 
{
	private List<WayPoints> _WayPoints;
	public int _CurrentIndex;
	
	public WayPoints _CurrentDebug;
	public string _Name;
	public bool _Debug;

	void Start () 
	{
		_WayPoints = new List<WayPoints>();
		_CurrentDebug = null;
		_CurrentIndex = 0;
		Load();
	}
	
	public void DebugDraw()
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.DebugDraw();
		}
	}

	public void AddWayPoints(WayPoints pWaypoints)
	{
		_WayPoints.Add(pWaypoints);
	}

	public void AddWayPointsInsert(WayPoints pWaypoints)
	{
		_WayPoints.Insert(_CurrentIndex + 1, pWaypoints);
	}

	public WayPoints GetWayPoints(int iIndex)
	{
		if(_WayPoints.Count == 0)
		{
			return null;
		}
		
		if(iIndex > _WayPoints.Count)
		{
			return null;
		}

		return _WayPoints[iIndex];
	}

	public void SetDebug(int iIndex)
	{
		if(_WayPoints.Count == 0)
		{
			return;
		}

		if(iIndex > _WayPoints.Count)
		{
			return;
		}

		_CurrentDebug = _WayPoints[iIndex];
	}

	public void Mouse(float fX, float fZ)
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.Mouse(fX,fZ);
		}
	}

	public void MouseDown()
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.MouseDown();
		}
	}

	public void MouseUp()
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.MouseUp();
		}
	}

	public void AddPoint()
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.AddPoint();
		}
	}

	public void DeletePoint()
	{
		if(_CurrentDebug != null)
		{
			_CurrentDebug.DeletePoint();
		}
	}

	public void Next()
	{
		if(_CurrentIndex < _WayPoints.Count-1)
		{
			++_CurrentIndex;
			_CurrentDebug = _WayPoints[_CurrentIndex];
		}
	}

	public void New()
	{
		_CurrentIndex = _WayPoints.Count;
		WayPoints pWP = new WayPoints();
		pWP.AddPoint(28.0f,0.0f);
		pWP.AddPoint(26.0f,0.0f);
		pWP.AddPoint(-26.0f,0.0f);
		pWP.AddPoint(-28.0f,0.0f);
		AddWayPoints(pWP);
		_CurrentDebug = _WayPoints[_CurrentIndex];
	}

	public void NewInsert()
	{
		WayPoints pWP = new WayPoints();
		pWP.AddPoint(28.0f,0.0f);
		pWP.AddPoint(26.0f,0.0f);
		pWP.AddPoint(-26.0f,0.0f);
		pWP.AddPoint(-28.0f,0.0f);
		AddWayPointsInsert(pWP);
		_CurrentDebug = _WayPoints[_CurrentIndex+1];
		_CurrentIndex++;

	}

	public void ErasePath()
	{
		if(_CurrentIndex == 0)
		{
			_WayPoints = new List<WayPoints>();
			_CurrentDebug = null;
			_CurrentIndex = 0;
			WayPoints pWP = new WayPoints();
			pWP.AddPoint(28.0f,0.0f);
			pWP.AddPoint(26.0f,0.0f);
			pWP.AddPoint(-26.0f,0.0f);
			pWP.AddPoint(-28.0f,0.0f);
			AddWayPoints(pWP);
			_CurrentDebug = _WayPoints[_CurrentIndex];
			return;
		}

		_WayPoints.RemoveAt(_CurrentIndex);

		if(_CurrentIndex > _WayPoints.Count-1)
		{
			_CurrentIndex = _WayPoints.Count-1;
		}

		_CurrentDebug = _WayPoints[_CurrentIndex];
	}

	public void Previous()
	{
		if(_CurrentIndex != 0)
		{
			--_CurrentIndex;
			_CurrentDebug = _WayPoints[_CurrentIndex];
		}
	}

	public void Load()
	{

		_WayPoints = new List<WayPoints>();
		_CurrentDebug = null;

		TextAsset pXMLText = Resources.Load("Paths") as TextAsset;

		//Debug.Log("XML = " + pXMLText.text);

		XmlDocument pXML = new XmlDocument();
		pXML.Load(new StringReader(pXMLText.text));
		
		string pName = "path";
		XmlNodeList pPaths = pXML.GetElementsByTagName(pName);

		//Debug.Log("Number of paths = " + pPaths.Count);

		for(int iNode = 0; iNode < pPaths.Count; ++iNode)
		{
			//Debug.Log("Node xml = " + pPaths[iNode].InnerXml);

			XmlNode pPath = pPaths[iNode];

			//Debug.Log("pPath XML = " + pPath.InnerXml);

			WayPoints pWayPoints = new WayPoints();


			for(int i = 0; i < pPath.ChildNodes.Count; ++i)
			{
				//Debug.Log("Name = " + pPath.ChildNodes[i].InnerXml);

				XmlNode pPoint = pPath.ChildNodes[i];

				XmlNode pX = pPoint.FirstChild;
				XmlNode pZ = pPoint.LastChild;

				//Debug.Log("X = " + pX.InnerText + " Z = " + pZ.InnerText);

				pWayPoints.AddPoint(float.Parse(pX.InnerText),float.Parse(pZ.InnerText));
			}

			AddWayPoints(pWayPoints);
		}
	}

	public void Save()
	{
		FileInfo pFileInfo = new FileInfo(Application.dataPath + "/Resources/paths.txt");

		// Tonk the file if it already exists
		if(pFileInfo.Exists)
		{
			pFileInfo.Delete();
		}

		// Create the stream writer and dump the file out to the resources directory
		StreamWriter pStream;
		pStream = pFileInfo.CreateText();

		StringWriter pStr = new StringWriter();

		XmlTextWriter pXML = new XmlTextWriter(pStr);
		pXML.WriteStartDocument();
		pXML.WriteWhitespace("\n");
		pXML.WriteStartElement("paths");
		pXML.WriteWhitespace("\n");
		pXML.WriteWhitespace("\n");

		for(int i = 0; i < _WayPoints.Count; ++i)
		{
			pXML.WriteStartElement("path");
			pXML.WriteWhitespace("\n");
			_WayPoints[i].WriteXML(pXML);
			pXML.WriteEndElement();
			pXML.WriteWhitespace("\n");
			pXML.WriteWhitespace("\n");
		}

		pXML.WriteEndElement();
		pXML.WriteEndDocument();

		pStream.Write(pStr.ToString());
		pStream.Close();
	}

}
