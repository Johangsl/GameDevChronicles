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

public class Ground : MonoBehaviour 
{
	private Mesh				_Mesh;
	private Vector3[]			_Vertices;
	private Vector2[] 			_UV;
	private Vector2[]			_UV1;
	private Vector2[]			_UV2;
	private int[] 				_Triangles;
	private MeshCollider		_Collider;

	public float 				_LowerX;
	public float				_HigherX;
	public float				_LowerZ;
	public float				_HigherZ;

	public static Ground		_Instance = null;

	private LineRenderer		_Lines;
	public bool 				_Debug;

	private Vector3 vTR;
	private Vector3 vTL;
	private Vector3 vBR;
	private Vector3 vBL;
	private Vector3 _Colour;


	void Awake() 
	{
		_Instance = this;
		_Mesh = this.GetComponent<MeshFilter>().mesh;
		_Vertices = new Vector3[4];
		_UV = new Vector2[4];
		_UV1 = new Vector2[4];
		_UV2 = new Vector2[4];
		_Triangles = new int[6];

	 	vTR = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 20.0f));
	    vTL = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 20.0f));
	    vBR = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 20.0f));
	    vBL = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 20.0f));

	    Vector3 vTR1 = vTR;
	    Vector3 vTL1 = vTL;
	    Vector3 vBR1 = vBR;
	    Vector3 vBL1 = vBL;

	    vTR1.x *= 1.5f;
	    vTL1.x *= 1.5f;
	    vBL1.x *= 1.5f;
	    vBR1.x *= 1.5f;

	    vTR1.z *= 1.5f;
	    vTL1.z *= 1.5f;
	    vBL1.z *= 1.5f;
	    vBR1.z *= 1.5f;	    

	    _LowerX = vTR.x;
	    _HigherX = vTL.x;
	    _LowerZ = vTR.z;
	    _HigherZ = vBR.z;


	    vTR *= 1.2f;
	    vTL *= 1.2f;
	    vBR *= 1.2f;
	    vBL *= 1.2f;

	    _Mesh.Clear();
	    _Vertices[0] = vTL1;
	    _Vertices[1] = vTR1;
	    _Vertices[2] = vBR1;
	    _Vertices[3] = vBL1;

	    int iVariation = Random.Range(0,6);
	    float fOffset = (float)(iVariation);
	    fOffset *= 0.156f;

	    _UV[0] = new Vector2(0.0f,0.0f);
	    _UV[1] = new Vector2(0.0f,0.156f);
	    _UV[2] = new Vector2(0.156f,0.156f);
	    _UV[3] = new Vector2(0.156f,0.0f);

	    _UV[0].x += fOffset;
	    _UV[0].y += fOffset;
	    _UV[1].x += fOffset;
	    _UV[1].y += fOffset;
	    _UV[2].x += fOffset;
	    _UV[2].y += fOffset;
	    _UV[3].x += fOffset;
	    _UV[3].y += fOffset;

	    iVariation = Random.Range(0,6);
	    fOffset = (float)(iVariation);
	    fOffset *= 0.156f;


	    _UV1[0] = new Vector2(0.0f,0.0f);
	    _UV1[1] = new Vector2(0.0f,0.156f);
	    _UV1[2] = new Vector2(0.156f,0.156f);
	    _UV1[3] = new Vector2(0.156f,0.0f);

	    _UV1[0].x += fOffset;
	    _UV1[0].y += fOffset;
	    _UV1[1].x += fOffset;
	    _UV1[1].y += fOffset;
	    _UV1[2].x += fOffset;
	    _UV1[2].y += fOffset;
	    _UV1[3].x += fOffset;
	    _UV1[3].y += fOffset;


	    _Triangles[0] = 0;
	    _Triangles[1] = 1;
	    _Triangles[2] = 2;

	    _Triangles[3] = 0;
	    _Triangles[4] = 2;
	    _Triangles[5] = 3;

	    _Mesh.vertices = _Vertices;
	    _Mesh.uv = _UV;
	    _Mesh.uv2 = _UV1;
	    //_Mesh.uv2 = _UV1;
	    _Mesh.triangles = _Triangles;
	    _Mesh.RecalculateBounds();

	    _Collider = this.GetComponent<MeshCollider>();
	    _Collider.sharedMesh = _Mesh;

	    _Colour = new Vector3(1.0f,0.0f,0.0f);
	}

	void Update()
	{
	    //if(!Director._Instance._Debug)
	    //{
	    	Director._Instance.AddLine(_Colour,vTL,vTR);
	    	Director._Instance.AddLine(_Colour,vTR,vBR);
	    	Director._Instance.AddLine(_Colour,vBR,vBL);
	    	Director._Instance.AddLine(_Colour,vBL,vTL);
	    //}
	}


}
