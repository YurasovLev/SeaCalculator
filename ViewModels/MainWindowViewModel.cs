using SeaCalculator.Models;

namespace SeaCalculator.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public ReceiverManager receiverManager { get; }
    public int CountOfColumns { get => receiverManager.ReceiverModes.Count + 1; }
    public MainWindowViewModel() {
        receiverManager = new();
        #if DEBUG
            for(int i = 1; i <= 2; i++)
                receiverManager.AddReceiver().Name = "TestReceiver" + i;
            for(int j = 1; j <= 1; j++)
                receiverManager.AddReceiverMode().Name = "TestMode" + j;
        #endif
    }
}