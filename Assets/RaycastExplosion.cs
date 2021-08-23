using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastExplosion : MonoBehaviour
{
    public Camera _camera;

    public float radius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("Mouse Clicked");
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            ray.direction *= 10f;
            if (Physics.Raycast(ray, out var hitInfo))
            {
                print("Ray collided");
                VoxelModel voxelModel;
                if (hitInfo.transform.TryGetComponent(out voxelModel))
                {
                    print("Voxel Model finded");
                    voxelModel.DestroyVoxels(hitInfo.point,radius);
                }
            }
        }
    }
}
