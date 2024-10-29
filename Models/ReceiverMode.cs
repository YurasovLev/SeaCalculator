using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    private double _activePower;
    public double ActivePower {get => _activePower; private set => SetProperty(ref _activePower, value); }
    private double _reactivePower;
    public double ReactivePower {get => _reactivePower; private set => SetProperty(ref _reactivePower, value); }
    public ReceiverMode(Receiver _receiver) {
        receiver = _receiver;
        WorkModesList = typeof(WorkMode).GetEnumValues().Cast<WorkMode>().ToList();
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