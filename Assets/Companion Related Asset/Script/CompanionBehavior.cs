using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]

public class CompanionBehavior : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshPro textMeshProObject;
    [Space]
    [Header("Offset value")]
    [Range(-0.1f,0.1f)] [SerializeField] private float horizontalOffset = 0;
    [Range(-0.1f, 0.1f)] [SerializeField] private float verticalOffset = 0;
    [Range(0.1f, 0.5f)] [SerializeField] private float forwardOffset = 0.3f;
    [Space] 
    [Header("Scenario Scripts")] [SerializeField] private List<ScenarioScript> scenarioScripts;
    [Space]
    [Header("Other settings")]
    [Range(0, 1)] [SerializeField] private float smoothFactor = 0.2f;

    private ScenarioScript _currenScript;
    //private CompanionPhase _currentPhase;
    //private List<string> _currenScripts = new List<string>();
    //private bool _haveQuestion;

    private int _textCount = 0;
    private GameObject _arCamera;
    
    private void Awake()
    {
        
        CheckDuplicate(scenarioScripts);
        SetData(CompanionPhase.Introduction);
        textMeshProObject.text = _currenScript.scriptLines[0];
        _textCount += 1;
        _arCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void CheckDuplicate(List<ScenarioScript> list)
    {
        List<CompanionPhase> seen = new List<CompanionPhase>();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (!seen.Contains(list[i].phase))
            {
                seen.Add(list[i].phase);
            }
            else
            {
                list.RemoveAt(i);
            }
        }
        
    }

    // Update is called once per frame
    private void Update()
    {
        //update position when moving
        Vector3 targetPosition = _arCamera.transform.position + (_arCamera.transform.forward*forwardOffset)
                                                              + (_arCamera.transform.right*horizontalOffset) 
                                                              + (_arCamera.transform.up*verticalOffset);
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothFactor);
        //look at camera
        transform.LookAt(_arCamera.gameObject.transform);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                {
                    if (_currenScript != null)
                    {
                        if (_textCount < _currenScript.scriptLines.Count)
                        {
                            textMeshProObject.text = _currenScript.scriptLines[_textCount++];
                        }else if (_currenScript.containQuestion)
                        {
                            textBox.SetActive(false);
                            uiManager.SetMenuStatus(false);
                            uiManager.SetQuestionUIStatus(true);
                            
                            uiManager.AddQuestion(_currenScript.question,_currenScript.rightAnswer,_currenScript.wrongAnswer);
                        }
                        else
                        {
                            textBox.SetActive(false);
                            uiManager.SetMenuStatus(true);
                        }
                    }
                }
        
            }
        }
    }

    public void OnPhaseChange(CompanionPhase phase)
    {
        ResetText();
        SetData(phase);
        uiManager.SetMenuStatus(false);
        uiManager.SetQuestionUIStatus(false);
    }

     void ResetText()
    {
        textBox.SetActive(true);
        _textCount = 0;
        textMeshProObject.text = _currenScript.scriptLines[_textCount++];
    }

     void SetData(CompanionPhase phase)
    {
        
        //_currentPhase = phase;

        foreach (ScenarioScript script in scenarioScripts)
        {
            if (script.phase == phase)
            {
                _currenScript = script;
                //_currenScripts = script.scriptLines;
                //_haveQuestion = script.containQuestion;
                break;
            }
        }
    }

}
