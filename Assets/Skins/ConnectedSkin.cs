using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class ConnectedSkin : SkinBase
{
    public Tile[,] s_connectedTiles = new Tile[8, 24];

    private Dictionary<TileBase, TileBase> TileCutTopEntityMapping = new();
    private Dictionary<TileBase, TileBase> TileCutBottomEntityMapping = new();

    public ConnectedSkin()
    {
        SkinType = SkinType.Connected;
    }
    public override void PostLoading()
    {
        TileCutTopEntityMapping.Clear();
        TileCutBottomEntityMapping.Clear();
        for (int i = 0; i < 8; ++i) {
            for (int j = 0; j < SkinDefinition.TileCutTopMapping.Length; ++j)
            {
                TileCutTopEntityMapping.Add(s_connectedTiles[i, j], s_connectedTiles[i, SkinDefinition.TileCutTopMapping[j]]);
                TileCutBottomEntityMapping.Add(s_connectedTiles[i, j], s_connectedTiles[i, SkinDefinition.TileCutBottomMapping[j]]);
            }
        }
    }

    public override List<Tile> GetPieceTiles(PieceShape shape, int type)
    {
        List<Tile> tiles = new();
        foreach (var block in shape.blocks)
        {
            int neighborHash = 0;
            // 7 6 5
            // 4   3
            // 2 1 0
            // 0 is highest bit
            for (int j = -1; j <= 1; ++j)
            {
                for (int i = 1; i >= -1; --i)
                {
                    if ((i | j) == 0) continue;
                    neighborHash <<= 1;
                    if (shape.blocks.Contains(block + new Vector2Int(i, j)))
                    {
                        neighborHash++;
                    }
                }
            }
            int tileId = SkinDefinition.TileHashMapping[neighborHash];
            tiles.Add(s_connectedTiles[type, tileId]);
        }
        return tiles;
    }

    public override TileBase GetTileCutTop(TileBase tile)
    {
        return TileCutTopEntityMapping[tile];
    }
    public override TileBase GetTileCutBottom(TileBase tile)
    {
        return TileCutBottomEntityMapping[tile];
    }
}
