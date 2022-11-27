using Assets.Skins;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets.Definitions;

public class PieceEntity : MonoBehaviour
{
    public static float s_FallInterval = 1f;
    public static float s_RepeatInterval = 0.03f;
    public static float s_WaitBeforeRepeatInterval = 0.15f;
    public static float s_TimeTillFreeze = 1.5f;
    public static float s_TotalTimeTillFreeze = 5f;

    private PieceShape _shape;
    public PieceShape Shape
    {
        get => _shape;
        set {
            _shape = value;
            Vector3[] localPositions = _shape.ToVector3Array();
            int i = 0;
            foreach (Transform child in transform)
            {
                child.localPosition = localPositions[i];
                i++;
            }
        }
    }

    // 0=Up, 1=Right, 2=Down, 3=Left
    public int direction;
    public PieceType PieceType;

    public bool IsGhost = false;

    private Playfield _playfield;

    private int _pieceId;
    public int PieceId { 
        get => _pieceId;
        set { 
            _kickTable = _playfield.rule.kickTableOfPiece[value];
            _shapes = _playfield.rule.shapesOfPiece[value];
            _shape = _shapes[direction];
            _pieceId = value;
        }
    }

    private KickTable _kickTable;
    private PieceShape[] _shapes;


    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
    }


    public void Initialize()
    {
        isGrounded = false;
        freezeTimer = 0f;
        totalFreezeTimer = 0f;
    }

    public void GhostDrop()
    {
        Vector3 v = new(0, -1, 0);
        while (IsValidWithOffset(0,-1)) transform.position += v;
    }

    public PieceEntity HardDrop()
    {
        Vector3 v = new(0, -1, 0);
        while (IsValidWithOffset(0, -1)) transform.position += v;
        return _playfield.FreezePiece(false);
    }
    public bool Move(int x, int y)
    {
        Vector3 v = new(x, y, 0);
        if (IsValidWithOffset(x, y))
        {
            transform.position += v;
            PostMovement();
            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!isGrounded) return;

        freezeTimer += Time.smoothDeltaTime;
        totalFreezeTimer += Time.smoothDeltaTime;

        if (freezeTimer >= s_TimeTillFreeze || totalFreezeTimer >= s_TotalTimeTillFreeze)
            _playfield.FreezePiece(true);

    }

    public void ResetFreezeTimer()
    {
        freezeTimer = 0f;
    }

    public void PostMovement()
    {
        //BottomTest();
        isGrounded = !IsValidWithOffset(0, -1);
        ResetFreezeTimer();
        

        // Update ghosted piece position
        PieceEntity GhostedPiece = _playfield.GhostedPiece;
        GhostedPiece.transform.position = transform.position;
        GhostedPiece.GhostDrop();
    }

    public bool isGrounded = false;
    public float freezeTimer;
    public float totalFreezeTimer;


    public bool Rotate(int rotation)
    {
        int newDirection = (direction + rotation) % 4;
        Vector2Int[] attempts = _kickTable[rotation, direction];
        PieceShape newShape = _shapes[newDirection];

        foreach (Vector2Int attempt in attempts)
        {
            if(!_playfield.HitTest(transform.position, newShape, attempt))
            {
                Shape = newShape;
                _playfield.GhostedPiece.Shape = newShape;
                transform.localPosition += new Vector3(attempt.x, attempt.y, 0f);
                direction = newDirection;

                // For connected texture
                UpdatePieceTexture(_playfield.skin);
                _playfield.GhostedPiece.UpdatePieceTexture(_playfield.skin);

                PostMovement();

                return true;
            }
        }
        return false;

    }

    public void UpdatePieceTexture(SkinBase skin)
    {
        if (skin.SkinType != SkinType.Connected) return;
        List<Vector2Int> shape = new();
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            shape.Add(new Vector2Int(x, y));
        }
        List<Sprite> sprites = skin.GetPieceSprites(new PieceShape(shape), PieceId);
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[i];
        }
    }


    public void ResetShape()
    {
        direction = 0;
        PieceId = _pieceId;
        Shape = _shape;
    }


    private bool IsValidWithOffset(int x, int y)
    {
        return !_playfield.HitTest(transform.position, _shape, new Vector2Int(x,y));
    }


}
