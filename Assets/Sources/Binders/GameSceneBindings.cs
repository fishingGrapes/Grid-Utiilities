using Zenject;

namespace VH
{
    public class GameSceneBindings : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Services.ILogService>().To<Services.UnityLogService>().AsSingle().NonLazy();
        }
    }
}
