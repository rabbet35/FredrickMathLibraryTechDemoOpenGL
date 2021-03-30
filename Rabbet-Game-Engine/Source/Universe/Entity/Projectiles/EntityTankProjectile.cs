﻿using OpenTK.Mathematics;
using RabbetGameEngine.Models;
using System;
namespace RabbetGameEngine
{
    class EntityTankProjectile : EntityProjectile
    {
        public EntityTankProjectile(World p, Vector3 pos, Vector3 direction, float barrelPitch, float initialYaw) : base(p, pos, direction)
        {
            this.yaw = initialYaw ;
            this.pitch = barrelPitch;
            this.entityModel = new EntityTankProjectileModel(this);
            this.hasModel = true;
        }

        public override void onTick()
        {
            base.onTick();

            //rotating projectile based on velocity to simulate areodynamics
            if(velocity.Length >= 0.00001F)
            {
                Vector3 velocityNormalVec = Vector3.Normalize(velocity);
                yaw = (float)((Math.Atan2(velocityNormalVec.Z, velocityNormalVec.X) * 180) / Math.PI);
                pitch = (float)(Math.Atan2(velocityNormalVec.Y, velocityNormalVec.Xz.Length) * (180D / Math.PI));
            }
        }

        public override void ceaseToExist()
        {
            this.currentPlanet.doExplosionAt(previousTickPos);
            base.ceaseToExist();
        }
    }
}
