using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ExtensionUtils
{
    public static IEnumerable<T[]> TakeEvery<T>(this IEnumerable<T> collection, int count)
    {
        int i = 0;
        T[] values = new T[count];

        foreach (var item in collection)
        {
            values[i] = item;
            i++;

            if(i == count)
            {
                yield return values;
                i = 0;
                values = new T[count];
            }
        }

        if (i == 0) yield break;
        yield return values;
    }

    public static TEnum ToEnum<TEnum>(this string str) where TEnum : struct
    {
        return Enum.Parse<TEnum>(str);
    }

    public static void DestroyAllChildren(this Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            UnityEngine.Object.Destroy(parent.GetChild(i).gameObject);
        }
    }

    public static IEnumerable<Vector3> ToCollection_Of_Vector3(this int[] coords)
    {
        return coords.TakeEvery(2).Select(e => new Vector3(e[0], e[1], 0f));
    }
    public static IEnumerable<Vector2Int> ToCollection_Of_Vector2Int(this int[] coords)
    {
        return coords.TakeEvery(2).Select(e => new Vector2Int(e[0], e[1]));
    }
}