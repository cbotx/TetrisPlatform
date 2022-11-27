using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




public struct PieceShape
{
    public Vector2Int[] blocks;
    public Vector2Int this[int index] { get => blocks[index]; }

    public Vector3[] ToVector3Array() => blocks.Select(e => new Vector3(e.x, e.y, 0)).ToArray();

    public static PieceShape operator +(PieceShape ps, Vector2Int offset){
        return new(ps.blocks.Select(e => e + offset));
    }

    public PieceShape(IEnumerable<Vector2Int> blocks)
    {
        this.blocks = blocks.ToArray();
    }
    public PieceShape(params int[] coords) : this(coords.ToCollection_Of_Vector2Int()) { }

    private delegate Vector2Int BlockRotateAround(int x, int y, int cx, int cy);
    private readonly static BlockRotateAround[] BlockRotateArounds = new BlockRotateAround[] {
        (x,y,cx,cy) => new(x, y),                                   // face Up
        (x,y,cx,cy) => new(y + (cx - cy) / 2, -x + (cx + cy) / 2),  // face Right
        (x,y,cx,cy) => new(-x + cx, -y + cy),                       // face Down
        (x,y,cx,cy) => new(-y + (cx + cy) / 2, x + (-cx + cy) / 2), // face Left
    };


    public PieceShape RotateAround(Vector2 center, int rotation)
    {
        int cx = Mathf.RoundToInt(center.x * 2);
        int cy = Mathf.RoundToInt(center.y * 2);

        Vector2Int[] blocks = this.blocks;

        return new(blocks.Select(e =>
            BlockRotateArounds[rotation](
                e.x, e.y,
                cx, cy
            )
        ));
    }


}