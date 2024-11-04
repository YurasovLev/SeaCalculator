using SeaCalculator.Models;

namespace SeaCalculator.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public ReceiverManager receiverManager { get; }
    public double HeightOfReceivers { get => receiverManager.Receivers.Count * 50; }
    public int CountOfColumns { get => receiverManager.ReceiverModes.Count + 1; }
    public MainWindowViewModel() {
        receiverManager = new();
        #if DEBUG
            for(int i = 1; i <= 28; i++)
                receiverManager.AddReceiver().Name = "TestReceiver" + i;
            for(int j = 1; j <= 5; j++)
                receiverManager.AddReceiverMode().Name = "TestMode" + j;
        #endif
    }
}