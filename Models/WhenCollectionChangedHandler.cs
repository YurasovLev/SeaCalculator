using System;
using System.Collections.Specialized;

namespace SeaCalculator.Models;

public class WhenCollectionChangedHandler<T> {
    private Action<T> WhenAdded { get; init;}
    private Action<T> WhenRemoved { get; init;}
    public WhenCollectionChangedHandler(Action<T> whenAdded, Action<T> whenRemoved) {
        WhenAdded = whenAdded;
        WhenRemoved = whenRemoved;
    }
    public void Handler(object? collection, NotifyCollectionChangedEventArgs args) {
        switch(args.Action) {
            case NotifyCollectionChangedAction.Add:
                if(args.NewItems is not null)
                    foreach(T item in args.NewItems)
                        WhenAdded(item);
                break;
            case NotifyCollectionChangedAction.Remove:
                if(args.OldItems is not null)
                    foreach(T item in args.OldItems)
                        WhenRemoved(item);
                break;
            case NotifyCollectionChangedAction.Replace:
                if(args.NewItems is not null)
                    foreach(T item in args.NewItems)
                        WhenAdded(item);
                if(args.OldItems is not null)
                    foreach(T item in args.OldItems)
                        WhenRemoved(item);
                break;
        }
    }
}