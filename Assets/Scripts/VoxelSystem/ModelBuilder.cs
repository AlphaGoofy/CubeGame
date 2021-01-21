using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBuilder
{

    private VoxelModel model;

    private GameObject obj;

    public ModelBuilder(GameObject chunkObj)
    {
        this.obj = chunkObj;
    }

    public VoxelModel GetModel()
    {
        return model;
    }

    /*public void CreateBlock(Vector3 v)
    {
        AddFrontFace(v);
        AddBackFace(v);
        AddRightFace(v);
        AddLeftFace(v);
        AddTopFace(v);
        AddBottomFace(v);
    }*/

    public void AddFrontFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(0, 0, 0) + v);
        model.vertices.Add(new Vector3(1, 0, 0) + v);
        model.vertices.Add(new Vector3(1, 1, 0) + v);
        model.vertices.Add(new Vector3(0, 1, 0) + v);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 2);

        if(blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        } else if(blockType == 2)
        {
            model.uv.AddRange(UVManager.GetGrassSideUV());
        } else if(blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void AddBackFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(0, 0, 1) + v);
        model.vertices.Add(new Vector3(1, 0, 1) + v);
        model.vertices.Add(new Vector3(1, 1, 1) + v);
        model.vertices.Add(new Vector3(0, 1, 1) + v);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 1);

        if (blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        }
        else if (blockType == 2)
        {
            model.uv.AddRange(UVManager.GetGrassSideUV());
        }
        else if (blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void AddRightFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(1, 0, 0) + v);
        model.vertices.Add(new Vector3(1, 0, 1) + v);
        model.vertices.Add(new Vector3(1, 1, 1) + v);
        model.vertices.Add(new Vector3(1, 1, 0) + v);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 2);

        if (blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        }
        else if (blockType == 2)
        {
            model.uv.AddRange(UVManager.GetGrassSideUV());
        }
        else if (blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if(blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void AddLeftFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(0, 0, 0) + v);
        model.vertices.Add(new Vector3(0, 0, 1) + v);
        model.vertices.Add(new Vector3(0, 1, 1) + v);
        model.vertices.Add(new Vector3(0, 1, 0) + v);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 1);

        if (blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        }
        else if (blockType == 2)
        {
            model.uv.AddRange(UVManager.GetGrassSideUV());
        }
        else if (blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void AddTopFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(0, 1, 0) + v);
        model.vertices.Add(new Vector3(1, 1, 0) + v);
        model.vertices.Add(new Vector3(1, 1, 1) + v);
        model.vertices.Add(new Vector3(0, 1, 1) + v);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 2);

        if (blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        }
        else if (blockType == 2)
        {
            model.uv.AddRange(UVManager.GetGrassTopUV());
        }
        else if (blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void AddBottomFace(Vector3 v, int blockType)
    {
        int current = model.vertices.Count;

        model.vertices.Add(new Vector3(0, 0, 0) + v);
        model.vertices.Add(new Vector3(1, 0, 0) + v);
        model.vertices.Add(new Vector3(1, 0, 1) + v);
        model.vertices.Add(new Vector3(0, 0, 1) + v);
        model.triangles.Add(current + 3);
        model.triangles.Add(current + 0);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 1);
        model.triangles.Add(current + 2);
        model.triangles.Add(current + 3);

        if (blockType == 1)
        {
            model.uv.AddRange(UVManager.GetStoneUV());
        }
        else if (blockType == 2)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 3)
        {
            model.uv.AddRange(UVManager.GetDirtUV());
        }
        else if (blockType == 4)
        {
            model.uv.AddRange(UVManager.GetWoodUV());
        }
        else if (blockType == 5)
        {
            model.uv.AddRange(UVManager.GetLeafUV());
        }
    }

    public void Start()
    {
        model = new VoxelModel();
    }

    public void Clear()
    {
        model = new VoxelModel();
    }

}
