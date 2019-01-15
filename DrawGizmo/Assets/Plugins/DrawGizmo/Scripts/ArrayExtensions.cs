using UnityEngine;
using System.Collections.Generic;

public static class ArrayExtensions
{
    /// <summary>
    /// Returns all elements in this array, concatenated.
    /// </summary>
    public static string ToStringAll<T>(this T[] array, string sep = "")
    {
        string result = string.Empty;

        for (int i = 0; i < array.Length - 1; i++)
        {
            result += array[i].ToString() + sep;
        }

        if (array.Length > 0)
        {
            result += array[array.Length - 1];
        }

        return result;
    }
}