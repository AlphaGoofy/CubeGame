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

    public List<VoxelChunk> loadedChunks;

    public Random random;

    public VoxelWorld(GameObject chunkPrefab)
    {

        random = new Random(1234);

        this.chunkPrefab = chunkPrefab;

        loadedChunks = new List<VoxelChunk>();
    }

    public void PerformWorldUpdateAccordingToPlayerPosition(string currentChunkId)
    {
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
        int currentX = int.Parse(currentChunkId.Split(new char[] { '.' })[0]);
        int currentZ = int.Parse(currentChunkId.Split(new char[] { '.' })[1]);

        List<string> chunksToCheck = new List<string>();

        int chunkViewDistance = 3;

        int startX = currentX - chunkViewDistance;
        int startZ = currentZ - chunkViewDistance;

        int a = chunkViewDistance * 2 + 1;

        for (int x = 0; x < a; x++)
        {
            for (int z = 0; z < a; z++)
            {
                chunksToCheck.Add((startX + x) + "." + (startZ + z));
            }
        }

        foreach (string s in chunksToCheck)
        {
            if (!ChunkLoaded(s))
                LoadChunk(int.Parse(s.Split(new char[] { '.' })[0]), int.Parse(s.Split(new char[] { '.' })[1]));
        }

        List<VoxelChunk> toUnload = new List<VoxelChunk>();

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
        loadedChunks.Add(new VoxelChunk(this, new Vector3(idX * 16, 0, idZ * 16)));
    }

    public void UpdateAllMeshes()
    {
        foreach (VoxelChunk c in loadedChunks)
        {
            GameObject obj;
            if (c.chunkObject == null)
            {
                obj = GameObject.Instantiate(chunkPrefab, new Vector3(c.start.x + .5f, .5f, c.start.z + .5f), Quaternion.identity);
                c.chunkObject = obj;
            } else
                obj = c.chunkObject;

            if(!c.calculatedMesh)
            {
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

    public void UpdateChunkMesh(string id)
    {
        foreach (VoxelChunk c in loadedChunks)
        {
            if (c.chunkId == id)
                c.chunkObject.GetComponent<MeshFilter>().mesh = c.chunkMesh;
        }
    }

    public bool ChunkLoaded(string id)
    {
        foreach(VoxelChunk c in loadedChunks)
        {
            if (c.chunkId == id)
                return true;
        }
        return false;
    }

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
