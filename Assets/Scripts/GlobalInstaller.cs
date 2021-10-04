using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GlobalInstaller", menuName = "Installers/GlobalInstaller")]
public class GlobalInstaller : ScriptableObjectInstaller<GlobalInstaller>
{
	[SerializeField] private AssetsContext _legacyContext;

	Vector3Value _vector3ValueInstance = new Vector3Value();
	AttackTargetValue _attackTargetValueInstance = new AttackTargetValue();

	public override void InstallBindings()
	{
		Container.Bind<AssetsContext>().FromInstance(_legacyContext);

		Container.Bind<CommandCreatorBase<IProduceUnitCommand>>()
			.To<ProduceUnitCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IAttackCommand>>()
			.To<AttackCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IMoveCommand>>()
			.To<MoveCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IPatrolCommand>>()
			.To<PatrolCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IStopCommand>>()
			.To<StopCommandCreator>().AsTransient();

		Container.Bind<CommandButtonsModel>().AsTransient();

		Container.Bind<Vector3Value>().FromInstance(_vector3ValueInstance);
		Container.Bind<SelectableValue>().AsCached();
		Container.Bind<AttackTargetValue>().FromInstance(_attackTargetValueInstance);

		Container.Bind<IAwaitable<IAttackTarget>>().FromInstance(_attackTargetValueInstance);
		Container.Bind<IAwaitable<Vector3>>().FromInstance(_vector3ValueInstance);
	}
}