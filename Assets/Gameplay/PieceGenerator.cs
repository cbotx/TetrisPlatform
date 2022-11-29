using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Skins;
using Assets.Definitions;
public class PieceGenerator
{
    public Transform field;
    public ISkin skin;
    public TetrisRule rule;

    public PieceGenerator(Transform field, ISkin skin, TetrisRule rule)
    {
        this.field = field;
        this.skin = skin;
        this.rule = rule;
    }

    // Piece Structure:
    //
    //        ----PieceEntity (Script)
    //       /
    //  GameObject----Grid
    //     \   \
    //      \   ----Tilemap---Tile[,]
    //       \
    //        ----TilemapRenderer--(Optional)Material---Shader
    //
    public PieceEntity NewPiece(int pieceId, bool isGhost)
    {
        PieceType pieceType = rule[pieceId];
        PieceShape baseShape = pieceType.BaseShape;

        GameObject pieceGameObject = new()
        {
            name = (isGhost ? "Ghost" : "") + pieceType.Name
        };
        pieceGameObject.AddComponent<Grid>();
        Tilemap tilemap = pieceGameObject.AddComponent<Tilemap>();
        TilemapRenderer tilemapRenderer = pieceGameObject.AddComponent<TilemapRenderer>();
        tilemapRenderer.sortingOrder = isGhost ? 0 : 1;

        List<TileBase> tiles = isGhost ? skin.GetPieceTiles(baseShape, BlockType.Ghost) : skin.GetPieceTiles(baseShape, (BlockType)pieceId);
 
        for (int i = 0; i < 4; ++i)
        {
            tilemap.SetTile(new Vector3Int(baseShape[i].x, baseShape[i].y), tiles[i]);
        }
        
        pieceGameObject.transform.SetParent(field);

        // Add Script last to ensure Awake() doesn't get null pointers
        PieceEntity pieceEntity = pieceGameObject.AddComponent<PieceEntity>();
        pieceEntity.PieceType = pieceType;
        pieceEntity.PieceId = pieceId;
        pieceEntity.IsGhost = isGhost;
        pieceEntity.enabled = false;
        return pieceEntity;
    }

}
