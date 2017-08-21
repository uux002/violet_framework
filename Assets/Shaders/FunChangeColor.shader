// Change color shader(with alpha)
Shader "Fun/Change Color"
{
	Properties
	{
	[TCP2HeaderHelp(BASE, Base Properties)]
		// COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_MaskedColor ("Masked Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)
		_HighlightMultiplier ("Highlight Multiplier", Range(0,4)) = 1
		_ShadowMultiplier ("Shadow Multiplier", Range(0,4)) = 1
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
	[TCP2Separator]
		
		// COLORS RAMP
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
	[TCP2Separator]
		
	[Header(Masks)]
		_Mask1 ("Mask 1 (Color)", 2D) = "black" {}
	[TCP2Separator]
	
	[TCP2HeaderHelp(SPECULAR, Specular)]
		//SPECULAR
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range(0.0,2)) = 0.1
		_SpecSmooth ("Smoothness", Range(0,1)) = 1.0
	[TCP2Separator]
	
		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		
		#pragma surface surf ToonyColorsCustom 
		#pragma target 3.0
		
		#pragma multi_compile TCP2_RAMPTEXT
		#pragma multi_compile TCP2_SPEC_TOON
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		fixed4 _MaskedColor;
		sampler2D _MainTex;
		sampler2D _Mask1;
		
		fixed _Shininess;
		
		struct Input
		{
			half2 uv_MainTex;
			half2 uv_Mask1;
		};
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		fixed _HighlightMultiplier;
		fixed _ShadowMultiplier;
		sampler2D _Ramp;
		fixed _SpecSmooth;
		
		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
		};
		
		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			s.Normal = normalize(s.Normal);
			
			fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			
			_SColor = lerp(_HColor, _SColor, _SColor.a * _ShadowMultiplier);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb * _HighlightMultiplier, ramp);
			
			//Specular
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (s.Normal, h));
			float spec = pow(ndh, s.Specular*128.0) * s.Gloss * 2.0;
			spec = smoothstep(0.5-_SpecSmooth*0.5, 0.5+_SpecSmooth*0.5, spec);
			spec *= atten;
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			c.rgb += _LightColor0.rgb * _SpecColor.rgb * spec;
			c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec;
			return c;
		}

		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 mask1 = tex2D(_Mask1, IN.uv_Mask1);

			_Color *= lerp(fixed4(1,1,1,1), _MaskedColor, mask1.a);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
			
			//Specular
			o.Gloss = 1;
			o.Specular = _Shininess;
		}
		ENDCG
	}
	
	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector_SG"
}
