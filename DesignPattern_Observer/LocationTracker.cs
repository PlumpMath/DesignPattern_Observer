using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern_Observer
{
    public class LocationTracker : IObservable<Location>
    {
        // List to save the observers how need to be notified
        private List<IObserver<Location>> observers;

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationTracker()
        {
            observers = new List<IObserver<Location>>();
        }
                
        /// <summary>
        /// Add observer to list with observers
        /// </summary>
        /// <param name="observer"> object that wants to observe an other object</param>
        /// <returns>Unsubscriber object to remove itself from subscriber list</returns>
        /// 
        public IDisposable Subscribe(IObserver<Location> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        /// <summary>
        /// private class to remove the observer from the observer list
        /// </summary>
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<Location>> _observers;
            private IObserver<Location> _observer;

            public Unsubscriber(List<IObserver<Location>> observers, IObserver<Location> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }


        /// <summary>
        /// Inform the observers with a new location
        /// </summary>
        /// <param name="loc"> location object that also can be null</param>
        public void TrackLocation(Nullable<Location> loc)
        {
            foreach (var observer in observers)
            {
                if (!loc.HasValue)
                    observer.OnError(new LocationUnknownException());
                else
                    observer.OnNext(loc.Value);
            }
        }

        /// <summary>
        /// Stop the observer
        /// </summary>
        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }
}
