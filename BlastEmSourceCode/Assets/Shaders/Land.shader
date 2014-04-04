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

 Shader "Custom/Land" 
 {
	Properties 
	{
		_BaseTex ("Base (RGB)", 2D) = "white" {}
		_MainTex ("Main (RGB)", 2D) = "white" {}
		_SecondTex ("Second (RGB)", 2D) = "white" {}
		_Alpha ("Alpha",float) = 1.0
		_Button ("Button",float) = 0.0		
		_Over ("Over",float) = 0.0						
	}
	
	SubShader 
	{
		Tags { "Queue" = "Background" }

	 	Pass
	 	{
			Cull Off
			Blend Off
		
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
			        
				pos.y *= sin(fTime * 20.0) * 1.5;

				pos.y *= 1.1;
			   					   					   				   				   
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.uv = v.texcoord;
			   	o.uv1 = v.texcoord1;	
			   	o.uv2 = v.texcoord;	
			   	o.uv2.x += _Time * 1.5;		
				o.uv.x += _Time * 2.0;
			   	o.uv1.x += _Time * 4.0;
			   	o.uv.y += _Time * 1.0;
			   	o.uv1.y += _Time * 1.1;

			    o.color.x = (pos.y + 1.5) / 3.0;
			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 texel;
				half4 base = tex2D (_BaseTex, i.uv2);
			    half4 texcol = tex2D (_MainTex, i.uv);
			    half4 texcol2 = tex2D(_SecondTex, i.uv1);

			    texcol.z *= 4.7;
			    texcol2.z *= 3.2;
			    base *= (1.0 - (i.color.x));
			  
			    float fLerp = i.color.x;
			    saturate(fLerp);
				texel = lerp(texcol,texcol2,fLerp);	
				texel *= i.color.x;
				return (base + texel * 1.0) * _Alpha; 
			}
			
			ENDCG	
		}
	}
	FallBack "Diffuse"
}
