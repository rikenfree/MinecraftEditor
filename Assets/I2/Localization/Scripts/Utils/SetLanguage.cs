using UnityEngine;
using UnityEngine.UI;
namespace I2.Loc
{
    [AddComponentMenu("I2/Localization/SetLanguage Button")]
    public class SetLanguage : MonoBehaviour
    {
        public string _Language;

#if UNITY_EDITOR
        public LanguageSource mSource;
#endif

        private void Start()
        {
            if (LocalizationManager.CurrentLanguage == _Language)
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = GuiManager2.instance.LanguageSelect;
            }
        }

        private void Update()
        {
            if (LocalizationManager.CurrentLanguage != _Language)
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = GuiManager2.instance.LanguageDeselect;
            }
        }

        void OnClick()
        {
            ApplyLanguage();
        }

        public void ApplyLanguage()
        {
            if (LocalizationManager.HasLanguage(_Language))
            {
                LocalizationManager.CurrentLanguage = _Language;
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = GuiManager2.instance.LanguageSelect;
            }
        }
    }
}