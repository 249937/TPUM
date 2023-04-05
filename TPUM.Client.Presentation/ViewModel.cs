using System.ComponentModel;
using System.Runtime.CompilerServices;
using TPUM.Client.Presentation.Model;
using System;
using System.Collections.Generic;

namespace TPUM.Client.Presentation.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand CommandAddProduct 
        { 
            get; 
            private set; 
        }
        public RelayCommand CommandRemoveProduct 
        { 
            get; 
            private set; 
        }
        public RelayCommand CommandFindProduct 
        { 
            get; 
            private set; 
        }

        private string productNameInputText;
        public string ProductNameInputText
        {
            get 
            { 
                return productNameInputText; 
            }
            set 
            {
                SetProperty(ref productNameInputText, value);
            }
        }

        private string productPriceInputText;
        public string ProductPriceInputText
        {
            get 
            { 
                return productPriceInputText; 
            }
            set 
            { 
                SetProperty(ref productPriceInputText, value);
            }
        }

        private string productGuidInputText;
        public string ProductGuidInputText
        {
            get 
            { 
                return productGuidInputText; 
            }
            set 
            {
                SetProperty(ref productGuidInputText, value);
            }
        }

        private string outputText;
        public string OutputText
        {
            get 
            { 
                return outputText; 
            }
            set 
            { 
                SetProperty(ref outputText, value); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChaned(propertyName);
            return true;
        }

        private void OnPropertyChaned(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private MainModelAbstract model = null;

        public MainViewModel(MainModelAbstract model)
        {
            this.model = model;

            CommandAddProduct = new RelayCommand(ExecuteCommandAddProduct);
            CommandRemoveProduct = new RelayCommand(ExecuteCommandRemoveProduct);
            CommandFindProduct = new RelayCommand(ExecuteCommandFindProducts);

            this.model.OnProductAdded += HandleProductAdded;
            this.model.OnProductRemoved += HandleProductRemoved;
        }

        public void HandleProductAdded(ProductAbstract product)
        {
            OutputText = "ADDED: [Product] GUID: " + product.GetGuid() + ", Name: " + product.GetName() + ", Price: " + product.GetPrice();
        }

        public void HandleProductRemoved(ProductAbstract product)
        {
            OutputText = "REMOVED: [Product] GUID: " + product.GetGuid() + ", Name: " + product.GetName() + ", Price: " + product.GetPrice();
        }

        private void ExecuteCommandAddProduct()
        {
            model.AddProduct(ProductNameInputText, float.Parse(ProductPriceInputText));
        }

        private void ExecuteCommandRemoveProduct()
        {
            model.RemoveProduct(new Guid(ProductGuidInputText));
        }

        private void ExecuteCommandFindProducts()
        {
            List<ProductAbstract> products = model.FindProducts(ProductNameInputText);
            OutputText = "";
            foreach (ProductAbstract product in products)
            {
                string guid = (product.GetGuid()).ToString();
                string price = (product.GetPrice()).ToString();
                OutputText = OutputText + "FOUND: [Product] GUID: " + guid + ", Name: " + product.GetName() + ", Price: " + price + "\n";
            }
        }

        public static MainViewModel CreateViewModel()
        {
            return new MainViewModel(MainModelAbstract.CreateModel());
        }

        internal static MainViewModel CreateViewModel(MainModelAbstract model)
        {
            if (model == null)
            {
                return CreateViewModel();
            }
            return new MainViewModel(model);
        }
    }
}
