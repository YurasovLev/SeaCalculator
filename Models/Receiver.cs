using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;

public partial class Receiver : ObservableObject {
    [ObservableProperty]
    private string _name = "";
    [ObservableProperty]
    private uint _count;
    [ObservableProperty]
    private double _ratedSteadyPower;
    [ObservableProperty]
    private double _cos;
    [ObservableProperty]
    private double _efficiency;
    private double _ratedPowerConsumption;
    public double RatedPowerConsumption { get => _ratedPowerConsumption; private set => SetProperty(ref _ratedPowerConsumption, value); }
    public ObservableCollection<ReceiverModeParameters> ModesParameters { get; }
    public Receiver() {
        ModesParameters = new();
        PropertyChanged += PropertyCalcHandler;
    }
    ~Receiver() {
        PropertyChanged -= PropertyCalcHandler;
    }
    private void PropertyCalcHandler(object? s, PropertyChangedEventArgs e) {
        switch(e.PropertyName) {
            case "Efficiency":
            case "RatedSteadyPower":
                try {
                    RatedPowerConsumption = Math.Round(RatedSteadyPower / Efficiency, 2);
                } catch (DivideByZeroException) {
                    RatedPowerConsumption = double.NaN;
                }
                break;
        }
    }
    public ReceiverModeParameters AddMode(ReceiverMode receiverMode) {
        ReceiverModeParameters parameters = receiverMode.AddReceiverModeParametersTo(this);
        ModesParameters.Add(parameters);
        return parameters;
    }
    public ReceiverModeParameters RemoveMode(ReceiverMode receiverMode) {
        ReceiverModeParameters parameters = receiverMode.RemoveReceiverModeParametersFrom(this);
        ModesParameters.Remove(parameters);
        return parameters;
    }
}