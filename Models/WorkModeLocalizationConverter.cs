using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using SeaCalculator.Assets;

namespace SeaCalculator.Models;

public class WorkModeLocalizationConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameters, CultureInfo cultureInfo) {
        if(value is List<ReceiverModeParameters.WorkMode> modes && targetType == typeof(IEnumerable)) {
            return new List<object?>(){
                Lang.ReceiverModesWorkModeContinuesTitle,
                Lang.ReceiverModesWorkModePeriodicTitle,
                Lang.ReceiverModesWorkModeEpisodicTitle
            };
        }
        if(value is ReceiverModeParameters.WorkMode mode)
            return mode switch {
                ReceiverModeParameters.WorkMode.Continuous => Lang.ReceiverModesWorkModeContinuesTitle,
                ReceiverModeParameters.WorkMode.Periodic => Lang.ReceiverModesWorkModePeriodicTitle,
                ReceiverModeParameters.WorkMode.Episodic => Lang.ReceiverModesWorkModeEpisodicTitle,
                _ => new BindingNotification(new InvalidCastException(), BindingErrorType.DataValidationError)
            };
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameters, CultureInfo cultureInfo) {
        if(value is string mode && targetType == typeof(ReceiverModeParameters.WorkMode)) {
            if(mode == Lang.ReceiverModesWorkModeContinuesTitle) return ReceiverModeParameters.WorkMode.Continuous;
            if(mode == Lang.ReceiverModesWorkModePeriodicTitle) return ReceiverModeParameters.WorkMode.Periodic;
            if(mode == Lang.ReceiverModesWorkModeEpisodicTitle) return ReceiverModeParameters.WorkMode.Episodic;
        }
        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}