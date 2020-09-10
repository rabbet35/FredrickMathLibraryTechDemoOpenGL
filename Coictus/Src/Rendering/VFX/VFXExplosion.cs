﻿using OpenTK;
using System;
namespace Coictus.VFX
{
    public class VFXExplosion : VFXBase
    {
        protected static readonly String shaderDir = ResourceUtil.getShaderFileDir("ColorTextureFog3D.shader");
        protected static readonly String textureDir = ResourceUtil.getTextureFileDir("Explosion.png");
        protected static readonly String modelDir = ResourceUtil.getOBJFileDir("IcoSphere.obj");
        private static Random rand = new Random();

        [Obsolete("This vfx was used for explosions before the addition of point particle vfx")]
        public VFXExplosion(Vector3d pos) : base(pos, 1.0F, shaderDir, textureDir, modelDir, 0.5F, VFXRenderType.tirangles)
        {
            scale = 3.0F;
            setPitch((float)rand.NextDouble() * 180D);
            setYaw((float)rand.NextDouble() * 180D);
            setRoll((float)rand.NextDouble() * 180D);
        }

    }
}