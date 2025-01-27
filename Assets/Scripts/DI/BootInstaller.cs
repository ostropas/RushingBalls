using Controllers;
using Data;
using Gameplay;
using UI;
using UnityEngine;
using Zenject;

namespace DI
{
    public class BootInstaller : MonoInstaller
    {
        public Transform UIParent;
        
        public override void InstallBindings()
        {
            Container.Bind<BalanceConfig>().FromScriptableObjectResource("Config/Balance").AsSingle();
            Container.Bind<ViewManager>().AsSingle();
            Container.Bind<LevelController>().AsTransient();
            Container.Bind<LevelsStorage>().FromScriptableObjectResource("Levels/LevelsStorage").AsSingle();
            Container.Bind<PlayerDataController>().AsSingle().NonLazy();
            Container.Bind<GamefieldController>().FromFactory<GamefieldController.GamefieldFactory>().AsTransient();
            Container.Bind<BootController>().AsSingle();

            Container.Bind<Transform>().FromInstance(UIParent).AsSingle();
            
            BindUIController<MainMenuController>("UI/MainMenu"); 
            BindUIController<GameplayPanelController>("UI/TopGameplayPanel"); 
            BindUIController<MultiplierMenuController>("UI/MultiplierMenu"); 
            BindUIController<LeaderboardMenuController>("UI/LeaderboardMenu");

            // Game entry point
            Container.Resolve<BootController>().StartGame();
        }

        private void BindUIController<T>(string path) where T : BaseUIController
        {
            Container.Bind<T>()
                .FromIFactory(c => 
                    c.To<IFactory<T>>()
                    .FromMethod(context => new UIControllerFactory.BaseUIControllerFactory<T>(context.Container, UIParent, path))
                ).AsTransient();
        }
    }
}
