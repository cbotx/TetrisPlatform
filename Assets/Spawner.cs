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
    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
        _pieceQueue = _playfield.GetComponentInChildren<PieceQueue>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GenerateBag();
        _pieceQueue.SetSwap(GetPieceEntityFromBag());
        for (int i = 0; i < PieceQueue.QueueSize; ++i) _pieceQueue.PushPiece(GetPieceEntityFromBag());
        PieceEntity pieceEntity = _pieceQueue.PushAndGetPiece(GetPieceEntityFromBag());
        SpawnPieceOnField(pieceEntity);
    }

    private void SpawnPieceOnField(PieceEntity pieceEntity)
    {
        // Transfer piece to field
        pieceEntity.transform.position = transform.position;
        pieceEntity.transform.localScale = new Vector3(1, 1, 1);
        pieceEntity.Initialize();
        _playfield.FieldPiece = pieceEntity;

        // Spawn ghost piece
        PieceEntity ghostPiece = Instantiate(pieceEntity, transform.position, Quaternion.identity, _playfield.transform);
        ghostPiece.enabled = false;
        _playfield.GhostedPiece = ghostPiece;
        ghostPiece.GhostDrop();

        // Change ghost piece opacity
        foreach (Transform child in ghostPiece.transform)
        {
            var renderer = child.gameObject.GetComponent<SpriteRenderer>();
            var color = renderer.color;
            color.a = GhostAlpha;
            renderer.color = color;
        }
    }

    public void NextPiece()
    {
        PieceEntity pieceEntity = _pieceQueue.PushAndGetPiece(GetPieceEntityFromBag());
        SpawnPieceOnField(pieceEntity);
    }

    public void Swap()
    {
        // Delete ghost object
        Destroy(_playfield.GhostedPiece.gameObject);

        // Swap pieces
        PieceEntity pieceEntity = _pieceQueue.Swap(_playfield.FieldPiece);
        SpawnPieceOnField(pieceEntity);
    }

    private void GenerateBag()
    {
        System.Random rnd = new();
        _bag = Enumerable.Range(0, 7).OrderBy(c => rnd.Next()).ToArray();
    }

    private PieceEntity GetPieceEntityFromBag()
    {
        PieceEntity pieceEntity = PieceEntities[_bag[_bagIdx]].GetComponent<PieceEntity>();
        ++_bagIdx;
        if (_bagIdx >= 7)
        {
            GenerateBag();
            _bagIdx = 0;
        }
        return pieceEntity;

    }
}
