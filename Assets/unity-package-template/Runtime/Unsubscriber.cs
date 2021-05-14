
using System;
using System.Collections.Generic;

namespace UNKO.Utils
{
    public class Unsubscriber<T> : IDisposable
    {
        private readonly HashSet<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public Unsubscriber(HashSet<IObserver<T>> observers, IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observer != null)
                _observers.Remove(_observer);
        }
    }

    public class UnsubscriberPool<T> : SimplePool<Unsubscriber<T>>
    {
        protected override Unsubscriber<T> OnRequireNewInstance(Unsubscriber<T> originItem)
        {
            return base.OnRequireNewInstance(originItem);
        }
    }
}