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
    [SerializeField] private TextMeshProUGUI answer1Text;
    [SerializeField] private TextMeshProUGUI answer2Text;
    
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

    public void AddQuestionData(string question, string rightAnswer, string wrongAnswer)
    {
        Random random = new Random();
        _randomInt = random.Next(0, 2);
        
        questionText.text = question;
        
        if (_randomInt == 0)
        {
            answer1Text.text = rightAnswer;
            answer2Text.text = wrongAnswer;
        }
        else
        {
            answer2Text.text = rightAnswer;
            answer1Text.text = wrongAnswer;
        }
    }

    public void GetButton(int index)
    {
        if (index == _randomInt)
        {
            beeCompanion.OnRightAnswer();
        }
        else
        {
            beeCompanion.OnWrongAnswer();
        }
    }
}
