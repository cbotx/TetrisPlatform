using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public class PieceType
{
    public string Name { get; set; }


    private void UpdateShapes()
    {
        for (int i = 1; i < 4; i++)
            shapes[i] = shapes[0].RotateAround(rotationCenter, i);
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
