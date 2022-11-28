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
    public Tile[,] s_connectedTiles = new Tile[7, 24];
    public Texture2D BaseGhostTexture { get; set; }
    private Texture2D _ghostTexture;

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
        for (int i = 0; i < 7; ++i) {
            for (int j = 0; j < SkinDefinitions.TileCutTopMapping.Length; ++j)
            {
                TileCutTopEntityMapping.Add(s_connectedTiles[i, j], s_connectedTiles[i, SkinDefinitions.TileCutTopMapping[j]]);
                TileCutBottomEntityMapping.Add(s_connectedTiles[i, j], s_connectedTiles[i, SkinDefinitions.TileCutBottomMapping[j]]);
            }
        }
        GenerateGhostTexture();
    }
    private void GenerateGhostTexture()
    {
        int baseWidth = s_connectedTiles[0, 0].sprite.texture.width;
        int baseHeight = s_connectedTiles[0, 0].sprite.texture.height;
        _ghostTexture = new Texture2D(baseWidth, baseHeight);
        for (int i = 0; i < 7; ++i)
        {
            int dst_x = SkinDefinitions.ConnectedTexturePosition[i, 0] * TileWidth;
            int dst_y = baseHeight - (SkinDefinitions.ConnectedTexturePosition[i, 1] + 6) * TileWidth;
            Graphics.CopyTexture(BaseGhostTexture, 0, 0, 0, BaseGhostTexture.height - TileWidth * 6, TileWidth * 4, TileWidth * 6, _ghostTexture, 0, 0, dst_x, dst_y);
        }
    }

    public override List<TileBase> GetPieceTiles(PieceShape shape, int type)
    {
        List<TileBase> tiles = new();
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
            int tileId = SkinDefinitions.TileHashMapping[neighborHash];
            tiles.Add(s_connectedTiles[type, tileId]);
        }
        return tiles;
    }

    public override List<Sprite> GetPieceSprites(PieceShape shape, int type)
    {
        List<Sprite> sprites = new();
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
            int tileId = SkinDefinitions.TileHashMapping[neighborHash];
            sprites.Add(s_connectedTiles[type, tileId].sprite);
        }
        return sprites;
    }
    public override TileBase GetTileCutTop(TileBase tile)
    {
        return TileCutTopEntityMapping[tile];
    }
    public override TileBase GetTileCutBottom(TileBase tile)
    {
        return TileCutBottomEntityMapping[tile];
    }
    public override void ApplyGhostShader(TilemapRenderer renderer)
    {
        renderer.material = MaterialDefinitions.material_AlphaMask;
        renderer.material.SetTexture("_Alpha", _ghostTexture);
    }
}
