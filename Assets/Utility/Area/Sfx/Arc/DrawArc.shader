// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DrawArc"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TintColor("TintColor", Color) = (1,1,1,1)
		_InnerDiscardRadius("InnerDiscardRadius", Range(0,0.5)) = 0.19
		_OutterBoundWidth("OutterBoundWidth", Float) = 0.005
		_InnerBoundWidth("InnerBoundWidth", Float) = 0.1
		_Angle ("Angle", Range(0, 180)) = 60
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 100

		Pass
		{
			ColorMask RGB
			Cull Off Lighting Off ZWrite Off
			Blend SrcAlpha One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed _InnerDiscardRadius;
			fixed _OutterBoundWidth;
			fixed _InnerBoundWidth;
			fixed _Angle;

			struct v2f
			{
				fixed4 vertex : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed3 data : COLOR;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord.xy - fixed2(0.5,0.5);
				o.data.x = step(90, _Angle);
				o.data.y = tan(lerp(_Angle, 180 - _Angle, o.data.x) * 0.017453); // 角度转弧度
				o.data.z = o.data.x * 2 - 1;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed distance = sqrt(i.uv.x * i.uv.x + i.uv.y * i.uv.y);
				fixed4 final = fixed4(0,0,0,0);
				fixed4 zeroCol = fixed4(0,0,0,0);

				// 纹理是圆的四分之一，求出偏移的UV
				fixed2 newUV = fixed2(abs(i.uv.x) / 0.5, abs(i.uv.y) / 0.5);
				// 使用偏移的UV对纹理进行采样
				fixed4 col = tex2D(_MainTex, newUV);
				col = _TintColor * col * 2.0f;

				// 靠近边缘则抗锯齿
				// 外边界抗锯齿
				fixed sideDist = abs(0.5 - distance);
				col.a = lerp(col.a, col.a - (1 - sideDist / _OutterBoundWidth), step(sideDist, _OutterBoundWidth));
				// 内圈边界抗锯齿
				sideDist = abs(distance - _InnerDiscardRadius);
				col.a = lerp(col.a, col.a - (1 - sideDist / _InnerBoundWidth), step(sideDist, _InnerBoundWidth));

				fixed functionY = abs(i.uv.x) / i.data.y;
				final = lerp(col, zeroCol, step(i.data.z, 0) + i.data.z * step(functionY, abs(i.uv.y)));
				fixed4 temp = lerp(zeroCol, col, i.data.x);
				final = lerp(final, temp, step(i.data.z, 0) + i.data.z * step(i.uv.y, 0));

				// 剔除内圈和超过半径的部分
				final = lerp(final, zeroCol, step(distance, _InnerDiscardRadius));
				final = lerp(final, zeroCol, step(0.5, distance));

				return final;
			}
			ENDCG
		}
		}
}