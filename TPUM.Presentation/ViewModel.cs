namespace TPUM.Presentation
{
    internal class ViewModel
    {
        private Model MyModel { get; set; }

        internal ViewModel(Model model = default(Model))
        {
            MyModel = model ?? new Model();
        }
    }
}
