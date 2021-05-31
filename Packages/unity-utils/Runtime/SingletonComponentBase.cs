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
                if (s_isQuitApp)
                {
                    return default;
                }

                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                    s_instance.InitSingleton();
                }

                return s_instance;
            }
        }

        private static T s_instance { get; set; }
        private static bool s_isQuitApp { get; set; }

        void Awake()
        {
            if (s_instance == null)
            {
                InitSingleton();
            }
        }

        protected virtual void InitSingleton()
        {
        }

        private void OnApplicationQuit()
        {
            s_isQuitApp = true;
        }
    }
}
