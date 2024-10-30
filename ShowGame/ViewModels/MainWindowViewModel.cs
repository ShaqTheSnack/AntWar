using ReactiveUI;


namespace ShowGame.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static MainWindowViewModel? Instance { get; private set; }
        private ViewModelBase _contentViewModel;
        public ViewModelBase ContentViewModel
        {
            get => _contentViewModel;
            private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
        }

        public void SetViewModel(ViewModelBase model)
        {
            ContentViewModel = model;
        }

        public MainWindowViewModel()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            _contentViewModel = new StartScreenViewModel(this);

        }
    }
}
