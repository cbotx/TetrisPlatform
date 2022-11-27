using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Skins;
using Assets.Definitions;
public static class PieceConstructor
{

    private readonly static int[,,] pieceTable =
    {
        {{ 0, 0 }, {1, 0}, {-1, 1 }, { 0, 1 }},
        {{-1, 0 }, {0, 0}, { 1, 0 }, { 1, 1 }},
        {{ 0, 0 }, {1, 0}, { 0, 1 }, { 1, 1 }},
        {{-1, 0 }, {0, 0}, { 0, 1 }, { 1, 1 }},
        {{-1, 0 }, {0, 0}, { 1, 0 }, { 2, 0 }},
        {{-1, 0 }, {0, 0}, { 1, 0 }, {-1, 1 }},
        {{-1, 0 }, {0, 0}, { 1, 0 }, { 0, 1 }},
    };

    private readonly static Vector3[] pieceRotationPointTable =
    {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
        new Vector3(0.5f, 0.5f, 0),
        new Vector3(0, 0, 0),
        new Vector3(0.5f, -0.5f, 0),
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 0),
    };
    private readonly static string[] nameTable = { "Z", "L", "O", "S", "I", "J", "T" };
    public static PieceEntity ConstructPiece(int pieceId, Transform parent, bool isGhost, SkinBase skin)
    {
        TetrisRule pieces = parent.GetComponent<Playfield>().rule;
        PieceType pieceType = pieces[pieceId];

        GameObject pieceGameObject = new();
        pieceGameObject.name = (isGhost ? "Ghost" : "") + nameTable[pieceId];

        List<Tile> tiles = skin.GetPieceTiles(GameDefinitions.Tetrominos[pieceId].BaseShape, pieceId);
        List<Tile> ghostTiles = null;
        if (isGhost)
        {
            ghostTiles = skin.GetPieceTiles(GameDefinitions.Tetrominos[pieceId].BaseShape, 7);
        }
            for (int i = 0; i < 4; ++i)
        {
            GameObject tileObject = new();
            tileObject.name = i.ToString();
            SpriteRenderer renderer = tileObject.AddComponent<SpriteRenderer>();
            //tileObject.AddComponent<BlockRotation>()
            //    .Set(pieceType, i);
            
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
            tileObject.transform.position = new Vector2(pieceTable[pieceId, i, 0], pieceTable[pieceId, i, 1]);
        }
        pieceGameObject.transform.SetParent(parent);
        PieceEntity pieceEntity = pieceGameObject.AddComponent<PieceEntity>();
        pieceEntity.PieceType = pieceType;
        pieceEntity.RotationPoint = pieceRotationPointTable[pieceId];
        pieceEntity.PieceId = pieceId;
        pieceEntity.IsGhost = isGhost;
        return pieceEntity;
    }
}
