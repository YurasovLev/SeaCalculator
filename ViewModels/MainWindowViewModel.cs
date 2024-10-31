using SeaCalculator.Models;

namespace SeaCalculator.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public ReceiverManager receiverManager { get; }
    public int CountOfColumns { get => receiverManager.ReceiverModes.Count + 1; }
    public MainWindowViewModel() {
        receiverManager = new();
        #if DEBUG
            for(int i = 1; i <= 5; i++)
                receiverManager.AddReceiver().Name = "TestReceiver" + i;
            for(int j = 1; j <= 3; j++)
                receiverManager.AddReceiverMode().Name = "TestMode" + j;
            receiverManager.AddReceiverMode().Name = "FinalTestMode";
            receiverManager.AddReceiver().Name = "FinalTestReceiver";
            Receiver TestToRemove = receiverManager.AddReceiver();
            ReceiverMode TestModeToRemove = receiverManager.AddReceiverMode();
            TestToRemove.Name = "Removed!!";
            TestModeToRemove.Name = "Removed!!";
            receiverManager.RemoveReceiver(TestToRemove);
            receiverManager.RemoveReceiverMode(TestModeToRemove);
        #endif
    }
}