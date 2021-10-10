using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelData: MonoBehaviour
{
    public byte[] linearData;

    public int sizeX;

    public int sizeY;

    public int sizeZ;

    private void Start()
    {
        GetComponent<Chunk>().CreateChunk(new Vector3(sizeX,sizeY,sizeZ),transform.position,linearData );    
    }
}
