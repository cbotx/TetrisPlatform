using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class DefaultSkin : SkinBase
{
    public Sprite[] s_simpleSprites = new Sprite[8];
    public Tile[] s_tiles = new Tile[8];
    
    public DefaultSkin()
    {
        SkinType = SkinType.Default;
    }
    public override void PostLoading()
    {

    }

    public override List<Tile> GetPieceTiles(PieceShape shape, int type)
    {
        List<Tile> tiles = new();
        for (int i = 0; i < shape.blocks.Length; ++i)
        {
            tiles.Add(s_tiles[type]);
        }
        return tiles;
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
