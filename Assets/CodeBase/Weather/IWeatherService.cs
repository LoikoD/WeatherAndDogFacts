using Cysharp.Threading.Tasks;
using System;
using UniRx;

namespace CodeBase.Weather
{
    public interface IWeatherService
    {
        UniTask<WeatherData> GetWeather();
        void StartAutoUpdate();
        void StopAutoUpdate();

        event Action<WeatherData> OnWeatherUpdated;
    }
}