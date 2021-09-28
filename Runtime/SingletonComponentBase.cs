using UnityEngine;

#pragma warning disable IDE1006
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

        public virtual void InitSingleton()
        {
            _isInitSingleton = true;
        }

        private void OnApplicationQuit()
        {
            _isQuitApp = true;
        }
    }
}
#pragma warning restore IDE1006
