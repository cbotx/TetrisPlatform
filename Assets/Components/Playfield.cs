using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Skins;

public class Playfield : MonoBehaviour
{
    public const int s_Width = 10;
    public const int s_Height = 50;
    private bool[,] s_Field = new bool[s_Width, s_Height];

    public PieceEntity GhostedPiece { get; set; }
    public PieceEntity FieldPiece { get; set; }

    public Spawner spawner = null;

    public SkinBase skin { get; set; }

    private TilemapField _tilemapField = null;

    public TetrisRule rule = GameDefinitions.Tetris_SRS_Plus;
    public KeyConfigs control = ControlDefinitions.cirq;
    public PieceGenerator pieceGenerator;



    // public string SkinFileName = "jstris7.png";
    // public string SkinFileName = "simple_connected.zip";
    public string SkinFileName = "Shefs_kiss.gif";

    private void Awake()
    {
        spawner = GetComponentInChildren<Spawner>();
        skin = SkinLoader.LoadSkin(SkinFileName);
        _tilemapField = GetComponentInChildren<TilemapField>();
        _tilemapField.SetSkin(skin);

        pieceGenerator = new PieceGenerator(transform, skin, rule);
    }

    private void Start()
    {
    }
    public bool HasEntityAt(int x, int y)
    {
        return s_Field[x, y];
    }

    public bool HitTest(Vector3 position, PieceShape shape, Vector2Int offset)
    {
        int cx = Mathf.RoundToInt(position.x) + offset.x;
        int cy = Mathf.RoundToInt(position.y) + offset.y;

        foreach (var pos in shape.blocks)
        {
            int x = cx + pos.x;
            int y = cy + pos.y;
            if (x < 0 || x >= Playfield.s_Width || y < 0 || y >= Playfield.s_Height) return true;
            if (HasEntityAt(x,y)) return true;
        }

        return false;
    }

    public void Restart()
    {
        s_Field = new bool[s_Width, s_Height];
        _tilemapField.Clear();
        Destroy(FieldPiece.gameObject);
        Destroy(GhostedPiece.gameObject);
        spawner.Restart();
    }

    public PieceEntity Swap()
    {
        Destroy(GhostedPiece.gameObject);
        return spawner.Swap();
    }

    public PieceEntity FreezePiece()
    {
        // Set field
        List<Vector2Int> shape = new();
        foreach (Transform child in FieldPiece.transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            s_Field[x, y] = true;
            shape.Add(new Vector2Int(x, y));
        }
        _tilemapField.AddPieceTiles(new PieceShape(shape), FieldPiece.PieceId);
         
        Destroy(FieldPiece.gameObject);
        Destroy(GhostedPiece.gameObject);
        ClearLines();

        // Spawn new piece
        return spawner.NextPiece();
    }
    private void ClearLines()
    {
        int[] offsets = new int[s_Height];
        int emptyLines = 0;
        for (int i = 0; i < s_Height; ++i)
        {
            bool full = true;
            for (int j = 0; j < s_Width; ++j)
            {
                if (!s_Field[j, i])
                {
                    full = false;
                    break;
                }
            }
            if (full)
            {
                ++emptyLines;
                DeleteLineEntities(i);
            }
            else
            {
                offsets[i] = emptyLines;
            }
        }
        if (emptyLines > 0)
        {
            for (int i = 0; i < s_Height; ++i)
            {
                if (offsets[i] > 0)
                {
                    MoveLineEntities(i, offsets[i]);
                }
            }
        }
    }
    private void DeleteLineEntities(int lineIdx)
    {
        for (int j = 0; j < s_Width; ++j)
        {
            s_Field[j, lineIdx] = false;
        }
        _tilemapField.DeleteLine(lineIdx);
    }

    private void MoveLineEntities(int lineIdx, int offset)
    {
        for (int j = 0; j < s_Width; ++j)
        {
            if (s_Field[j, lineIdx])
            {
                _tilemapField.SetTile(new Vector3Int(j, lineIdx - offset, 0), _tilemapField.GetTile(new Vector3Int(j, lineIdx)));
                _tilemapField.SetTile(new Vector3Int(j, lineIdx), null);

                s_Field[j, lineIdx - offset] = true;
                s_Field[j, lineIdx] = false;
            }
        }
    }
}
