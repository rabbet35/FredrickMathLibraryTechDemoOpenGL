﻿//This shader will be for a simple point sun
#shader vertex
#version 330 core

uniform mat4 projectionMatrix;
uniform mat4 viewMatrix;
uniform vec3 sunPos;
uniform vec2 viewPortSize;
void main()
{
	float sunRadius = 0.75;
	gl_Position = projectionMatrix * viewMatrix * vec4(sunPos, 1);
	gl_PointSize = viewPortSize.y * sunRadius;
}

#shader fragment
#version 330 core
out vec4 color;
uniform vec3 sunColor;
uniform sampler2D ditherTex;
void main()
{
    vec2 centerVec = gl_PointCoord - vec2(0.5F);
    float coordLength = length(centerVec);
	float fade = pow(sqrt(1.05 - coordLength), 32);
	color = vec4(sunColor, fade);
	color.a += texture2D(ditherTex, gl_FragCoord.xy / 8.0).r / 32.0 - (1.0 / 128.0);//dithering
	if (color.a <= 0.005) discard;
}