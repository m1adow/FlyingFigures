using FlyingFigures.Localization;
using FlyingFigures.ViewModel.Commands;
using System.ComponentModel;
using System.Globalization;

namespace FlyingFigures.ViewModel
{
    public class Translator : INotifyPropertyChanged
    {
        private string? _language = "en";

        public string? Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private string? _viewMenuItem;

        public string? ViewMenuItem
        {
            get => _viewMenuItem;
            set
            {
                _viewMenuItem = value;
                OnPropertyChanged(nameof(ViewMenuItem));
            }
        }

        private string? _languageMenuItem;

        public string? LanguageMenuItem
        {
            get => _languageMenuItem;
            set
            {
                _languageMenuItem = value;
                OnPropertyChanged(nameof(LanguageMenuItem));
            }
        }

        private string? _englishLanguage;

        public string? EnglishLanguage
        {
            get => _englishLanguage;
            set
            {
                _englishLanguage = value;
                OnPropertyChanged(nameof(EnglishLanguage));
            }
        }

        private string? _russianLanguage;

        public string? RussianLanguage
        {
            get => _russianLanguage;
            set
            {
                _russianLanguage = value;
                OnPropertyChanged(nameof(RussianLanguage));
            }
        }

        private string? _rectangleButton;

        public string? RectangleButton
        {
            get => _rectangleButton;
            set
            {
                _rectangleButton = value;
                OnPropertyChanged(nameof(RectangleButton));
            }
        }

        private string? _triangleButton;

        public string? TriangleButton 
        { 
            get => _triangleButton; 
            set
            {
                _triangleButton = value;
                OnPropertyChanged(nameof(TriangleButton));
            }
        }

        private string? _circleButton;

        public string? CircleButton 
        { 
            get => _circleButton; 
            set
            {
                _circleButton = value;
                OnPropertyChanged(nameof(CircleButton));
            } 
        }

        public ChangeCultureInfoCommand? ChangeCultureInfoCommand { get; set; }
      
        public event PropertyChangedEventHandler? PropertyChanged;

        public Translator()
        {
            ChangeCultureInfoCommand = new ChangeCultureInfoCommand(this);
            UpdateNames();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChangeCultureInfo(string localization)
        {
            Language = localization;
            UpdateNames();
        }

        private void UpdateNames()
        {
            if (Language == "ru-RU")
            {
                ViewMenuItem = Resource1.ViewMenuItem;
                LanguageMenuItem = Resource1.LanguageMenuItem;
                EnglishLanguage = Resource1.EnglishLanguage;
                RussianLanguage = Resource1.RussianLanguage;
                RectangleButton = Resource1.RectangleButton;
                TriangleButton = Resource1.TriangleButton;
                CircleButton = Resource1.CircleButton;
            }
            else if (Language == "en")
            {
                ViewMenuItem = Resource_en.ViewMenuItem;
                LanguageMenuItem = Resource_en.LanguageMenuItem;
                EnglishLanguage = Resource_en.EnglishLanguage;
                RussianLanguage = Resource_en.RussianLanguage;
                RectangleButton = Resource_en.RectangleButton;
                TriangleButton = Resource_en.TriangleButton;
                CircleButton = Resource_en.CircleButton;
            }
        }
    }
}
