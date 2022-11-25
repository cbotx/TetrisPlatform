using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TetrisPieces
{
    public PieceType[] pieceTypes;


    public PieceType this[int index]
    {
        get => pieceTypes[index];
        set { pieceTypes[index] = value; }
    }

    public TetrisPieces(PieceType[] pieceTypes)
    {
        this.pieceTypes = pieceTypes;
    }

}
