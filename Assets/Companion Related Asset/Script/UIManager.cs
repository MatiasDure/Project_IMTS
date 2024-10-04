using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;
using Range = UnityEngine.SocialPlatforms.Range;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CompanionBehavior beeCompanion;
    [SerializeField] private GameObject scanToStartText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject questionUI;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private RawImage image1;
    [SerializeField] private RawImage image2;
    [SerializeField] private RawImage image3;
    [SerializeField] private List<RawImage> images;
    [SerializeField] private GameObject badgeUI;
    [SerializeField] private GameObject panel;
    
    private int _randomInt; 
    private void Awake()
    {
        scanToStartText.SetActive(true);
        menu.SetActive(false);
    }

    public void ChangePhase(int phaseIndex)
    {
        phaseIndex = Mathf.Clamp(phaseIndex, 0, 2);
        CompanionPhase phase = CompanionPhase.Introduction;
        switch (phaseIndex)
        {
            case 0:
                phase = CompanionPhase.Introduction;
                break;
            case 1:
                phase = CompanionPhase.Scenario1;
                break;
            case 2:
                phase = CompanionPhase.Scenario2;
                break;
        }
        
        beeCompanion.OnPhaseChange(phase);
    }

    public void SetScanTextStatus(bool status)
    {
        scanToStartText.gameObject.SetActive(status);
    }

    public void SetMenuStatus(bool status)
    {
        menu.gameObject.SetActive(status);
    }

    public void SetQuestionUIStatus(bool status)
    {
        questionUI.SetActive(status);
        
    }

    public void AddQuestionIcon(string question, Texture rightIcon, List<Texture> wrongIcon)
    {
        Random random = new Random();
        _randomInt = random.Next(0, 3);
        
        questionText.text = question;

        switch (_randomInt)
        {
            case 0:
                image1.texture = rightIcon;
                image2.texture = wrongIcon[0];
                image3.texture = wrongIcon[1];
                break;
            case 1:
                image1.texture = wrongIcon[0];
                image2.texture = rightIcon;
                image3.texture = wrongIcon[1];
                break;
            case 2:
                image1.texture = wrongIcon[0];
                image2.texture = wrongIcon[1];
                image3.texture = rightIcon;
                break;
        }
    }

    public void AddQuestionIcons(string question, List<SpriteForButton> buttons)
    {
        questionText.text = question;

        for (int i = 0; i < 4; i++)
        {
            images[i].texture = buttons[i].icon;
        }
    }

    public void SetBadgeStatus(Texture badge, bool status)
    {   
        panel.SetActive(status);
        badgeUI.SetActive(status);
        badgeUI.GetComponent<RawImage>().texture = badge;
    }

    public void OnButtonPressed(int index)
    {
        beeCompanion.OnButtonPressed(index);
    }
}
