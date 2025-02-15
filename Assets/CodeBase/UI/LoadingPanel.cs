using UnityEngine;

namespace CodeBase.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        public void ShowLoading() => gameObject.SetActive(true);
        public void HideLoading() => gameObject.SetActive(false);
    }
}
