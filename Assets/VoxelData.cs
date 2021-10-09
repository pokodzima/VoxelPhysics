using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VoxelData: MonoBehaviour
{
    public byte[] linearData;

    public int sizeX;

    public int sizeY;

    public int sizeZ;

    private void Start()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    if (linearData[x + sizeX *(y + sizeY * z)] != 0)
                    {
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x, y, z);
                    }
                }
            }
        }
    }
}
