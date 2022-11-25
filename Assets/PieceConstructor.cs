using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceConstructor : MonoBehaviour
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
    public static PieceEntity ConstructPiece(int pieceId, Transform parent, bool isGhost)
    {
        TetrisRule pieces = parent.GetComponent<Playfield>().rule;
        PieceType pieceType = pieces[pieceId];

        GameObject pieceGameObject = new();
        pieceGameObject.name = (isGhost ? "Ghost" : "") + nameTable[pieceId];
        for (int i = 0; i < 4; ++i)
        {
            GameObject tileObject = new();
            tileObject.name = i.ToString();
            tileObject.AddComponent<SpriteRenderer>();
            //tileObject.AddComponent<BlockRotation>()
            //    .Set(pieceType, i);

            SpriteRenderer renderer = tileObject.GetComponent<SpriteRenderer>();
            renderer.sprite = SkinLoader.s_simpleSprites[isGhost ? 7 : pieceId];
            renderer.sortingOrder = isGhost ? 0 : 1;

            tileObject.transform.SetParent(pieceGameObject.transform);
            tileObject.transform.position = new Vector2(pieceTable[pieceId, i, 0], pieceTable[pieceId, i, 1]);
        }
        pieceGameObject.transform.SetParent(parent);
        pieceGameObject.AddComponent<PieceEntity>();
        PieceEntity pieceEntity = pieceGameObject.GetComponent<PieceEntity>();
        pieceEntity.PieceType = pieceType;
        pieceEntity.RotationPoint = pieceRotationPointTable[pieceId];
        pieceEntity.PieceId = pieceId;
        return pieceEntity;
    }
}
