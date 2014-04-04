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
 Shader "Custom/PlayButtonFire" 
 {
	Properties 
	{
		_BaseTex ("Base (RGB)", 2D) = "white" {}
		_MainTex ("Main (RGB)", 2D) = "white" {}
		_SecondTex ("Second (RGB)", 2D) = "white" {}
		_Alpha("Alpha",float) = 1.0
		_Button ("Button",float) = 0.0	
		_Over ("Over",float) = 0.0					
	}
	
	SubShader 
	{
		Tags { "Queue" = "Transparent" }

	 	Pass
	 	{
			Cull Off
			//Blend SrcAlpha OneMinusSrcAlpha
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
			sampler2D _MainTex;
			sampler2D _SecondTex;

			struct v2f 
			{
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    float2  uv1 : TEXCOORD1;
			    float2  uv2 : TEXCOORD2;
			    float4 color : COLOR;
			};
			
			struct appdata 
			{
    			float4 vertex : POSITION;
  			    float2  texcoord : TEXCOORD0;
  			    float2  texcoord1 : TEXCOORD1;
  				float4 color : COLOR;
 			};
			
			float4 _BaseTex_ST;
			float4 _MainTex_ST;
			float4 _SecondTex_ST;
			float _Alpha;
			float _Button;
			float _Over;

			
			v2f vert (appdata v)
			{
			    v2f o;
			    
			    float fTime = _Time + (v.color.x * 1000.0);
			    float4 pos = v.vertex;
			   					   					   				   				   
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.uv = v.texcoord;
			   	o.uv1 = v.texcoord1;	
			   	o.uv2 = v.texcoord;	

				o.uv.x += _Time * 6.0;
			   	o.uv.y -= _Time * 4.0;

			   	o.uv1.x -= _Time * 6.0;
			   	o.uv1.y -= _Time * 3.1;

			    o.color.x = (pos.y + 1.5) / 3.0;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 texel;

				half2 uv2 = i.uv2;

				half2 uv = i.uv;

				half2 uv1 = i.uv1;

				half4 base = tex2D (_BaseTex, uv2);
			    half4 texcol = tex2D (_MainTex, uv);
			    half4 texcol2 = tex2D(_SecondTex, uv1);

			   // texcol.z *= 4.7;
			    //texcol2.z *= 3.2;
			  	texel = texcol * texcol2;
			  	half4 vCol = half4(1.0,0.0,0.0,0.0);
			  	vCol *= texcol;
			  	vCol *= base;
			  	vCol *= 2.0;


				return vCol * _Alpha; 
			}
			
			ENDCG	
		}
	}
	FallBack "Diffuse"
}
