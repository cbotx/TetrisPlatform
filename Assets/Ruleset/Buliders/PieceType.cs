using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public class PieceType
{
    public string Name { get; set; }

    public Vector3 barycenter;
    private void UpdateShapes()
    {
        for (int i = 1; i < 4; i++)
            shapes[i] = shapes[0].RotateAround(rotationCenter, i);


        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (Vector2Int pos in BaseShape.blocks) {
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.y < minY) minY = pos.y;
            if (pos.y > maxY) maxY = pos.y;
        }

        barycenter = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0f);
    }

    public PieceShape this[int rotation] { get => shapes[rotation]; }

    public PieceShape BaseShape
    {
        get => shapes[0];
        set { shapes[0] = value; }
    }

    public PieceShape[] shapes = new PieceShape[4];

    public Vector2 rotationCenter;

    public PieceType(string name, PieceShape baseShape, Vector2 rotationCenter)
    {
        Name = name;
        BaseShape = baseShape;
        this.rotationCenter = rotationCenter;

        UpdateShapes();
    }
}
