Shader "Custom/Form" {
	Properties {
		_Radius ("Radius", Range(0.0, 0.5)) = 0.4
		_Width ("Width", Range(0.0, 1.0)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			static const float2 UV_CENTER = float2(0.5, 0.5);

			float _Radius;
			float _Width;
			fixed4 _Color;

			struct vertexInput {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
  
			vertexOutput vert(vertexInput i) {
				vertexOutput o;
				o.pos =  UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				o.color = i.color;
				return o;
			}

			float4 frag(vertexOutput i) : COLOR {
				float dist = length(UV_CENTER - i.uv);
				float p = smoothstep(_Radius + _Width, _Radius, dist);
				return i.color * p;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}