Shader "Custom/SnakeYello" {
	Properties{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0

		sampler2D _MainTex;

	struct Input {
		float2 uv_MainTex;
	};

	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
		fixed4 tmpYello = (c.r + c.g) / 2;

		c.g = tmpYello;
		c.r = tmpYello;

		o.Albedo = c.rgb;
		// Metallic and smoothness come from slider variables
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
