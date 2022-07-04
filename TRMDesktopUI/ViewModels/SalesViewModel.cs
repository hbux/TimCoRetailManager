using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        private IConfigHelper _configHelper;
        private IMapper _mapper;
        private StatusInfoViewModel _statusInfo;
        private IWindowManager _windowManager;
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _itemQuantity;

        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, 
            IConfigHelper configHelper, IMapper mapper, StatusInfoViewModel statusInfo, IWindowManager windowManager)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
            _statusInfo = statusInfo;
            _windowManager = windowManager;
            _cart = new BindingList<CartItemDisplayModel>();

            _itemQuantity = 1;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadProducts();
            }
            catch (UnauthorizedAccessException)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                _statusInfo.UpdateMessage("Unauthorized Error", "You do not have permission to view the sales page.");
                await _windowManager.ShowDialogAsync(_statusInfo, null, settings);

                await TryCloseAsync();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                _statusInfo.UpdateMessage("Fatal Error", ex.Message);
                await _windowManager.ShowDialogAsync(_statusInfo, null, settings);

                await TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var products = _mapper.Map<List<ProductDisplayModel>>(await _productEndpoint.GetAll());
            Products = new BindingList<ProductDisplayModel>(products);
        }
        
        public BindingList<ProductDisplayModel> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get
            {
                return _cart;
            }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get
            {
                return _selectedCartItem;
            }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        public int ItemQuantity
        {
            get
            {
                return _itemQuantity;
            }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
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
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
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
                if (Cart.Count > 0)
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
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();

                return total.ToString("C");
            }
        }

        public decimal CalculateSubTotal()
        {
            decimal taxAmount = 0;

            foreach (var item in Cart)
            {
                taxAmount += item.Product.RetailPrice * item.QuantityInCart;
            }

            return taxAmount;
        }

        public decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            return taxAmount;
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel()
                {
                    Product = _selectedProduct,
                    QuantityInCart = _itemQuantity,
                };

                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (CartItemDisplayModel item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel()
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);

            await ResetSalesViewModel();
        }

        public async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();

            // TODO: Add clearing the selected cart item if it does not do it itself

            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
    }
}
