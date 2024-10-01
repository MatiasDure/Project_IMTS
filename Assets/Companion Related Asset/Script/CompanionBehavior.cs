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
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject beePlane;
    [SerializeField] private GameObject textBox;
    [SerializeField] private TextMeshProUGUI textMeshProObject;
    [SerializeField] private GameObject thinkBurble;
    [SerializeField] private GameObject hintPlane;
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

    private bool _answering = false;
    private bool _finishAnswer = false;
    private bool _showBadge = false;
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

        if (Input.GetMouseButtonDown(0))
        {
            // Touch touch = Input.GetTouch(0);
            // if (touch.phase == TouchPhase.Began)
            // {
            //  {
            if (_currenScript != null)
            {
                if (_textCount < _currenScript.expressionToTexts.Count)
                {
                   // soundManager.PlayTalkSound();
                    beePlane.GetComponent<MeshRenderer>().material= 
                        _currenScript.expressionToTexts[_textCount].expression;
                    textMeshProObject.text = _currenScript.expressionToTexts[_textCount++].text;
                }
                else if (_currenScript.containQuestion && !_answering &&!_finishAnswer)
                {
                    _answering = true;
                    OpenQuestion();
                    //_currenScript = null;
                }else if (_finishAnswer && !_showBadge)
                {
                    //show badge
                    soundManager.PlayBadgeSound();
                    _showBadge = true;
                    uiManager.SetBadgeStatus(_currenScript.badge, true);
                }
                else if(!_currenScript.containQuestion || _showBadge)
                {
                    OpenMenu();
                    _currenScript = null;
                }
            }
                    
                    
            //     }
            //
            // }
        }
    }

    public void OnPhaseChange(CompanionPhase phase)
    {
        _answering = false;
        _finishAnswer = false;
        _showBadge = false;
        beePlane.SetActive(true);
        SetData(phase);
        ResetText();
        uiManager.SetMenuStatus(false);
        uiManager.SetQuestionUIStatus(false);
    }

     void ResetText()
    {
        soundManager.PlayTalkSound();
        textBox.SetActive(true);
        _textCount = 0;
        textMeshProObject.text = _currenScript.expressionToTexts[_textCount++].text;
        beePlane.GetComponent<MeshRenderer>().material= 
            _currenScript.expressionToTexts[0].expression;
    }

     void SetData(CompanionPhase phase)
    {
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
         soundManager.PlayHappySound();
         _answering = false;
         _finishAnswer = true;
         thinkBurble.SetActive(false);
         uiManager.SetQuestionUIStatus(false);
         beePlane.GetComponent<MeshRenderer>().material = _currenScript.beeWithItem;
     }

     public void OnWrongAnswer()
     {
         soundManager.PlaySadSound();
         beePlane.GetComponent<MeshRenderer>().material = _currenScript.wrongReaction;
     }

     void OpenMenu()
     {
         uiManager.SetBadgeStatus(_currenScript.badge, false);
         beePlane.SetActive(false);
         thinkBurble.SetActive(false);
         textBox.SetActive(false);
         uiManager.SetQuestionUIStatus(false);
         uiManager.SetMenuStatus(true);
     }

     void OpenQuestion()
     {
         //thinkBurble.SetActive(true);
         hintPlane.GetComponent<MeshRenderer>().material = _currenScript.hint;
         
         textBox.SetActive(false);
         uiManager.SetMenuStatus(false);
         uiManager.SetQuestionUIStatus(true);

         //uiManager.AddQuestionData(_currenScript.question,_currenScript.rightAnswer,_currenScript.wrongAnswer);
         
         uiManager.AddQuestionIcon(_currenScript.question,_currenScript.rightIcon,_currenScript.wrongIcon);
     }

}
