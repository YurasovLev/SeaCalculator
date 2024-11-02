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
    public readonly Guid ID;
    public ObservableCollection<ReceiverModeParameters> receiverModeParameters { get; }
    public ReceiverMode(Guid id, string name = "") {
        ID = id;
        Name = name;
        receiverModeParameters = new();
    }
    public ReceiverModeParameters AddReceiverModeParametersTo(Receiver receiver) {
        ReceiverModeParameters parameters = new(receiver, ID);
        receiverModeParameters.Add(parameters);
        return parameters;
    }
    public ReceiverModeParameters RemoveReceiverModeParametersFrom(Receiver receiver) {
        ReceiverModeParameters parameters = receiver.ModesParameters.First(m => m.ID == ID);
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
    private readonly Receiver receiver;
    public Guid ID { get; }
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
    static ReceiverModeParameters() {
        WorkModesList = typeof(WorkMode).GetEnumValues().Cast<WorkMode>().ToList();
    }
    public ReceiverModeParameters(Receiver _receiver, Guid id) {
        receiver = _receiver;
        ID = id;

        PropertyChanged += PropertyCalcHandler;
        receiver.PropertyChanged += PropertyCalcHandler;
    }
    ~ReceiverModeParameters() {
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