using Zenject;
using UnityEngine;

public class GathererUnitInstaller : MonoInstaller
{
    [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;


    public override void InstallBindings()
    {
        Container.Bind<float>().WithId("InteractionDistance").FromInstance(3f);
        Container.Bind<float>().WithId("GatherSpeed").FromInstance(2); 
        Container.Bind<int>().WithId("MaximumResourceLoad").FromInstance(10);

        Container
            .Bind<ITickable>()
            .FromInstance(_factionMemberParallelInfoUpdater);
        Container.Bind<IFactionMember>().FromComponentInChildren();
        Container.Bind<ICommandsQueue>().FromComponentInChildren();
    }
}