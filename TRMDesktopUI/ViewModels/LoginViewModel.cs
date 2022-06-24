using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Helpers;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        // Error setting the value of password within a <PasswordBox> on the LoginView.xaml
        // refer to https://stackoverflow.com/questions/30631522/caliburn-micro-support-for-passwordbox
        private string _userName = "b7012116@my.shu.ac.uk";
        private string _password = "12345Hh!";
        private string _errorMessage;
        private IApiHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName 
        { 
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => _userName);
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => _password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        // This will enable or disable the login button
        public bool CanLogIn
        {
            get
            {
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                if (ErrorMessage?.Length > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";

                var result = await _apiHelper.Authenticate(UserName, Password);
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                await _events.PublishOnUIThreadAsync(new LogOnEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
