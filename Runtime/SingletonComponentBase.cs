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
                    return default;

                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                    s_instance.InitSingleton();
                }

                return s_instance;
            }
        }

        private static T s_instance;
        protected static bool s_isQuitApp { get; private set; } = false;

        protected virtual void InitSingleton()
        {
        }

        private void OnApplicationQuit()
        {
            s_isQuitApp = true;
        }
    }
}
