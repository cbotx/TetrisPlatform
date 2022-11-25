using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



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
        kickTableOfPiece = pieceTypes.Select((_, i) => 
            new KickTable(
                rotationSystem.SpinTables_R[i],
                rotationSystem.SpinTables_L[i],
                rotationSystem.SpinTables_180[i]
            )
        ).ToArray();
    }


}
