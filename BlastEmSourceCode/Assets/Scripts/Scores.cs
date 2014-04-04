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

public class Scores : MonoBehaviour
{
    public static List<Score> _Scores = null;

    public static List<Text>   _Index;
    public static List<Text>   _Names;
    public static List<Text>   _ScoreLines;
    public static int _TableStart;
    public static string _Name = null;
    public static Text _NameField; 
 
    void Start()
    {
        _Name = "BLAST EM  ";

        _Index = new List<Text>();
        _Names = new List<Text>();
        _ScoreLines = new List<Text>();

        Text pText = null;

        for(int i = 0; i < 10; ++i)
        {
            pText = new Text();
            pText.Disable();
            _Index.Add(pText);

            pText = new Text();
            pText.Disable();
            _Names.Add(pText);

            pText = new Text();
            pText.Disable();
            _ScoreLines.Add(pText);
        }

        _NameField = new Text();
        _NameField.Disable();
    }

    public void GetScoresTable()
    {
        _TableStart = 0;
        // Score code removed for security
    }

    public void AddScore(string pName, int iScore, int iTimePlayed)
    {
        // Score code removed for security
    }

    public static void ScoreUp()
    {

        if(_TableStart >= 10)
        {
            Debug.Log("Score up");
            _TableStart -= 10;
            SetupScoreScreen();
        }
    }

    public static void ScoreDown()
    {
        if((_TableStart + 10) < _Scores.Count)
        {
            Debug.Log("Score down");
            _TableStart += 10;
            SetupScoreScreen();
       }
    }

    public static void SetupScoreScreen()
    {

        if(_Scores == null)
        {
            return;
        }

        if(_Scores.Count == 0)
        {
            return;
        }

        float fX = -10.0f;
        float fZ = 4.5f;

        int iCount = _TableStart + 10;

        if(iCount > _Scores.Count)
        {
            iCount = _Scores.Count - _TableStart;
        }

        if(iCount > 10)
        {
            iCount = 10;
        }

        Debug.Log("iCount = " + iCount + " Scores count = " + _Scores.Count + " Table start = " + _TableStart);

        Text pText = null;

        for(int i = 0; i < 10; ++i)
        {
            pText = _Index[i];
            pText.Disable();

            pText = _Names[i];
            pText.Disable();

            pText = _ScoreLines[i];
            pText.Disable();
        }

        for(int i = 0; i < iCount; ++i)
        {
            pText = _Index[i];
            pText.Setup();
            pText.SetText(-6.0f,fZ,"" + (_TableStart + i + 1));

            pText = _Names[i];
            pText.Setup();
            pText.SetText(-4.0f,fZ,_Scores[_TableStart + i]._Name);

            pText = _ScoreLines[i];
            pText.Setup();
            pText.SetText(2.0f,fZ,_Scores[_TableStart + i]._Score);

            fZ -= 1.0f;
        }

    }

    public static void ShutdownScoreScreen()
    {
        if(_Index == null)
        {
            return;
        }

        Text pText = null;

        for(int i = 0; i < 10; ++i)
        {
            pText = _Index[i];
            pText.Disable();

            pText = _Names[i];
            pText.Disable();

            pText = _ScoreLines[i];
            pText.Disable();

        }

    }

    public static void SetupNameScreen()
    {
        _NameField.Setup();
        _NameField.SetText(-9.0f,0.0f,_Name,2.0f);
    }

    public static void ShutdownNameScreen()
    {
        _NameField.Disable();
    }

    public static void CharUp(int iIndex)
    {
        if(_Name[iIndex] >= 'A' && _Name[iIndex] < 'Z')
        {
           char pChar = _Name[iIndex];
           ++pChar;
           _Name = _Name.Remove(iIndex,1);
           _Name =  _Name.Insert(iIndex,"" + pChar);                  
        }
        else if(_Name[iIndex] == 'Z')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"0");                  
        }
        else if(_Name[iIndex] >= '0' && _Name[iIndex] < '9')
        {
            char pChar = _Name[iIndex];
           ++pChar;
           _Name = _Name.Remove(iIndex,1);
           _Name =  _Name.Insert(iIndex,"" + pChar);                  
           
        }
        else if(_Name[iIndex] == '9')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex," ");                  
        }
        else if(_Name[iIndex] == ' ')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"@");                  
        }
        else if(_Name[iIndex] == '@')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"A");                  
        }

        _NameField.Disable();
        _NameField.Setup();
        _NameField.SetText(-9.0f,0.0f,_Name,2.0f);

    }

    public static void CharDown(int iIndex)
    {
        if(_Name[iIndex] > 'A' && _Name[iIndex] <= 'Z')
        {
           char pChar = _Name[iIndex];
           --pChar;
           _Name = _Name.Remove(iIndex,1);
           _Name =  _Name.Insert(iIndex,"" + pChar);                  
        }
        else if(_Name[iIndex] == 'A')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"@");                  
        }
        else if(_Name[iIndex] > '0' && _Name[iIndex] <= '9')
        {
            char pChar = _Name[iIndex];
           --pChar;
           _Name = _Name.Remove(iIndex,1);
           _Name =  _Name.Insert(iIndex,"" + pChar);                  
        }
        else if(_Name[iIndex] == '0')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"Z");                  
        }
        else if(_Name[iIndex] == '@')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex," ");                  
        }
        else if(_Name[iIndex] == ' ')
        {
            _Name = _Name.Remove(iIndex,1);
            _Name =  _Name.Insert(iIndex,"9");                  
        }

        _NameField.Disable();
        _NameField.Setup();
        _NameField.SetText(-9.0f,0.0f,_Name,2.0f);

    }
 
}
