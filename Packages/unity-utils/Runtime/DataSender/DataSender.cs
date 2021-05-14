
using System;
using System.Collections.Generic;

namespace UNKO.Utils
{
    public class DataSender<T> : IObservable<T>, IDisposable
    {
        protected HashSet<IObserver<T>> _observers = new HashSet<IObserver<T>>();
        protected SimplePool<Unsubscriber<T>> _pool = new SimplePool<Unsubscriber<T>>();
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

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observers.Contains(observer) == false)
                _observers.Add(observer);
            observer.OnNext(_lastSendedData);

            Unsubscriber<T> unsubscriber = _pool.Spawn();
            unsubscriber.Reset(_observers, observer, (item) => _pool.DeSpawn(item));

            return unsubscriber;
        }


        public void Subscribe(IEnumerable<IObserver<T>> observers)
        {
            foreach (IObserver<T> observer in observers)
            {
                _observers.Add(observer);
                observer.OnNext(_lastSendedData);
            }
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
            _pool.DeSpawnAll();
            _observers = null;
        }
    }
}