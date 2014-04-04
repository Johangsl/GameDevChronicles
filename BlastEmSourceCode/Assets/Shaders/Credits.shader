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

 Shader "Custom/Credits" 
 {
	Properties 
	{
		_BaseTex ("Base (RGB)", 2D) = "white" {}
		_Alpha ("Alpha",float) = 1.0
	}
	
	SubShader 
	{
		Tags { "Queue" = "Transparent" }

	 	Pass
	 	{
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend One One
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
			float _Alpha;

			
			v2f vert (appdata v)
			{
			    v2f o;
			    
			    float4 pos = v.vertex;		        
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.uv = v.texcoord;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half2 uv = i.uv;
				half4 base = tex2D (_BaseTex, uv);
				return base * _Alpha; 
			}
			
			ENDCG	
		}
	}
	FallBack "Diffuse"
}
