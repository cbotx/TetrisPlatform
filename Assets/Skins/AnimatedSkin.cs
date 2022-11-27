using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class AnimatedSkin : SkinBase
{
    public AnimatedTile[] s_tiles = new AnimatedTile[8];
    public Tile[] s_static_tiles = new Tile[8];
    public int AnimationSpeed = SkinDefinitions.DefaultAnimationSpeed;

    public AnimatedSkin()
    {
        SkinType = SkinType.Default;
    }
    public override void PostLoading()
    {

    }

    public override List<TileBase> GetPieceTiles(PieceShape shape, int type)
    {
        List<TileBase> tiles = new();
        for (int i = 0; i < shape.blocks.Length; ++i)
        {
            tiles.Add(s_tiles[type]);
        }
        return tiles;
    }


    public override List<Sprite> GetPieceSprites(PieceShape shape, int type)
    {
        List<Sprite> sprites = new();
        for (int i = 0; i < shape.blocks.Length; ++i)
        {
            sprites.Add(s_static_tiles[type].sprite);
        }
        return sprites;
    }
    public override TileBase GetTileCutTop(TileBase tile)
    {
        return tile;
    }
    public override TileBase GetTileCutBottom(TileBase tile)
    {
        return tile;
    }
    public override void SetMaskForTile(SpriteRenderer renderer, Texture2D texture, int pieceId)
    {
        float offsetUnit = 1.0f * (TileWidth + 1) / texture.width;
        renderer.material = MaterialDefinitions.material_AlphaMask;
        renderer.material.SetTexture("_Alpha", texture);
        renderer.material.SetTextureOffset("_Alpha", new Vector2(offsetUnit * (7 - pieceId), 0));
    }
}
