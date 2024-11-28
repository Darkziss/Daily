using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;

namespace Daily.ViewModels
{
    public partial class ThoughtEditPageViewModel : ObservableObject, IResetView
    {
        [ObservableProperty] private bool _isNameEntryReadOnly = true;
        [ObservableProperty] private bool _isTextEditorReadOnly = true;

        [ObservableProperty] private string _name = string.Empty;
        [ObservableProperty] private string _text = string.Empty;

        public Command<bool> ChangeNameReadOnlyCommand { get; }
        public Command<bool> ChangeTextReadOnlyCommand { get; }

        public ThoughtEditPageViewModel()
        {
            ChangeNameReadOnlyCommand = new Command<bool>((isReadOnly) =>
            {
                IsNameEntryReadOnly = isReadOnly;
            });

            ChangeTextReadOnlyCommand = new Command<bool>((isReadOnly) =>
            {
                IsTextEditorReadOnly = isReadOnly;
            });
        }

        public void ResetView()
        {
            IsNameEntryReadOnly = true;

            Name = string.Empty;
            Text = string.Empty;
        }
    }
}