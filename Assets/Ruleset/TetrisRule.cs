using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class KickTable
{
    //[rotate][from_direction][tryIndex]
    PieceShape[][][] table = new PieceShape[4][][];

    public PieceShape[] this[int rotation, int from_direction] {
        get => table[rotation][from_direction];
    }
    
    public KickTable(PieceType pieceType, SpinTable R, SpinTable L, SpinTable D180)
    {
        for (int i = 0; i < 4; i++)
        {
            table[i] = new PieceShape[0][];
        }

        if (R != null)
            table[1] = Enumerable.Range(0, 4).Select(i => {
                PieceShape shape = pieceType[(i + 1) % 4];
                return R[i].attempts.Select(o => shape + o).ToArray();
            }).ToArray();

        if (D180 != null)
            table[2] = Enumerable.Range(0, 4).Select(i => {
                PieceShape shape = pieceType[(i + 2) % 4];
                return D180[i].attempts.Select(o => shape + o).ToArray();
            }).ToArray();

        if (L != null)
            table[3] = Enumerable.Range(0, 4).Select(i => {
                PieceShape shape = pieceType[(i + 3) % 4];
                return L[i].attempts.Select(o => shape + o).ToArray();
            }).ToArray();

    }




}


public class TetrisRule
{
    private PieceType[] pieceTypes;
    private RotationSystem rotationSystem;

    public KickTable[] kickTableOfPiece;
    public PieceShape[] shapeOfPiece;

    public PieceType this[int index]
    {
        get => pieceTypes[index];
        set { pieceTypes[index] = value; }
    }

    public TetrisRule(PieceType[] pieceTypes, RotationSystem rotationSystem)
    {
        this.pieceTypes = pieceTypes;
        this.rotationSystem = rotationSystem;

        shapeOfPiece = pieceTypes.Select(e => e.BaseShape).ToArray();
        kickTableOfPiece = pieceTypes.Select((e,i) => 
            new KickTable(e,
                rotationSystem.SpinTables_R[i],
                rotationSystem.SpinTables_L[i],
                rotationSystem.SpinTables_180[i]
            )
        ).ToArray();
    }


}
