using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.AI.Structures;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.AI
{
    class PCAI : IAI
    {
        [DllImport(@"pc_core.dll")]
        static extern void __halt__();

        [DllImport(@"pc_core.dll")]
        static extern void __core_init__(string path);

        [DllImport(@"pc_core.dll")]
        static extern void __destroy__();

        [DllImport(@"pc_core.dll")]
        static extern void __calculate_best_move__(int pc_idx,
                                                   int[] pieces,
                                                   int bag_used,
                                                   int[] field,
                                                   int depth,
                                                   out int ori,
                                                   out int x,
                                                   out int y);

        private int _pcIdx = -1;
        private int _depth = 0;
        private static string _path = "data/";

        // orientation -> [pieceId, direction]
        private static readonly int[,] _oriMapping = new int[19, 2]
        {
            {4, 1 },
            {4, 0 },
            {2, 0 },
            {6, 0 },
            {6, 2 },
            {6, 3 },
            {6, 1 },
            {3, 1 },
            {3, 0 },
            {0, 1 },
            {0, 0 },
            {5, 3 },
            {5, 1 },
            {5, 2 },
            {5, 0 },
            {1, 1 },
            {1, 3 },
            {1, 0 },
            {1, 2 },
        };
        public void Initialize()
        {
            __core_init__(_path);
        }
        public async Task<BoardAction> CalculateBestMove(BoardState state)
        {
            if (_pcIdx == -1) _pcIdx = state.bagIdx;
            int[] field = new int[4];
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    if (state.field[j, i]) field[i] |= 1 << (9 - j);
                }
            }
            return await Task<BoardAction>.Factory.StartNew(
                () =>
                {
                    __calculate_best_move__(_pcIdx, state.pieces, state.bagUsed, field, _depth, out int targetOri, out int targetX, out int targetY);
                    if (targetOri == -1)
                    {
                        // TODO: Handle no solution
                        __destroy__();
                        throw new NotImplementedException();
                    }
                    int pieceId = _oriMapping[targetOri, 0];
                    int direction = _oriMapping[targetOri, 1];
                    Debug.Log($"pieceId={pieceId}, direction={direction}, x={targetX}, y={targetY}");
                    bool isSwap = true;
                    if (state.pieces[1] == pieceId) isSwap = false;
                    ++_depth;
                    if (_depth == 10)
                    {
                        // PC
                        _depth = 0;
                        _pcIdx = (state.bagIdx + 1) % 7;
                    }
                    return new BoardAction(isSwap, direction, targetX, targetY);
                }
            );
        }

        public void CancelCalculation()
        {
            __halt__(); // TODO: Halt is not implemented in dll
        }

        public void SoftReset()
        {
            CancelCalculation();
            _pcIdx = -1;
            _depth = 0;
        }

        public void Destroy()
        {
            __destroy__();
        }
    }
}
