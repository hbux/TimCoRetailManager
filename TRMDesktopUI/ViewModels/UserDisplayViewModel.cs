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
        private UserModel _selectedUser;
        private string _selectedUserName;
        private string _selectedUserRole;
        private string _selectedAvailableRole;
        private BindingList<string> _userRoles = new BindingList<string>();
        private BindingList<string> _availableRoles = new BindingList<string>();

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

        public UserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;

                UserRoles.Clear();
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();

                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        public string SelectedUserRole
        {
            get
            {
                return _selectedUserRole;
            }
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        public string SelectedAvailableRole
        {
            get
            {
                return _selectedAvailableRole;
            }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        public string SelectedUserName
        {
            get
            {
                return _selectedUserName;
            }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        public BindingList<string> UserRoles
        {
            get
            {
                return _userRoles;
            }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        public BindingList<string> AvailableRoles
        {
            get
            {
                return _availableRoles;
            }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

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

        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();

            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async Task AddSelectedRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public async void RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

            UserRoles.Remove(SelectedAvailableRole);
            AvailableRoles.Add(SelectedAvailableRole);
        }
    }
}
