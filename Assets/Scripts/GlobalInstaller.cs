using UnityEngine;
using Zenject;
using System;
using System.Collections.Generic;
using UniRx;

[CreateAssetMenu(fileName = "GlobalInstaller", menuName = "Installers/GlobalInstaller")]
public class GlobalInstaller : ScriptableObjectInstaller<GlobalInstaller>
{
	[SerializeField] AssetsContext _legacyContext;
	[SerializeField] Sprite _chomperSprite;
	[SerializeField] Sprite _gathererSprite;

	Vector3Value _vector3ValueInstance = new Vector3Value();
	AttackTargetValue _attackTargetValueInstance = new AttackTargetValue();
	SelectableValue _selectableValue = new SelectableValue();

	Dictionary<ResourceType, int> _chomperCost = new Dictionary<ResourceType, int>() { { ResourceType.Crystal, 50} };
	Dictionary<ResourceType, int> _gathererCost = new Dictionary<ResourceType, int>() { { ResourceType.Crystal, 30 } };

	ReactiveDictionary<int, Dictionary<ResourceType, int>> _resources = new ReactiveDictionary<int, Dictionary<ResourceType, int>>()
	{
		{ 1, new Dictionary<ResourceType, int>(){ { ResourceType.Crystal, 80 } } },
		{ 2, new Dictionary<ResourceType, int>(){ { ResourceType.Crystal, 80 } } }
	};

	public override void InstallBindings()
	{
		Container.Bind<AssetsContext>().FromInstance(_legacyContext);

		Container.Bind<CommandCreatorBase<IProduceCombatUnitCommand>>()
			.To<ProduceCombatUnitCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IProduceGathererUnitCommand>>()
			.To<ProduceGathererUnitCommandCreator>().AsTransient();

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

		Container.Bind<CommandCreatorBase<IHoldPositionCommand>>()
			.To<HoldPositionCommandCreator>().AsTransient();

		Container.Bind<CommandCreatorBase<IGatherResourceCommand>>()
			.To<GatherResourceCommandCreator>().AsTransient();

		Container.Bind<CommandButtonsModel>().AsTransient();
		Container.Bind<BottomCenterModel>().AsTransient();

		Container.Bind<Vector3Value>().FromInstance(_vector3ValueInstance);
		Container.Bind<SelectableValue>().FromInstance(_selectableValue);
		Container.Bind<AttackTargetValue>().FromInstance(_attackTargetValueInstance);

		Container.Bind<IAwaitable<IAttackable>>().FromInstance(_attackTargetValueInstance);
		Container.Bind<IAwaitable<Vector3>>().FromInstance(_vector3ValueInstance);

		Container.Bind<IObservable<ISelectable>>().FromInstance(_selectableValue);

		Container.Bind<float>().WithId("Chomper").FromInstance(5f);
		Container.Bind<string>().WithId("Chomper").FromInstance("Chomper");
		Container.Bind<Sprite>().WithId("Chomper").FromInstance(_chomperSprite);

		Container.Bind<float>().WithId("Gatherer").FromInstance(3f);
		Container.Bind<string>().WithId("Gatherer").FromInstance("Gatherer");
		Container.Bind<Sprite>().WithId("Gatherer").FromInstance(_gathererSprite);

		Container.Bind<Dictionary<int, int>>().AsSingle();

		Container.Bind<Dictionary<ResourceType, int>>().WithId("Chomper").FromInstance(_chomperCost);
		Container.Bind<Dictionary<ResourceType, int>>().WithId("Gatherer").FromInstance(_gathererCost);

		Container.Bind<ReactiveDictionary<int, Dictionary<ResourceType, int>>>().FromInstance(_resources);
	}
}