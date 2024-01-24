using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using YouRest.HotelierWebApp.Data.Providers;

namespace YouRest.HotelierWebApp.Shared
{
    public partial class MainLayout
    {
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] AuthenticationStateProvider Provider { get; set; }
        public bool ShowRegistryPage { get; set; } = false;
        public Sidebar sidebar = default!;
        private IEnumerable<NavItem> navItems = default!;

        public async Task<SidebarDataProviderResult> SidebarDataProvider(SidebarDataProviderRequest request)
        {
            if (navItems is null)
                navItems = GetNavItems();

            return await Task.FromResult(request.ApplyTo(navItems));
        }

        public IEnumerable<NavItem> GetNavItems()
        {
            navItems = new List<NavItem>
        {
            new NavItem { Id = "1", Href = "/", IconName = IconName.GraphUpArrow, Text = "Статистика", IconColor = IconColor.Warning, Match=NavLinkMatch.All},
            new NavItem { Id = "2", Href = "/hotels", IconName = IconName.Buildings, Text = "Мои Отели", IconColor = IconColor.Warning},
            new NavItem { Id = "3", Href = "/about", IconName = IconName.InfoCircleFill, Text = "О Нас",IconColor = IconColor.Primary},
            new NavItem { Id = "4", Href = "/logout", IconName = IconName.BoxArrowInLeft, Text = "Выход"},
        };

            return navItems;
        }

        public async Task LogoutAsync()
        {
            await ((CustomAuthValidateProvider)Provider).MakeUserAnonymous();
            NavigationManager.NavigateTo("/login");
        }
        public void ToRegisterPage()
        {

            ShowRegistryPage = true;
        }
        public void ToLoginPage()
        {
           
            ShowRegistryPage = false;
        }
    }
}