using UnityEngine;

namespace UNKO.Utils
{
    public abstract class SingletonComponentBase<T> : MonoBehaviour
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
                    _instance.InitSingleton();
                }

                return _instance;
            }
        }

        private static T _instance { get; set; }
        private static bool _isQuitApp { get; set; }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                InitSingleton();
            }
        }

        protected virtual void InitSingleton()
        {
        }

        private void OnApplicationQuit()
        {
            _isQuitApp = true;
        }
    }
}
