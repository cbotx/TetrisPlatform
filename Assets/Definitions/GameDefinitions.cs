using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
public static class GameDefinitions
{
    public static PieceType[] Tetriminos = new PieceType[]{
        new PieceType("Z",
            new PieceShape( 0, 0 ,  1, 0 , -1, 1 ,  0, 1),
            new Vector2(0, 0)
        ),
        new PieceType("L",
            new PieceShape(-1, 0 ,  0, 0 ,  1, 0 ,  1, 1),
            new Vector2(0, 0)
        ),
        new PieceType("O",
            new PieceShape( 0, 0 ,  1, 0 ,  0, 1 ,  1, 1),
            new Vector2(0.5f, 0.5f)
        ),
        new PieceType("S",
            new PieceShape(-1, 0 ,  0, 0 ,  0, 1 ,  1, 1),
            new Vector2(0, 0)
        ),
        new PieceType("I",
            new PieceShape(-1, 0 ,  0, 0 ,  1, 0 ,  2, 0),
            new Vector2(0.5f, -0.5f)
        ),
        new PieceType("J",
            new PieceShape(-1, 0 ,  0, 0 ,  1, 0 , -1, 1),
            new Vector2(0, 0)
        ),
        new PieceType("T",
            new PieceShape(-1, 0 ,  0, 0 ,  1, 0 ,  0, 1),
            new Vector2(0, 0)
        ),
    };


    private static readonly int[,,] _T_SRSP = {
        {{ 0, 0 }, {-1, 0}, {-1,  1}, {0, -2}, {-1, -2}},
        {{ 0, 0 }, { 1, 0}, { 1, -1}, {0,  2}, { 1,  2}},
        {{ 0, 0 }, { 1, 0}, { 1,  1}, {0, -2}, { 1, -2}},
        {{ 0, 0 }, {-1, 0}, {-1, -1}, {0,  2}, {-1,  2}},
    };

    private static readonly int[,,] _T_SRSP_I = {
        {{ 0, 0 }, {-2, 0}, { 1, 0}, {-2, -1}, { 1,  2}},
        {{ 0, 0 }, {-1, 0}, { 2, 0}, {-1,  2}, { 2, -1}},
        {{ 0, 0 }, { 2, 0}, {-1, 0}, { 2,  1}, {-1, -2}},
        {{ 0, 0 }, { 1, 0}, {-2, 0}, { 1, -2}, {-2,  1}},
    };

    private static readonly int[,,] _T_SRSP_180 = {
        {{ 0, 0 }, { 0,  1}, { 1,  1}, {-1,  1}, { 1, 0}, {-1, 0}},
        {{ 0, 0 }, { 0, -1}, {-1, -1}, { 1, -1}, {-1, 0}, { 1, 0}},
        {{ 0, 0 }, { 1,  0}, { 1,  2}, { 1,  1}, { 0, 2}, { 0, 1}},
        {{ 0, 0 }, {-1,  0}, {-1,  2}, {-1,  1}, { 0, 2}, { 0, 1}},
    };

    private static SpinTable _SRSP____R = new(_T_SRSP);
    private static SpinTable _SRSP____L = _SRSP____R.GetSymmetric();
    private static SpinTable _SRSP_I__R = new(_T_SRSP_I);
    private static SpinTable _SRSP_I__L = _SRSP_I__R.GetSymmetric();
    private static SpinTable _SRSP__180 = new(_T_SRSP_180);

    public static RotationSystem SRS_Plus = RotationSystem.Tetris(
        _SRSP____R, _SRSP____L,
        _SRSP_I__R, _SRSP_I__L,
        _SRSP__180
    );


    // Z L O S I J T
    public static TetrisRule Tetris_SRS_Plus = new(Tetriminos, SRS_Plus);


    static GameDefinitions(){
        Debug.Log($"R=\n{_SRSP____R}\n\nL=\n{_SRSP____L}\n\nD180=\n{_SRSP__180}");

    }

}
