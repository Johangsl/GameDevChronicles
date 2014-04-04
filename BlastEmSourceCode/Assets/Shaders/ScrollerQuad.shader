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

 Shader "Custom/ScrollerQuad" 
 {
	Properties 
	{
		_BaseTex ("Base (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		_Alpha ("Alpha",float) = 1.0
		_Button ("Button",float) = 0.0		
		_Over ("Over",float) = 0.0				
	}
	
	SubShader 
	{
		Tags { "Queue" = "Transparent+1" }

	 	Pass
	 	{
			Cull Off
			Blend One One
			zwrite off
			ztest always
		
			CGPROGRAM
// Upgrade NOTE: excluded shader from Xbox360; has structs without semantics (struct v2f members colour)
#pragma exclude_renderers xbox360
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			
			#include "UnityCG.cginc"
			
			sampler2D _BaseTex;
			sampler2D _MaskTex;

			struct v2f 
			{
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			};
			
			struct appdata 
			{
    			float4 vertex : POSITION;
  			    float2  texcoord : TEXCOORD0;
 			};
			
			float4 _BaseTex_ST;
			float4 _MaskTex_ST;


			float _Alpha;
			float _Button;
			float _Over;
			
			v2f vert (appdata v)
			{
			    v2f o;
			    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = v.texcoord;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half2 uv = i.uv;
				uv.y += sin((_Time * 20.0) - (i.uv.x * 10.0)) * 0.08;
				half4 base = tex2D (_BaseTex, uv);
				half4 mask = tex2D (_MaskTex, uv);
				return (base * mask) * _Alpha; 
			}
			
			ENDCG	
		}
	}
	FallBack "Diffuse"
}
