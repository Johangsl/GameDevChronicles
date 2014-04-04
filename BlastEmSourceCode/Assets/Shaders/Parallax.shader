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

 Shader "Custom/Parallax" 
 {
	Properties 
	{
		_L1Tex ("L1 (RGB)", 2D) = "white" {}
		_L2Tex ("L2 (RGB)", 2D) = "white" {}
		_L3Tex ("L3 (RGB)", 2D) = "white" {}
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
			
			sampler2D _L1Tex;
			sampler2D _L2Tex;
			sampler2D _L3Tex;

			struct v2f 
			{
			    float4  pos : SV_POSITION;
			    float2  uv1 : TEXCOORD0;
			    float2  uv2 : TEXCOORD1;
			    float2  uv3 : TEXCOORD2;
			};
			
			float4 _L1Tex_ST;
			float4 _L2Tex_ST;
			float4 _L3Tex_ST;

			v2f vert (appdata_full v)
			{
			    v2f o;
			    float4 pos = v.vertex;
			    o.pos = mul (UNITY_MATRIX_MVP, pos);
			    o.uv1 = TRANSFORM_TEX (v.texcoord, _L1Tex);
			    o.uv2 = TRANSFORM_TEX (v.texcoord, _L2Tex);
			    o.uv3 = TRANSFORM_TEX (v.texcoord, _L3Tex);

			    o.uv1.y += 0.3 * _Time;
			    o.uv2.y += 4.0 * _Time;
			    o.uv3.y += 6.0 * _Time;

			    return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 FinalCol;
			    half4 texcol1 = tex2D (_L1Tex, i.uv1);
			    half4 texcol2 = tex2D (_L2Tex, i.uv2);
			    half4 texcol3 = tex2D (_L3Tex, i.uv3);

			    FinalCol = texcol1;
			    FinalCol.xyz *= texcol1.w;

			   	half4 test = half4(1.0,1.0,1.0,1.0);
			   	half4 mask = step(test,texcol2);
			   	FinalCol *= mask;
			   	texcol2.xyz *= texcol2.w;
			    FinalCol += texcol2;

			   	mask = step(test,texcol3);
			   	FinalCol *= mask;
			   	texcol3.xyz *= texcol3.w;
			    FinalCol += texcol3;

		
				texcol1 *= 0.6;
			    return texcol1;
			}
			
			ENDCG	
		}
	}

	FallBack "VertexLit"
}
