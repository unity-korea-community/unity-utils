using UnityEngine;

namespace UNKO.Utils
{
    public abstract class SingletonSOBase<T> : ScriptableObject
        where T : SingletonSOBase<T>
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>("");
                    if (_instance == null)
                    {
                        Debug.LogError($"{nameof(T)} instance is null");
                    }
                }

                return _instance;
            }
        }

        private static T _instance { get; set; }
    }
}