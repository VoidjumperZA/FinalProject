// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DetailTex("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				//raycast to corner
				float4 ray : TEXCOORD1;
			};

			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				//send interpolated ray to fragment shader
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			float4 _CameraWS;

			VertOut vert(VertIn v)
			{
				VertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

			sampler2D _MainTex;
			sampler2D _DetailTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance;
			float _ScanWidth;
			float _LeadSharp;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;
			float4 _HBarColor;

			float4 horizBars(float2 p)
			{
				return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
			}

			float4 horizTex(float2 p)
			{
				return tex2D(_DetailTex, float2(p.x * 30, p.y * 40));
			}

			half4 frag (VertOut i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				//sample depth value at this fragment
				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				//value between 0 and 1 for the depth
				float linearDepth = Linear01Depth(rawDepth);
				//worldspace direction from camera to the far plane, magnitude equals distance to frag
				float4 worldspaceDirection = linearDepth * i.interpolatedRay;
				float3 worldspacePosition = _WorldSpaceCameraPos + worldspaceDirection;
				half4 scannerColour = half4(0, 0, 0, 0);

				//worldspace val compared to origin of scanner
				float dist = distance(worldspacePosition, _WorldSpaceScannerPos);

				//is frag inside distance and range of scan?
				if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth/* && linearDepth < 1*/)
				{
					//distance of scan - fragment's distance and div by width to get 0 - 1 value
					float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
															//edge colour more intense
					//blends scan colours together
					half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
					scannerColour = lerp(_TrailColor, edge, diff);// + horizBars(i.uv) * _HBarColor;
						//controls visibility not alpha channels
					scannerColour *= diff;
				}

				return col + scannerColour;
			}
			ENDCG
		}
	}
}
