using System;
using System.ComponentModel;
using Avalonia.Diagnostics.Screenshots;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.Models;

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
    [ObservableProperty]
    public double _ratedPowerConsumption;
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