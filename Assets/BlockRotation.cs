using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;




public class BlockRotation: MonoBehaviour
{
    public PieceType pieceType;
    public int index;

    public int rotation;

    public BlockRotation Set(PieceType pieceType, int index)
    {
        this.pieceType = pieceType;
        this.index = index;
        return this;
    }

    public void Rotate(int f)
    {
        rotation = (rotation + f) % 4;
        transform.localPosition = pieceType.shape[rotation].blocks[index];
    }


}

