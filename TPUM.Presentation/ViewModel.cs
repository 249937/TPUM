using System.ComponentModel;
using System.Runtime.CompilerServices;
using TPUM.Presentation.Model;
using System;
using System.Collections.Generic;

namespace TPUM.Presentation.ViewModel
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
        
        public MainViewModel()
        {
            CommandAddProduct = new RelayCommand(ExecuteCommandAddProduct);
            CommandRemoveProduct = new RelayCommand(ExecuteCommandRemoveProduct);
            CommandFindProduct = new RelayCommand(ExecuteCommandFindProducts);
            MainModelAbstract.Instance.OnProductAdded += HandleProductAdded;
            MainModelAbstract.Instance.OnProductRemoved += HandleProductRemoved;
        }

        public void HandleProductAdded(Model.ProductAbstract product)
        {
            OutputText = "ADDED: [Product] GUID: " + product.GetGuid() + ", Name: " + product.GetName() + ", Price: " + product.GetPrice();
        }

        public void HandleProductRemoved(Model.ProductAbstract product)
        {
            OutputText = "REMOVED: [Product] GUID: " + product.GetGuid() + ", Name: " + product.GetName() + ", Price: " + product.GetPrice();
        }

        private void ExecuteCommandAddProduct()
        {
            MainModelAbstract.Instance.AddProduct(ProductNameInputText, float.Parse(ProductPriceInputText));
        }

        private void ExecuteCommandRemoveProduct()
        {
            MainModelAbstract.Instance.RemoveProduct(new Guid(ProductGuidInputText));
        }

        private void ExecuteCommandFindProducts()
        {
            List<Model.ProductAbstract> products = MainModelAbstract.Instance.FindProducts(ProductNameInputText);
            OutputText = "";
            foreach (Model.ProductAbstract product in products)
            {
                string guid = (product.GetGuid()).ToString();
                string price = (product.GetPrice()).ToString();
                OutputText = OutputText + "FOUND: [Product] GUID: " + guid + ", Name: " + product.GetName() + ", Price: " + price + "\n";
            }
        }
    }
}
