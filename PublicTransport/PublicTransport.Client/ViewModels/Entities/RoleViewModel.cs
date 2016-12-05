using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Entities
{
    public class RoleViewModel : ReactiveObject
    {
        private bool _selected;

        public RoleDto Role { get; set; }

        public bool Selected
        {
            get { return _selected; }
            set { this.RaiseAndSetIfChanged(ref _selected, value); }
        }

        public RoleViewModel(RoleDto role, bool selected)
        {
            Role = role;
            _selected = selected;
        }
    }
}
