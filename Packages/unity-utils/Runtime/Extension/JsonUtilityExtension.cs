using UnityEngine;

public static class JsonUtilityEx
{
    public static bool TryFromJson<T>(string json, out T result)
        => TryFromJson(json, out result, Debug.LogError);

    public static bool TryFromJson<T>(string json, out T result, System.Action<string> OnError)
    {
        result = default;
        bool isSuccess = true;

        try
        {
            result = JsonUtility.FromJson<T>(json);
        }
        catch (System.Exception error)
        {
            OnError(error.ToString());
            isSuccess = false;
        }

        return isSuccess;
    }
}