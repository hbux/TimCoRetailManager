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
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private IUserEndpoint _userEndpoint;
        private StatusInfoViewModel _statusInfo;
        private IWindowManager _windowManager;
        private BindingList<UserModel> _users;

        public UserDisplayViewModel(IUserEndpoint userEndpoint, StatusInfoViewModel statusinfo, IWindowManager windowManager)
        {
            _userEndpoint = userEndpoint;
            _statusInfo = statusinfo;
            _windowManager = windowManager;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                _statusInfo.UpdateMessage("Error", ex.Message);
                await _windowManager.ShowDialogAsync(_statusInfo, null, settings);

                await TryCloseAsync();
            }
        }

        public async Task LoadUsers()
        {
            var users = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(users);
        }

        public BindingList<UserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }
    }
}
