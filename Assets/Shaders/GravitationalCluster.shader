Shader "SpaceObjects/GravitationalCluster"
{
    Properties
    {
        _IOR ("Index Of Refraction", float) = 1
        _EventHorizonThreshold ("Event Horizon Threshold", float) = 1
        _EvHorGrade( "Event Horizon Exponential", float) = 1
        _Distortionfreq ("Distortion Frequency", float)  = 1
        _DistortionAmp ("Distortion Amplitude", float)  = 1
        [HDR]_QuazarCol ("Quazar color", color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100
        ZWrite on
        GrabPass
        {
            "_BGTex"
        }
        
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : POSITION1;
            };

            sampler2D _MainTex;
            sampler2D _BGTex;
            float4 _MainTex_ST;
            float _IOR;
            float _EventHorizonThreshold;
            float _DistortionAmp;
           
            float _Distortionfreq;
            float _EvHorGrade;
            fixed4 _QuazarCol;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = ComputeGrabScreenPos(o.vertex);
                o.normal = v.normal;
                o.viewDir = ObjSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float rim = dot(normalize(i.viewDir), normalize(i.normal));
                float distorsion = 1 - saturate(saturate(pow(_EvHorGrade,pow(rim,_EventHorizonThreshold) - 1)));
                float inerglow = 1 - saturate(pow(300,rim - 1));
                half4 bgcolor = tex2Dproj(_BGTex, i.uv + sin(((_IOR * (cos(_Time.y * _Distortionfreq) * _DistortionAmp)) * dot(i.viewDir, i.normal))));
                
                half4 col = bgcolor * inerglow + _QuazarCol * distorsion;
                col.a = 1;
                return col;
            }
            ENDCG
        }
    }
}
