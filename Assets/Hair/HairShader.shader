Shader "Ybiii/Hair Shader"
{
	Properties
	{
		//Base UV Hair Texture. PNG with Alpha.
		[NoScaleOffset] _MainTex("Base UV Hair Texture", 2D) = "white" {}
		[NoScaleOffset] _MainTex2("Base UV Hair Texture 2", 2D) = "white" {}
		[NoScaleOffset] _MainTex3("Base UV Hair Texture 3", 2D) = "white" {}
		
		//Full Color Range Tinting of Hair
		_Color1("Color Adjustment", Color) = (1, 1, 1, 1)
		_Color2("Color Adjustment2", Color) = (1, 1, 1, 1)
		_Color3("Color Adjustment2", Color) = (1, 1, 1, 1)
		
		[Gamma] _Richness("Richness of Hair ", Range(0,1)) = 1.0
		_Glossiness("Wetness/Gloss", Range(0,1)) = 0.25
		
		//Normal Map Texture.
		[NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
		_BumpScale("Normal Map Adjustment", Float) = 1.0

		//Add transparnecy to ends of hair
		_Cutoff("Alpha Cutoff", Range(0.01,1)) = 0.5

		[NoScaleOffset] _OC("OC", 2D) = "white" {}
		_OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0

		_MaskTex("Mask", 2D) = "white" {}

		
	}
	
	SubShader
	{
		Tags{ "Queue" ="AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		Fog { Color(0,0,0,0) }
		ZWrite Off
		Cull Off

		Pass
		{
			ColorMask 0
			ZWrite On

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f 
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			fixed _Cutoff;

			v2f vert(appdata_img v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 mainColor = tex2D(_MainTex, i.texcoord);
				clip(mainColor.a - _Cutoff);
				return 0;
			}
			ENDCG
		}

		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			struct v2f 
			{
				V2F_SHADOW_CASTER;
				float2 texcoord : TEXCOORD1;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.texcoord = v.texcoord;
				return o;
			}

			sampler2D _MainTex;
			fixed _Cutoff;

			float4 frag(v2f i) : SV_Target
			{
				fixed4 mainColor = tex2D(_MainTex, i.texcoord);
				clip(mainColor.a - _Cutoff);
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade nolightmap
		#pragma target 3.0
		
		struct Input 
		{
			float2 uv_MainTex;
			float2 uv_MainTex2;
			float2 uv_MainTex3;
			float2 uv_MaskTex;
			float2 uv_OC;
			fixed facing : VFACE;
		};

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;
		sampler2D _MaskTex;
		sampler2D _BumpMap;
		sampler2D _OC;

		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;
		half _Glossiness;
		half _Richness;		
		half _BumpScale;
		fixed _Cutoff;
		half _OcclusionStrength;

		void surf(Input IN, inout SurfaceOutputStandard o) 
		{		
			fixed4 masks = tex2D(_MaskTex, IN.uv_MaskTex);
			fixed4 clr = tex2D(_MainTex, IN.uv_MainTex) * masks.r * _Color1;
			clr = clr + tex2D(_MainTex2, IN.uv_MainTex) * masks.g * _Color2;
			clr = clr + tex2D(_MainTex3, IN.uv_MainTex) * masks.b * _Color3;
			//fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex) * _Color1;
			//fixed4 specTex = tex2D(_SpecMap, IN.uv_MainTex);
			//o.Albedo = mainColor.rgb;
			o.Albedo = clr.rgb;
			o.Metallic = _Richness;
			o.Smoothness = _Glossiness;
			o.Occlusion = _OcclusionStrength * tex2D(_OC, IN.uv_OC);
			o.Alpha = saturate(clr.a / _Cutoff);

			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);

			if (IN.facing < 0.5)
			{
				o.Normal *= -1.0;
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}