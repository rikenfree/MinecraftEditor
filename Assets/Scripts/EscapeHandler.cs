using UnityEngine;
using System.Collections.Generic;

public class EscapeHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> mainPanels;

    [SerializeField] private List<GameObject> settingPanels;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundControllerMain.instance.PlayClickSound();
            CloseMainPanels();
            Debug.LogWarning("Key is pressed");
        }
    }

    private void CloseMainPanels()
    {
        bool anyClosed = false;

        foreach (GameObject obj in mainPanels)
        {
            if (obj.activeSelf)
            {
                if (obj.CompareTag("CapSetting"))
                {
                    CloseSettingPanels(obj);
                }
                else
                {
                    obj.SetActive(false);
                    Debug.LogWarning($"Closed panel: {obj.name}");
                }

                anyClosed = true;
                break; // close only one per press
            }
        }

        if (!anyClosed)
        {
            Debug.LogWarning("No active panels left — Changing to scene 3");
            ManageScene.instance.ChangeToScene(3);
        }
    }

    private void CloseSettingPanels(GameObject panel)
    {
        bool anyActive = false;

        foreach (GameObject obj in settingPanels)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                Debug.LogWarning("Closed sub-setting panel");
                anyActive = true;
                break;
            }
        }

        if (!anyActive)
        {
            panel.SetActive(false);
            Debug.LogWarning("Closed main setting panel — no subpanels were active");
        }
    }
}
