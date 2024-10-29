using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;

public class ReceiverManager : ObservableObject {
    public ObservableCollection<Receiver> Receivers { get; }
    public ObservableCollection<ReceiverModeMemento> ReceiverModes { get; }
    public ReceiverManager()
    {
        Receivers = new();
        ReceiverModes = new();
        ReceiverModes.CollectionChanged += ReceiverModeUpdatesHandler;

        #if DEBUG
        for(int i = 1; i <= 5; i++)
            AddReceiver().Name = "Test" + i;
        for(int j = 1; j <= 3; j++)
            AddReceiverMode().Name = "TestMode" + j;
        #endif
    }
    ~ReceiverManager() {
        ReceiverModes.CollectionChanged -= ReceiverModeUpdatesHandler;
    }
    public Receiver AddReceiver() {
        Receiver receiver = new();
        Receivers.Add(receiver);
        return receiver;
    }
    public ReceiverModeMemento AddReceiverMode() {
        ReceiverModeMemento memento = new(Guid.NewGuid());
        ReceiverModes.Add(memento);
        return memento;
    }
    public ReceiverModeMemento RemoveReceiverMode(ReceiverModeMemento memento) {
        ReceiverModes.Remove(memento);
        return memento;
    }
    public void ReceiverModeUpdatesHandler(object? sender, NotifyCollectionChangedEventArgs e) {
        switch(e.Action) {
            case NotifyCollectionChangedAction.Add:
                if(e.NewItems is not null)
                    foreach(ReceiverModeMemento mode in e.NewItems)
                        foreach(var receiver in Receivers)
                            receiver.AddMode(mode);
                break;
            case NotifyCollectionChangedAction.Replace:
                if(e.OldItems is not null)
                    foreach(ReceiverModeMemento mode in e.OldItems)
                        foreach(var receiver in Receivers)
                            receiver.RemoveMode(mode);
                if(e.NewItems is not null)
                    foreach(ReceiverModeMemento mode in e.NewItems)
                        foreach(var receiver in Receivers)
                            receiver.AddMode(mode);
                break;
            case NotifyCollectionChangedAction.Remove:
                if(e.OldItems is not null)
                    foreach(ReceiverModeMemento mode in e.OldItems)
                        foreach(var receiver in Receivers)
                            receiver.RemoveMode(mode);
                break;
        }
    }
}