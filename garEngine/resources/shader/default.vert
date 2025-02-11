﻿#version 330

layout(location = 0) in vec3 vPosition;
layout(location = 1) in vec3 vNormal;
layout(location = 2) in vec2 vTexCoord;
layout(location = 3) in vec3 vTangent;

uniform mat4 mvp;
uniform mat4 model;

uniform vec3 viewVec;
uniform vec3 lightPos;
uniform mat4 lightSpaceMatrix;
uniform mat4 view;

out mat4 viewMat;
out vec2 fTexCoord;
out vec3 FragPos;
out mat3 TBN;
out vec3 tangent;
out vec3 bitangent;
out vec3 fNormal;
out vec3 fViewVec;
out vec3 fLightPos;
out vec4 FragPosLightSpace;




void main() {
    fTexCoord = vTexCoord;
    mat3 normalMatrix = transpose(inverse(mat3(model)));
    vec3 T = normalize(normalMatrix * vTangent);
    vec3 N = normalize(normalMatrix * vNormal);
    vec3 B = cross(N, T);
    bitangent = B;
    fNormal = N;
    tangent = T;
    TBN = transpose(mat3(T, B, N));   
    fViewVec = viewVec;
    fLightPos = lightPos;
    FragPos = vec3(model * vec4(vPosition, 1.0));  
    FragPosLightSpace = lightSpaceMatrix * vec4(FragPos, 1.0);
    gl_Position = mvp * vec4(vPosition, 1.0);
    viewMat = view;
}

