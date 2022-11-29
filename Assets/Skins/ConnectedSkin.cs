using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class ConnectedSkin : ISkin, ISkinConnected
{
    public SkinType SkinType { get; set; }
    public int TileWidth { get; set; }

    public Tile[,] s_connectedTiles = new Tile[12, 24];

    private Dictionary<TileBase, TileBase> TileCutTopEntityMapping = new();
    private Dictionary<TileBase, TileBase> TileCutBottomEntityMapping = new();

    public ConnectedSkin(string filePath)
    {
        SkinType = SkinType.Connected;
        Debug.Log(filePath);
        Dictionary<string, Texture2D> textures = FileUtils.LoadTexturesFromZip(filePath);
        int maxWidth = 0;
        foreach(var item in textures)
        {
            maxWidth = Mathf.Max(maxWidth, item.Value.width);
        }
        foreach (var item in textures)
        {
            bool isMino = item.Value.width == maxWidth;

            Texture2D texture = item.Value;

            int tileSize = isMino ? SkinDefinitions.GetConnectedMinoTextureTileSize(texture.width) : SkinDefinitions.GetConnectedGhostTextureTileSize(texture.width);
            TileWidth = tileSize;
            for (int i = 0; i < 12; ++i)
            {
                BlockType type = (BlockType)i;
                if (isMino && (type == BlockType.Ghost || type == BlockType.Cross)) continue;
                if (!isMino && (type != BlockType.Ghost && type != BlockType.Cross)) continue;
                int tileCount = (type == BlockType.Garbage || type == BlockType.Wall) ? 16 : 24;
                for (int j = 0; j < tileCount; ++j)
                {
                    s_connectedTiles[i, j] = ScriptableObject.CreateInstance<Tile>();
                    s_connectedTiles[i, j].sprite = GetSpriteInConnectedTexture(texture, i, j, tileSize);
                }
            }
        }
        GenerateTileEntityMapping();
    }
    private static Sprite GetSpriteInConnectedTexture(Texture2D texture, int type, int tileIdx, int unitSize)
    {
        int x = (SkinDefinitions.ConnectedTexturePosition[type, 0] + tileIdx % 4) * unitSize;
        int y = texture.height - (SkinDefinitions.ConnectedTexturePosition[type, 1] + tileIdx / 4) * unitSize - unitSize;
        return Sprite.Create(texture, new Rect(new Vector2(x + 1, y + 1), new Vector2(unitSize - 2, unitSize - 2)), new Vector2(0.5f, 0.5f), unitSize - 2);
    }
    public void GenerateTileEntityMapping()
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
    }

    public List<TileBase> GetPieceTiles(PieceShape shape, BlockType type)
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
            tiles.Add(s_connectedTiles[(int)type, tileId]);
        }
        return tiles;
    }
    public TileBase GetTileCutTop(TileBase tile)
    {
        return TileCutTopEntityMapping[tile];
    }
    public TileBase GetTileCutBottom(TileBase tile)
    {
        return TileCutBottomEntityMapping[tile];
    }
}
