using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class DefaultSkin : SkinBase
{
    public Sprite[] s_simpleSprites = new Sprite[7];
    public Tile[] s_tiles = new Tile[7];
    public Texture2D BaseGhostTexture { get; set; }
    private Texture2D _ghostTexture;

    public DefaultSkin()
    {
        SkinType = SkinType.Default;
    }
    public override void PostLoading()
    {
        GenerateGhostTexture();
    }

    private void GenerateGhostTexture()
    {
        _ghostTexture = new Texture2D(BaseGhostTexture.width, BaseGhostTexture.height);
        for (int i = 0; i < 7; ++i) {
            Graphics.CopyTexture(BaseGhostTexture, 0, 0, (TileWidth + 1) * 7, 0, TileWidth, TileWidth, _ghostTexture, 0, 0, (TileWidth + 1) * i, 0);
        }
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
            sprites.Add(s_tiles[type].sprite);
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

    public override void ApplyGhostShader(TilemapRenderer renderer)
    {
        renderer.material = MaterialDefinitions.material_AlphaMask;
        renderer.material.SetTexture("_Alpha", _ghostTexture);
    }
}
