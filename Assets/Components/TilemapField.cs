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
    private ISkin _skin = null;
    private void Awake()
    {
        _width = Playfield.s_Width;
        _height = Playfield.s_Height;
        _tilemap = GetComponent<Tilemap>();
    }

    public void SetSkin(ISkin skin)
    {
        _skin = skin;
    }

    public void DeleteLine(int lineIdx)
    {
        for (int j = 0; j < _width; ++j)
        {
            _tilemap.SetTile(new Vector3Int(j, lineIdx), null);
            ISkinConnected connectedSkin = _skin as ISkinConnected;
            if (connectedSkin != null)
            {
                if (lineIdx > 0)
                {
                    Vector3Int position = new(j, lineIdx - 1);
                    TileBase tile = _tilemap.GetTile(position);
                    if (tile) _tilemap.SetTile(position, connectedSkin.GetTileCutTop(tile));
                }
                if (lineIdx < _height - 1)
                {
                    Vector3Int position = new(j, lineIdx + 1);
                    TileBase tile = _tilemap.GetTile(position);
                    if (tile) _tilemap.SetTile(position, connectedSkin.GetTileCutBottom(tile));
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
