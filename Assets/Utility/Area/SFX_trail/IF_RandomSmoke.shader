// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/IF_RandomSmoke"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
        _Offset ("Offset", Float) = 0
        _Alpha("Alpha", Float) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha One
        Cull Off
        ZWrite Off
        Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
                fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float _Offset;
            float _Alpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x += _Offset;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= i.color.rga;
                col.a = _Alpha * i.color.a;
				return col;
			}
			ENDCG
		}
	}
}
