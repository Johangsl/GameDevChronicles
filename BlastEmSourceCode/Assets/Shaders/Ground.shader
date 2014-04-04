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

 Shader "Custom/Ground" 
 {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader 
	{
		Tags { "Queue" = "Geometry"  }
		
	 	Pass
	 	{

			Cull off
			Blend off
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members colour)
#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;

			struct v2f 
			{
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    float2  uv2 : TEXCOORD1;

			};
			
			float4 _MainTex_ST;

			v2f vert (appdata_full v)
			{
			    v2f o;
			    float4 pos = v.vertex;
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
			    o.uv.y += 0.3 * _Time;
			    o.uv2 = TRANSFORM_TEX (v.texcoord1, _MainTex);
			    o.uv2.y += 0.35 * _Time;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
			    half4 texcol = tex2D (_MainTex, i.uv);
			    half4 texcol2 = tex2D (_MainTex, i.uv2);
			    return texcol + texcol2;
			}
			
			ENDCG	
		}
	}

	FallBack "VertexLit"
}
