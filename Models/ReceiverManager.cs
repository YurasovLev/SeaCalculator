using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;

public class ReceiverManager : ObservableObject {
    private readonly WhenCollectionChangedHandler<ReceiverMode> ReceiverModeUpdatesHandler;
    public ObservableCollection<Receiver> Receivers { get; }
    public ObservableCollection<ReceiverMode> ReceiverModes { get; }
    public GeneratorLoad generatorLoad { get; }
    public ReceiverManager()
    {
        Receivers = new();
        ReceiverModes = new();
        generatorLoad = new (ReceiverModes);
        ReceiverModeUpdatesHandler = new(
            AddedMode => {
                foreach(var receiver in Receivers)
                    receiver.AddMode(AddedMode);
            },
            RemovedMode => {
                foreach(var receiver in Receivers)
                    receiver.RemoveMode(RemovedMode);
            }
        );
        ReceiverModes.CollectionChanged += ReceiverModeUpdatesHandler.Handler;
    }
    ~ReceiverManager() {
        ReceiverModes.CollectionChanged -= ReceiverModeUpdatesHandler.Handler;
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
        ReceiverMode mode = new();
        ReceiverModes.Add(mode);
        return mode;
    }
    public ReceiverMode RemoveReceiverMode(ReceiverMode parameters) {
        ReceiverModes.Remove(parameters);
        return parameters;
    }
}