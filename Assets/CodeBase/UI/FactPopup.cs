using CodeBase.Facts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class FactPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private CanvasGroup _canvasGroup;

        public void Show(BreedFact fact)
        {
            _titleText.text = fact.Name;
            _descriptionText.text = fact.Description;

            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_panel);

            _canvasGroup.DOFade(1f, 0.3f);
        }

        public void Close()
        {
            _canvasGroup.DOFade(0f, 0.3f)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}