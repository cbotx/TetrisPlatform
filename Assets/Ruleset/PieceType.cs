using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public struct BlockShape
{
    public Vector3[] blocks;

    public BlockShape(IEnumerable<Vector3> blocks)
    {
        this.blocks = blocks.ToArray();
    }
    public BlockShape(params int[] coords): this(coords.ToCollection_Of_Vector3()) { }

    private Vector3 BlockRotateAround(Vector2Int position, Vector2Int center_x2, PieceRotation rotation)
    {
        int x = position.x, y = position.y;
        int cx = center_x2.x, cy = center_x2.y;

        switch (rotation) {
            case PieceRotation.Right:
                return new Vector3(-y + (cx + cy) / 2, x + (-cx + cy) / 2, 0);
            case PieceRotation.Left:
                return new Vector3(y + (cx - cy) / 2, -x + (cx + cy) / 2, 0);
            case PieceRotation.Rot180:
                return new Vector3(-x + cx, -y + cy);
        }

        return new Vector3(x, y, 0);
    }

    public BlockShape RotateAround(Vector2 center, PieceRotation rotation)
    {
        Vector2Int center_x2 = new((int) center.x * 2, (int)center.y * 2);
        Vector3[] rotatedBlocks = new Vector3[blocks.Length];
        for (int i = 0; i < blocks.Length; i++)
            rotatedBlocks[i] = BlockRotateAround(
                new Vector2Int((int) blocks[i].x, (int) blocks[i].y), 
                center_x2, rotation);

        return new BlockShape(rotatedBlocks);
    }
}

public class PieceType
{
    public string Name { get; set; }


    private void UpdateShapes()
    {
        for (int i = 1; i < 4; i++)
            shape[i] = shape[0].RotateAround(rotationCenter, (PieceRotation)i);
    }

    public BlockShape BaseShape
    {
        get => shape[0];
        set { shape[0] = value; }
    }

    public BlockShape[] shape = new BlockShape[4];

    public Vector2 rotationCenter;

    public PieceType(string name, BlockShape baseShape, Vector2 rotationCenter)
    {
        Name = name;
        BaseShape = baseShape;
        this.rotationCenter = rotationCenter;

        UpdateShapes();
    }
}
