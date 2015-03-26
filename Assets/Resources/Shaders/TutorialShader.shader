Shader "My Shaders/Natural Light" {
    Properties {
      _MainTex ("Main Texture", 2D) = "white" {}
      _SecTex  ("Secondary Texture", 2D) = "white"{}
      _Color ("Main Color", Color) = (1,1,1,0.5)
      _Div ("Color Range", Range(20,255)) = 255
      _Water ("Water Level", Range(0,100)) = 100
      _WColor("Water Color", Color) = (0,0,1,0.5)
    }
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Lambert vertex:vert
      #include "UnityCG.cginc"

      sampler2D _MainTex;
      sampler2D _SecTex;
      float4 _Color;
      float _Div;
      float _Water;
      float4 _WColor;
      
      struct Input {
          float2 uv_MainTex;
          float3 customColor;
          float yPos;
      };
      void vert (inout appdata_full v, out Input o) {
          //o.customColor = abs(v.normal);
          //o.customColor = (_Color+v.color) * (v.vertex.y)/ _Div;
          float4 pos = mul (_Object2World, v.vertex);
          o.customColor = _Color*2 + (_Color) * (pos.y)/ (_Div);
          
          if (_Water >= pos.y+0.5f){
            o.customColor *= _WColor*2 - (_WColor - (_WColor * (pos.y / _Water)));
          }
          
          o.yPos = pos.y;
          
      }
      void surf (Input IN, inout SurfaceOutput o) {
      	  if (IN.yPos <= (_Water + 2.5f)){
      	  	o.Albedo = tex2D(_SecTex, IN.uv_MainTex).rgb;
      	  }else{
          	o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
          }
          o.Albedo *= IN.customColor;
      }
      ENDCG
    } 
    Fallback "Diffuse"
  }
