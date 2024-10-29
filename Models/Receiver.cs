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
    public ObservableCollection<ReceiverMode> ReceiverModes { get; }
    public Receiver() {
        ReceiverModes = new();
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
                    RatedPowerConsumption = RatedSteadyPower / Efficiency;
                } catch (DivideByZeroException) {
                    RatedPowerConsumption = double.NaN;
                }
                break;
        }
    }
    public ReceiverMode AddMode(ReceiverModeMemento memento) {
        ReceiverMode mode = new(this, memento);
        ReceiverModes.Add(mode);
        return mode;
    }
    public ReceiverMode RemoveMode(ReceiverModeMemento memento) {
        ReceiverMode mode = ReceiverModes.First(m => m.ID == memento.ID);
        ReceiverModes.Remove(mode);
        return mode;
    }
}