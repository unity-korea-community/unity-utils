using System;
using UnityEngine;

namespace UNKO.Utils
{
    public class UnitytComponentPool<T> : SimplePool<T>
        where T : UnityEngine.Component
    {
        Transform _parent;

        public UnitytComponentPool(T originItem, int initializeSize = 0) : base(originItem, initializeSize)
        {
        }

        public UnitytComponentPool(Func<T> onCreateInstance, int initializeSize = 0) : base(onCreateInstance, initializeSize)
        {
        }

        public UnitytComponentPool<T> SetParents(Transform parent)
        {
            _parent = parent;

            return this;
        }

        protected override T OnRequireNewInstance(T originItem)
        {
            T newInstance = base.OnRequireNewInstance(originItem);
            if (_parent != null)
                newInstance.transform.SetParent(_parent);

            newInstance.gameObject.SetActive(false);
            return newInstance;
        }

        protected override void OnSpawn(T spawnTarget)
        {
            base.OnSpawn(spawnTarget);

            spawnTarget.gameObject.SetActive(true);
        }
    }
}
