using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;
public partial class ReceiverMode : ObservableObject {
    public static List<WorkMode> WorkModesList { get; }
    public enum WorkMode {
        Continuous,
        Periodic,
        Episodic
    }
    private readonly Receiver receiver;
    public readonly Guid ID;
    [ObservableProperty]
    private WorkMode _mode;
    [ObservableProperty]
    private double _loadFactor;
    [ObservableProperty]
    private double _cos;
    [ObservableProperty]
    private uint _workingReceiversCount;
    private double _activePower;
    public double ActivePower {get => _activePower; private set => SetProperty(ref _activePower, value); }
    private double _reactivePower;
    public double ReactivePower {get => _reactivePower; private set => SetProperty(ref _reactivePower, value); }
    static ReceiverMode() {
        WorkModesList = typeof(WorkMode).GetEnumValues().Cast<WorkMode>().ToList();
    }
    public ReceiverMode(Receiver _receiver, ReceiverModeMemento memento) {
        receiver = _receiver;
        ID = memento.ID;
        Mode = memento.Mode;
        LoadFactor = memento.LoadFactor;
        Cos = memento.Cos;

        PropertyChanged += PropertyCalcHandler;
        receiver.PropertyChanged += PropertyCalcHandler;
    }
    ~ReceiverMode() {
        PropertyChanged -= PropertyCalcHandler;
        receiver.PropertyChanged -= PropertyCalcHandler;
    }
    private void PropertyCalcHandler(object? s, PropertyChangedEventArgs e) {
        switch(e.PropertyName) {
            case "RatedSteadyPower":
            case "WorkingReceiversCount":
            case "LoadFactor":
                try {
                    ActivePower = receiver.RatedSteadyPower * WorkingReceiversCount * LoadFactor;
                } catch (DivideByZeroException) {
                    ActivePower = double.NaN;
                }
                break;
            case "ActivePower":
            case "Cos":
                try {
                    ReactivePower = ActivePower * Math.Tan(Math.Acos(Cos));
                } catch (DivideByZeroException) {
                    ReactivePower = double.NaN;
                }
                break;
        }
    }
}
public class ReceiverModeMemento {
    public string Name { get; set; }
    public readonly Guid ID;
    public readonly ReceiverMode.WorkMode Mode;
    public readonly double LoadFactor;
    public readonly double Cos;
    public ReceiverModeMemento(in Guid id) {
        ID = id;
        Name = "";
    }
    public ReceiverModeMemento(in Guid id, in string name, in ReceiverMode.WorkMode mode, in double loadFactor, in double cos) {
        ID = id;
        Name = name;
        Mode = mode;
        LoadFactor = loadFactor;
        Cos = cos;
    }
}