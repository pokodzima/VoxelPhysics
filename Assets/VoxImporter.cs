using System;
using System.Collections;
using System.Collections.Generic;
using CsharpVoxReader;
using CsharpVoxReader.Chunks;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Material = UnityEngine.Material;
using Object = UnityEngine.Object;

[ScriptedImporter(1,"vox")]
public class VoxImporter : ScriptedImporter,IVoxLoader
{
    private GameObject model;
    public override void OnImportAsset(AssetImportContext ctx)
    {
        model = new GameObject();
        model.AddComponent<VoxelData>();
        model.AddComponent<Chunk>();

        VoxReader reader = new VoxReader(ctx.assetPath,this);
        reader.Read();
        
        ctx.AddObjectToAsset("VoxelModel",model);
        ctx.SetMainObject(model);
    }

    public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
    {
        VoxelData voxelData = model.GetComponent<VoxelData>();
        voxelData.linearData = new byte[sizeX*sizeY*sizeZ];
        int index = 0;
        for (int z = 0; z < sizeX; z++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    voxelData.linearData[index] = data[x, y, z];
                    index++;
                }
            }
        }
        Debug.Log(index);
        voxelData.sizeX = sizeX;
        voxelData.sizeY = sizeY;
        voxelData.sizeZ = sizeZ;
    }

    public void LoadPalette(uint[] palette)
    {
        
    }

    public void SetModelCount(int count)
    {
        
    }

    public void SetMaterialOld(int paletteId, MaterialOld.MaterialTypes type, float weight, MaterialOld.PropertyBits property, float normalized)
    {
        
    }

    public void NewTransformNode(int id, int childNodeId, int layerId, string name, Dictionary<string, byte[]>[] framesAttributes)
    {
        
    }

    public void NewGroupNode(int id, Dictionary<string, byte[]> attributes, int[] childrenIds)
    {
        
    }

    public void NewShapeNode(int id, Dictionary<string, byte[]> attributes, int[] modelIds, Dictionary<string, byte[]>[] modelsAttributes)
    {
        
    }

    public void NewMaterial(int id, Dictionary<string, byte[]> attributes)
    {
        
    }

    public void NewLayer(int id, string name, Dictionary<string, byte[]> attributes)
    {
        
    }
}
