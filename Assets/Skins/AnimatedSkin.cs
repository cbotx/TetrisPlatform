using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Utils;
using Assets.Definitions;
using Assets.Skins;
public sealed class AnimatedSkin : SkinBase
{
    public AnimatedTile[] s_tiles = new AnimatedTile[7];
    public Tile[] s_static_tiles = new Tile[7];
    public int AnimationSpeed = SkinDefinitions.DefaultAnimationSpeed;
    public List<Texture2D> BaseGhostTextures { get; set; }
    private Texture2D _ghostTexture;

    public AnimatedSkin()
    {
        SkinType = SkinType.Default;
    }
    public override void PostLoading()
    {
        GenerateGhostTexture();
    }
    private void GenerateGhostTexture()
    {
        // Stack frames on y
        int frameCount = BaseGhostTextures.Count;
        _ghostTexture = new Texture2D(BaseGhostTextures[0].width, BaseGhostTextures[0].height * frameCount);
        for (int i = 0; i < frameCount; ++i)
        {
            for (int j = 0; j < 7; ++j)
            {
                Graphics.CopyTexture(BaseGhostTextures[i], 0, 0, (TileWidth + 1) * 7, 0, TileWidth, TileWidth, _ghostTexture, 0, 0, (TileWidth + 1) * j, TileWidth * i);
            }
        }
        _ghostTexture.Apply();
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
    public override void ApplyGhostShader(TilemapRenderer renderer)
    {
        renderer.material = MaterialDefinitions.material_AlphaMask;
        renderer.material.SetTexture("_Alpha", _ghostTexture);
        MaterialAnimator animator = renderer.gameObject.AddComponent<MaterialAnimator>();
        animator.Initialize(renderer.material, "_Alpha", BaseGhostTextures.Count, SkinDefinitions.DefaultAnimationSpeed);
    }
}
