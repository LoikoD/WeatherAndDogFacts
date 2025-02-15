using CodeBase.Facts;
using CodeBase.Weather;
using Zenject;

namespace CodeBase.Core
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IRequestQueue>().To<RequestQueue>().AsSingle();
            Container.Bind<IWeatherService>().To<WeatherService>().AsSingle();
            Container.Bind<IFactService>().To<FactService>().AsSingle();
        }
    }
}