using System;
using System.Collections.Generic;
using UnityEngine;

namespace UNKO.Utils
{
    public static class DataSenderUnityExtension
    {
        public static DataSender<T> InitComponents<T>(this DataSender<T> target, MonoBehaviour owner)
        {
            IObserver<T>[] our = owner.GetComponents<IObserver<T>>();
            target.Subscribe(our);

            return target;
        }

        public static DataSender<T> InitChildrenComponents<T>(this DataSender<T> target, MonoBehaviour owner)
        {
            IObserver<T>[] children = owner.GetComponentsInChildren<IObserver<T>>();
            target.Subscribe(children);

            return target;
        }

        public static DataSender<T> InitParentsComponents<T>(this DataSender<T> target, MonoBehaviour owner)
        {
            IObserver<T>[] children = owner.GetComponentsInParent<IObserver<T>>();
            target.Subscribe(children);

            return target;
        }

        public static DataSender<T> InitSiblingComponents<T>(this DataSender<T> target, MonoBehaviour owner)
        {
            List<IObserver<T>> siblings = new List<IObserver<T>>();
            Transform transform = owner.transform;
            Transform parnet = transform.parent;
            for (int i = 0; i < parnet.childCount; i++)
            {
                Transform sibling = parnet.GetChild(i);
                if (sibling == transform)
                {
                    continue;
                }

                sibling.GetComponents<IObserver<T>>(siblings);
                target.Subscribe(siblings);
            }

            return target;
        }
    }
}
