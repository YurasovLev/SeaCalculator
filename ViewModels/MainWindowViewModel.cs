using SeaCalculator.Models;

namespace SeaCalculator.ViewModels;
public partial class MainWindowViewModel : ViewModelBase
{
    public ReceiverManager receiverManager { get; }
    public MainWindowViewModel() {
        receiverManager = new();
    }
}