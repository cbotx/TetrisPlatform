using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public class KickTests
{
    public Vector2Int[] attempts;

    public Vector2Int this[int attemptIndex]
    {
        get => attempts[attemptIndex];
        set { attempts[attemptIndex] = value; }
    }


    public KickTests(IEnumerable<Vector2Int> offsets)
    {
        this.attempts = offsets.ToArray();
    }
    public KickTests(params int[] coords) : this(coords.ToCollection_Of_Vector2Int()) { }

    public KickTests GetSymmetric()
    {
        return new(attempts.Select(e=>new Vector2Int(-e.x,-e.y)));
    }
}


public class SpinTable: IEnumerable<KickTests>
{
    public KickTests[] kickTests;
    public KickTests this[int from_direction]
    {
        get => kickTests[from_direction];
        set { kickTests[from_direction] = value; }
    }

    public SpinTable(IEnumerable<KickTests> kickTests)
    {
        this.kickTests = kickTests.ToArray();
    }
    public SpinTable(params KickTests[] kickTests)
    {
        this.kickTests = kickTests;
    }

    public SpinTable(int[,,] array)
    {
        int l0 = array.GetLength(0);
        int l1 = array.GetLength(1);
        kickTests = new KickTests[l0];
        for (int i = 0; i < l0; i++)
        {
            var offsets = new Vector2Int[l1];
            for (int j = 0; j < l1; j++)
            {
                offsets[j] = new Vector2Int(array[i, j, 0], array[i, j, 1]);
            }
            kickTests[i] = new KickTests(offsets);
        }
    }

    public override string ToString()
    {
        return string.Join("\n", 
            kickTests.Select(e => string.Join(", ",
                e.attempts.Select(
                    o => $"({o.x,2}, {o.y,2})"
                )
            ))
        );
    }

    //public SpinTable FillSymmetric()
    //{
    //    kickTests = kickTests.Concat(kickTests.Select(e => e.GetSymmetric())).ToArray();
    //    return this;
    //}
    public SpinTable GetSymmetric()
    {
        int length = kickTests.Length;
        int maxIndex = length - 1;
        return new(Enumerable.Range(0, length).Select(i =>
            kickTests[(i + maxIndex) % length].GetSymmetric()
        ));
    }

    public IEnumerator<KickTests> GetEnumerator()
    {
        foreach (var item in kickTests)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        foreach (var item in kickTests)
        {
            yield return item;
        }
    }
}