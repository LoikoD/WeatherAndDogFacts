using CodeBase.Facts;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using DG.Tweening;

namespace CodeBase.UI
{
    public class FactsView : MonoBehaviour
    {
        [SerializeField] private Transform _factsContainer;
        [SerializeField] private Button _factButtonPrefab;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private FactPopup _factPopup;
        [SerializeField] private LoadingPanel _loadingPanel;

        private readonly List<Button> _factButtons = new();
        private IFactService _factService;

        [Inject]
        public void Construct(IFactService factService)
        {
            _factService = factService;
        }

        private void Start()
        {
            LoadBreeds().Forget();
        }

        private async UniTaskVoid LoadBreeds()
        {
            _loadingPanel.ShowLoading();
            try
            {
                List<BreedData> breeds = await _factService.GetBreeds();
                CreateBreedButtons(breeds.Take(10).ToArray());
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load breeds: {e.Message}");
            }
            finally
            {
                _loadingPanel.HideLoading();
            }
        }

        private void CreateBreedButtons(BreedData[] breeds)
        {
            ClearFactButtons();

            for (int i = 0; i < breeds.Length; i++)
            {
                BreedData breed = breeds[i];
                Button button = Instantiate(_factButtonPrefab, _factsContainer);
                TMP_Text text = button.GetComponentInChildren<TMP_Text>();
                text.text = $"{i + 1} - {breed.Name}";

                button.OnClickAsObservable()
                    .Subscribe(_ => OnFactClicked(breed.Id))
                    .AddTo(this);

                _factButtons.Add(button);
            }
        }

        private async void OnFactClicked(string breedId)
        {
            _loadingPanel.ShowLoading();
            try
            {
                BreedFact breedFact = await _factService.GetBreedFact(breedId);
                _factPopup.Show(breedFact);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load breed fact: {e.Message}");
            }
            finally
            {
                _loadingPanel.HideLoading();
            }
        }

        private void ClearFactButtons()
        {
            foreach (var button in _factButtons)
            {
                Destroy(button.gameObject);
            }
            _factButtons.Clear();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _canvasGroup.DOFade(1f, 0.3f);
            LoadBreeds().Forget();
        }

        public void Hide()
        {
            _factPopup.Close();
            _canvasGroup.DOFade(0f, 0.3f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}