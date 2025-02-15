using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class TabController : MonoBehaviour
    {
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private FactsView _factsView;
        [SerializeField] private Button _weatherTabButton;
        [SerializeField] private Button _factsTabButton;

        private void Start()
        {
            ShowWeatherTab();
        }

        public void ShowWeatherTab()
        {
            _weatherView.Show();
            _factsView.Hide();
            UpdateTabButtonsState(true);
        }

        public void ShowFactsTab()
        {
            _weatherView.Hide();
            _factsView.Show();
            UpdateTabButtonsState(false);
        }

        private void UpdateTabButtonsState(bool isWeatherTab)
        {
            _weatherTabButton.interactable = !isWeatherTab;
            _factsTabButton.interactable = isWeatherTab;
        }
    }
}