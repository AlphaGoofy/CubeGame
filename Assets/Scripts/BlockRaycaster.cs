using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRaycaster : MonoBehaviour
{

    public LayerMask layerMask;

    public GameManager gameManager;

    void Start()
    {
        
    }

    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

            //Debug.DrawRay(ray.origin, ray.direction * 20f, Color.red, 5.0f);

            if (Physics.Raycast(ray, out RaycastHit hit, 20.0f, layerMask))
            {

                print("---");

                //Vector3Int v = new Vector3Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.y), Mathf.FloorToInt(hit.point.z));

                Vector3 vector = new Vector3(hit.point.x, hit.point.y, hit.point.z) - ray.direction * .25f;

                int iX = Mathf.FloorToInt(hit.point.x / 16);// * (v.x < 0 ? -1 : 1);
                int iZ = Mathf.FloorToInt(hit.point.z / 16);// * (v.z < 0 ? -1 : 1);

                string chunkId = iX + "." + iZ;

                print(chunkId);

                int maxA = iX * 16;
                int maxB = iZ * 16;

                if (maxA < 0)
                    maxA *= -1;
                if (maxB < 0)
                    maxB *= -1;

                print(maxA + "|" + maxB);

                int lX = Mathf.FloorToInt(vector.x);
                int lZ = Mathf.FloorToInt(vector.z);

                if (lX < 0)
                    lX *= -1;
                if (lZ < 0)
                    lZ *= -1;

                int x = Mathf.Max(lX, maxA) - Mathf.Min(lX, maxA);
                int z = Mathf.Max(lZ, maxB) - Mathf.Min(lZ, maxB);
                
                print(x + "@" + z);

                gameManager.world.GetChunk(chunkId).chunkBlocks[x, Mathf.FloorToInt(vector.y), z] = 1;
                gameManager.world.GetChunk(chunkId).CalculateChunkMesh();
                gameManager.world.UpdateAllMeshes();
            }
        }
    }
}
