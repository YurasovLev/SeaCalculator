using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;

public class ReceiverManager : ObservableObject {
    public ObservableCollection<Receiver> Receivers { get; }
    public ObservableCollection<ReceiverMode> ReceiverModes { get; }
    public ReceiverManager()
    {
        Receivers = new();
        ReceiverModes = new();
        ReceiverModes.CollectionChanged += ReceiverModeUpdatesHandler;
    }
    ~ReceiverManager() {
        ReceiverModes.CollectionChanged -= ReceiverModeUpdatesHandler;
    }
    public Receiver AddReceiver() {
        Receiver receiver = new();
        foreach(ReceiverMode mode in ReceiverModes)
            receiver.AddMode(mode);
        Receivers.Add(receiver);
        return receiver;
    }
    public Receiver RemoveReceiver(Receiver receiver) {
        foreach(ReceiverMode mode in ReceiverModes)
            mode.RemoveReceiverModeParametersFrom(receiver);
        Receivers.Remove(receiver);
        return receiver;
    }
    public ReceiverMode AddReceiverMode() {
        ReceiverMode mode = new(Guid.NewGuid());
        ReceiverModes.Add(mode);
        return mode;
    }
    public ReceiverMode RemoveReceiverMode(ReceiverMode parameters) {
        ReceiverModes.Remove(parameters);
        return parameters;
    }
    public void ReceiverModeUpdatesHandler(object? sender, NotifyCollectionChangedEventArgs e) {
        switch(e.Action) {
            case NotifyCollectionChangedAction.Add:
                if(e.NewItems is not null)
                    foreach(ReceiverMode mode in e.NewItems)
                        foreach(var receiver in Receivers)
                            receiver.AddMode(mode);
                break;
            case NotifyCollectionChangedAction.Replace:
                if(e.OldItems is not null)
                    foreach(ReceiverMode mode in e.OldItems)
                        foreach(var receiver in Receivers)
                            receiver.RemoveMode(mode);
                if(e.NewItems is not null)
                    foreach(ReceiverMode mode in e.NewItems)
                        foreach(var receiver in Receivers)
                            receiver.AddMode(mode);
                break;
            case NotifyCollectionChangedAction.Remove:
                if(e.OldItems is not null)
                    foreach(ReceiverMode mode in e.OldItems)
                        foreach(var receiver in Receivers)
                            receiver.RemoveMode(mode);
                break;
        }
    }
}