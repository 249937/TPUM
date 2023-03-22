namespace TPUM.Presentation
{
    internal class View
    {
        private ViewModel MyViewModel { get; set; } = default(ViewModel);

        public View(ViewModel viewModel = null)
        {
            MyViewModel = viewModel ?? new ViewModel();
        }
    }
}
