using System;
using UnityEngine;

#pragma warning disable IDE1006
namespace UNKO.Utils
{
    public abstract class SingletonComponentBase<T> : MonoBehaviour, IDisposable
        where T : SingletonComponentBase<T>
    {
        public static T instance
        {
            get
            {
                if (_isQuitApp)
                {
                    return default;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newObject = new GameObject(typeof(T).Name);
                        _instance = newObject.AddComponent<T>();
                    }

                    if (_isInitSingleton == false)
                    {
                        _instance.InitSingleton();
                    }
                }

                return _instance;
            }
        }

        private static T _instance { get; set; }
        private static bool _isQuitApp { get; set; }
        private static bool _isInitSingleton { get; set; }

        protected virtual void Awake()
        {
            if (_isInitSingleton == false)
            {
                InitSingleton();
            }
        }

        public static T GetOrCreateInstance()
        {
            return instance;
        }

        public static void DestroySingleton()
        {
            _instance?.Dispose();
        }

        public virtual void InitSingleton()
        {
            _isInitSingleton = true;
        }

        private void OnApplicationQuit()
        {
            _isQuitApp = true;
        }

        public void Dispose()
        {
            if ((this is null) == false && (gameObject is null) == false)
            {
                Destroy(gameObject);
            }
            _instance = null;
            _isInitSingleton = false;
        }
    }
}
#pragma warning restore IDE1006
