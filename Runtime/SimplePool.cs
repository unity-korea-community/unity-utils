using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UNKO.Utils
{
    public class SimplePool<T> : IDisposable
        where T : class
    {
        [SerializeField]
        protected List<T> _allInstance = new List<T>(); public IReadOnlyList<T> AllInstance => _allInstance;
        [SerializeField]
        protected List<T> _use = new List<T>(); public IReadOnlyList<T> Use => _use;
        [SerializeField]
        protected List<T> _notUse = new List<T>(); public IReadOnlyList<T> NotUse => _notUse;

        [SerializeField]
        private T _originItem = null; public T OriginItem => _originItem;
        protected Func<T, T> _OnCreateInstance;

        public SimplePool(T originItem)
        {
            _OnCreateInstance = (origin) => Activator.CreateInstance<T>();
            Init(originItem, 0);
        }

        public SimplePool(T originItem, int initializeSize)
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

        public void PrePooling(int prePoolCount)
        {
            for (int i = 0; i < prePoolCount; i++)
            {
                Spawn();
            }

            DeSpawnAll();
        }

        public bool IsEmptyPool()
            => _notUse.Count == 0 && _allInstance.Count > 0;

        public T Spawn()
        {
            T spawnItem;
            if (_notUse.Count > 0)
            {
                int lastIndex = _notUse.Count - 1;
                spawnItem = _notUse[lastIndex];
                _notUse.RemoveAt(lastIndex);
            }
            else
            {
                spawnItem = OnRequireNewInstance(OriginItem);
                _allInstance.Add(spawnItem);
            }

            OnSpawn(spawnItem);
            _use.Add(spawnItem);
            return spawnItem;
        }

        public void DeSpawn(T item)
        {
            if (item == null)
            {
                Debug.LogError($"despawn item is null");
                return;
            }

            if (_use.Contains(item) == false)
            {
                return;
            }

            OnDespawn(item);
            _use.Remove(item);
            _notUse.Add(item);
        }

        public void DeSpawnAll()
        {
            while (_use.Count > 0)
            {
                DeSpawn(_use.Last());
            }
        }

        public virtual void OnDisposeItem(T item) { }

        protected virtual T OnRequireNewInstance(T originItem) => _OnCreateInstance(originItem);
        protected virtual void OnSpawn(T spawnTarget) { }
        protected virtual void OnDespawn(T despawnTarget) { }

        protected void Init(T originItem, int initializeSize)
        {
            _originItem = originItem;
            PrePooling(initializeSize);
        }

        public virtual void Dispose()
        {
            CollectionExtension.ForEach(_allInstance, this.OnDisposeItem);
            _allInstance.Clear();

            _use.Clear();
            _notUse.Clear();
        }
    }
}
