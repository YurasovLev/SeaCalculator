using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.Models;
public partial class ReceiverMode : ObservableObject {
    public enum Modes {
        Continuous,
        Periodic,
        Episodic
    }
    private readonly Receiver receiver;
    [ObservableProperty]
    private string _name = "";
    [ObservableProperty]
    private Modes _mode;
    [ObservableProperty]
    private double _loadFactor;
    [ObservableProperty]
    private double _cos;
    [ObservableProperty]
    private uint _workingReceiversCount;
    public double ActivePower {get => receiver.RatedSteadyPower * WorkingReceiversCount * LoadFactor; }
    public double ReactivePower {get => ActivePower * Math.Tan(Math.Acos(Cos)); }
    public ReceiverMode(Receiver _receiver) {
        receiver = _receiver;
    }
}