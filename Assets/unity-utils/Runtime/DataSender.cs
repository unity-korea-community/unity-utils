
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UNKO.Utils
{
    public partial class DataSender<T> : IObservable<T>, IDisposable
    {
        protected HashSet<IObserver<T>> _observers = new HashSet<IObserver<T>>();
        protected T _lastSendedData = default;

        public void SendData(T data)
        {
            foreach (var item in _observers)
                item.OnNext(data);

            _lastSendedData = data;
        }

        public void Reset()
        {
            Dispose();
            _observers = new HashSet<IObserver<T>>();
        }

        public DataSender<T> InitChildrenComponents(MonoBehaviour owner)
        {
            IObserver<T>[] children = owner.GetComponentsInChildren<IObserver<T>>();
            Subscribe(children);

            return this;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observers.Contains(observer) == false)
                _observers.Add(observer);
            observer.OnNext(_lastSendedData);

            return new Unsubscriber<T>(_observers, observer);
        }

        public void Subscribe(params IObserver<T>[] observers)
        {
            foreach (IObserver<T> observer in observers)
            {
                _observers.Add(observer);
                observer.OnNext(_lastSendedData);
            }
        }

        public void UnSubscribe(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        public void Dispose()
        {
            foreach (IObserver<T> observer in _observers)
                observer.OnCompleted();

            _observers.Clear();
            _observers = null;
        }
    }
}