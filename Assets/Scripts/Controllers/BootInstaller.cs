using Data;
using Gameplay;
using Zenject;

namespace Controllers
{
    public class BootInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LevelsStorage>().FromScriptableObjectResource("Levels/LevelsStorage").AsSingle();
            Container.Bind<PlayerDataController>().FromNew().AsSingle().NonLazy();
            Container.Bind<GamefieldController>().FromFactory<GamefieldController.GamefieldFactory>().AsTransient().NonLazy();
        }
    }
}
