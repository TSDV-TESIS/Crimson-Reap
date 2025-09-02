Shader "Custom/DisabelZWrite"
{
	SubShader{
		Tags{
				"RenderType" ="Opaque"
			}
			Pass{
				ZWrite Off
				}
		}
	}
