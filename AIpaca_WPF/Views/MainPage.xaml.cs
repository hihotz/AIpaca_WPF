﻿using AIpaca_App.Data;
using AIpaca_App.Resources.Localization;
using AIpaca_App.ViewModels;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace AIpaca_App.Views
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            // MainViewModel 인스턴스를 생성하고 BindingContext로 설정합니다.
            var databaseService = new DatabaseService();
            _viewModel = new MainViewModel(databaseService);
            BindingContext = _viewModel;
            _viewModel.SetOriginalLang(LeftLanguagePicker.SelectedIndex);
            _viewModel.SetTranslatedLang(RightLanguagePicker.SelectedIndex);
        }

        public string InputEditor
        {
            get => inputEditor.Text;
            set => inputEditor.Text = value;
        }

        public string TranslatedEditor
        {
            get => translatedEditor.Text;
            set => translatedEditor.Text = value;
        }

        private void OnSwitchLanguageClicked(object sender, EventArgs e)
        {
            // 현재 선택된 언어를 기억
            var temp = LeftLanguagePicker.SelectedIndex;
            // 선택된 언어를 서로 바꾸기
            LeftLanguagePicker.SelectedIndex = RightLanguagePicker.SelectedIndex;
            RightLanguagePicker.SelectedIndex = temp;

            // 현재 Editor에서 텍스트를 임시 저장
            var tempText = inputEditor.Text;
            // 두 텍스트의 값을 서로 교환
            inputEditor.Text = translatedEditor.Text;
            translatedEditor.Text = tempText;
        }

        private void LeftLanguagePicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // ViewModel의 SetOriginalLang 메서드 호출
            if (BindingContext is MainViewModel viewModel)
            {
                viewModel.SetOriginalLang(LeftLanguagePicker.SelectedIndex);
            }
        }

        private void RightLanguagePicker_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // ViewModel의 SetOriginalLang 메서드 호출
            if (BindingContext is MainViewModel viewModel)
            {
                viewModel.SetTranslatedLang(RightLanguagePicker.SelectedIndex);
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); // must be called

            if (width > height)
            {
                // Landscape layout
                TextFieldsGrid.RowDefinitions.Clear();
                TextFieldsGrid.ColumnDefinitions.Clear();

                TextFieldsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                TextFieldsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                Grid.SetRow(inputEditor.Parent as Frame, 0);
                Grid.SetColumn(inputEditor.Parent as Frame, 0);

                Grid.SetRow(translatedEditor.Parent as Frame, 0);
                Grid.SetColumn(translatedEditor.Parent as Frame, 1);
            }
            else
            {
                // Portrait layout
                TextFieldsGrid.RowDefinitions.Clear();
                TextFieldsGrid.ColumnDefinitions.Clear();

                TextFieldsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                TextFieldsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetRow(inputEditor.Parent as Frame, 0);
                Grid.SetColumn(inputEditor.Parent as Frame, 0);

                Grid.SetRow(translatedEditor.Parent as Frame, 1);
                Grid.SetColumn(translatedEditor.Parent as Frame, 0);
            }
        }

        private async void OnCopyButtonClicked(object sender, EventArgs e)
        {
            var stackLayout = sender as VerticalStackLayout;
            if (stackLayout != null)
            {
                var labels = stackLayout.Children.OfType<Label>().ToList();
                string labelText = string.Join("\n", labels.Select(l => l.Text).ToArray());

                if (!string.IsNullOrEmpty(labelText))
                {
                    await Clipboard.Default.SetTextAsync(labelText);
                    await Toast.Make(AppResources.copy_successful, ToastDuration.Long).Show();
                }
                else
                {
                    await Toast.Make(AppResources.copy_failed, ToastDuration.Long).Show();
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            LeftLanguagePicker.SelectedIndexChanged -= LeftLanguagePicker_SelectedIndexChanged;
            RightLanguagePicker.SelectedIndexChanged -= RightLanguagePicker_SelectedIndexChanged;
        }
    }
        
}
