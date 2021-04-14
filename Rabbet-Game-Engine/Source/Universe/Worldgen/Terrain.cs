﻿using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace RabbetGameEngine
{
    public class Terrain
    {
        //radius of chunks to initially generate around origin/spawn
        public static readonly int spawnChunkRadius = 8;

        private Dictionary<Vector3i, Chunk> chunkMap = null;
        
        public Terrain(Random rand)
        {
            chunkMap = new Dictionary<Vector3i, Chunk>();
        }

        /// <summary>
        /// returns voxel type at the provided world coordinates. returns null for air or if coordinate is in null chunk.
        /// </summary>
        public VoxelType getVoxelAt(int x, int y, int z)
        {
            Chunk c = getChunkAtCoord(x, y, z);
            return c == null ? null : VoxelType.getVoxelById(c.getVoxelAt(x & Chunk.CHUNK_SIZE_MINUS_ONE, y & Chunk.CHUNK_SIZE_MINUS_ONE, z & Chunk.CHUNK_SIZE_MINUS_ONE));
        }

        /// <summary>
        /// returns voxel ID at the provided world coordinates. returns 0 for air or if coordinate is in null chunk.
        /// </summary>
        public byte getVoxelIdAt(int x, int y, int z)
        {
            Chunk c = getChunkAtCoord(x, y, z);
            return c == null ? (byte)0 : c.getVoxelAt(x & Chunk.CHUNK_SIZE_MINUS_ONE, y & Chunk.CHUNK_SIZE_MINUS_ONE, z & Chunk.CHUNK_SIZE_MINUS_ONE);
        }

        public void setVoxelIdAt(int x, int y, int z, byte id)
        {
            Chunk c = getChunkAtCoord(x, y, z);
            if(c != null) 
                c.setVoxelAt(x & Chunk.CHUNK_SIZE_MINUS_ONE, y & Chunk.CHUNK_SIZE_MINUS_ONE, z & Chunk.CHUNK_SIZE_MINUS_ONE, id);
        }


        public Chunk getChunkAtCoord(float x, float y, float z)
        {
            if (chunkMap.TryGetValue(getChunkCoord(new Vector3(x,y,z)), out Chunk c))
            {
                return c;
            }
            return null;
        }

        public Chunk getChunkAtCoord(Vector3 pos)
        {
            if(chunkMap.TryGetValue(getChunkCoord(pos), out Chunk c))
            {
                return c;
            }
            return null;
        }

        public Vector3i getChunkCoord(Vector3 pos)
        {
            pos /= Chunk.CHUNK_SIZE;
            return new Vector3i((int)pos.X, (int)pos.Y, (int)(pos.Z));
        }

        public void generateSpawnChunks(Vector3 spawnPos)
        {
            Vector3i origin = getChunkCoord(spawnPos);
            for (int x = 0; x < spawnChunkRadius; x++)
                for (int z = 0; z < spawnChunkRadius; z++)
                {
                    chunkMap.Add(new Vector3i(x - spawnChunkRadius/2, 0, z - spawnChunkRadius / 2), new Chunk(x - spawnChunkRadius / 2, 0, z - spawnChunkRadius / 2));
                }
        }
        public void unLoad()
        {
            foreach(Chunk c in chunkMap.Values)
            {
                c.unLoad();
            }
        }

        public Dictionary<Vector3i, Chunk> chunks
        {
            get => chunkMap;
        }
    }
}
