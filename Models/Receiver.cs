using System;
using System.ComponentModel;
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
    public ObservableCollectionOfObservableObjects<ReceiverMode> ReceiverModes { get; }
    public Receiver() {
        ReceiverModes = new();
        PropertyChanged += RatedPowerConsumptionHandler;
    }
    ~Receiver() {
        PropertyChanged -= RatedPowerConsumptionHandler;
    }
    private void RatedPowerConsumptionHandler(object? s, PropertyChangedEventArgs e) {
        switch(e.PropertyName) {
            case "Efficiency":
            case "RatedSteadyPower":
                try {
                    RatedPowerConsumption = RatedSteadyPower / Efficiency;
                } catch (DivideByZeroException) {
                    RatedPowerConsumption = double.NaN;
                }
                break;
        }
    }
    public ReceiverMode AddMode() {
        ReceiverMode mode = new(this);
        ReceiverModes.Add(mode);
        return mode;
    }
}