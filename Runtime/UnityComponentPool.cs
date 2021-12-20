using System;
using UnityEngine;

namespace UNKO.Utils
{
    [Serializable]
    public class UnityComponentPool<TComponent> : SimplePool<TComponent>
        where TComponent : Component
    {
        Transform _parent;

        public UnityComponentPool(TComponent originItem) : base(originItem)
        {
            originItem.gameObject.SetActive(false);
            _OnCreateInstance = (TComponent origin) => GameObject.Instantiate(origin);
        }

        public UnityComponentPool(TComponent originItem, int initializeSize) : base(originItem, initializeSize)
        {
            originItem.gameObject.SetActive(false);
            _OnCreateInstance = (TComponent origin) => GameObject.Instantiate(origin);
        }

        public UnityComponentPool(Func<TComponent> onCreateInstance) : base(onCreateInstance)
        {
        }

        public UnityComponentPool(Func<TComponent> onCreateInstance, int initializeSize) : base(onCreateInstance, initializeSize)
        {
        }

        public void SetParents(Transform parent)
        {
            _parent = CreateParents_IfNull(parent);
        }

        protected override TComponent OnRequireNewInstance(TComponent originItem)
        {
            TComponent newInstance = base.OnRequireNewInstance(originItem);
            newInstance.transform.SetParent(_parent);

            newInstance.gameObject.SetActive(false);
            return newInstance;
        }

        protected override void OnSpawn(TComponent spawnTarget)
        {
            base.OnSpawn(spawnTarget);

            if (spawnTarget == null)
            {
                return;
            }

            spawnTarget.gameObject.SetActive(true);

#if UNITY_EDITOR
            UpdateUI();
#endif
        }

        protected override void OnDespawn(TComponent despawnTarget)
        {
            base.OnDespawn(despawnTarget);

            if (despawnTarget == null)
            {
                return;
            }

            GameObject despawnObject = despawnTarget.gameObject;
            if (despawnObject.activeInHierarchy)
            {
                despawnObject.SetActive(false);
            }

            despawnObject.transform.SetParent(_parent);

#if UNITY_EDITOR
            UpdateUI();
#endif
        }

        public override void OnDisposeItem(TComponent item)
        {
            base.OnDisposeItem(item);

            if (item == null)
            {
                return;
            }

            GameObject.Destroy(item.gameObject);
        }

        private Transform CreateParents_IfNull(Transform parent)
        {
            if (parent == null)
            {
                parent = new GameObject(GetName()).transform;
            }

            return parent;
        }

        private void UpdateUI()
        {
            if (_parent == null)
            {
                _parent = CreateParents_IfNull(null);
            }
            _parent.gameObject.name = $"{GetName()}.Pool/use:{_use.Count}/notUse:{_notUse.Count}/all:{_allInstance.Count}";
        }

        private string GetName()
        {
            const int MAX_NAME_LENGTH = 5;

            string name = typeof(TComponent).Name;
            name = name.Substring(0, MAX_NAME_LENGTH);

            return $"{name}.{OriginItem.name}";
        }
    }
}
