using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Prefab to create new chunks
    public GameObject chunkPrefab;
    
    public Transform player;
    // Reference to the current world, no MonoBehaviour 
    public VoxelWorld world;
    // Current id of the chunk the player is in
    public int currentChunkX, currentChunkZ;
    
    public string currentChunkId, lastChunkId;
    // Time to update all chunks
    public long lastUpdateTime;

    void Start()
    {
        // Create a new world and determine the chunk the player is in
        world = new VoxelWorld(chunkPrefab);

        currentChunkX = Mathf.FloorToInt(player.position.x / 16);
        currentChunkZ = Mathf.FloorToInt(player.position.z / 16);

        currentChunkId = currentChunkX + "." + currentChunkZ;
        lastChunkId = currentChunkId;

        //world.PerformWorldUpdateAccordingToPlayerPosition(currentChunkId);
        // Create and update the meshes around the player
        world.LoadChunksAccordingToPlayerPosition(currentChunkId);
        world.UpdateAllMeshes();
    }

    void Update()
    {
        // Check whether the player has entered a new chunk or not
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
            // Load the new chunks and update the meshes
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
