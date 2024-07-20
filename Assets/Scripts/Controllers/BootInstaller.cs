using Data;
using Gameplay;
using UI;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BootInstaller : MonoInstaller
    {
        public Transform UIParent;
        
        public override void InstallBindings()
        {
            Container.Bind<ViewManager>().FromNew().AsSingle();
            Container.Bind<LevelsStorage>().FromScriptableObjectResource("Levels/LevelsStorage").AsSingle();
            Container.Bind<PlayerDataController>().FromNew().AsSingle().NonLazy();
            Container.Bind<GamefieldController>().FromFactory<GamefieldController.GamefieldFactory>().AsCached();

            Container.Bind<Transform>().FromInstance(UIParent).AsSingle();

            Container.Bind<MainMenuController>().FromFactory<MainMenuControllerFactory>().AsTransient();
            Container.Bind<GameplayPanelController>().FromFactory<GameplayPanelControllerFactory>().AsTransient();
            
            Container.Bind<BootController>().AsSingle().NonLazy();
        }
    }
}
