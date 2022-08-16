#ifndef MY_CURVEY_CG_INCLUDED
#define MY_CURVEY_CG_INCLUDED

#include "UnityCG.cginc"


#define BEND_AMOUNT float4(0,-0.04,0,0)
#define FALLOFF_STRENGTH 1.5f
#pragma multi_compile __ CURVEY_ENABLED

void MyVert(inout appdata_full v, float4 pos)
{
   #ifdef CURVEY_ENABLED
    
    //float4 world = UnityObjectToClipPos(v.vertex);
    float4 world =mul(unity_ObjectToWorld, v.vertex);
    float dist = length(world.xyz - pos.xyz);
    dist = max(0, dist - 1);
    //float4 value = mul(unity_WorldToObject, world);
    //o.vertex = UnityObjectToClipPos(value);
    dist = pow(dist, FALLOFF_STRENGTH);
    float4 localDist = mul(unity_WorldToObject, dist * BEND_AMOUNT);
    //v.vertex.xyz += dist * BEND_AMOUNT;
    v.vertex.xyz += localDist;
    //v.vertex.y += v.normal * BEND_AMOUNT * dist;
    
   #endif
}

#endif
