using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class KickTable
{
    //[rotate][from_direction][tryIndex]
    Vector2Int[][][] table = new Vector2Int[4][][];

    public Vector2Int[] this[int rotation, int from_direction] {
        get => table[rotation][from_direction];
    }
    
    public KickTable(PieceType pieceType, SpinTable R, SpinTable L, SpinTable D180)
    {
        for (int i = 0; i < 4; i++)
        {
            table[i] = new Vector2Int[0][];
        }

        if (R != null)
            table[1] = Enumerable.Range(0, 4).Select(i => R[i].attempts).ToArray();

        if (D180 != null)
            table[2] = Enumerable.Range(0, 4).Select(i => D180[i].attempts).ToArray();

        if (L != null)
            table[3] = Enumerable.Range(0, 4).Select(i => L[i].attempts).ToArray();

    }




}


public class TetrisRule
{
    private PieceType[] pieceTypes;
    private RotationSystem rotationSystem;

    
    public KickTable[] kickTableOfPiece;
    public PieceShape[][] shapesOfPiece;

    public PieceType this[int index]
    {
        get => pieceTypes[index];
        set { pieceTypes[index] = value; }
    }

    public TetrisRule(PieceType[] pieceTypes, RotationSystem rotationSystem)
    {
        this.pieceTypes = pieceTypes;
        this.rotationSystem = rotationSystem;

        shapesOfPiece = pieceTypes.Select(e => e.shapes).ToArray();
        kickTableOfPiece = pieceTypes.Select((e,i) => 
            new KickTable(e,
                rotationSystem.SpinTables_R[i],
                rotationSystem.SpinTables_L[i],
                rotationSystem.SpinTables_180[i]
            )
        ).ToArray();
    }


}
