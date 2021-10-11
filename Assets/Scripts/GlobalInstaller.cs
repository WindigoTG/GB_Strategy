using UnityEngine;
using Zenject;
using System;

[CreateAssetMenu(fileName = "GlobalInstaller", menuName = "Installers/GlobalInstaller")]
public class GlobalInstaller : ScriptableObjectInstaller<GlobalInstaller>
{
	[SerializeField] AssetsContext _legacyContext;
	[SerializeField] Sprite _chomperSprite;

	Vector3Value _vector3ValueInstance = new Vector3Value();
	AttackTargetValue _attackTargetValueInstance = new AttackTargetValue();
	SelectableValue _selectableValue = new SelectableValue();

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

		Container.Bind<CommandCreatorBase<ISetRallyPointCommand>>()
			.To<SetRallyPointCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IRemoveRallyPointCommand>>()
			.To<RemoveRallyPointCommandCreator>().AsTransient();

		Container.Bind<CommandButtonsModel>().AsTransient();
		Container.Bind<BottomCenterModel>().AsTransient();

		Container.Bind<Vector3Value>().FromInstance(_vector3ValueInstance);
		Container.Bind<SelectableValue>().FromInstance(_selectableValue);
		Container.Bind<AttackTargetValue>().FromInstance(_attackTargetValueInstance);

		Container.Bind<IAwaitable<IAttackTarget>>().FromInstance(_attackTargetValueInstance);
		Container.Bind<IAwaitable<Vector3>>().FromInstance(_vector3ValueInstance);

		Container.Bind<IObservable<ISelectable>>().FromInstance(_selectableValue);

		Container.Bind<float>().WithId("Chomper").FromInstance(5f);
		Container.Bind<string>().WithId("Chomper").FromInstance("Chomper");
		Container.Bind<Sprite>().WithId("Chomper").FromInstance(_chomperSprite);
	}
}