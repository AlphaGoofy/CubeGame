using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelChunk
{
    // The representation of a chunk

    // The actual GameObject
    public GameObject chunkObject;

    public VoxelWorld world;

    public Vector3 start; // Position of the chunk in the world

    public Mesh chunkMesh = null;

    public int[,,] chunkBlocks = new int[16, 16, 16];//0 Luft; 1 Stein; 2 Gras; 3 Erde

    public float noiseScale = 1.0f;

    public string chunkId;

    public int idX, idZ;

    public bool calculatedMesh = false; // Used to tell the world whether this chunk needs to be recalculated or not

    public VoxelChunk(VoxelWorld world, Vector3 start)
    {
        this.world = world;
        this.start = start;
        this.idX = Mathf.FloorToInt(start.x / 16);
        this.idZ = Mathf.FloorToInt(start.z / 16);
        chunkId = idX + "." + idZ; 

        float[,] height = new float[16, 16]; // Height map

        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                //Coordinate / Size (Value between 0 and 1) * scale + offset
                float h = Mathf.PerlinNoise((float) x / 16 * noiseScale + start.x, (float) z / 16 * noiseScale + start.z);
                height[x, z] = h;
            }
        }

        for (int x = 0; x < 16; x++) // Everything below the highest solid block will be stone
        {
            for (int z = 0; z < 16; z++)
            {
                int a = (int)Mathf.Clamp(height[x, z] * 10f, 0f, 15f);

                //---
                
                /*int b;
                if ((b = world.random.Next(0, 200)) > 50 && (b < 55) && Valid(new Vector3Int(x, a + 1, z)))//Test-Baum bauen
                {
                    chunkBlocks[x, a + 1, z] = 4;

                    for (int i = a + 1; i < a + 4; i++)
                    {
                        if (Valid(new Vector3Int(x, i, z)))
                            chunkBlocks[x, i, z] = 4;
                    }

                    for(int i = -2; i < 3; i++)
                    {
                        for (int j = -2; j < 3; j++)
                        {
                            for (int k = a + 4; k < a + 7; k++)
                            {
                                if (Valid(new Vector3Int(x + i, k, z + j)))
                                {
                                    chunkBlocks[x + i, k, z + j] = 5;
                                }
                            }
                        }
                    }

                }*/
                //---

                chunkBlocks[x, a, z] = 2; // The highest block will be set to grass

                for (int i = (a-1); i > -1; i--) // Iterate over all blocks beneath the grass
                {
                    if((a-i) <= 2) // Dirt below the grass
                    {
                        chunkBlocks[x, i, z] = 3;
                    } else // Everything lower is set to stone
                    {
                        chunkBlocks[x, i, z] = 1;
                    } 
                }
            }
        }
    }

    public void CalculateChunkMesh() // Iterates over all blocks to only add visible faces to the model
    {
        List<Vector3Int> airBlocks = new List<Vector3Int>(); // All emtpy blocks in the chunk

        // Store all block of air
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    if (chunkBlocks[x, y, z] == 0)
                        airBlocks.Add(new Vector3Int(x, y, z));
                }
            }
        }

        ModelBuilder modelBuilder = new ModelBuilder();

        modelBuilder.Start();

        foreach(Vector3Int v in airBlocks)
        {
            // Check the neighbours of the air, a block can only be seen if has air next to one side
            // Valid() will return true if the block is still inside of the chunk
            if (Valid(v + new Vector3Int(1, 0, 0)) && chunkBlocks[v.x + 1, v.y, v.z] != 0)
                modelBuilder.AddLeftFace(v + new Vector3Int(1, 0, 0), chunkBlocks[v.x + 1, v.y, v.z]); // If the block right to the air is solid a left face has to be created
            if (Valid(v + new Vector3Int(-1, 0, 0)) && chunkBlocks[v.x - 1, v.y, v.z] != 0)
                modelBuilder.AddRightFace(v + new Vector3Int(-1, 0, 0), chunkBlocks[v.x - 1, v.y, v.z]);
            if (Valid(v + new Vector3Int(0, 0, 1)) && chunkBlocks[v.x, v.y, v.z + 1] != 0)
                modelBuilder.AddFrontFace(v + new Vector3Int(0, 0, 1), chunkBlocks[v.x, v.y, v.z + 1]);
            if (Valid(v + new Vector3Int(0, 0, -1)) && chunkBlocks[v.x, v.y, v.z - 1] != 0)
                modelBuilder.AddBackFace(v + new Vector3Int(0, 0, -1), chunkBlocks[v.x, v.y, v.z - 1]);
            if (Valid(v + new Vector3Int(0, 1, 0)) && chunkBlocks[v.x, v.y + 1, v.z] != 0)
                modelBuilder.AddBottomFace(v + new Vector3Int(0, 1, 0), chunkBlocks[v.x, v.y + 1, v.z]);
            if (Valid(v + new Vector3Int(0, -1, 0)) && chunkBlocks[v.x, v.y - 1, v.z] != 0)
                modelBuilder.AddTopFace(v + new Vector3Int(0, -1, 0), chunkBlocks[v.x, v.y - 1, v.z]);
        }

        // The outer blocks need to be checked extra, because they might be hidden by another chunk
        if (world.ChunkLoaded(Mathf.FloorToInt(start.x / 16) + "." + (Mathf.FloorToInt(start.z / 16) - 1)))
        {
            VoxelChunk chunk = world.GetChunk(Mathf.FloorToInt(start.x / 16) + "." + (Mathf.FloorToInt(start.z / 16) - 1)); // The chunk next to this chunk
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (chunk.chunkBlocks[x, y, 15] == 0 && chunkBlocks[x, y, 0] != 0) // If the block is not of type air and also not covered by the other chunk, a face has to be created
                        modelBuilder.AddFrontFace(new Vector3(x, y, 0), chunkBlocks[x, y, 0]);
                }
            }
        }
        // The same as the code above, only for another side
        if (world.ChunkLoaded(Mathf.FloorToInt(start.x / 16) + "." + (Mathf.FloorToInt(start.z / 16) + 1)))
        {
            VoxelChunk chunk = world.GetChunk(Mathf.FloorToInt(start.x / 16) + "." + (Mathf.FloorToInt(start.z / 16) + 1));
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (chunk.chunkBlocks[x, y, 0] == 0 && chunkBlocks[x, y, 15] != 0)
                        modelBuilder.AddBackFace(new Vector3(x, y, 15), chunkBlocks[x, y, 15]);
                }
            }
        }
        // The same as the code above, only for another side
        if (world.ChunkLoaded((Mathf.FloorToInt(start.x / 16) - 1) + "." + Mathf.FloorToInt(start.z / 16)))
        {
            VoxelChunk chunk = world.GetChunk((Mathf.FloorToInt(start.x / 16) - 1) + "." + Mathf.FloorToInt(start.z / 16));
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (chunk.chunkBlocks[15, y, z] == 0 && chunkBlocks[0, y, z] != 0)
                        modelBuilder.AddLeftFace(new Vector3(0, y, z), chunkBlocks[0, y, z]);
                }
            }
        }
        // The same as the code above, only for another side
        if (world.ChunkLoaded((Mathf.FloorToInt(start.x / 16) + 1) + "." + Mathf.FloorToInt(start.z / 16)))
        {
            VoxelChunk chunk = world.GetChunk((Mathf.FloorToInt(start.x / 16) + 1) + "." + Mathf.FloorToInt(start.z / 16));
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (chunk.chunkBlocks[0, y, z] == 0 && chunkBlocks[15, y, z] != 0)
                        modelBuilder.AddRightFace(new Vector3(15, y, z), chunkBlocks[15, y, z]);
                }
            }
        }

        // Recreate the mesh
        chunkMesh = new Mesh();
        // Set all vertices, triangles and uvs
        chunkMesh.vertices = modelBuilder.GetModel().vertices.ToArray();
        chunkMesh.triangles = modelBuilder.GetModel().triangles.ToArray();
        chunkMesh.uv = modelBuilder.GetModel().uv.ToArray();

        // Add a collider if there is none
        MeshCollider collider;
        if (chunkObject.GetComponent<MeshCollider>() == null)
            collider = (MeshCollider)chunkObject.AddComponent(typeof(MeshCollider));
        else
            collider = chunkObject.GetComponent<MeshCollider>();

        collider.sharedMesh = chunkMesh; // Set the mesh
        //
        chunkMesh.RecalculateNormals(); // Recalculate the normals to avoid lightning problems

        modelBuilder.Clear();

        calculatedMesh = true; // The mesh is calculated and can be used by other chunks to check if this chunk covers any of their blocks

    }

    private bool Valid(Vector3Int v) // Checks if a position is still inside the chunk
    {
        return v.x >= 0 && v.x < 16 && v.y >= 0 && v.y < 16 && v.z >= 0 && v.z < 16;
    }

}
