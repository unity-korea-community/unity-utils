#pragma warning disable IDE1006
namespace UNKO.Utils
{
    public abstract class SingletonBase<T>
        where T : SingletonBase<T>, new()
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.InitSingleton();
                }

                return _instance;
            }
        }

        private static T _instance { get; set; }

        public virtual void InitSingleton()
        {
        }

        static public T CreateInstance()
        {
            return instance;
        }
    }
}
#pragma warning restore IDE1006
