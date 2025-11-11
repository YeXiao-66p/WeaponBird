using System;
using UnityEngine;

class Resloader
{
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    internal static T Load<T>(object resource)
    {
        throw new NotImplementedException();
    }
}