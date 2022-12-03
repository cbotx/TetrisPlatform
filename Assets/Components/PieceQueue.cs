using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceQueue : MonoBehaviour
{
    public const int QueueSize = 5;
    public List<PieceEntity> Queue = new();
    public PieceEntity SwapPiece = null;

    [SerializeField]
    private GameObject _swapGO;
    [SerializeField]
    private GameObject _nextQueueGO;

    private Playfield _playfield = null;
    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
    }

    public PieceEntity PushAndGetPiece(PieceEntity pieceEntity)
    {
        PushPiece(pieceEntity);
        PieceEntity firstPieceEntity = Queue[0];
        Queue.RemoveAt(0);
        firstPieceEntity.transform.SetParent(_playfield.transform, false);
        firstPieceEntity.enabled = true;
        return firstPieceEntity;
    }


    public void Clear()
    {
        _swapGO.transform.DestroyAllChildren();
        _nextQueueGO.transform.DestroyAllChildren();
        Queue.Clear();
        SwapPiece = null;
    }

    public void PushPiece(PieceEntity pieceEntity)
    {
        foreach (PieceEntity obj in Queue)
        {
            obj.transform.localPosition += new Vector3(0, 3f, 0);
        }

        pieceEntity.transform.SetParent(_nextQueueGO.transform, false);
        pieceEntity.transform.localPosition = -pieceEntity.PieceType.barycenter ;
        pieceEntity.enabled = false;
        _playfield.BoardState.PushPiece(pieceEntity.PieceId);
        Queue.Add(pieceEntity);
    }

    public void SetSwap(PieceEntity pieceEntity)
    {
        pieceEntity.transform.SetParent(_swapGO.transform, false);
        pieceEntity.transform.localPosition = -pieceEntity.PieceType.barycenter;
        pieceEntity.enabled = false;
        SwapPiece = pieceEntity;
        _playfield.BoardState.SetPiece(0, pieceEntity.PieceId);
    }

    public PieceEntity Swap(PieceEntity pieceEntity)
    {
        pieceEntity.ResetShape();

        pieceEntity.transform.SetParent(_swapGO.transform, false);
        pieceEntity.transform.localPosition = -pieceEntity.PieceType.barycenter;
        pieceEntity.transform.rotation = Quaternion.identity;
        pieceEntity.enabled = false;

        PieceEntity oldPiece = SwapPiece;
        oldPiece.enabled = true;
        oldPiece.transform.SetParent(_playfield.transform, false);
        SwapPiece = pieceEntity;
        _playfield.BoardState.Swap();
        return oldPiece;
    }

}
