﻿using DG.Tweening;
using I2.Loc;
using Main.Controller;
using Main.View;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Launguage1
{
    public string Name;
    public string Code;
}

public class LaunguageLocalization1 : MonoBehaviour
{
    public static LaunguageLocalization1 instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        InitializeLanguageButtons();
        ApplyInitialLanguageSelection();
    }

    public string currentlanguage
    {
        get
        {
            return PlayerPrefs.GetString("language", "en");
        }
        set
        {
            PlayerPrefs.SetString("language", value);
            PlayerPrefs.Save();
        }
    }

    [Space]
    [Header("I2 Localize Refrence")]

    public Color selected;
    public Color unSelected;

    public Color selectedText;
    public Color unSelectedText;


    public Transform languagebtnparent;
    public List<Launguage> Language = new List<Launguage>();

    private int oldSelectedNob = -1;

    public GameObject LanguagePanel;
    public MasterCanvas1 MasterCanvas;

    private void InitializeLanguageButtons()
    {
        for (int i = 0; i < Language.Count; i++)
        {
            Transform t = languagebtnparent.GetChild(i);
            t.GetChild(0).GetComponent<TextMeshProUGUI>().text = Language[i].Name;

            Button button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            int index = i;
            button.onClick.AddListener(() => AddEvent(index));
        }
    }

    private void ApplyInitialLanguageSelection()
    {
        string currentLang = currentlanguage;
        for (int i = 0; i < Language.Count; i++)
        {
            if (Language[i].Code == currentLang)
            {
                oldSelectedNob = i;
                UpdateLanguageSelection(i, true);
                return;
            }
        }

        oldSelectedNob = 0;
        UpdateLanguageSelection(0, true);
    }

    public void AddEvent(int index)
    {
        Debug.Log("AddEvent => " + index);
        SoundController1.Instance.PlayClickSound();
        OnLanguageSelect(index);
    }

    public void OnLanguageSelect(int index)
    {
        Debug.Log("OnLanguageSelect");
        if (oldSelectedNob != index)
        {
            UpdateLanguageSelection(index);
        }
    }

    private void UpdateLanguageSelection(int index, bool initialSetup = false)
    {
        if (oldSelectedNob >= 0 && oldSelectedNob < languagebtnparent.childCount)
        {
            Transform oldT = languagebtnparent.GetChild(oldSelectedNob);
            oldT.GetComponent<Image>().color = unSelected;
            oldT.GetChild(0).GetComponent<TextMeshProUGUI>().color = unSelectedText;
            oldT.GetChild(1).gameObject.SetActive(true);
            oldT.GetChild(2).gameObject.SetActive(false);
        }

        Transform newT = languagebtnparent.GetChild(index);
        if (!initialSetup)
        {
            newT.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        }
        newT.GetComponent<Image>().color = selected;
        newT.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedText;
        newT.GetChild(1).gameObject.SetActive(false);
        newT.GetChild(2).gameObject.SetActive(true);

        currentlanguage = Language[index].Code;
        LocalizationManager.CurrentLanguageCode = currentlanguage;
        oldSelectedNob = index;
    }

    public void SetToDefault()
    {
        SoundController1.Instance.PlayClickSound();
        OnLanguageSelect(0);
    }

    public void LanguagePanelClose()
    {
        SoundController1.Instance.PlayClickSound();
        LanguagePanel.SetActive(false);
        string translateKey = CapeController.Instance.currentcap.CurrentHeaderName;
        string s = LocalizationManager.GetTranslation(translateKey);
        MasterCanvas.HeaderText.text = s;
    }
}