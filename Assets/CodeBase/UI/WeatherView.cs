using CodeBase.Weather;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI
{
    public class WeatherView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _weatherText;
        [SerializeField] private RawImage _weatherIcon;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LoadingPanel _loadingPanel;

        private IWeatherService _weatherService;
        private bool _isLoading;

        [Inject]
        public void Construct(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        private void Start()
        {
            Observable.FromEvent<WeatherData>(
                h => _weatherService.OnWeatherUpdated += h,
                h => _weatherService.OnWeatherUpdated -= h)
                .Subscribe(UpdateUI)
                .AddTo(this);
        }

        private void UpdateUI(WeatherData weatherData)
        {
            _weatherText.text = $"Сегодня - {weatherData.Temperature}F";
            _weatherIcon.texture = weatherData.IconTexture;

            if (_isLoading)
            {
                _loadingPanel.HideLoading();
                _isLoading = false;
            }
        }

        public void Show()
        {
            _isLoading = true;
            _loadingPanel.ShowLoading();
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1f, 0.3f);
            _weatherService.StartAutoUpdate();
        }

        public void Hide()
        {
            _weatherService.StopAutoUpdate();
            _canvasGroup.DOFade(0f, 0.3f)
                .OnComplete(() => gameObject.SetActive(false));
        }

    }
}