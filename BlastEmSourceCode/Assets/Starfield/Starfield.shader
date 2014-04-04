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

 Shader "Custom/Starfield" 
 {
	Properties 
	{
		_Speed ("Speed",float) = 1.0
		_Warp ("Warp",float) = 0.1
		_SpeedJitter ("Speed jitter",float) = 10.0
		_UpperZ ("Upper Z",float) = -2.0
		_LowerZ ("LowerZ",float) = 2.0
	}
	
	SubShader 
	{
		Tags { "Queue" = "Background+1"  }
		
	 	Pass
	 	{

			//LOD 200
			Cull Off
			//zwrite off
			ztest always
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members colour)
#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			

			struct v2f 
			{
			    float4 pos : SV_POSITION;
			    float4 color : COLOR;
			};
			
			float _Speed;
			float _Warp;
			float _SpeedJitter;
			float _UpperZ;
			float _LowerZ;
			float _TwinkleJitter;

			
			v2f vert (appdata_full v)
			{
			    v2f o;
			    float4 pos = v.vertex;
			    float fRangeJitter = v.normal.x;
			    fRangeJitter *= 1000.0;
			    float fSpeed = 1.0 * _Warp;
			    float fSpeedJitter = v.normal.y * _SpeedJitter;
			    float fMovementTime = (_Time.x + fRangeJitter) * (_Speed + fSpeedJitter);
			    float fMoveRange = fmod(fMovementTime,1);
			    pos.x += lerp( _UpperZ,_LowerZ,fMoveRange);
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.color = v.color;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
			    return i.color;
			}
			
			ENDCG	
		}
	}
	FallBack "VertexLit"
}
