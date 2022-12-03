using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Skins;
using Assets.AI;
using Assets.AI.Structures;


public class Playfield : MonoBehaviour
{
    public const int s_Width = 10;
    public const int s_Height = 50;
    private bool[,] s_Field = new bool[s_Width, s_Height];
    public BoardState BoardState;

    public PieceEntity GhostedPiece { get; set; }
    public PieceEntity FieldPiece { get; set; }

    public Spawner spawner = null;

    public ISkin skin { get; set; }

    private TilemapField _tilemapField = null;

    private InputHandler _inputHandler;
    private AIHandler _aiHandler;

    public TetrisRule rule = GameDefinitions.Tetris_SRS_Plus;
    public KeyConfigs control = KeyConfigDefinitions.cirq;
    public ControllerConfigs handling = ControllerConfigDefinitions.expert;  // ControllerConfigs.Default; ControllerConfigs.Default; //
    public PieceGenerator pieceGenerator;
    public TetrisGameplay game;


    // public string SkinFileName = "jstris7.png";
    // public string SkinFileName = "simple_connected.zip";
    public string SkinFileName = "Shefs_kiss.gif";

    private void Awake()
    {
        spawner = GetComponentInChildren<Spawner>();

        skin = SkinFactory.LoadSkin(SkinFileName);
        _tilemapField = GetComponentInChildren<TilemapField>();
        _tilemapField.SetSkin(skin);

        // _inputHandler = GetComponent<InputHandler>();
        _aiHandler = gameObject.AddComponent<AIHandler>();

        pieceGenerator = new PieceGenerator(transform, skin, rule);
        BoardState = new BoardState(s_Field, 0);

        game = new TetrisGameplay();
    }

    private void Start()
    {
        spawner.Restart();
        _aiHandler.SetAI(new PCAI());
        _aiHandler.UpdateBoard(UpdateEvent.NextPieceEvent, BoardState);
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
        canSwap = true;
    }

    public bool canSwap = true;

    public PieceEntity Swap()
    {
        if (!canSwap) return null;

        canSwap = false;
        Destroy(GhostedPiece.gameObject);
        return spawner.Swap();
    }

    public PieceEntity FreezePiece(bool isAutoFrzoen)
    {
        canSwap = true;
        // Set field
        for (int i = 0; i < 4; ++i)
        {
            var position = new Vector3Int(FieldPiece.Shape[i].x, FieldPiece.Shape[i].y);
            int x = Mathf.RoundToInt(FieldPiece.transform.position.x) + FieldPiece.Shape[i].x;
            int y = Mathf.RoundToInt(FieldPiece.transform.position.y) + FieldPiece.Shape[i].y;
            _tilemapField.SetTile(new Vector3Int(x, y), FieldPiece.m_Tilemap.GetTile(position));
            s_Field[x, y] = true;
        }
        
         
        Destroy(FieldPiece.gameObject);
        Destroy(GhostedPiece.gameObject);
        ClearLines();

        if (_inputHandler != null && isAutoFrzoen) _inputHandler.AutoFrozen();

        // Spawn new piece
        PieceEntity nextPiece = spawner.NextPiece();
        BoardState.BagIdxIncrease();
        if (_aiHandler != null) _aiHandler.UpdateBoard(UpdateEvent.NextPieceEvent, BoardState);

        return nextPiece;
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
