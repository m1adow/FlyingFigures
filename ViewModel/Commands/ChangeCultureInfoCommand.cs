using FlyingFigures.ViewModel.Helpers;
using System;
using System.Windows.Input;

namespace FlyingFigures.ViewModel.Commands
{
    public class ChangeCultureInfoCommand : ICommand
    {
        public Translator? Translator { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ChangeCultureInfoCommand(Translator? translator)
        {
            Translator = translator;
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter.ToString() == Translator.Language)
                return false;
            return true;
        }

        public void Execute(object? parameter)
        {
            Translator?.ChangeCultureInfo(parameter.ToString());
        }
    }
}
