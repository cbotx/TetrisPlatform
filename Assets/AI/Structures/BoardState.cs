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
        public int[] pieces = new int[7];
        public int bagIdx;
        public int bagUsed;
        public BoardState(bool[,] field, int bagIdx) {
            this.field = field;
            this.bagIdx = bagIdx;
        }

        public void SetPieces(int swapPiece, int fieldPiece, int[] queuePieces)
        {
            pieces[0] = swapPiece;
            pieces[1] = fieldPiece;
            for (int i = 0; i < 7 - 2; ++i) pieces[i + 2] = queuePieces[i];
        }

        public void Swap()
        {
            int temp = pieces[0];
            pieces[0] = pieces[1];
            pieces[1] = temp;
        }

        public void SetPiece(int position, int pieceId)
        {
            pieces[position] = pieceId;
            bagUsed |= 1 << pieceId;
            if (bagUsed == 0b1111111) bagUsed = 0;
        }

        public void PushPiece(int newPiece)
        {
            for (int i = 2; i < 7; ++i)
            {
                pieces[i - 1] = pieces[i];
            }
            pieces[6] = newPiece;
            bagUsed |= 1 << newPiece;
            if (bagUsed == 0b1111111) bagUsed = 0;
        }

        public void BagIdxIncrease()
        {
            bagIdx = (bagIdx + 1) % 7;
        }
    }

    public class BoardAction
    {
        public bool doSwap;
        public int direction;  // 0=Up, 1=Right, 2=Down, 3=Left

        // coords of the **bottomleft** block of the piece
        public int posX;
        public int posY;
        public BoardAction(bool swap, int direction, int x, int y)
        {
            doSwap = swap;
            this.direction = direction;
            posX = x;
            posY = y;
        }
    }
}
