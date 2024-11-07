using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SeaCalculator.Models;

public class GeneratorLoad : ObservableObject {
    private readonly WhenCollectionChangedHandler<ReceiverMode> ReceiverModesUpdateHandler;
    public ObservableCollection<GeneratorLoadParameters> loadParameters { get; }
    private ObservableCollection<ReceiverMode> receiverModes;
    public GeneratorLoad(ObservableCollection<ReceiverMode> _receiverModes) {
        receiverModes = _receiverModes;
        loadParameters = new ObservableCollection<GeneratorLoadParameters>();
        foreach(var mode in receiverModes)
            AddGeneratorLoadParametersToReceiverMode(mode);
        ReceiverModesUpdateHandler = new(
            AddedMode => AddGeneratorLoadParametersToReceiverMode(AddedMode),
            RemovedMode => RemoveGeneratorLoadParametersToReceiverMode(RemovedMode)
        );
        receiverModes.CollectionChanged += ReceiverModesUpdateHandler.Handler;
    }
    ~GeneratorLoad() {
        receiverModes.CollectionChanged -= ReceiverModesUpdateHandler.Handler;
    }
    public GeneratorLoadParameters AddGeneratorLoadParametersToReceiverMode(ReceiverMode receiverMode) {
        GeneratorLoadParameters parameters = new GeneratorLoadParameters(receiverMode);
        loadParameters.Add(parameters);
        return parameters;
    }
    public GeneratorLoadParameters RemoveGeneratorLoadParametersToReceiverMode(ReceiverMode receiverMode) {
        GeneratorLoadParameters parameters = loadParameters.First(p => p.receiverMode == receiverMode);
        loadParameters.Remove(parameters);
        return parameters;
    }
}

public partial class GeneratorLoadParameters : ObservableObject {
    private readonly WhenCollectionChangedHandler<ReceiverModeParameters> ReceiverParametersRegisterHandler;
    public readonly ReceiverMode receiverMode;
    [ObservableProperty]
    public double continuouslyOperatingActivePower;
    [ObservableProperty]
    public double continuouslyOperatingReactivePower;
    [ObservableProperty]
    public double periodicOperatingActivePower;
    [ObservableProperty]
    public double periodicOperatingReactivePower;
    [ObservableProperty]
    public double totalActivePower;
    [ObservableProperty]
    public double totalReactivePower;
    [ObservableProperty]
    public double coefficientTimeDifference;
    [ObservableProperty]
    public double powerLossFactor;
    [ObservableProperty]
    public double generatorActivePower;
    [ObservableProperty]
    public double generatorReactivePower;
    [ObservableProperty]
    public double generatorFullPower;
    [ObservableProperty]
    public double weightedAveragePowerFactor;
    public GeneratorLoadParameters(ReceiverMode _receiverMode) {
        receiverMode = _receiverMode;
        PropertyChanged += CalcParametersHandler;
        ReceiverParametersRegisterHandler = new(
            AddedParameters => AddedParameters.PropertyChanged += CalcParametersHandler,
            RemovedParameters => RemovedParameters.PropertyChanged -= CalcParametersHandler
        );
        foreach(var parameters in receiverMode.receiverModeParameters)
            parameters.PropertyChanged += CalcParametersHandler;
        receiverMode.receiverModeParameters.CollectionChanged += ReceiverParametersRegisterHandler.Handler;
    }
    ~GeneratorLoadParameters() {
        PropertyChanged -= CalcParametersHandler;
        receiverMode.receiverModeParameters.CollectionChanged -= ReceiverParametersRegisterHandler.Handler;
        foreach(var parameters in receiverMode.receiverModeParameters)
            parameters.PropertyChanged -= CalcParametersHandler;
    }
    private double CalcContinuouslyOperatingActivePower() {
        var parameters = receiverMode.receiverModeParameters.Where(p => p.Mode == ReceiverModeParameters.WorkMode.Continuous);
        if(parameters.Count() > 0)
            return parameters.Sum(p => p.ActivePower);
        return 0;
    }
    private double CalcPeriodicOperatingActivePower() {
        var parameters = receiverMode.receiverModeParameters.Where(p => p.Mode == ReceiverModeParameters.WorkMode.Periodic);
        if(parameters.Count() > 0)
            return parameters.Sum(p => p.ActivePower);
        return 0;
    }
    private double CalcContinuouslyOperatingReactivePower() {
        var parameters = receiverMode.receiverModeParameters.Where(p => p.Mode == ReceiverModeParameters.WorkMode.Continuous);
        if(parameters.Count() > 0)
            return parameters.Sum(p => p.ReactivePower);
        return 0;
    }
    private double CalcPeriodicOperatingReactivePower() {
        var parameters = receiverMode.receiverModeParameters.Where(p => p.Mode == ReceiverModeParameters.WorkMode.Periodic);
        if(parameters.Count() > 0)
            return parameters.Sum(p => p.ReactivePower);
        return 0;
    }
    private void CalcParametersHandler(object? sender, PropertyChangedEventArgs e) {
        if(sender is ReceiverModeParameters parameters)
            switch (e.PropertyName) {
                case "Mode":
                        ContinuouslyOperatingActivePower = CalcContinuouslyOperatingActivePower();
                        PeriodicOperatingActivePower = CalcPeriodicOperatingActivePower();
                        ContinuouslyOperatingReactivePower = CalcContinuouslyOperatingReactivePower();
                        PeriodicOperatingReactivePower = CalcPeriodicOperatingReactivePower();
                    break;
                case "ActivePower":
                    if(parameters.Mode == ReceiverModeParameters.WorkMode.Continuous)
                        ContinuouslyOperatingActivePower = CalcContinuouslyOperatingActivePower();
                    if(parameters.Mode == ReceiverModeParameters.WorkMode.Periodic)
                        PeriodicOperatingActivePower = CalcPeriodicOperatingActivePower();
                    break;
                case "ReactivePower":
                    if(parameters.Mode == ReceiverModeParameters.WorkMode.Continuous)
                        ContinuouslyOperatingReactivePower = CalcContinuouslyOperatingReactivePower();
                    if(parameters.Mode == ReceiverModeParameters.WorkMode.Periodic)
                        PeriodicOperatingReactivePower = CalcPeriodicOperatingReactivePower();
                    break;
            }
        if(sender == this) {
            switch (e.PropertyName) {
                case "ContinuouslyOperatingActivePower":
                case "PeriodicOperatingActivePower":
                    TotalActivePower = Math.Round(ContinuouslyOperatingActivePower + PeriodicOperatingActivePower, 2);
                    break;
                case "ContinuouslyOperatingReactivePower":
                case "PeriodicOperatingReactivePower":
                    TotalReactivePower = Math.Round(ContinuouslyOperatingReactivePower + PeriodicOperatingReactivePower, 2);
                    break;
                case "CoefficientTimeDifference":
                    CalcParametersHandler(sender, new("TotalActivePower"));
                    CalcParametersHandler(sender, new("TotalReactivePower"));
                    break;
                case "PowerLossFactor":
                    CalcParametersHandler(sender, new("TotalActivePower"));
                    break;
                case "TotalActivePower":
                    GeneratorActivePower = Math.Round(TotalActivePower * CoefficientTimeDifference * PowerLossFactor, 2);
                    break;
                case "TotalReactivePower":
                    GeneratorReactivePower = Math.Round(TotalReactivePower * CoefficientTimeDifference, 2);
                    break;
                case "GeneratorActivePower":
                case "GeneratorReactivePower":
                    GeneratorFullPower = Math.Round(Math.Sqrt(Math.Pow(GeneratorActivePower, 2) + Math.Pow(GeneratorReactivePower, 2)), 2);
                    WeightedAveragePowerFactor = Math.Round(GeneratorActivePower / GeneratorReactivePower, 2);
                    break;
            }
        }
    }
}