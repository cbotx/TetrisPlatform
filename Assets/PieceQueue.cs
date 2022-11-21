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
        _swapGO = GameObject.Find("SwapQueue");
        _nextQueueGO = GameObject.Find("NextQueue");
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
        PieceEntity newPieceEntity = Instantiate(pieceEntity, _playfield.transform);

        newPieceEntity.transform.SetParent(_nextQueueGO.transform, false);
        newPieceEntity.transform.position = _nextQueueGO.transform.position;
        newPieceEntity.enabled = false;
        _queue.Add(newPieceEntity);
    }

    public void SetSwap(PieceEntity pieceEntity)
    {
        PieceEntity newPieceEntity = Instantiate(pieceEntity, _playfield.transform);

        newPieceEntity.transform.SetParent(_swapGO.transform, false);
        newPieceEntity.transform.position = _swapGO.transform.position;
        newPieceEntity.enabled = false;
        _swapPiece = newPieceEntity;
    }

    public PieceEntity Swap(PieceEntity pieceEntity)
    {
        pieceEntity.transform.SetParent(_swapGO.transform, false);
        pieceEntity.transform.position = _swapGO.transform.position;
        pieceEntity.enabled = false;
        PieceEntity oldPiece = _swapPiece;
        oldPiece.enabled = true;
        oldPiece.transform.SetParent(_playfield.transform, false);
        _swapPiece = pieceEntity;
        return oldPiece;
    }

}
