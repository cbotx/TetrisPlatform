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

        List<Sprite> sprites = skin.GetPieceSprites(pieceType.BaseShape, pieceId);
        List<Sprite> ghostSprites = null;
        if (isGhost)
            ghostSprites = skin.GetPieceSprites(pieceType.BaseShape, 7);
 
        for (int i = 0; i < 4; ++i)
        {
            GameObject tileObject = new() {
                name = i.ToString()
            };

            SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprites[i];
            renderer.sortingOrder = isGhost ? 0 : 1;

            if (isGhost)
            {
                skin.SetMaskForTile(renderer, ghostSprites[i].texture, pieceId);
            }

            tileObject.transform.SetParent(pieceGameObject.transform);
            tileObject.transform.position = blocks[i];
        }
        pieceGameObject.transform.SetParent(field);

        PieceEntity pieceEntity = pieceGameObject.AddComponent<PieceEntity>();
        pieceEntity.PieceType = pieceType;
        pieceEntity.PieceId = pieceId;
        pieceEntity.IsGhost = isGhost;
        pieceEntity.enabled = false;
        return pieceEntity;
    }
}
