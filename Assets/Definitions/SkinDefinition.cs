using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Definitions
{
    public static class SkinDefinition
    {
        private static int _T_ = 1;
        private static int _F_ = 0;
        private static int ___ = -1;

        public static int[] TileHashMapping = new int[257];

        private static readonly int[,] TileNeighborRules =
        {
            {___, _F_, ___,
             _F_,      _F_,
             ___, _T_, ___},

            {___, _F_, ___,
             _F_,      _T_,
             ___, _T_, _T_},

            {___, _F_, ___,
             _T_,      _T_,
             _T_, _T_, _T_},

            {___, _F_, ___,
             _T_,      _F_,
             _T_, _T_, ___},

            {___, _T_, ___,
             _F_,      _F_,
             ___, _T_, ___},

            {___, _T_, _T_,
             _F_,      _T_,
             ___, _T_, _T_},

            {_T_, _T_, _T_,
             _T_,      _T_,
             _T_, _T_, _T_},

            {_T_, _T_, ___,
             _T_,      _F_,
             _T_, _T_, ___},

            {___, _T_, ___,
             _F_,      _F_,
             ___, _F_, ___},

            {___, _T_, _T_,
             _F_,      _T_,
             ___, _F_, ___},

            {_T_, _T_, _T_,
             _T_,      _T_,
             ___, _F_, ___},

            {_T_, _T_, ___,
             _T_,      _F_,
             ___, _F_, ___},

            {___, _F_, ___,
             _F_,      _F_,
             ___, _F_, ___},

            {___, _F_, ___,
             _F_,      _T_,
             ___, _F_, ___},

            {___, _F_, ___,
             _T_,      _T_,
             ___, _F_, ___},

            {___, _F_, ___,
             _T_,      _F_,
             ___, _F_, ___},

            {___, _F_, ___,
             _F_,      _T_,
             ___, _T_, _F_},

            {___, _F_, ___,
             _T_,      _F_,
             _F_, _T_, ___},

            {_F_, _T_, _F_,
             _T_,      _T_,
             ___, _F_, ___},

            {___, _T_, _F_,
             _F_,      _T_,
             ___, _T_, _F_},

            {___, _T_, _F_,
             _F_,      _T_,
             ___, _F_, ___},

            {_F_, _T_, ___,
             _T_,      _F_,
             ___, _F_, ___},

            {___, _F_, ___,
             _T_,      _T_,
             _F_, _T_, _F_},

            {_F_, _T_, ___,
             _T_,      _F_,
             _F_, _T_, ___},

        };
        public static readonly int[] TileCutTopMapping =
        {
            0, 1, 2, 3, 0, 1, 2, 3, 12, 13, 14, 15, 12, 13, 14, 15, 16, 17, 14, 16, 13, 15, 22, 17
        };
        public static readonly int[] TileCutBottomMapping =
        {
            12, 13, 14, 15, 8, 9, 10, 11, 8, 9, 10, 11, 12, 13, 14, 15, 13, 15, 18, 20, 20, 21, 14, 21
        };
        static SkinDefinition()
        {
            GenerateTileHashMapping();
        }

        private static void GenerateTileHashMapping()
        {
            for (int i = 0; i <= 256; ++i)
            {
                TileHashMapping[i] = -1;

                // 0 1 2
                // 3   4
                // 5 6 7
                for (int j = 0; j < TileNeighborRules.GetLength(0); ++j)
                {
                    bool isMatch = true;
                    for (int k = 0; k < 8; ++k)
                    {
                        if (TileNeighborRules[j, k] != ___ && (TileNeighborRules[j, k] != 0) != ((i & 1 << k) != 0))
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    if (isMatch)
                    {
                        TileHashMapping[i] = j;
                        break;
                    }
                }
            }
        }
    }
}
