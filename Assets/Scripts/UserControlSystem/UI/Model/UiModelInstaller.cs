using Zenject;

public class UiModelInstaller : MonoInstaller
{

	public override void InstallBindings()
	{
		Container
		.Bind<BottomCenterModel>()
		.AsSingle();
	}
}