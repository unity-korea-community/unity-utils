using System;
using System.Collections.Generic;
using System.Linq;

namespace UNKO.Utils
{
    public class SimplePool<T>
        where T : class
    {
        protected List<T> _allInstance = new List<T>(); public IReadOnlyList<T> allInstance => _allInstance;
        protected List<T> _use = new List<T>(); public IReadOnlyList<T> use => _use;
        protected List<T> _notUse = new List<T>(); public IReadOnlyList<T> notUse => _notUse;

        protected T _originItem { get; private set; }
        protected Func<T, T> _OnCreateInstance;

        public SimplePool(T originItem)
        {
            _OnCreateInstance = (origin) => Activator.CreateInstance<T>();
            Init(originItem, 0);
        }

        public SimplePool(T originItem, int initializeSize = 0)
        {
            _OnCreateInstance = (origin) => Activator.CreateInstance<T>();
            Init(originItem, initializeSize);
        }

        public SimplePool(Func<T> onCreateInstance)
        {
            _OnCreateInstance = (origin) => onCreateInstance();
            Init(onCreateInstance(), 0);
        }

        public SimplePool(Func<T> onCreateInstance, int initializeSize)
        {
            _OnCreateInstance = (origin) => onCreateInstance();
            Init(onCreateInstance(), initializeSize);
        }

        public T Spawn()
        {
            T spawnItem = null;
            if (_notUse.Count > 0)
            {
                int lastIndex = _notUse.Count - 1;
                spawnItem = _notUse[lastIndex];
                _notUse.RemoveAt(lastIndex);
            }
            else
            {
                spawnItem = OnRequireNewInstance(_originItem);
                _allInstance.Add(spawnItem);
            }

            OnSpawn(spawnItem);
            _use.Add(spawnItem);
            return spawnItem;
        }

        public void DeSpawn(T item)
        {
            if (_use.Contains(item) == false)
                return;

            OnDespawn(item);
            _use.Remove(item);
            _notUse.Add(item);
        }

        public void DeSpawnAll()
        {
            while (_use.Count > 0)
                DeSpawn(_use.Last());
        }

        protected virtual T OnRequireNewInstance(T originItem) => _OnCreateInstance(originItem);
        protected virtual void OnSpawn(T spawnTarget) { }
        protected virtual void OnDespawn(T despawnTarget) { }

        protected void Init(T originItem, int initializeSize)
        {
            _originItem = originItem;

            for (int i = 0; i < initializeSize; i++)
                Spawn();
            DeSpawnAll();
        }
    }
}
