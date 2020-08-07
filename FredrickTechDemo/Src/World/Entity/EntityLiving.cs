﻿using FredrickTechDemo.FredsMath;
using System;

namespace FredrickTechDemo
{
    public class EntityLiving : Entity
    {
        
        protected Vector3D frontVector;//vector pointing to the direction the entity is facing
        protected Vector3D upVector;
        protected Vector3D movementVector; //a unit vector representing this entity's movement values. z is front and backwards, x is side to side.
        protected bool isJumping = false;
        protected double headRoll; // roll of the living entity head
        public static readonly double defaultWalkSpeed = 0.3572F;
        protected double walkSpeed = defaultWalkSpeed;
        public EntityLiving() : base()
        {
            frontVector = new Vector3D(0.0F, 0.0F, -1.0F);
            upVector = new Vector3D(0.0F, 1.0F, 0.0F);
            movementVector = new Vector3D(0.0F, 0.0F, 0.0F);
        }

        public EntityLiving(Vector3D pos) : base(pos)
        {
            frontVector = new Vector3D(0.0F, 0.0F, -1.0F);
            upVector = new Vector3D(0.0F, 1.0F, 0.0F);
            movementVector = new Vector3D(0.0F, 0.0F, 0.0F);
        }

        public override void onTick()
        {
            base.onTick();//do first

            alignVectors();

            moveByMovementVector();
        }

        /*Changes velocity based on state and movement vector, movement vector is changed by movement functions such as walkFowards()*/
        private void moveByMovementVector()
        {
            //modify walk speed here i.e slows, speed ups etc
            double walkSpeedModified = walkSpeed;

            if (!isGrounded) if (!isFlying) walkSpeedModified = 0.0072D; else walkSpeedModified = 0.02D;//reduce movespeed when jumping or mid air and reduce movespeed when flying as to not accellerate out of control


            //change velocity based on movement
            //movement vector is a unit vector.
            movementVector.Normalize();//normalize vector so player is same speed in any direction
            velocity += frontVector * movementVector.z * walkSpeedModified;//fowards and backwards movement
            velocity += Vector3D.normalize(Vector3D.cross(frontVector, upVector)) * movementVector.x * walkSpeedModified;//strafing movement

            movementVector *= 0;//reset movement vector

            if (isJumping)// if player jumping or flying up
            {
                velocity.y += 0.32D;//jump //TODO make jumping create a vector that maintains movement velocity in the xz directions and reaches 1.25 y in hegiht
                isJumping = false;
            }
        }

        /*When called, aligns vectors according to the entities state and rotations.*/
        private void alignVectors()
        {
            /*correcting front vector based on new pitch and yaw*/
            frontVector.x = (double)(Math.Cos(MathUtil.radians(pitch)) * Math.Cos(MathUtil.radians(headRoll)));
            if (isFlying)
            {
                frontVector.y = (double)Math.Sin(MathUtil.radians(headRoll));
            }
            else
            {//if the living entity isnt flying, it shouldnt be able to move up or down willingly
                frontVector.y = 0;
            }
            frontVector.z = (double)(Math.Sin(MathUtil.radians(pitch)) * Math.Cos(MathUtil.radians(headRoll)));
            frontVector.Normalize();
        }

        public void jump()
        {
            if (isGrounded && velocity.y <= 0)
            {
              
                isJumping = true;
            }
        }
        public void walkFowards()
        {
            movementVector.z++;
        }
        public void walkBackwards()
        {
            movementVector.z--;
        }
        public void strafeRight()
        {
            movementVector.x++;
        }
        public void strafeLeft()
        {
            movementVector.x--;
        }

        public void setHeadRoll(double roll)
        {
            this.headRoll = roll;
        }
        public double getHeadPitch()
        {
            return this.headRoll;
        }
        public Vector3D getFrontVector()
        {
            return this.frontVector;
        }

        public Vector3D getUpVector()
        {
            return this.upVector;
        }

        
    }
}