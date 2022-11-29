using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
using System.Linq;

public sealed class AnimatedSkin : ISkin
{
    public SkinType SkinType { get; set; }
    public int TileWidth { get; set; }

    public AnimatedTile[] s_tiles = new AnimatedTile[12];
    public int AnimationSpeed = SkinDefinitions.DefaultAnimationSpeed;
    public Color32[] AvgColor { get; set; }
    public AnimatedSkin(string filePath)
    {
        SkinType = SkinType.Animated;
        AvgColor = new Color32[8];
        (List<Texture2D> textures, int fps) = FileUtils.LoadTextureFromGIF(filePath);
        int tileSize = textures.First().height;
        int gapSize = SkinDefinitions.GetDefaultTextureGapSize(textures.First().width, textures.First().height);
        TileWidth = tileSize;
        for (int i = 0; i < 12; ++i)
        {
            if (i < 7) AvgColor[i] = GraphicsUtils.AverageColorFromTexture(textures.First(), new RectInt(i * (tileSize + gapSize), 0, tileSize, tileSize));
            s_tiles[i] = ScriptableObject.CreateInstance<AnimatedTile>();
            Sprite[] sprites = new Sprite[textures.Count];
            for (int j = 0; j < textures.Count; ++j)
            {
                sprites[j] = Sprite.Create(textures[j], new Rect(new Vector2(i * (tileSize + gapSize) + gapSize, gapSize), new Vector2(tileSize - gapSize * 2, tileSize - gapSize * 2)), new Vector2(0.5f, 0.5f), tileSize - gapSize * 2);
            }
            s_tiles[i] = ScriptableObject.CreateInstance<AnimatedTile>();
            s_tiles[i].m_AnimatedSprites = sprites;
            s_tiles[i].m_MinSpeed = fps * SkinDefinitions.AnimationSpeedMultiplier;
            s_tiles[i].m_MaxSpeed = fps * SkinDefinitions.AnimationSpeedMultiplier;
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
