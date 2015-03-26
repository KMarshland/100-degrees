Shader "My Shaders/_Test_02_VLLighting" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Center("Center Point", Vector) = (0,0,0,0)
		_Intensity("Light Intensity", Range(1,50)) = 10
		//_BumpMap ("Bump (RGB) Illumin (A)", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
	    CGPROGRAM
	    #pragma surface surf Lambert vertex:vert
	    #include "UnityCG.cginc"
	
	    sampler2D _MainTex;
	    //sampler2D _SecTex;
	    float4 _Color;
	    float4 _Center;
	    float _Intensity;
	    
	    struct Input {
	        float2 uv_MainTex;
	        float3 customColor;
	        //float yPos;
	    };
	    void vert (inout appdata_full v, out Input o) {
	        //o.customColor = abs(v.normal);
	        //o.customColor = (_Color+v.color) * (v.vertex.y)/ _Div;
	        float4 pos = mul (_Object2World, v.vertex);
	        //o.customColor = _Color*2 + (_Color) * (pos.y)/ (_Div);
	        if (pos.x < 0) pos.x *= -1;
	        if (pos.y < 0) pos.y *= -1;
	        if (pos.z < 0) pos.z *= -1;
	        
	        o.customColor = (v.color*2 + _Color / (pow(pos.x*pos.x/_Intensity + pos.y*pos.y/_Intensity + pos.z*pos.z/_Intensity, 0.5f)))/2f;
	        // /_Intensity
	    }
	    void surf (Input IN, inout SurfaceOutput o) {
	        o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
	        o.Albedo *= IN.customColor;
	    }
	    ENDCG
	} 
	FallBack "Diffuse"
}

