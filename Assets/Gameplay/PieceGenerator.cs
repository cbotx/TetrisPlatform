using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Skins;
using Assets.Definitions;
public class PieceGenerator
{
    public Transform field;
    public SkinBase skin;
    public TetrisRule rule;

    public PieceGenerator(Transform field, SkinBase skin, TetrisRule rule)
    {
        this.field = field;
        this.skin = skin;
        this.rule = rule;
    }

    public PieceEntity NewPiece(int pieceId, bool isGhost)
    {
        PieceType pieceType = rule[pieceId];

        GameObject pieceGameObject = new() {
            name = (isGhost ? "Ghost" : "") + pieceType.Name
        };

        Vector3[] blocks = pieceType.BaseShape.ToVector3Array();

        List<Tile> tiles = skin.GetPieceTiles(pieceType.BaseShape, pieceId);
        List<Tile> ghostTiles = null;
        if (isGhost)
            ghostTiles = skin.GetPieceTiles(pieceType.BaseShape, 7);
 
        for (int i = 0; i < 4; ++i)
        {
            GameObject tileObject = new()¡¡{
                name = i.ToString()
            };

            SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = tiles[i].sprite;
            renderer.sortingOrder = isGhost ? 0 : 1;

            if (isGhost)
            {
                renderer.material = MaterialDefinitions.material_AlphaMask;
                renderer.material.SetTexture("_Alpha", ghostTiles[i].sprite.texture);
                float offsetUnit = 1.0f * skin.TileWidth / ghostTiles[i].sprite.texture.width;
                renderer.material.SetTextureScale("_Alpha", new Vector2(2, 2));
                renderer.material.SetTextureOffset("_Alpha", new Vector2(-offsetUnit * SkinDefinitions.ConnectedTexturePosition[pieceId, 0], offsetUnit * SkinDefinitions.ConnectedTexturePosition[pieceId, 1]));
            }

            tileObject.transform.SetParent(pieceGameObject.transform);
            tileObject.transform.position = blocks[i];
        }
        pieceGameObject.transform.SetParent(field);

        PieceEntity pieceEntity = pieceGameObject.AddComponent<PieceEntity>();
        pieceEntity.PieceType = pieceType;
        pieceEntity.PieceId = pieceId;
        pieceEntity.IsGhost = isGhost;
        return pieceEntity;
    }
}
