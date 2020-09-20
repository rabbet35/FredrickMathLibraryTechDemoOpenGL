﻿#shader vertex
#version 330 core
//location is the location of the value in the vertex atrib array
//for vec4 position, the gpu automatically fills in the 4th component with a 1.0F. This means you can treat position as a vec4 no problem. (no need for messy conversions)
layout(location = 0) in vec4 position;
layout(location = 1) in vec4 vertexColor;
layout(location = 2) in vec2 texCoord;

uniform float fogDensity = 0.0075;
uniform float fogGradient = 2.5;

out vec2 vTexCoord;
out vec4 vColor;
out float visibility;//for fog

//matrix for projection transformations.
uniform mat4 projectionMatrix;
//matrix for camera transformations.
uniform mat4 viewMatrix;
//matrix for model transformations. All transformations in this matrix are relative to the model origin.
uniform mat4 modelMatrix;

void main()
{
	vec4 worldPosition = modelMatrix * position;

	vec4 positionRelativeToCam = viewMatrix * worldPosition;

	gl_Position = projectionMatrix * positionRelativeToCam;

	float distanceFromCam = length(positionRelativeToCam.xyz);
	visibility = exp(-pow((distanceFromCam * fogDensity), fogGradient));
	visibility = clamp(visibility, 0.0, 1.0);

	vTexCoord = texCoord;
	vColor = vertexColor;
}

/*#############################################################################################################################################################################################*/
//Out variables from vertex shader are passed into the fragment shaders in variables, part of glsl language.
#shader fragment
#version 330 core
in vec2 vTexCoord;
in vec4 vColor;
in float visibility;
out vec4 color;

uniform sampler2D uTexture;
uniform int frame = 0;
uniform vec3 fogColor;

float rand3D(in vec3 xyz) 
{
	return fract(tan(distance(xyz.xy * 1.6180339F, xyz.xy) * xyz.z) * xyz.y);
}

void main()
{
	vec4 textureColor = texture(uTexture, vTexCoord) * vColor;// *texture(uTexture, vTexCoord); // mixes colour and textures

	if (!bool(textureColor.a))
	{
		discard;
	}

	if (textureColor.a < 0.99)
	{
		if(rand3D(gl_FragCoord.xyz + (float(frame) * 0.001F)) > textureColor.a)//do stochastic transparency
		discard;
	}

	color = mix(vec4(fogColor, 1.0F), vec4(textureColor.rgb, 1.0F), visibility);
}