using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public const int s_Width = 10;
    public const int s_Height = 20;
    private Transform[,] s_Field = new Transform[s_Width, s_Height];

    public PieceEntity GhostedPiece { get; set; }
    public PieceEntity FieldPiece { get; set; }

    private Spawner _spawner = null;

    private void Awake()
    {
        _spawner = GetComponentInChildren<Spawner>();
    }
    public bool HasEntityAt(int x, int y)
    {
        return s_Field[x, y] != null;
    }

    public void FreezePiece()
    {
        // Set field
        foreach (Transform child in FieldPiece.transform)
        {
            int x = Mathf.RoundToInt(child.transform.position.x);
            int y = Mathf.RoundToInt(child.transform.position.y);
            s_Field[x, y] = child;
        }

        // Reparent subtiles
        for (int i = FieldPiece.transform.childCount - 1; i >= 0; --i)
        {
            FieldPiece.transform.GetChild(i).SetParent(transform);
        }
        Destroy(FieldPiece.gameObject);
        Destroy(GhostedPiece.gameObject);
        ClearLines();

        // Spawn new piece
        _spawner.NextPiece();
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
                if (s_Field[j, i] == null)
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
            Destroy(s_Field[j, lineIdx].gameObject);
            s_Field[j, lineIdx] = null;
        }
    }

    private void MoveLineEntities(int lineIdx, int offset)
    {
        for (int j = 0; j < s_Width; ++j)
        {
            if (s_Field[j, lineIdx])
            {
                s_Field[j, lineIdx].transform.position += new Vector3(0, -offset, 0);
                s_Field[j, lineIdx - offset] = s_Field[j, lineIdx];
                s_Field[j, lineIdx] = null;
            }
        }
    }
}
