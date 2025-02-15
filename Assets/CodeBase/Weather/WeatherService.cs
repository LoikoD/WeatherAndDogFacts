using CodeBase.Core;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace CodeBase.Weather
{
    public class WeatherService : IWeatherService
    {
        private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        private readonly IRequestQueue _requestQueue;

        private CancellationTokenSource _updateCts;
        private bool _isUpdating;

        public event Action<WeatherData> OnWeatherUpdated;

        public WeatherService(IRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;
        }

        public async UniTask<WeatherData> GetWeather()
        {
            WeatherData weatherData = await _requestQueue.EnqueueRequest(ApiUrl, ParseWeather);
            weatherData.SetIconTexture(await _requestQueue.EnqueueTextureRequest(weatherData.IconUrl));

            return weatherData;
        }

        public void StartAutoUpdate()
        {
            if (_isUpdating) return;

            _isUpdating = true;
            _updateCts = new CancellationTokenSource();
            AutoUpdate(_updateCts.Token).Forget();
        }

        public void StopAutoUpdate()
        {
            _isUpdating = false;
            _updateCts?.Cancel();
            _updateCts?.Dispose();
            _updateCts = null;
            _requestQueue.CancelAllRequests();
        }

        private async UniTaskVoid AutoUpdate(CancellationToken cancellationToken)
        {
            while (_isUpdating)
            {
                try
                {
                    WeatherData weatherData = await GetWeather();

                    if (cancellationToken.IsCancellationRequested) return;

                    OnWeatherUpdated?.Invoke(weatherData);

                    await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException) { }
            }
        }

        private WeatherData ParseWeather(string json)
        {
            WeatherApiResponse weatherJson = JsonUtility.FromJson<WeatherApiResponse>(json);

            WeatherPeriod weatherPeriod = weatherJson.properties.periods[0];

            return new WeatherData(weatherPeriod.temperature, weatherPeriod.icon);
        }
    }
}