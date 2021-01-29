using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVManager
{

    // Holds arrays of uv coordinates

    public static Vector2[] GetStoneUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0f, .66666f);
        uv[1] = new Vector2(.5f, .66666f);
        //--
        uv[2] = new Vector2(.5f, 1f);
        uv[3] = new Vector2(0f, 1f);
        return uv;
    }

    public static Vector2[] GetGrassTopUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(.5f, .66666f);
        uv[1] = new Vector2(1f, .66666f);
        //--
        uv[2] = new Vector2(1f, 1f);
        uv[3] = new Vector2(.5f, 1f);
        return uv;
    }

    public static Vector2[] GetGrassSideUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0f, .33333f);
        uv[1] = new Vector2(.5f, .33333f);
        //--
        uv[2] = new Vector2(.5f, .66666f);
        uv[3] = new Vector2(0f, .66666f);
        return uv;
    }

    public static Vector2[] GetDirtUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(.5f, .33333f);
        uv[1] = new Vector2(1f, .33333f);
        //--
        uv[2] = new Vector2(1f, .66666f);
        uv[3] = new Vector2(.5f, .66666f);
        return uv;
    }

    public static Vector2[] GetWoodUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0f, 0f);
        uv[1] = new Vector2(.5f, 0f);
        //--
        uv[2] = new Vector2(.5f, .33333f);
        uv[3] = new Vector2(0f, .33333f);
        return uv;
    }

    public static Vector2[] GetLeafUV()
    {
        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(.5f, 0f);
        uv[1] = new Vector2(1f, 0f);
        //--
        uv[2] = new Vector2(1f, .33333f);
        uv[3] = new Vector2(.5f, .33333f);
        return uv;
    }

}
