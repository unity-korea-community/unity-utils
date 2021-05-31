
using System;
using System.Collections.Generic;

namespace UNKO.Utils
{
    public class Unsubscriber<T> : IDisposable
    {
        private HashSet<IObserver<T>> _observers;
        private IObserver<T> _observer;
        private Action<Unsubscriber<T>> _onDisplose;

        public void Reset(HashSet<IObserver<T>> observers, IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
            this._onDisplose = null;
        }

        public void Reset(HashSet<IObserver<T>> observers, IObserver<T> observer, Action<Unsubscriber<T>> onDisplose)
        {
            this._observers = observers;
            this._observer = observer;
            this._onDisplose = onDisplose;
        }

        public void Dispose()
        {
            if (_observer != null)
            {
                _observers.Remove(_observer);
            }

            _onDisplose?.Invoke(this);
        }
    }
}
