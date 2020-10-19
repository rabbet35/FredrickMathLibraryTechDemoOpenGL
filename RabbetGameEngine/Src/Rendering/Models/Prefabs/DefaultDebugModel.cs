﻿
using OpenTK;
using RabbetGameEngine.SubRendering;
using System;
namespace RabbetGameEngine.Models
{

    //just an ugly cube for when a model fails to load
    public static class DefaultDebugModel
    {
        public static string shaderName = "EntityWorld_F";
        public static readonly Vertex[] vertices = new Vertex[]
        {   
            //zPos
            new Vertex(-0.5F, -0.5F, 0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.0F, 0.5F),//0
            new Vertex(0.5F, -0.5F, 0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.5F, 0.5F),//1
            new Vertex(-0.5F, 0.5F, 0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.0F, 1F),//2
            new Vertex(0.5F, 0.5F, 0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.5F, 1F),//3
            //zNeg
            new Vertex(0.5F, -0.5F, -0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.0F, 0.5F),//0
            new Vertex(-0.5F, -0.5F, -0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.5F, 0.5F),//1
            new Vertex(0.5F, 0.5F, -0.5F, 0.8F, 0.8F, 0.8F, 1F,  0.0F, 1F),//2
            new Vertex(-0.5F, 0.5F, -0.5F, 0.8F, 0.8F, 0.8F, 1F, 0.5F, 1F),//3
            //xPos
            new Vertex(0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.0F, 0.5F),//0
            new Vertex(0.5F, -0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.5F, 0.5F),//1
            new Vertex(0.5F,0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.0F, 1F),//2
            new Vertex(0.5F, 0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.5F, 1F),//3
            //xNeg
            new Vertex(-0.5F, -0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.0F, 0.5F),//0
            new Vertex(-0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.5F, 0.5F),//1
            new Vertex(-0.5F,0.5F, -0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.0F, 1F),//2
            new Vertex(-0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 0.5F, 1F, 0.5F, 1F),//3
            //yPos
            new Vertex(0.5F, 0.5F, -0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 0.5F, 0.5F),//0
            new Vertex(-0.5F, 0.5F, -0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.5F),//1
            new Vertex(0.5F, 0.5F, 0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 0.5F, 1F),//2
            new Vertex(-0.5F, 0.5F, 0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1F),//3
            //yNeg
            new Vertex(-0.5F, -0.5F, -0.5F, 0.4F, 0.4F, 0.4F, 1F, 0.0F, 0F),//0
            new Vertex(0.5F, -0.5F, -0.5F, 0.4F, 0.4F, 0.4F, 1F, 0.5F, 0F),//1
            new Vertex(-0.5F, -0.5F, 0.5F, 0.4F, 0.4F, 0.4F, 1F, 0.0F, 0.5F),//2
            new Vertex(0.5F, -0.5F, 0.5F, 0.4F, 0.4F, 0.4F, 1F, 0.5F, 0.5F)//3
        };

        public static readonly uint[] indices = QuadBatcher.getIndicesForQuadCount(6);

        /*generates a new model using copies of this models arrays.*/
        public static Model getNewModel()
        {
            Vertex[] verticesCopy = new Vertex[vertices.Length];
            Array.Copy(vertices, verticesCopy, vertices.Length);
            return new Model(verticesCopy).translateVertices(new Vector3(0, 0.5F, 0));
        }
        public static ModelDrawable getNewModelDrawable()
        {
            Vertex[] verticesCopy = new Vertex[vertices.Length];
            uint[] indicesCopy = new uint[indices.Length];
            Array.Copy(vertices, verticesCopy, vertices.Length);
            Array.Copy(indices, indicesCopy, indices.Length);
            return (ModelDrawable)new ModelDrawable(shaderName, "debug", verticesCopy, indicesCopy).translateVertices(new Vector3(0, 0.5F, 0));
        }


    }
}