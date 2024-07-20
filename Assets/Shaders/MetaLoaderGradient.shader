Shader "Unlit/BackGradient"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_ColorFrom ("ColorFrom", color) = (0,0,0,0)
		_ColorTo ("ColorTo", color) = (0,0,0,0)
        _RadiusMultiplier ("Radius", Range(0, 5)) = 1
        _GradientPivot ("Position", Vector) = (0, 0, 0, 0)
	}
	SubShader
	{
		Tags
		 {
			 "Queue"="Transparent" 
		 }
		Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite Off
        ZTest Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

            float4 _ColorFrom;
            float4 _ColorTo;
            float _RadiusMultiplier;

			v2f vert (appdata v)
			{
				v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.color = v.color;
                return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
                fixed2 pixelPoint = fixed2(i.uv.x, i.uv.y);
                float xDist = pow(pixelPoint.x - 0.5, 2);
                float yDist = pow(pixelPoint.y - 0.5, 2);
                float dist = sqrt(xDist + yDist) / _RadiusMultiplier;
                dist = clamp(dist, 0, 1);
                fixed4 col = lerp(_ColorFrom, _ColorTo, dist);
				col.a = i.color.a;
                return col;
			}
			ENDCG
		}
	}
}