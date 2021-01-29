using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class VoxelWorld
{
    public GameObject chunkPrefab;

    public List<VoxelChunk> loadedChunks; // All active chunks

    public Random random;

    public VoxelWorld(GameObject chunkPrefab)
    {

        random = new Random(1234);

        this.chunkPrefab = chunkPrefab;

        loadedChunks = new List<VoxelChunk>();
    }

    // Remove
    public void PerformWorldUpdateAccordingToPlayerPosition(string currentChunkId)
    {
        // Updates the world based on the players position

        int currentX = int.Parse(currentChunkId.Split(new char[] { '.' })[0]);
        int currentZ = int.Parse(currentChunkId.Split(new char[] { '.' })[1]);

        List<string> chunksToCheck = new List<string>();

        chunksToCheck.Add(currentChunkId);
        chunksToCheck.Add((currentX + 1) + "." + (currentZ));
        chunksToCheck.Add((currentX - 1) + "." + (currentZ));
        chunksToCheck.Add((currentX) + "." + (currentZ + 1));
        chunksToCheck.Add((currentX) + "." + (currentZ - 1));
        chunksToCheck.Add((currentX + 1) + "." + (currentZ + 1));
        chunksToCheck.Add((currentX - 1) + "." + (currentZ + 1));
        chunksToCheck.Add((currentX + 1) + "." + (currentZ - 1));
        chunksToCheck.Add((currentX - 1) + "." + (currentZ - 1));

        foreach(string s in chunksToCheck)
        {
            if (!ChunkLoaded(s))
                LoadChunk(int.Parse(s.Split(new char[] { '.' })[0]), int.Parse(s.Split(new char[] { '.' })[1]));
        }

        List<VoxelChunk> toUnload = new List<VoxelChunk>();

        foreach(VoxelChunk c in loadedChunks)
        {
            bool b = false;
            foreach(string s in chunksToCheck)
            {
                if (s == c.chunkId)
                    b = true;
            }
            if (!b)
            {
                GameObject.Destroy(c.chunkObject);
                toUnload.Add(c);
            }   
        }

        foreach(VoxelChunk c in toUnload)
        {
            loadedChunks.Remove(c);
        }
    }

    public void LoadChunksAccordingToPlayerPosition(string currentChunkId)
    {
        // Loads new chunks according to the players positon
        int currentX = int.Parse(currentChunkId.Split(new char[] { '.' })[0]);
        int currentZ = int.Parse(currentChunkId.Split(new char[] { '.' })[1]);

        List<string> chunksToCheck = new List<string>(); // All chunks which needs to be checked

        int chunkViewDistance = 3; // The amount of chunks which will be loaded in each direction starting from the players position

        int startX = currentX - chunkViewDistance;
        int startZ = currentZ - chunkViewDistance;

        int a = chunkViewDistance * 2 + 1; // Start from the lowest left chunk, because the chunkViewDistance based on the players position (center) it has to be doubled

        for (int x = 0; x < a; x++)
        {
            for (int z = 0; z < a; z++)
            {
                chunksToCheck.Add((startX + x) + "." + (startZ + z)); // All chunks which will be the active area
            }
        }

        // If there is any new chunk it has to be created
        foreach (string s in chunksToCheck)
        {
            if (!ChunkLoaded(s))
                LoadChunk(int.Parse(s.Split(new char[] { '.' })[0]), int.Parse(s.Split(new char[] { '.' })[1]));
        }

        List<VoxelChunk> toUnload = new List<VoxelChunk>();
        // All chunks which are out of range by the new chunk center chunk have to be destroyed (or cached and unloaded)
        // Chunks are not instantly removed from the list to avoid concurrent modifications
        foreach (VoxelChunk c in loadedChunks)
        {
            bool b = false;
            foreach (string s in chunksToCheck)
            {
                if (s == c.chunkId)
                    b = true;
            }
            if (!b)
            {
                GameObject.Destroy(c.chunkObject);
                toUnload.Add(c);
            }
        }

        foreach (VoxelChunk c in toUnload)
        {
            loadedChunks.Remove(c);
        }
    }

    public void LoadChunk(int idX, int idZ)
    {
        // Load a specific chunk
        loadedChunks.Add(new VoxelChunk(this, new Vector3(idX * 16, 0, idZ * 16)));
    }

    public void UpdateAllMeshes()
    {
        // Updates the faces of all meshes
        foreach (VoxelChunk c in loadedChunks)
        {
            GameObject obj;
            if (c.chunkObject == null) // If the chunk has no GameObject one will be created
            {
                obj = GameObject.Instantiate(chunkPrefab, new Vector3(c.start.x + .5f, .5f, c.start.z + .5f), Quaternion.identity);
                c.chunkObject = obj;
            } else
                obj = c.chunkObject;

            if(!c.calculatedMesh)
            {
                // Calculate the mesh and recalculate the neighbour chunks, if existing, to remove any faces which are not visible, or set faces which are now visible
                c.CalculateChunkMesh();
                if (ChunkLoaded((c.idX + 1) + "." + (c.idZ)) && GetChunk((c.idX + 1) + "." + c.idZ).calculatedMesh)
                {
                    GetChunk((c.idX + 1) + "." + c.idZ).CalculateChunkMesh();
                }
                if (ChunkLoaded((c.idX - 1) + "." + (c.idZ)) && GetChunk((c.idX - 1) + "." + c.idZ).calculatedMesh)
                {
                    GetChunk((c.idX - 1) + "." + c.idZ).CalculateChunkMesh();
                }
                if (ChunkLoaded((c.idX) + "." + (c.idZ + 1)) && GetChunk((c.idX) + "." + (c.idZ + 1)).calculatedMesh)
                {
                    GetChunk((c.idX) + "." + (c.idZ + 1)).CalculateChunkMesh();
                }
                if (ChunkLoaded((c.idX) + "." + (c.idZ - 1)) && GetChunk((c.idX) + "." + (c.idZ - 1)).calculatedMesh)
                {
                    GetChunk((c.idX) + "." + (c.idZ - 1)).CalculateChunkMesh();
                }
            }

            obj.GetComponent<MeshFilter>().mesh = c.chunkMesh;

            c.chunkObject = obj;
        }
    }

    // Update a specific chunk
    public void UpdateChunkMesh(string id)
    {
        foreach (VoxelChunk c in loadedChunks)
        {
            if (c.chunkId == id)
                c.chunkObject.GetComponent<MeshFilter>().mesh = c.chunkMesh;
        }
    }

    // Check if a chunk is currently loaded
    public bool ChunkLoaded(string id)
    {
        foreach(VoxelChunk c in loadedChunks)
        {
            if (c.chunkId == id)
                return true;
        }
        return false;
    }

    // Try to directly get a specific chunk
    public VoxelChunk GetChunk(string id)
    {
        foreach (VoxelChunk c in loadedChunks)
        {
            if (c.chunkId.Equals(id))
                return c;
        }
        return null;
    }

}
