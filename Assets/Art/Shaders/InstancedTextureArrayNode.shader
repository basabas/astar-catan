Shader "Bas/Unlit/InstancedTextureArrayNode"
{
	Properties
	{
		_NodeTextureArray("Tex", 2DArray) = "" {}
	}
		SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma require 2darray

			sampler2D _MainTex;

			StructuredBuffer<float4> positionBuffer;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			v2f vert(appdata v, uint instanceID : SV_InstanceID)
			{
				float4 data = positionBuffer[instanceID];
				float3 worldPosition = float3(data.x, 0, data.y) + float3(v.vertex.x, v.vertex.y * data.z, v.vertex.z);
				v2f o;
				o.vertex = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
				o.uv.xy = v.uv.xy;
				o.uv.z = data.w;
				o.color = float4(data.z,1,1,1);
				return o;
			}
			UNITY_DECLARE_TEX2DARRAY(_NodeTextureArray);

			fixed4 frag(v2f i) : SV_Target
			{
				return UNITY_SAMPLE_TEX2DARRAY(_NodeTextureArray, i.uv) * i.color;
			}
			ENDCG
		}
	}
}
