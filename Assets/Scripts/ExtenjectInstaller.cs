using Zenject;

public class ExtenjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameController>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<FiguresController>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<BagController>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<Timer>()
            .FromComponentInHierarchy()
            .AsSingle();
    }
}