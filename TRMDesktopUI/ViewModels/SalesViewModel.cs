using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;
        private string _itemQuantity;
        
        public BindingList<string> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => _products);
            }
        }

        public BindingList<string> Cart
        {
            get
            {
                return _cart;
            }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => _cart);
            }
        }

        public string ItemQuantity
        {
            get
            {
                return _itemQuantity;
            }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => _itemQuantity);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                if ()
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                if ()
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanCheckOut
        {
            get
            {
                if (_cart.Count > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public string SubTotal 
        { 
            get
            {
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                return "$0.00";
            }
        }

        public void AddToCart()
        {
            
        }

        public void RemoveFromCart()
        {

        }

        public void CheckOut()
        {

        }
    }
}
