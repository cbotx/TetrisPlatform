using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] PieceEntities;

    private int[] _bag = new int[7];
    private int _bagIdx = 0;
    public const float GhostAlpha = 0.3f;

    private Playfield _playfield = null;
    private PieceQueue _pieceQueue = null;
    private PieceGenerator _gen;

    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
        _pieceQueue = _playfield.GetComponentInChildren<PieceQueue>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void InitializePieces()
    {
        _gen = _playfield.pieceGenerator;
        _bagIdx = 0;
        GenerateBag();
        _pieceQueue.SetSwap(GetNextPieceEntity());

        for (int i = 0; i < PieceQueue.QueueSize; ++i) _pieceQueue.PushPiece(GetNextPieceEntity());
        PieceEntity pieceEntity = _pieceQueue.PushAndGetPiece(GetNextPieceEntity());
        SpawnPieceOnField(pieceEntity);
    }

    public void Restart()
    {
        _pieceQueue.Clear();
        InitializePieces();
    }

    private PieceEntity SpawnPieceOnField(PieceEntity pieceEntity)
    {
        // Transfer piece to field
        pieceEntity.transform.position = transform.position;
        pieceEntity.Initialize();
        _playfield.FieldPiece = pieceEntity;

        // Spawn ghost piece
        PieceEntity ghostPiece = _gen.NewPiece(pieceEntity.PieceId, true);
        ghostPiece.transform.position = pieceEntity.transform.position;
        ghostPiece.enabled = false;
        _playfield.GhostedPiece = ghostPiece;
        ghostPiece.GhostDrop();

        return pieceEntity;
    }

    public PieceEntity NextPiece()
    {
        PieceEntity pieceEntity = _pieceQueue.PushAndGetPiece(GetNextPieceEntity());
        return SpawnPieceOnField(pieceEntity);
    }

    public PieceEntity Swap()
    {
        PieceEntity pieceEntity = _pieceQueue.Swap(_playfield.FieldPiece);
        return SpawnPieceOnField(pieceEntity);
    }

    private void GenerateBag()
    {
        System.Random rnd = new();
        _bag = Enumerable.Range(0, 7).OrderBy(c => rnd.Next()).ToArray();
    }

    private int GetNextPieceFromBag()
    {
        int pieceId = _bag[_bagIdx];
        ++_bagIdx;
        if (_bagIdx >= 7)
        {
            GenerateBag();
            _bagIdx = 0;
        }
        return pieceId;
    }

    private PieceEntity GetNextPieceEntity() {
        return _gen.NewPiece(GetNextPieceFromBag(), false);
    }
}
