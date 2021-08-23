using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelSystem;

[RequireComponent(typeof(MeshFilter))]
public class VoxelModel : MonoBehaviour
{
    public ComputeShader voxelizer;
    public int resolution;
    
    private float _unit;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;
    private List<Voxel_t> _voxelGrid;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _meshFilter);
        TryGetComponent(out _meshCollider);
        TryGetComponent(out _meshRenderer);
        Voxelize();
    }

    private void Voxelize()
    {
        var mesh = _meshFilter.sharedMesh;

        GPUVoxelData gpuVoxelData = GPUVoxelizer.Voxelize(voxelizer, mesh, resolution);
        _voxelGrid = gpuVoxelData.GetData().ToList();
        _unit = gpuVoxelData.UnitLength;
                
        gpuVoxelData.Dispose();
        
        BuildMesh();
    }

    public void DestroyVoxels(Vector3 pos, float rad)
    {
        for (int i = 0; i < _voxelGrid.Count; i++)
        {
            if (OverlapSphere(transform.TransformPoint(_voxelGrid[i].position), pos, rad))
            {
                print("Destroyed Voxel");
                _voxelGrid.RemoveAt(i);
                i--;
            }
        }

        BuildMesh();
    }

    private bool OverlapSphere(Vector3 pointPos,Vector3 spherePos, float rad)
    {
        return (pointPos.x - spherePos.x) * (pointPos.x - spherePos.x) +
            (pointPos.y - spherePos.y) * (pointPos.y - spherePos.y) +
            (pointPos.z - spherePos.z) * (pointPos.z - spherePos.z) < rad * rad;
    }

    private void BuildMesh()
    {
        _meshFilter.sharedMesh = VoxelMesh.Build(_voxelGrid.ToArray(), _unit);
        _meshCollider.sharedMesh = _meshFilter.sharedMesh;
    }

    private void OnDrawGizmosSelected()
    {
        if (_voxelGrid == null) return;
        Gizmos.color = Color.red;
        var unitVector = new Vector3(_unit,_unit,_unit);
        foreach (var voxel in _voxelGrid)
        {
            Gizmos.DrawCube(transform.TransformPoint(voxel.position),unitVector);
        }
        Gizmos.color = Color.white;
    }
}
