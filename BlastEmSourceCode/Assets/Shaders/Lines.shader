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

 Shader "Custom/Lines" 
 {
	Properties 
	{
	}
	
	SubShader 
	{
		Tags { "Queue" = "Overlay+3"  }
		
	 	Pass
	 	{
			Cull Off
			zwrite off
			ztest always
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members colour)
#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			

			struct v2f 
			{
			    float4  pos : SV_POSITION;
			    float4  color : COLOR;
			};
			
			
			v2f vert (appdata_full v)
			{
			    v2f o;
			    float4 pos = v.vertex;
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
