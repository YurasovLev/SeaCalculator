using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;
public partial class ReceiverMode : ObservableObject {
    public List<WorkMode> WorkModesList { get; }
    public enum WorkMode {
        Continuous,
        Periodic,
        Episodic
    }
    private readonly Receiver receiver;
    [ObservableProperty]
    private string _name = "";
    [ObservableProperty]
    private WorkMode _mode;
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
        WorkModesList = typeof(WorkMode).GetEnumValues().Cast<WorkMode>().ToList();
    }
}