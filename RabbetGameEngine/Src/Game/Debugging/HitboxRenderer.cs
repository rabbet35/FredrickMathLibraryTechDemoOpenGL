﻿using OpenTK;
using RabbetGameEngine.Models;
using RabbetGameEngine.Physics;
using RabbetGameEngine.SubRendering;
using System.Collections.Generic;
namespace RabbetGameEngine.Debugging
{
    /*This class handles the rendering of hitboxes for debugging purposes.*/
    public static class HitboxRenderer
    {
        private static Model aabbModelPrefab;

        static HitboxRenderer()
        {
            aabbModelPrefab = new Model(CubePrefab.cubeVertices, LineCombiner.getIndicesForLineQuadCount(6)).setColor(CustomColor.magenta);
        }

        /*called on tick. Adds all of the provided colliders to a list of hitboxes to be dynamically batched and drawn.*/
        public static void addAllHitboxesToBeRendered(List<AABB> worldColliders, Dictionary<int, Entity> entities)
        {
            foreach (AABB hitBox in worldColliders)
            {
                addBoxToBeRendered(hitBox);
            }

            foreach(Entity ent in entities.Values)
            {
                addHitboxToBeRendered(ent.getCollider());
            }
        }

        private static void addHitboxToBeRendered(ICollider hitBox)
        {
            if(hitBox == null)
            {
                return;
            }

            switch(hitBox.getType())
            {
                case ColliderType.aabb:
                    addBoxToBeRendered((AABB)hitBox);
                    break;

                case ColliderType.point:
                    addPointToBeRendered((PointCollider)hitBox);
                    break;
            }
        }

        public static void addBoxToBeRendered(AABB box)
        {
            //add a copy of the aabb line model transformed to aabb collider specs
            Renderer.requestRender(BatchType.lines, null, aabbModelPrefab.copyModel().transformVertices(new Vector3((float)box.extentX * 2, (float)box.extentY * 2, (float)box.extentZ * 2), Vector3.Zero, box.centerVec));
          
        }

        public static void addPointToBeRendered(PointCollider point)
        {
            //TODO: impliment rendering of point colliders as points possibly with lines to show x,y,z units
        }
    }
}
