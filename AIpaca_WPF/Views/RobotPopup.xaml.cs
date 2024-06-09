using AIpaca_App.Resources.Localization;
using AIpaca_App.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace AIpaca_App.Views;

public partial class RobotPopup : Popup
{
    private string _labelText;
    private RobotViewModel _robotViewModel;
    private MainPage mainpage;
    public RobotPopup(string labelText, RobotViewModel robotViewModel)
    {
        mainpage = new MainPage();
        InitializeComponent();
        _labelText = labelText;
        _robotViewModel = robotViewModel;
    }

    private async void OnCopyButtonClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_labelText))
        {
            await Clipboard.Default.SetTextAsync(_labelText);
            await Toast.Make(AppResources.copy_successful, ToastDuration.Long).Show();
        }
        else
        {
            await Toast.Make(AppResources.copy_failed, ToastDuration.Long).Show();
        }
        await CloseAsync();
    }
    private async void OnEvaluateButtonClicked(object sender, EventArgs e)
    {
        //        InputEditorTranslatedEditor
        mainpage.InputEditor = _labelText;
        mainpage.TranslatedEditor = "";
        await CloseAsync();
    }
}