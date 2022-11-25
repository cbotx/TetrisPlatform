using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
public static class GameDefinitions
{
    // Z L O S I J T
    public static TetrisPieces defaultTetrisPieces = new (new PieceType[] { 
        new PieceType("Z",
            new BlockShape( 0, 0 ,  1, 0 , -1, 1 ,  0, 1),
            new Vector2(0, 0)               
        ),                                  
        new PieceType("L",                  
            new BlockShape(-1, 0 ,  0, 0 ,  1, 0 ,  1, 1),
            new Vector2(0, 0)               
        ),                                  
        new PieceType("O",                  
            new BlockShape( 0, 0 ,  1, 0 ,  0, 1 ,  1, 1),
            new Vector2(0.5f, 0.5f)               
        ),                                  
        new PieceType("S",                  
            new BlockShape(-1, 0 ,  0, 0 ,  0, 1 ,  1, 1),
            new Vector2(0, 0)               
        ),                                  
        new PieceType("I",                  
            new BlockShape(-1, 0 ,  0, 0 ,  1, 0 ,  2, 0),
            new Vector2(0.5f, -0.5f)               
        ),                                  
        new PieceType("J",                  
            new BlockShape(-1, 0 ,  0, 0 ,  1, 0 , -1, 1),
            new Vector2(0, 0)               
        ),                                  
        new PieceType("T",                  
            new BlockShape(-1, 0 ,  0, 0 ,  1, 0 ,  0, 1),
            new Vector2(0, 0)               
        ),
    });

}
