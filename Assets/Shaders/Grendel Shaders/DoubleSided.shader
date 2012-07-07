Shader "DoubleSided" {
	Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	Pass {
		Cull Off
		Lighting Off
		SetTexture [_MainTex] { combine texture } 
	}
}
}