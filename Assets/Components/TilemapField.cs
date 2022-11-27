using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using Assets.Definitions;
using Assets.Skins;
public class TilemapField : MonoBehaviour
{
    private int _width;
    private int _height;
    private Tilemap _tilemap = null;
    private SkinBase _skin = null;
    private void Awake()
    {
        _width = Playfield.s_Width;
        _height = Playfield.s_Height;
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetSkin(SkinBase skin)
    {
        _skin = skin;
    }

    public void AddPieceTiles(PieceShape shape, int pieceId)
    {
        List<TileBase> tiles = _skin.GetPieceTiles(shape, pieceId);
        for (int i = 0; i < shape.blocks.Length; ++i)
        {
            _tilemap.SetTile(new Vector3Int(shape.blocks[i].x, shape.blocks[i].y), tiles[i]);
        }
    }

    public void DeleteLine(int lineIdx)
    {
        for (int j = 0; j < _width; ++j)
        {
            _tilemap.SetTile(new Vector3Int(j, lineIdx), null);
            if (_skin.SkinType == SkinType.Connected)
            {
                if (lineIdx > 0)
                {
                    Vector3Int position = new(j, lineIdx - 1);
                    TileBase tile = _tilemap.GetTile(position);
                    if (tile) _tilemap.SetTile(position, _skin.GetTileCutTop(tile));
                }
                if (lineIdx < _height - 1)
                {
                    Vector3Int position = new(j, lineIdx + 1);
                    TileBase tile = _tilemap.GetTile(position);
                    if (tile) _tilemap.SetTile(position, _skin.GetTileCutBottom(tile));
                }
            }
        }
    }

    public void Clear()
    {
        _tilemap.ClearAllTiles();
    }

    public void SetTile(Vector3Int position, TileBase tile)
    {
        _tilemap.SetTile(position, tile);
    }
    public TileBase GetTile(Vector3Int position)
    {
        return _tilemap.GetTile(position);
    }
}
