// Compiled shader for PC, Mac & Linux Standalone

//////////////////////////////////////////////////////////////////////////
// 
// NOTE: This is *not* a valid shader file, the contents are provided just
// for information and for debugging purposes only.
// 
//////////////////////////////////////////////////////////////////////////
// Skipping shader variants that would not be included into build of current scene.

Shader "Sprites/Default" {
Properties {
[PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
 _Color ("Tint", Color) = (1.000000,1.000000,1.000000,1.000000)
[MaterialToggle]  PixelSnap ("Pixel snap", Float) = 0.000000
[HideInInspector]  _RendererColor ("RendererColor", Color) = (1.000000,1.000000,1.000000,1.000000)
[HideInInspector]  _Flip ("Flip", Vector) = (1.000000,1.000000,1.000000,1.000000)
[PerRendererData]  _AlphaTex ("External Alpha", 2D) = "white" { }
[PerRendererData]  _EnableExternalAlpha ("Enable External Alpha", Float) = 0.000000
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }


 // Stats for Vertex shader:
 //         d3d9: 18 avg math (12..24)
 //        d3d11: 13 avg math (10..16)
 // Stats for Fragment shader:
 //         d3d9: 4 avg math (3..6), 1 avg texture (1..2)
 //        d3d11: 3 avg math (2..4), 1 avg texture (1..2)
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
  ZWrite Off
  Cull Off
  Blend One OneMinusSrcAlpha
  //////////////////////////////////
  //                              //
  //      Compiled programs       //
  //                              //
  //////////////////////////////////
//////////////////////////////////////////////////////
No keywords set in this variant.
-- Vertex shader for "d3d9":
// Stats: 12 math
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Matrix4x4 unity_MatrixVP at 4
Matrix4x4 unity_ObjectToWorld at 0
Vector4 _Color at 9
Vector4 _RendererColor at 8

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 _Color;
//   float4 _RendererColor;
//   row_major float4x4 unity_MatrixVP;
//   row_major float4x4 unity_ObjectToWorld;
//
//
// Registers:
//
//   Name                Reg   Size
//   ------------------- ----- ----
//   unity_ObjectToWorld c0       4
//   unity_MatrixVP      c4       4
//   _RendererColor      c8       1
//   _Color              c9       1
//

    vs_2_0
    def c10, 1, 0, 0, 0
    dcl_position v0
    dcl_color v1
    dcl_texcoord v2
    mad r0, v0.xyzx, c10.xxxy, c10.yyyx
    dp4 r1.x, c0, r0
    dp4 r1.y, c1, r0
    dp4 r1.z, c2, r0
    dp4 r1.w, c3, r0
    dp4 r2.x, c4, r1
    dp4 r2.y, c5, r1
    dp4 r2.z, c6, r1
    dp4 r2.w, c7, r1
    mul r0, v1, c9
    mul oD0, r0, c8
    mov oT0.xy, v2
    mad oPos.xy, r2.w, c255, r2
    mov oPos.zw, r2

// approximately 14 instruction slots used


-- Fragment shader for "d3d9":
// Stats: 3 math, 1 textures
Set 2D Texture "_MainTex" to slot 0

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   sampler2D _MainTex;
//
//
// Registers:
//
//   Name         Reg   Size
//   ------------ ----- ----
//   _MainTex     s0       1
//

    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    texld_pp r0, t0, s0
    mul_pp r0, r0, v0
    mul_pp r0.xyz, r0.w, r0
    mov_pp oC0, r0

// approximately 4 instruction slots used (1 texture, 3 arithmetic)


-- Vertex shader for "d3d11":
// Stats: 10 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Constant Buffer "$Globals" (48 bytes) on slot 0 {
  Vector4 _Color at 32
}
Constant Buffer "UnityPerDraw" (160 bytes) on slot 1 {
  Matrix4x4 unity_ObjectToWorld at 0
}
Constant Buffer "UnityPerFrame" (368 bytes) on slot 2 {
  Matrix4x4 unity_MatrixVP at 272
}
Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 3 {
  Vector4 _RendererColor at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyz 
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
      vs_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_constantbuffer CB2[21], immediateIndexed
      dcl_constantbuffer CB3[1], immediateIndexed
      dcl_input v0.xyz
      dcl_input v1.xyzw
      dcl_input v2.xy
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyzw
      dcl_output o2.xy
      dcl_temps 2
   0: mul r0.xyzw, v0.yyyy, cb1[1].xyzw
   1: mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
   2: mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
   3: add r0.xyzw, r0.xyzw, cb1[3].xyzw
   4: mul r1.xyzw, r0.yyyy, cb2[18].xyzw
   5: mad r1.xyzw, cb2[17].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb2[19].xyzw, r0.zzzz, r1.xyzw
   7: mad o0.xyzw, cb2[20].xyzw, r0.wwww, r1.xyzw
   8: mul r0.xyzw, v1.xyzw, cb0[2].xyzw
   9: mul o1.xyzw, r0.xyzw, cb3[0].xyzw
  10: mov o2.xy, v2.xyxx
  11: ret 
// Approximately 0 instruction slots used


-- Fragment shader for "d3d11":
// Stats: 2 math, 1 temp registers, 1 textures
Set 2D Texture "_MainTex" to slot 0

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_sampler s0, mode_default
      dcl_resource_texture2d (float,float,float,float) t0
      dcl_input_ps linear v1.xyzw
      dcl_input_ps linear v2.xy
      dcl_output o0.xyzw
      dcl_temps 1
   0: sample r0.xyzw, v2.xyxx, t0.xyzw, s0
   1: mul r0.xyzw, r0.xyzw, v1.xyzw
   2: mul o0.xyz, r0.wwww, r0.xyzx
   3: mov o0.w, r0.w
   4: ret 
// Approximately 0 instruction slots used


//////////////////////////////////////////////////////
Keywords set in this variant: ETC1_EXTERNAL_ALPHA 
-- Vertex shader for "d3d9":
// Stats: 12 math
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Matrix4x4 unity_MatrixVP at 4
Matrix4x4 unity_ObjectToWorld at 0
Vector4 _Color at 9
Vector4 _RendererColor at 8

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 _Color;
//   float4 _RendererColor;
//   row_major float4x4 unity_MatrixVP;
//   row_major float4x4 unity_ObjectToWorld;
//
//
// Registers:
//
//   Name                Reg   Size
//   ------------------- ----- ----
//   unity_ObjectToWorld c0       4
//   unity_MatrixVP      c4       4
//   _RendererColor      c8       1
//   _Color              c9       1
//

    vs_2_0
    def c10, 1, 0, 0, 0
    dcl_position v0
    dcl_color v1
    dcl_texcoord v2
    mad r0, v0.xyzx, c10.xxxy, c10.yyyx
    dp4 r1.x, c0, r0
    dp4 r1.y, c1, r0
    dp4 r1.z, c2, r0
    dp4 r1.w, c3, r0
    dp4 r2.x, c4, r1
    dp4 r2.y, c5, r1
    dp4 r2.z, c6, r1
    dp4 r2.w, c7, r1
    mul r0, v1, c9
    mul oD0, r0, c8
    mov oT0.xy, v2
    mad oPos.xy, r2.w, c255, r2
    mov oPos.zw, r2

// approximately 14 instruction slots used


-- Fragment shader for "d3d9":
// Stats: 6 math, 2 textures
Float _EnableExternalAlpha at 0

Set 2D Texture "_MainTex" to slot 0
Set 2D Texture "_AlphaTex" to slot 1

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   sampler2D _AlphaTex;
//   float _EnableExternalAlpha;
//   sampler2D _MainTex;
//
//
// Registers:
//
//   Name                 Reg   Size
//   -------------------- ----- ----
//   _EnableExternalAlpha c0       1
//   _MainTex             s0       1
//   _AlphaTex            s1       1
//

    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    dcl_2d s1
    texld_pp r0, t0, s1
    texld_pp r1, t0, s0
    lrp_pp r2.w, c0.x, r0.x, r1.w
    mov_pp r2.xyz, r1
    mul_pp r0, r2, v0
    mul_pp r0.xyz, r0.w, r0
    mov_pp oC0, r0

// approximately 7 instruction slots used (2 texture, 5 arithmetic)


-- Vertex shader for "d3d11":
// Stats: 10 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Constant Buffer "$Globals" (48 bytes) on slot 0 {
  Vector4 _Color at 32
}
Constant Buffer "UnityPerDraw" (160 bytes) on slot 1 {
  Matrix4x4 unity_ObjectToWorld at 0
}
Constant Buffer "UnityPerFrame" (368 bytes) on slot 2 {
  Matrix4x4 unity_MatrixVP at 272
}
Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 3 {
  Vector4 _RendererColor at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyz 
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
      vs_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_constantbuffer CB1[4], immediateIndexed
      dcl_constantbuffer CB2[21], immediateIndexed
      dcl_constantbuffer CB3[1], immediateIndexed
      dcl_input v0.xyz
      dcl_input v1.xyzw
      dcl_input v2.xy
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyzw
      dcl_output o2.xy
      dcl_temps 2
   0: mul r0.xyzw, v0.yyyy, cb1[1].xyzw
   1: mad r0.xyzw, cb1[0].xyzw, v0.xxxx, r0.xyzw
   2: mad r0.xyzw, cb1[2].xyzw, v0.zzzz, r0.xyzw
   3: add r0.xyzw, r0.xyzw, cb1[3].xyzw
   4: mul r1.xyzw, r0.yyyy, cb2[18].xyzw
   5: mad r1.xyzw, cb2[17].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb2[19].xyzw, r0.zzzz, r1.xyzw
   7: mad o0.xyzw, cb2[20].xyzw, r0.wwww, r1.xyzw
   8: mul r0.xyzw, v1.xyzw, cb0[2].xyzw
   9: mul o1.xyzw, r0.xyzw, cb3[0].xyzw
  10: mov o2.xy, v2.xyxx
  11: ret 
// Approximately 0 instruction slots used


-- Fragment shader for "d3d11":
// Stats: 4 math, 2 temp registers, 2 textures
Set 2D Texture "_MainTex" to slot 0
Set 2D Texture "_AlphaTex" to slot 1

Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 0 {
  Float _EnableExternalAlpha at 32
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_sampler s0, mode_default
      dcl_sampler s1, mode_default
      dcl_resource_texture2d (float,float,float,float) t0
      dcl_resource_texture2d (float,float,float,float) t1
      dcl_input_ps linear v1.xyzw
      dcl_input_ps linear v2.xy
      dcl_output o0.xyzw
      dcl_temps 2
   0: sample r0.xyzw, v2.xyxx, t1.xyzw, s1
   1: sample r1.xyzw, v2.xyxx, t0.xyzw, s0
   2: add r0.x, r0.x, -r1.w
   3: mad r1.w, cb0[2].x, r0.x, r1.w
   4: mul r0.xyzw, r1.xyzw, v1.xyzw
   5: mul o0.xyz, r0.wwww, r0.xyzx
   6: mov o0.w, r0.w
   7: ret 
// Approximately 0 instruction slots used


//////////////////////////////////////////////////////
Keywords set in this variant: PIXELSNAP_ON 
-- Vertex shader for "d3d9":
// Stats: 24 math
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Matrix4x4 unity_MatrixVP at 4
Matrix4x4 unity_ObjectToWorld at 0
Vector4 _Color at 10
Vector4 _RendererColor at 9
Vector4 _ScreenParams at 8

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 _Color;
//   float4 _RendererColor;
//   float4 _ScreenParams;
//   row_major float4x4 unity_MatrixVP;
//   row_major float4x4 unity_ObjectToWorld;
//
//
// Registers:
//
//   Name                Reg   Size
//   ------------------- ----- ----
//   unity_ObjectToWorld c0       4
//   unity_MatrixVP      c4       4
//   _ScreenParams       c8       1
//   _RendererColor      c9       1
//   _Color              c10      1
//

    vs_2_0
    def c11, 1, 0, 0.5, 0
    dcl_position v0
    dcl_color v1
    dcl_texcoord v2
    mad r0, v0.xyzx, c11.xxxy, c11.yyyx
    dp4 r1.x, c0, r0
    dp4 r1.y, c1, r0
    dp4 r1.z, c2, r0
    dp4 r1.w, c3, r0
    dp4 r3.z, c6, r1
    mul r0, v1, c10
    mul oD0, r0, c9
    dp4 r0.x, c4, r1
    dp4 r0.y, c5, r1
    dp4 r0.z, c7, r1
    rcp r0.w, r0.z
    mul r0.xy, r0.w, r0
    mov r1.z, c11.z
    mul r1.xy, r1.z, c8
    mad r0.xy, r0, r1, c11.z
    frc r1.zw, r0.xyxy
    add r0.xy, r0, -r1.zwzw
    rcp r2.x, r1.x
    rcp r2.y, r1.y
    mul r0.xy, r0, r2
    mul r3.xy, r0.z, r0
    mov r3.w, r0.z
    mov oT0.xy, v2
    mad oPos.xy, r3.w, c255, r3
    mov oPos.zw, r3

// approximately 26 instruction slots used


-- Fragment shader for "d3d9":
// Stats: 3 math, 1 textures
Set 2D Texture "_MainTex" to slot 0

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   sampler2D _MainTex;
//
//
// Registers:
//
//   Name         Reg   Size
//   ------------ ----- ----
//   _MainTex     s0       1
//

    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    texld_pp r0, t0, s0
    mul_pp r0, r0, v0
    mul_pp r0.xyz, r0.w, r0
    mov_pp oC0, r0

// approximately 4 instruction slots used (1 texture, 3 arithmetic)


-- Vertex shader for "d3d11":
// Stats: 16 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Constant Buffer "$Globals" (48 bytes) on slot 0 {
  Vector4 _Color at 32
}
Constant Buffer "UnityPerCamera" (144 bytes) on slot 1 {
  Vector4 _ScreenParams at 96
}
Constant Buffer "UnityPerDraw" (160 bytes) on slot 2 {
  Matrix4x4 unity_ObjectToWorld at 0
}
Constant Buffer "UnityPerFrame" (368 bytes) on slot 3 {
  Matrix4x4 unity_MatrixVP at 272
}
Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 4 {
  Vector4 _RendererColor at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyz 
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
      vs_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_constantbuffer CB1[7], immediateIndexed
      dcl_constantbuffer CB2[4], immediateIndexed
      dcl_constantbuffer CB3[21], immediateIndexed
      dcl_constantbuffer CB4[1], immediateIndexed
      dcl_input v0.xyz
      dcl_input v1.xyzw
      dcl_input v2.xy
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyzw
      dcl_output o2.xy
      dcl_temps 2
   0: mul r0.xyzw, v0.yyyy, cb2[1].xyzw
   1: mad r0.xyzw, cb2[0].xyzw, v0.xxxx, r0.xyzw
   2: mad r0.xyzw, cb2[2].xyzw, v0.zzzz, r0.xyzw
   3: add r0.xyzw, r0.xyzw, cb2[3].xyzw
   4: mul r1.xyzw, r0.yyyy, cb3[18].xyzw
   5: mad r1.xyzw, cb3[17].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb3[19].xyzw, r0.zzzz, r1.xyzw
   7: mad r0.xyzw, cb3[20].xyzw, r0.wwww, r1.xyzw
   8: div r0.xy, r0.xyxx, r0.wwww
   9: mul r1.xy, cb1[6].xyxx, l(0.500000, 0.500000, 0.000000, 0.000000)
  10: mul r0.xy, r0.xyxx, r1.xyxx
  11: round_ne r0.xy, r0.xyxx
  12: div r0.xy, r0.xyxx, r1.xyxx
  13: mul o0.xy, r0.wwww, r0.xyxx
  14: mov o0.zw, r0.zzzw
  15: mul r0.xyzw, v1.xyzw, cb0[2].xyzw
  16: mul o1.xyzw, r0.xyzw, cb4[0].xyzw
  17: mov o2.xy, v2.xyxx
  18: ret 
// Approximately 0 instruction slots used


-- Fragment shader for "d3d11":
// Stats: 2 math, 1 temp registers, 1 textures
Set 2D Texture "_MainTex" to slot 0

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_sampler s0, mode_default
      dcl_resource_texture2d (float,float,float,float) t0
      dcl_input_ps linear v1.xyzw
      dcl_input_ps linear v2.xy
      dcl_output o0.xyzw
      dcl_temps 1
   0: sample r0.xyzw, v2.xyxx, t0.xyzw, s0
   1: mul r0.xyzw, r0.xyzw, v1.xyzw
   2: mul o0.xyz, r0.wwww, r0.xyzx
   3: mov o0.w, r0.w
   4: ret 
// Approximately 0 instruction slots used


//////////////////////////////////////////////////////
Keywords set in this variant: ETC1_EXTERNAL_ALPHA PIXELSNAP_ON 
-- Vertex shader for "d3d9":
// Stats: 24 math
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Matrix4x4 unity_MatrixVP at 4
Matrix4x4 unity_ObjectToWorld at 0
Vector4 _Color at 10
Vector4 _RendererColor at 9
Vector4 _ScreenParams at 8

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   float4 _Color;
//   float4 _RendererColor;
//   float4 _ScreenParams;
//   row_major float4x4 unity_MatrixVP;
//   row_major float4x4 unity_ObjectToWorld;
//
//
// Registers:
//
//   Name                Reg   Size
//   ------------------- ----- ----
//   unity_ObjectToWorld c0       4
//   unity_MatrixVP      c4       4
//   _ScreenParams       c8       1
//   _RendererColor      c9       1
//   _Color              c10      1
//

    vs_2_0
    def c11, 1, 0, 0.5, 0
    dcl_position v0
    dcl_color v1
    dcl_texcoord v2
    mad r0, v0.xyzx, c11.xxxy, c11.yyyx
    dp4 r1.x, c0, r0
    dp4 r1.y, c1, r0
    dp4 r1.z, c2, r0
    dp4 r1.w, c3, r0
    dp4 r3.z, c6, r1
    mul r0, v1, c10
    mul oD0, r0, c9
    dp4 r0.x, c4, r1
    dp4 r0.y, c5, r1
    dp4 r0.z, c7, r1
    rcp r0.w, r0.z
    mul r0.xy, r0.w, r0
    mov r1.z, c11.z
    mul r1.xy, r1.z, c8
    mad r0.xy, r0, r1, c11.z
    frc r1.zw, r0.xyxy
    add r0.xy, r0, -r1.zwzw
    rcp r2.x, r1.x
    rcp r2.y, r1.y
    mul r0.xy, r0, r2
    mul r3.xy, r0.z, r0
    mov r3.w, r0.z
    mov oT0.xy, v2
    mad oPos.xy, r3.w, c255, r3
    mov oPos.zw, r3

// approximately 26 instruction slots used


-- Fragment shader for "d3d9":
// Stats: 6 math, 2 textures
Float _EnableExternalAlpha at 0

Set 2D Texture "_MainTex" to slot 0
Set 2D Texture "_AlphaTex" to slot 1

Shader Disassembly:
//
// Generated by Microsoft (R) HLSL Shader Compiler 10.1
//
// Parameters:
//
//   sampler2D _AlphaTex;
//   float _EnableExternalAlpha;
//   sampler2D _MainTex;
//
//
// Registers:
//
//   Name                 Reg   Size
//   -------------------- ----- ----
//   _EnableExternalAlpha c0       1
//   _MainTex             s0       1
//   _AlphaTex            s1       1
//

    ps_2_0
    dcl v0
    dcl t0.xy
    dcl_2d s0
    dcl_2d s1
    texld_pp r0, t0, s1
    texld_pp r1, t0, s0
    lrp_pp r2.w, c0.x, r0.x, r1.w
    mov_pp r2.xyz, r1
    mul_pp r0, r2, v0
    mul_pp r0.xyz, r0.w, r0
    mov_pp oC0, r0

// approximately 7 instruction slots used (2 texture, 5 arithmetic)


-- Vertex shader for "d3d11":
// Stats: 16 math, 2 temp registers
Uses vertex data channel "Vertex"
Uses vertex data channel "Normal"
Uses vertex data channel "TexCoord"

Constant Buffer "$Globals" (48 bytes) on slot 0 {
  Vector4 _Color at 32
}
Constant Buffer "UnityPerCamera" (144 bytes) on slot 1 {
  Vector4 _ScreenParams at 96
}
Constant Buffer "UnityPerDraw" (160 bytes) on slot 2 {
  Matrix4x4 unity_ObjectToWorld at 0
}
Constant Buffer "UnityPerFrame" (368 bytes) on slot 3 {
  Matrix4x4 unity_MatrixVP at 272
}
Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 4 {
  Vector4 _RendererColor at 0
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// POSITION                 0   xyzw        0     NONE   float   xyz 
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float   xyzw
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
      vs_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_constantbuffer CB1[7], immediateIndexed
      dcl_constantbuffer CB2[4], immediateIndexed
      dcl_constantbuffer CB3[21], immediateIndexed
      dcl_constantbuffer CB4[1], immediateIndexed
      dcl_input v0.xyz
      dcl_input v1.xyzw
      dcl_input v2.xy
      dcl_output_siv o0.xyzw, position
      dcl_output o1.xyzw
      dcl_output o2.xy
      dcl_temps 2
   0: mul r0.xyzw, v0.yyyy, cb2[1].xyzw
   1: mad r0.xyzw, cb2[0].xyzw, v0.xxxx, r0.xyzw
   2: mad r0.xyzw, cb2[2].xyzw, v0.zzzz, r0.xyzw
   3: add r0.xyzw, r0.xyzw, cb2[3].xyzw
   4: mul r1.xyzw, r0.yyyy, cb3[18].xyzw
   5: mad r1.xyzw, cb3[17].xyzw, r0.xxxx, r1.xyzw
   6: mad r1.xyzw, cb3[19].xyzw, r0.zzzz, r1.xyzw
   7: mad r0.xyzw, cb3[20].xyzw, r0.wwww, r1.xyzw
   8: div r0.xy, r0.xyxx, r0.wwww
   9: mul r1.xy, cb1[6].xyxx, l(0.500000, 0.500000, 0.000000, 0.000000)
  10: mul r0.xy, r0.xyxx, r1.xyxx
  11: round_ne r0.xy, r0.xyxx
  12: div r0.xy, r0.xyxx, r1.xyxx
  13: mul o0.xy, r0.wwww, r0.xyxx
  14: mov o0.zw, r0.zzzw
  15: mul r0.xyzw, v1.xyzw, cb0[2].xyzw
  16: mul o1.xyzw, r0.xyzw, cb4[0].xyzw
  17: mov o2.xy, v2.xyxx
  18: ret 
// Approximately 0 instruction slots used


-- Fragment shader for "d3d11":
// Stats: 4 math, 2 temp registers, 2 textures
Set 2D Texture "_MainTex" to slot 0
Set 2D Texture "_AlphaTex" to slot 1

Constant Buffer "UnityPerDrawSprite" (48 bytes) on slot 0 {
  Float _EnableExternalAlpha at 32
}

Shader Disassembly:
//
// Generated by Microsoft (R) D3D Shader Disassembler
//
//
// Input signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_POSITION              0   xyzw        0      POS   float       
// COLOR                    0   xyzw        1     NONE   float   xyzw
// TEXCOORD                 0   xy          2     NONE   float   xy  
//
//
// Output signature:
//
// Name                 Index   Mask Register SysValue  Format   Used
// -------------------- ----- ------ -------- -------- ------- ------
// SV_Target                0   xyzw        0   TARGET   float   xyzw
//
      ps_4_0
      dcl_constantbuffer CB0[3], immediateIndexed
      dcl_sampler s0, mode_default
      dcl_sampler s1, mode_default
      dcl_resource_texture2d (float,float,float,float) t0
      dcl_resource_texture2d (float,float,float,float) t1
      dcl_input_ps linear v1.xyzw
      dcl_input_ps linear v2.xy
      dcl_output o0.xyzw
      dcl_temps 2
   0: sample r0.xyzw, v2.xyxx, t1.xyzw, s1
   1: sample r1.xyzw, v2.xyxx, t0.xyzw, s0
   2: add r0.x, r0.x, -r1.w
   3: mad r1.w, cb0[2].x, r0.x, r1.w
   4: mul r0.xyzw, r1.xyzw, v1.xyzw
   5: mul o0.xyz, r0.wwww, r0.xyzx
   6: mov o0.w, r0.w
   7: ret 
// Approximately 0 instruction slots used


 }
}
}