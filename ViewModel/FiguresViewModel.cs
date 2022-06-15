using FlyingFigures.ViewModel.Helpers;

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
