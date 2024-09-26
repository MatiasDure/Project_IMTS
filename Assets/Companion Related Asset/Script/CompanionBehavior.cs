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
    [SerializeField] private GameObject beePlane;
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

    private int _textCount = 0;
    private GameObject _arCamera;
    
    private void Awake()
    {
        _currenScript = ScriptableObject.CreateInstance<ScenarioScript>();
        CheckDuplicate(scenarioScripts);
        SetData(CompanionPhase.Introduction);
        ResetText();
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
                        if (_textCount < _currenScript.expressionToTexts.Count)
                        {
                            beePlane.GetComponent<MeshRenderer>().material= 
                                _currenScript.expressionToTexts[_textCount].expression;
                            textMeshProObject.text = _currenScript.expressionToTexts[_textCount++].text;
                        }
                        else if (_currenScript.containQuestion)
                        {
                            OpenQuestion();
                            //_currenScript = null;
                        }
                        else
                        {
                            OpenMenu();
                            _currenScript = null;
                        }
                    }
                }
        
            }
        }
    }

    public void OnPhaseChange(CompanionPhase phase)
    {
        SetData(phase);
        ResetText();
        uiManager.SetMenuStatus(false);
        uiManager.SetQuestionUIStatus(false);
    }

     void ResetText()
    {
        textBox.SetActive(true);
        _textCount = 0;
        textMeshProObject.text = _currenScript.expressionToTexts[_textCount++].text;
        beePlane.GetComponent<MeshRenderer>().material= 
            _currenScript.expressionToTexts[_textCount].expression;
    }

     void SetData(CompanionPhase phase)
    {
        
        //_currentPhase = phase;

        foreach (ScenarioScript script in scenarioScripts)
        {
            if (script.phase == phase)
            {
                _currenScript = script;

                break;
            }
        }
    }

     public void OnRightAnswer()
     {
         textBox.SetActive(true);
         textMeshProObject.text = _currenScript.respondToRightAnswer.text;
         beePlane.GetComponent<MeshRenderer>().material = _currenScript.respondToRightAnswer.expression;
     }

     public void OnWrongAnswer()
     {
         textBox.SetActive(true);
         textMeshProObject.text = _currenScript.respondToWrongAnswer.text;
         beePlane.GetComponent<MeshRenderer>().material = _currenScript.respondToWrongAnswer.expression;
     }

     void OpenMenu()
     {
         textBox.SetActive(false);
         uiManager.SetQuestionUIStatus(false);
         uiManager.SetMenuStatus(true);
     }

     void OpenQuestion()
     {
         textBox.SetActive(false);
         uiManager.SetMenuStatus(false);
         uiManager.SetQuestionUIStatus(true);
                            
         uiManager.AddQuestionData(_currenScript.question,_currenScript.rightAnswer,_currenScript.wrongAnswer);
     }

}
