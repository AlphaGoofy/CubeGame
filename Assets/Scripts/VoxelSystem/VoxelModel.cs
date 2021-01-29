using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelModel
{

    // Holds the data a chunk needs to create its mesh, maybe replace with struct

    public List<Vector3> vertices;
    public List<int> triangles;
    public List<Vector2> uv;

    public VoxelModel()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
        uv = new List<Vector2>();
    }

}
