namespace WinCAN.App.Services;

/// <summary>Window-provided file dialogs; keeps the view model testable.</summary>
public interface IFileDialogService
{
    Task<string?> PickOpenLogAsync();

    Task<string?> PickSaveLogAsync(string suggestedName);

    Task<string?> PickOpenDbcAsync();

    Task ShowMessageAsync(string title, string message);
}
