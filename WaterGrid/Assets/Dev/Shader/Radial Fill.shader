Shader "Custom/RadialFill"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _FillAmount;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float angle = atan2(i.uv.y - 0.5, i.uv.x - 0.5) / (2 * UNITY_PI) + 0.5;
                float dist = length(i.uv - 0.5);
                if (angle > _FillAmount || dist > 0.5)
                    discard;

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
