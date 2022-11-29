using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class DefaultSkin : ISkin
{
    public SkinType SkinType { get; set; }
    public int TileWidth { get; set; }

    public Tile[] s_tiles = new Tile[12];

    public DefaultSkin(string filePath)
    {
        SkinType = SkinType.Default;
        Texture2D texture = FileUtils.LoadTextureFromFile(filePath);
        int tileSize = texture.height;
        int gapSize = SkinDefinitions.GetDefaultTextureGapSize(texture.width, texture.height);
        TileWidth = tileSize;
        for (int i = 0; i < 12; ++i)
        {
            s_tiles[i] = ScriptableObject.CreateInstance<Tile>();
            s_tiles[i].sprite = Sprite.Create(texture, new Rect(new Vector2(i * (tileSize + gapSize) + gapSize, gapSize), new Vector2(tileSize - gapSize * 2, tileSize - gapSize * 2)), new Vector2(0.5f, 0.5f), tileSize - gapSize * 2);
        }
    }

    public List<TileBase> GetPieceTiles(PieceShape shape, BlockType type)
    {
        List<TileBase> tiles = new();
        for (int i = 0; i < shape.blocks.Length; ++i)
        {
            tiles.Add(s_tiles[(int)type]);
        }
        return tiles;
    }

    public TileBase GetTileCutTop(TileBase tile)
    {
        return tile;
    }
    public TileBase GetTileCutBottom(TileBase tile)
    {
        return tile;
    }
}
