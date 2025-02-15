using UnityEngine;

namespace CodeBase.Weather
{
    public class WeatherData
    {
        private readonly int _temperature;
        private readonly string _iconUrl;
        private Texture2D _iconTexture;

        public int Temperature => _temperature;
        public string IconUrl => _iconUrl;
        public Texture2D IconTexture => _iconTexture;

        public WeatherData(int temperature, string iconUrl)
        {
            _temperature = temperature;
            _iconUrl = iconUrl;
        }

        public void SetIconTexture(Texture2D iconTexture)
        {
            _iconTexture = iconTexture;
        }
    }
}