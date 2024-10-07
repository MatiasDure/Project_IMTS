using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Color = System.Drawing.Color;
using Random = System.Random;
using Range = UnityEngine.SocialPlatforms.Range;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CompanionBehavior beeCompanion;
    [SerializeField] private GameObject scanToStartText;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject questionUI;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<RawImage> images;
    [SerializeField] private GameObject badgeUI;
    [SerializeField] private GameObject panel;
    private List<ColorBlock> _originalColors;
    
    private int _randomInt; 
    private void Awake()
    {
        _originalColors = new List<ColorBlock>();
        scanToStartText.SetActive(true);
        menu.SetActive(false);
        for (int i = 0; i < buttons.Count; i++)
        {
            _originalColors.Add(buttons[i].colors);
        }
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

    public void AddQuestionIcons(string question, List<SpriteForButton> buttons)
    {
        questionText.text = question;
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < buttons.Count; i++)
        {
            images[i].gameObject.SetActive(true);
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

    public void OnWrongButton(int index)
    {
        StartCoroutine(buttonWrong(index));
    }

    private IEnumerator buttonWrong(int index)
    {
        ColorBlock color = buttons[index].colors;

        color.selectedColor = UnityEngine.Color.red;
        
        buttons[index].colors = color;

        yield return new WaitForSeconds(0.03f);

        buttons[index].colors = _originalColors[index];

    }
}
