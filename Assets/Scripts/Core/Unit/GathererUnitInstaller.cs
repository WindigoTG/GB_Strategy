using Zenject;
using UnityEngine;

public class GathererUnitInstaller : MonoInstaller
{
    [SerializeField] private FactionMemberParallelInfoUpdater _factionMemberParallelInfoUpdater;


    public override void InstallBindings()
    {

        Container.Bind<IHealthHolder>().FromComponentInChildren();

        Container
            .Bind<ITickable>()
            .FromInstance(_factionMemberParallelInfoUpdater);
        Container.Bind<IFactionMember>().FromComponentInChildren();
        Container.Bind<ICommandsQueue>().FromComponentInChildren();
    }
}