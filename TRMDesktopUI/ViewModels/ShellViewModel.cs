using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SimpleContainer _container;
        private SalesViewModel _salesViewModel;
        private IEventAggregator _events;

        public ShellViewModel(SimpleContainer container, SalesViewModel salesViewModel, IEventAggregator events)
        {
            _container = container;
            _salesViewModel = salesViewModel;
            _events = events;

            _events.SubscribeOnUIThread(this);
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            return ActivateItemAsync(_salesViewModel);
        }
    }
}
