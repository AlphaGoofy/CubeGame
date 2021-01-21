using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestBuilderScript : MonoBehaviour
{
    public GameObject chunkPrefab;

    /*List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uv = new List<Vector2>();*/

    void Start()
    {
        /*Stopwatch sw = new Stopwatch();
        sw.Start();
        new VoxelWorld(chunkPrefab);
        sw.Stop();
        print("Time: " + sw.ElapsedMilliseconds);
        */
        /*Mesh mesh = new Mesh();

        ModelBuilder mB = new ModelBuilder();
        mB.Start();

        mB.CreateBlock(Vector3.zero);
        mB.CreateBlock(Vector3.up);
        mB.CreateBlock(Vector3.one);

        mesh.vertices = mB.GetModel().vertices.ToArray();
        mesh.triangles = mB.GetModel().triangles.ToArray();
        mesh.uv = mB.GetModel().uv.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        mB.Clear();*/

        GameObject obj = Instantiate(chunkPrefab);

        obj.AddComponent(typeof(MeshCollider));

    }

    void Foo()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                int current = vertices.Count;

                vertices.Add(new Vector3(x, 0, z));
                vertices.Add(new Vector3(x + 1, 0, z));
                vertices.Add(new Vector3(x + 1, 0, z + 1));
                vertices.Add(new Vector3(x, 0, z + 1));

                /*
                vertices.Add(new Vector3(x + 1, 0, z + 1));
                vertices.Add(new Vector3(x + 1, 0, z));

                vertices.Add(new Vector3(x, 0, z));
                vertices.Add(new Vector3(x, 0, z + 1));
                */

                triangles.Add(current); triangles.Add(current + 1); triangles.Add(current + 2);
                triangles.Add(current + 2); triangles.Add(current + 3); triangles.Add(current + 0);

                if (x % 2 == 0 && z % 2 == 0 || x % 2 != 0 && z % 2 != 0)
                {
                    uv.Add(new Vector2(.5f, 0f));
                    uv.Add(new Vector2(0f, 0f));

                    uv.Add(new Vector2(0f, .5f));
                    uv.Add(new Vector2(.5f, .5f));
                }
                else
                {
                    uv.Add(new Vector2(.5f, .5f));
                    uv.Add(new Vector2(0f, .5f));

                    uv.Add(new Vector2(0f, 1f));
                    uv.Add(new Vector2(.5f, 1f));
                }
            }
        }

        /*ModelCache modelCache = new ModelCache(vertices, triangles, uv);
        modelCache.Merge();

        Mesh mesh = new Mesh();
        mesh.vertices = modelCache.vertices.ToArray();
        mesh.triangles = modelCache.triangles.ToArray();
        mesh.uv = modelCache.uv.ToArray();

        meshFilter.mesh = mesh;*/

        /*for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                Vector3[] vertices = new Vector3[4];
                Vector2[] uv = new Vector2[4];
                vertices[0] = new Vector3(x, 0, z);
                vertices[1] = new Vector3(x + 1, 0, z);
                vertices[2] = new Vector3(x + 1, 0, z + 1);
                vertices[3] = new Vector3(x, 0, z + 1);

                triangles.Add(0);triangles.Add(1);triangles.Add(2);
                triangles.Add(2);triangles.Add(3);triangles.Add(0);
                
                if (x % 2 == 0 || z % 2 == 0)
                {
                    uv[0] = new Vector2(0f, 0f);
                    uv[1] = new Vector2(.5f, 0f);
                    uv[2] = new Vector2(.5f, .5f);
                    uv[3] = new Vector2(0f, .5f);
                } else
                {
                    uv[0] = new Vector2(.5f, .5f);
                    uv[1] = new Vector2(1f, .5f);
                    uv[2] = new Vector2(1f, 1f);
                    uv[3] = new Vector2(.5f, 1f);
                }

                this.vertices.AddRange(vertices);
                this.uv.AddRange(uv);
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();

        meshFilter.mesh = mesh;*/
    }

    void Update()
    {
        
    }
}
