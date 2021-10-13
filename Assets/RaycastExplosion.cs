using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastExplosion : MonoBehaviour
{
    public int radius;

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            ray.direction *= 50f;
            if (Physics.Raycast(ray, out var hit))
            {
                var chunk = hit.transform.GetComponent<Chunk>();
                for (int i = 0; i < chunk.chunkData.Length; i++)
                {
                    int x = i % chunk.width + (int)chunk.location.x;
                    int y = (i / chunk.width) % chunk.height + (int)chunk.location.y;
                    int z = i / (chunk.width * chunk.height) + (int)chunk.location.z;
                    if (Mathf.Pow(x - hit.point.x, 2)
                        + Mathf.Pow(y - hit.point.y, 2)
                        + Mathf.Pow(z - hit.point.z, 2) < radius * radius)
                    {
                        chunk.chunkData[x + chunk.width * (y + chunk.height * z)] = MeshUtils.BlockType.AIR;
                    }
                }
                chunk.RebuildChunk();
            }
        }
    }
}
