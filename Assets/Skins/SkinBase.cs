using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Definitions;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Skins
{
    public abstract class SkinBase
    {
        public SkinType SkinType = SkinType.Default;

        public int TileWidth { get; set; }

        public abstract void PostLoading();
        public abstract List<Tile> GetPieceTiles(PieceShape shape, int type);
        public abstract TileBase GetTileCutTop(TileBase tile);
        public abstract TileBase GetTileCutBottom(TileBase tile);
    }
}
