using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;
public partial class ReceiverMode : ObservableObject {
    [ObservableProperty]
    public string _name;
    public ObservableCollection<ReceiverModeParameters> receiverModeParameters { get; }
    public ReceiverMode(string name = "") {
        Name = name;
        receiverModeParameters = new();
    }
    public ReceiverModeParameters AddReceiverModeParametersTo(Receiver receiver) {
        ReceiverModeParameters parameters = new(receiver, this);
        receiverModeParameters.Add(parameters);
        return parameters;
    }
    public ReceiverModeParameters RemoveReceiverModeParametersFrom(Receiver receiver) {
        ReceiverModeParameters parameters = receiver.ModesParameters.First(m => ReferenceEquals(m.receiverMode, this));
        receiverModeParameters.Remove(parameters);
        return parameters;
    }
}
public partial class ReceiverModeParameters : ObservableObject {
    public static List<WorkMode> WorkModesList { get; }
    public enum WorkMode {
        Continuous,
        Periodic,
        Episodic
    }
    public readonly Receiver receiver;
    public readonly ReceiverMode receiverMode;
    [ObservableProperty]
    private WorkMode _mode;
    [ObservableProperty]
    private double? _loadFactor;
    [ObservableProperty]
    private double? _cos;
    [ObservableProperty]
    private uint? _workingReceiversCount;
    private double? _activePower;
    public double? ActivePower {get => _activePower; private set => SetProperty(ref _activePower, value); }
    private double? _reactivePower;
    public double? ReactivePower {get => _reactivePower; private set => SetProperty(ref _reactivePower, value); }
    static ReceiverModeParameters() {
        WorkModesList = typeof(WorkMode).GetEnumValues().Cast<WorkMode>().ToList();
    }
    public ReceiverModeParameters(Receiver _receiver, ReceiverMode mode) {
        receiver = _receiver;
        receiverMode = mode;

        PropertyChanged += PropertyCalcHandler;
        receiver.PropertyChanged += PropertyCalcHandler;
    }
    ~ReceiverModeParameters() {
        PropertyChanged -= PropertyCalcHandler;
        receiver.PropertyChanged -= PropertyCalcHandler;
    }
    private void PropertyCalcHandler(object? s, PropertyChangedEventArgs e) {
        switch(e.PropertyName) {
            case "RatedPowerConsumption":
            case "WorkingReceiversCount":
            case "LoadFactor":
                if( receiver.RatedPowerConsumption is not null && WorkingReceiversCount is not null && LoadFactor is not null )
                    ActivePower = Math.Round( receiver.RatedPowerConsumption.Value * WorkingReceiversCount.Value * LoadFactor.Value, 2);
                else ActivePower = null;
                break;
            case "ActivePower":
            case "Cos":
                if( ActivePower is not null && Cos is not null && Cos > 0)
                    ReactivePower = Math.Round(ActivePower.Value * Math.Tan(Math.Acos(Cos.Value)), 1);
                else ReactivePower = null;
                break;
        }
    }
}