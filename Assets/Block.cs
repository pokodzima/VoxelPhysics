using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    public Mesh mesh;
    Chunk parentChunk;

    public Block(Vector3 offset, MeshUtils.BlockType type, Chunk chunk)
    {
        parentChunk = chunk;

        if (type != MeshUtils.BlockType.AIR)
        {
            List<Quad> quads = new List<Quad>();
            if (!HasSolidNeighbour((int)offset.x, (int)offset.y - 1, (int)offset.z))
                quads.Add(new Quad(MeshUtils.BlockSide.BOTTOM, offset, type));
            if (!HasSolidNeighbour((int)offset.x, (int)offset.y + 1, (int)offset.z))
                quads.Add(new Quad(MeshUtils.BlockSide.TOP, offset, type));
            if (!HasSolidNeighbour((int)offset.x - 1, (int)offset.y, (int)offset.z))
                quads.Add(new Quad(MeshUtils.BlockSide.LEFT, offset, type));
            if (!HasSolidNeighbour((int)offset.x + 1, (int)offset.y, (int)offset.z))
                quads.Add(new Quad(MeshUtils.BlockSide.RIGHT, offset, type));
            if (!HasSolidNeighbour((int)offset.x, (int)offset.y, (int)offset.z + 1))
                quads.Add(new Quad(MeshUtils.BlockSide.FRONT, offset, type));
            if (!HasSolidNeighbour((int)offset.x, (int)offset.y, (int)offset.z - 1))
                quads.Add(new Quad(MeshUtils.BlockSide.BACK, offset, type));

            if (quads.Count == 0) return;

            Mesh[] sideMeshes = new Mesh[quads.Count];
            int m = 0;
            foreach (Quad q in quads)
            {
                sideMeshes[m] = q.mesh;
                m++;
            }

            mesh = MeshUtils.MergeMeshes(sideMeshes);
            mesh.name = "Cube_0_0_0";
        }
    }

    public bool HasSolidNeighbour(int x, int y, int z)
    {
        if (x < 0 || x >= parentChunk.width ||
            y < 0 || y >= parentChunk.height ||
            z < 0 || z >= parentChunk.depth)
        {
            return false;
        }
        if(parentChunk.chunkData[x + parentChunk.width * (y + parentChunk.depth * z)] == MeshUtils.BlockType.AIR
            || parentChunk.chunkData[x + parentChunk.width * (y + parentChunk.depth * z)] == MeshUtils.BlockType.WATER)
        return false;
        return true;
    }
}
