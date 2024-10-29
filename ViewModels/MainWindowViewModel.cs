using SeaCalculator.Models;

namespace SeaCalculator.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollectionOfObservableObjects<Receiver> Receivers { get; }
    public MainWindowViewModel() {
        Receivers = new();
        for(int i = 1; i <= 5; i++) {
            Receiver receiver = new();
            receiver.Name = "Test" + i;
            for(int j = 1; j <= 3; j++) {
                ReceiverMode receiverMode = receiver.AddMode();
                receiverMode.Name = "TestMode" + i + "." + j;
            }
            Receivers.Add(receiver);
        }
    }
}