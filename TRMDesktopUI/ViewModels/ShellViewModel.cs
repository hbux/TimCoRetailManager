using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesViewModel;
        private IEventAggregator _events;
        private ILoggedInUserModel _loggedInUser;
        private IApiHelper _apiHelper;

        public ShellViewModel(SalesViewModel salesViewModel, IEventAggregator events, 
            ILoggedInUserModel loggedInUser, IApiHelper apiHelper)
        {
            _salesViewModel = salesViewModel;
            _events = events;
            _loggedInUser = loggedInUser;
            _apiHelper = apiHelper;

            _events.SubscribeOnUIThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                if (string.IsNullOrEmpty(_loggedInUser.Token) == false)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task ExitApplication()
        {
            await TryCloseAsync();
        }

        public void LogOut()
        {
            _loggedInUser.ResetUser();
            _apiHelper.LogOffUser();
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
