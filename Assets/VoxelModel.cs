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
    public GameObject submeshPrefab;
    
    private float _unit;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    private MeshRenderer _meshRenderer;
    private List<Voxel_t> _voxelGrid;

    private List<Voxel_t> _subMeshVoxelGrid;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out _meshFilter);
        TryGetComponent(out _meshCollider);
        TryGetComponent(out _meshRenderer);
        Voxelize();
        _subMeshVoxelGrid = new List<Voxel_t>();
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
        int count = _voxelGrid.Count;
        for (int i = 0; i < _voxelGrid.Count; i++)
        {
            if (OverlapSphere(transform.TransformPoint(_voxelGrid[i].position), pos, rad))
            {
                print("Destroyed Voxel");
                _subMeshVoxelGrid.Add(_voxelGrid[i]);
                _voxelGrid.RemoveAt(i);
                i--;
            }
        }

        if (count != _voxelGrid.Count)
        {
            BuildMesh();
            BuildSubMesh();
        }
    }

    private bool OverlapSphere(Vector3 pointPos,Vector3 spherePos, float rad)
    {
        return (pointPos.x - spherePos.x) * (pointPos.x - spherePos.x) +
            (pointPos.y - spherePos.y) * (pointPos.y - spherePos.y) +
            (pointPos.z - spherePos.z) * (pointPos.z - spherePos.z) < rad * rad;
    }

    private void BuildMesh()
    {
        GetComponent<Rigidbody>().mass = _voxelGrid.Count;
        _meshFilter.sharedMesh = VoxelMesh.Build(_voxelGrid.ToArray(), _unit);
        _meshCollider.sharedMesh = _meshFilter.sharedMesh;
    }

    private void BuildSubMesh()
    {
        var submesh = VoxelMesh.Build(_subMeshVoxelGrid.ToArray(), _unit);
        /*Vector3 pos = Vector3.zero;
        for (int i = 0; i < _subMeshVoxelGrid.Count; i = i+ _subMeshVoxelGrid.Count/10)
        {
            pos += transform.TransformPoint(_subMeshVoxelGrid[i].position);
        }
        pos = pos / 10;*/
        var submeshObject = Instantiate(submeshPrefab);
        submeshObject.GetComponent<MeshFilter>().sharedMesh = submesh;
        submeshObject.GetComponent<MeshCollider>().sharedMesh = submesh;
        submeshObject.GetComponent<Rigidbody>().mass = _subMeshVoxelGrid.Count;
        _subMeshVoxelGrid.Clear();
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Dynamic"))
            DestroyVoxels(other.contacts[0].point,Mathf.Clamp(other.impulse.magnitude*0.0001f,0f,1f));
    }
}
