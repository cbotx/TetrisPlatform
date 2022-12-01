using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AI.Structures
{
    public class BoardState
    {
        public bool[,] field;
        public int[] pieces;
        public BoardState(bool[,] field, int[] pieces) {
            field.CopyTo(this.field, 0);
            pieces.CopyTo(this.pieces, 0);
        }
    }

    public class BoardAction
    {
        public bool doSwap;
        public int posX;
        public int posY;
        public BoardAction(bool swap, int x, int y)
        {
            doSwap = swap;
            posX = x;
            posY = y;
        }
    }
}
