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
            // Hide tickmark by default
            this.gameObject.transform.GetChild(0).gameObject.SetActive(LocalizationManager.CurrentLanguage == _Language);
        }

        public void ApplyLanguage()
        {
            if (LocalizationManager.HasLanguage(_Language))
            {
                LocalizationManager.CurrentLanguage = _Language;
                // Show tickmark for this button
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);

                // Hide tickmark for all other SetLanguage buttons
                foreach (var btn in FindObjectsByType<SetLanguage>(FindObjectsSortMode.None))
                {
                    if (btn != this)
                        btn.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
    }
}