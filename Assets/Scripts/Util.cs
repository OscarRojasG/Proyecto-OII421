using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]); // Intercambio de valores
        }
    }

    public static void Shuffle<T>(this IList<T> lista)
    {
        int n = lista.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T valor = lista[k];
            lista[k] = lista[n];
            lista[n] = valor;
        }
    }
}
