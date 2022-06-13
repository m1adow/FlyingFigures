using Figures;
using FlyingFigures.Localization;
using FlyingFigures.ViewModel.Helpers;
using System.Collections.Generic;

namespace FlyingFigures.ViewModel
{
    public class FiguresViewModel
    {
        public Translator? Translator { get; set; }

        public FiguresViewModel()
        {
            Translator = new Translator();
        }
    }
}
