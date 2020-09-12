﻿using OpenTK;
using System;
namespace Coictus
{
    /*Class for the player. Contains the players name, inventory etc.*/
    public class EntityPlayer : EntityLiving
    {
        private String playerName;
        private Camera camera;
        public bool paused = false;
        public bool debugScreenOn = false;


        public static readonly Vector3 eyeOffset = new Vector3(0.0F, 0.62F, 0.0F);
        public EntityPlayer(String name) : base()
        {
            isPlayer = true;
            this.playerName = name;
            camera = new Camera(this);
            this.setCollider(new AABBCollider(new Vector3(-0.5F, -1, -0.5F), new Vector3(0.5F, 1, 0.5F), this));
        }
        public EntityPlayer(String name, Vector3 spawnPosition) : base(spawnPosition)
        {
            isPlayer = true;
            this.playerName = name;
            camera = new Camera(this);
            this.setCollider(new AABBCollider(new Vector3(-0.5F, -1, -0.5F), new Vector3(0.5F, 1, 0.5F), this));
        }

        public override void onTick()
        {
            if (!paused)
            {
                base.onTick();//do first
            }
            camera.onTick();
        }

        public Vector3 getEyePosition()
        {
            return pos + EntityPlayer.eyeOffset;
        }

        public Vector3 getLerpEyePos()
        {
            return this.getLerpPos() + EntityPlayer.eyeOffset;
        }

        /*Called before game renders, each frame.*/
        public void onCameraUpdate()
        {
            if (!paused)
            {
                this.camera.onUpdate();
            }
        }

        public String getName()
        {
            return this.playerName;
        }

        public void togglePause()
        {
            if(!paused)
            {
                paused = true;
                TicksAndFps.pause();
            }
            else
            {
                paused = false;
                TicksAndFps.unPause();
            }
        }

        public override void applyCollision(Vector3 direction, float overlap)
        {
            if(!GameSettings.noclip)
            base.applyCollision(direction, overlap);
        }
        public Matrix4 getViewMatrix()
        {
            return camera.getViewMatrix();
        }
        public Camera getCamera()
        {
            return this.camera;
        }

        
    }
}
