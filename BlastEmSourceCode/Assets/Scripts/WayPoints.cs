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

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class WayPoints
{
	public List<Vector3> _Points;
	private List<Vector3> _Direction;
	private int _NumPaths;

	private Vector3 _Colour;

	private int _ClosestIndex;
	private bool _MouseDown;
	private float _MouseX;
	private float _MouseZ;
	private float _OffsetX;
	private float _OffsetZ;

	public string _Name;

	private List<float> _Sizes;
	private float _Magnitude;

	public enum WAYPOINT_RETURN
	{
		WAYPOINT_END,
		WAYPOINT_CONTINUE,
	};

	public WayPoints()
	{
		_Points = new List<Vector3>();
		_Direction = new List<Vector3>();
		_NumPaths = 0;
		_Colour = new Vector3(1.0f,0.0f,0.0f);
		_ClosestIndex = 99999999;

		_Sizes = null;
		_Magnitude = 0.0f;
	}

	public void StartPath(ref Vector3 pPoint, ref int iWayPointIndex)
	{
		if(_NumPaths == 0)
		{
			Debug.Log("No path exists");
		}

		pPoint = _Points[0];
		iWayPointIndex = 0;
	}

	public WAYPOINT_RETURN Walk(ref Vector3 pPoint, ref int iWayPointIndex, ref float fDist, float fSpeed,float fTimeDelta, float fMinDist = 0.1f)
	{
		Vector3 pB = _Points[iWayPointIndex+1];
		float fDistance = Vector3.Distance(pPoint,pB);

		if(fDistance <= fMinDist)
		{
			++iWayPointIndex;

			if(iWayPointIndex == _Points.Count-1)
			{
				return WAYPOINT_RETURN.WAYPOINT_END;
			}
		}

		pPoint += _Direction[iWayPointIndex] * fSpeed * fTimeDelta;

		return WAYPOINT_RETURN.WAYPOINT_CONTINUE;
	}

	public WAYPOINT_RETURN AdvanceWayPoint(ref int iWayPointIndex)
	{

		if(iWayPointIndex == _Points.Count-1)
		{
			return WAYPOINT_RETURN.WAYPOINT_END;
		}

		return WAYPOINT_RETURN.WAYPOINT_CONTINUE;
	}

	public void AddPoint(float fX, float fZ)
	{
		Vector3 pPoint = new Vector3(fX,0.0f,fZ);
		_Points.Add(pPoint);

		_NumPaths = _Points.Count - 1;
		

		BuildDirection();
	}

	public void Copy(WayPoints pWP)
	{
		_Points = new List<Vector3>();

		for(int i = 0; i < pWP._Points.Count; ++i)
		{
			Vector3 vPoint = new Vector3(pWP._Points[i].x,0.0f,pWP._Points[i].z);
			_Points.Add(vPoint);
		}

		_NumPaths = _Points.Count - 1;
		BuildDirection();
	}

	private void BuildDirection()
	{
		Vector3 pA;
		Vector3 pB;
		Vector3 pDirection;
		_Direction.Clear();

		for(int i = 0; i < _NumPaths; ++i)
		{
			pA = _Points[i];
			pB = _Points[i+1];
			pDirection = pB - pA;
			pDirection.Normalize();
			_Direction.Add(pDirection);
		}
	}

	public void MouseDown()
	{
		if(_MouseDown == false)
		{
			if(_ClosestIndex < 99999999)
			{

				if(_MouseX > _Points[_ClosestIndex].x)
				{
					_OffsetX = -(_Points[_ClosestIndex].x - _MouseX);
				}
				else
				{
					_OffsetX = _MouseX - _Points[_ClosestIndex].x;
				}

				if(_MouseZ > _Points[_ClosestIndex].z)
				{
					_OffsetZ = -(_Points[_ClosestIndex].z - _MouseZ);
				}
				else
				{
					_OffsetZ = _MouseZ - _Points[_ClosestIndex].z;
				}


			}
		}

		//Debug.Log("Mouse down");
		_MouseDown = true;
	}

	public void MouseUp()
	{
		//Debug.Log("Mouse up");
		_MouseDown = false;
	}

	public void Mouse(float fX, float fZ)
	{
		_MouseX = fX;
		_MouseZ = fZ;

		Vector3 vMouse = new Vector3(fX,0.0f,fZ);

		_ClosestIndex = 99999999;


		float fDist = 99999999.0f;
		int iIndex;

		for(int i = 0; i <= _NumPaths; ++i)
		{
			float fNewDist = Vector3.Distance(_Points[i],vMouse);

			//Debug.Log("Dist = " + fNewDist);

			if(fNewDist < fDist)
			{
				if(fNewDist < 2.0f)
				{
					_ClosestIndex = i;
					fDist = fNewDist;
				}
			}
		}

		if(_MouseDown)
		{
			if(_ClosestIndex < 99999999)
			{
				Vector3 vPoint = _Points[_ClosestIndex];
				vPoint.x = _MouseX + _OffsetX;
				vPoint.z = _MouseZ + _OffsetZ;
				_Points[_ClosestIndex] = vPoint;

				//Debug.Log("Moving ");
				BuildDirection();
			}
		}
	}

	public void AddPoint()
	{
		if(_ClosestIndex != 99999999)
		{
			Vector3 vPoint = Vector3.zero;

			if(_ClosestIndex != _Points.Count)
			{
				float fDist = Vector3.Distance(_Points[_ClosestIndex],_Points[_ClosestIndex+1]);
				fDist *= 0.5f;
				vPoint = _Points[_ClosestIndex] + _Direction[_ClosestIndex] * fDist;
				_Points.Insert(_ClosestIndex + 1,vPoint);
				_NumPaths = _Points.Count-1;
				BuildDirection();
			}
		}
	}

	public void DeletePoint()
	{
		if(_ClosestIndex != 99999999)
		{
			_Points.RemoveAt(_ClosestIndex);
			_NumPaths = _Points.Count-1;
			BuildDirection();
		}
	}

	public void MarchUp(float fSpeed)
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.z += fSpeed * Time.deltaTime;
			_Points[i] = vPoint;
		}

		BuildDirection();
	}

	public void MarchDown(float fSpeed)
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.z -= fSpeed * Time.deltaTime;
			_Points[i] = vPoint;
		}

		BuildDirection();
	}

	public void MarchLeft(float fSpeed)
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.x -= fSpeed * Time.deltaTime;
			_Points[i] = vPoint;
		}

		BuildDirection();
	}

	public void MarchRight(float fSpeed)
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.x += fSpeed * Time.deltaTime;
			_Points[i] = vPoint;
		}

		BuildDirection();
	}

	public void Reverse()
	{
		List<Vector3> vPoints = new List<Vector3>();

		int iIndex = _Points.Count-1;

		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = new Vector3(_Points[iIndex].x,0.0f,_Points[iIndex].z);
			vPoints.Add(vPoint);
			--iIndex;
		}

		_Points = vPoints;

		BuildDirection();
	}

	public void MirrorX()
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.x *= -1.0f;
			_Points[i] = vPoint;
		}
	}

	public void MirrorZ()
	{
		for(int i = 0; i < _Points.Count; ++i)
		{
			Vector3 vPoint = _Points[i];
			vPoint.z *= -1.0f;
			_Points[i] = vPoint;
		}
	}

	//x' = x cos f - y sin f
	//y' = y cos f + x sin f


	public void DebugDraw()
	{
		//return;
		//Debug.Log("NumPaths = " + _NumPaths);

		// Draw the lines
		for(int i = 0; i < _NumPaths; ++i)
		{
			Director._Instance.AddLine(_Colour,_Points[i],_Points[i+1]);
		}

		Vector3 vVA = Vector3.zero;
		Vector3 vHA = Vector3.zero;
		Vector3 vVB = Vector3.zero;
		Vector3 vHB = Vector3.zero;

		Vector3 vColour = new Vector3(1.0f,1.0f,1.0f);
		Vector3 vSelected = new Vector3(1.0f,0.0f,1.0f);

		// Draw the handles
		for(int i = 0; i <= _NumPaths; ++i)
		{
			vVA.x = _Points[i].x;
			vVA.y = _Points[i].y;
			vVA.z = _Points[i].z - 0.3f;

			vVB.x = _Points[i].x;
			vVB.y = _Points[i].y;
			vVB.z = _Points[i].z + 0.3f;

			vHA.x = _Points[i].x - 0.3f;
			vHA.y = _Points[i].y;
			vHA.z = _Points[i].z;

			vHB.x = _Points[i].x + 0.3f; 
			vHB.y = _Points[i].y;
			vHB.z = _Points[i].z;

			if(i == _ClosestIndex)
			{
				Director._Instance.AddLine(vSelected,vVA,vVB);
				Director._Instance.AddLine(vSelected,vHA,vHB);
			}
			else
			{
				Director._Instance.AddLine(vColour,vVA,vVB);
				Director._Instance.AddLine(vColour,vHA,vHB);
			}

		}

		float fTInc = 1.0f / 120.0f;
		float fT = 0.0f;
		Vector3 vA = Spline(0.0f);
		Vector3 vB = Vector3.zero;
		fT += fTInc;

		for(int i = 0; i < 120; ++i)
		{
			vB = Spline(fT);
			Director._Instance.AddLine(vColour,vA,vB);
			vA = vB;
			fT += fTInc;
		}
	}

	public void DebugDrawSpline(float fOffsetX, float fOffsetZ)
	{
		float fTInc = 1.0f / 120.0f;
		float fT = 0.0f;
		Vector3 vA = Spline(0.0f);
		Vector3 vB = Vector3.zero;
		Vector3 vColour = new Vector3(0.0f,0.8f,0.0f);
		Vector3 vColour1 = new Vector3(1.0f,0.0f,1.0f);


		Vector3 vStart = Spline(0.0f);
		Vector3 vEnd = Spline(1.0f);
		Vector3 vPoint = Vector3.zero;
		vPoint.x = fOffsetX;
		vPoint.z = fOffsetZ;
	
		Director._Instance.AddLine(vColour1,vStart,vPoint);
		Director._Instance.AddLine(vColour1,vEnd,vPoint);

		fT += fTInc;

		for(int i = 0; i < 120; ++i)
		{
			vB = Spline(fT);
			Director._Instance.AddLine(vColour,vA,vB);
			vA = vB;
			fT += fTInc;
		}		
	}

	public Vector3 Spline(float fT) 
	{
		// This walking of a set of spline points was something I learned from somebody else's code, I can't remember who but thanks whoever you are
		// Let me know and I'll add you in as a credit. Your implementation was better than mine.
		int iNumSections = _Points.Count - 3;
		int iCurrPt = Mathf.Min(Mathf.FloorToInt(fT * (float) iNumSections), iNumSections - 1);
		float fU = fT * (float) iNumSections - (float) iCurrPt;
				
		Vector3 vA = _Points[iCurrPt];
		Vector3 vB = _Points[iCurrPt + 1];
		Vector3 vC = _Points[iCurrPt + 2];
		Vector3 vD = _Points[iCurrPt + 3];

		// Scale Z
		vA.z *= 0.95f;
		vB.z *= 0.95f;
		vC.z *= 0.95f;
		vD.z *= 0.95f;
		
		Vector3 vReturn = 0.5f * ((-vA + 3.0f * vB - 3.0f * vC + vD) * (fU * fU * fU) + (2.0f * vA - 5.0f * vB + 4.0f * vC - vD) * (fU * fU) + (-vA + vC) * fU + 2.0f * vB);
		return vReturn;
	}

	public void WriteXML(XmlTextWriter pXML)
	{
		//Debug.Log("Saving path to XML");
		
		for(int i = 0; i < _Points.Count; ++i)
		{
			pXML.WriteStartElement("point");
			pXML.WriteWhitespace("\n");
			pXML.WriteElementString("x","" + _Points[i].x);
			pXML.WriteElementString("z","" + _Points[i].z);
			pXML.WriteWhitespace("\n");
			pXML.WriteEndElement();
			pXML.WriteWhitespace("\n");
		}
	}
}
