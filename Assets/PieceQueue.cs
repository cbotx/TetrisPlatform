using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceQueue : MonoBehaviour
{
    public const int QueueSize = 5;
    private List<PieceEntity> _queue = new();
    private PieceEntity _swapPiece = null;
    private GameObject _swapGO;
    private GameObject _nextQueueGO;

    private Playfield _playfield = null;
    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
        _swapGO = transform.Find("SwapQueue").gameObject;
        _nextQueueGO = transform.Find("NextQueue").gameObject;
    }

    public PieceEntity PushAndGetPiece(PieceEntity pieceEntity)
    {
        PushPiece(pieceEntity);
        PieceEntity firstPieceEntity = _queue[0];
        _queue.RemoveAt(0);
        firstPieceEntity.transform.SetParent(_playfield.transform, false);
        firstPieceEntity.enabled = true;
        return firstPieceEntity;
    }

    public void PushPiece(PieceEntity pieceEntity)
    {
        foreach (PieceEntity obj in _queue)
        {
            obj.transform.position += new Vector3(0, 2, 0);
        }

        // PieceEntity newPieceEntity = Instantiate(pieceEntity, _playfield.transform);

        pieceEntity.transform.SetParent(_nextQueueGO.transform, false);
        pieceEntity.transform.position = _nextQueueGO.transform.position;
        pieceEntity.enabled = false;
        _queue.Add(pieceEntity);
    }

    public void SetSwap(PieceEntity pieceEntity)
    {
        // PieceEntity newPieceEntity = Instantiate(pieceEntity, _playfield.transform);

        pieceEntity.transform.SetParent(_swapGO.transform, false);
        pieceEntity.transform.position = _swapGO.transform.position;
        pieceEntity.enabled = false;
        _swapPiece = pieceEntity;
    }

    public PieceEntity Swap(PieceEntity pieceEntity)
    {
        pieceEntity.transform.SetParent(_swapGO.transform, false);
        pieceEntity.transform.position = _swapGO.transform.position;
        pieceEntity.transform.rotation = Quaternion.identity;
        pieceEntity.enabled = false;
        PieceEntity oldPiece = _swapPiece;
        oldPiece.enabled = true;
        oldPiece.transform.SetParent(_playfield.transform, false);
        _swapPiece = pieceEntity;
        return oldPiece;
    }

}
