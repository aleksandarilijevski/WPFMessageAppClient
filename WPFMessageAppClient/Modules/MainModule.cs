using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace WPFMessageAppClient.Modules
{
    public class MainModule:IModule
    {

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainRegion", typeof(MainWindow));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWindow>();
        }
    }
}
