using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject chunkPrefab;

    public Transform player;

    public VoxelWorld world;

    public int currentChunkX, currentChunkZ;

    public string currentChunkId, lastChunkId;

    public long lastUpdateTime;

    void Start()
    {
        world = new VoxelWorld(chunkPrefab);

        currentChunkX = Mathf.FloorToInt(player.position.x / 16);
        currentChunkZ = Mathf.FloorToInt(player.position.z / 16);

        currentChunkId = currentChunkX + "." + currentChunkZ;
        lastChunkId = currentChunkId;

        //world.PerformWorldUpdateAccordingToPlayerPosition(currentChunkId);
        world.LoadChunksAccordingToPlayerPosition(currentChunkId);
        world.UpdateAllMeshes();
    }

    void Update()
    {
        currentChunkX = Mathf.FloorToInt(player.position.x / 16);
        currentChunkZ = Mathf.FloorToInt(player.position.z / 16);
        if(currentChunkId != (currentChunkX + "." + currentChunkZ))
            currentChunkId = currentChunkX + "." + currentChunkZ;

        if(lastChunkId != currentChunkId)
        {
            print(lastChunkId + " -> " + currentChunkId);
            lastChunkId = currentChunkId;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //world.PerformWorldUpdateAccordingToPlayerPosition(currentChunkId);
            world.LoadChunksAccordingToPlayerPosition(currentChunkId);
            world.UpdateAllMeshes();
            sw.Stop();
            lastUpdateTime = sw.ElapsedMilliseconds;
        }
    }

    void OnGUI()
    {
        GUI.TextField(new Rect(0, 0, 160, 20), "Chunks:\t" + world.loadedChunks.Count);
        GUI.TextField(new Rect(0, 20, 160, 20), "ChunkId:\t" + currentChunkId);
        GUI.TextField(new Rect(0, 40, 160, 20), "Updated:\t" + lastUpdateTime +"ms");
    }

}
