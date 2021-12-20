using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourEx
{
    public static void StartCoroutineNotOverlap(this MonoBehaviour target, string coroutineName)
    {
        target.StopCoroutine(coroutineName);
        target.StartCoroutine(coroutineName);
    }

    public static T GetOrAddComponent<T>(this Component target)
        where T : Component
        => target.gameObject.GetOrAddComponent<T>();

    public static T GetOrAddComponent<T>(this GameObject target)
        where T : Component
    {
        T returnComponent = target.GetComponent<T>();
        if (returnComponent != null)
        {
            return returnComponent;
        }

        return target.AddComponent<T>();
    }

    public static void SetActive(this Component target, bool active)
        => target.gameObject.SetActive(active);

    public static bool IsNull(this GameObject target)
    {
        return target is null;
    }

    public static bool IsNull(this Component target)
    {
        return target is null;
    }
}