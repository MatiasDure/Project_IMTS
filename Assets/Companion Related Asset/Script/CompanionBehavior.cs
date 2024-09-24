using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Toolbars;
using UnityEngine;

[System.Serializable]
public enum CompanionPhase
{
    Introduction,
    Scenario1,
    Scenario2
}
[System.Serializable]
public class CompanionScriptPerPhase
{
    public CompanionPhase phase;
    public List<string> lines = new List<string>();
} 

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
    [Header("Other settings")]
    [Range(0, 1)] [SerializeField] private float smoothFactor = 0.2f;
    [SerializeField] private List<CompanionScriptPerPhase> phaseScriptPerPhases = new List<CompanionScriptPerPhase>();

    private Dictionary<CompanionPhase, List<string>> _phaseScriptDictionary;
    private CompanionPhase _currentPhase;
    private List<string> _currenScript = new List<string>();

    private int _textCount = 0;
    private GameObject _arCamera;
    
    private void Awake()
    {
        _phaseScriptDictionary = new Dictionary<CompanionPhase, List<string>>();
        
        OverwriteDictionary(_phaseScriptDictionary, phaseScriptPerPhases);
        
        SetPhase(CompanionPhase.Introduction);

        textMeshProObject.text = _currenScript[0];
        _textCount += 1;
        _arCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    
    //paste inspector list to dictionary and delete duplicate (take the latest option)
    void OverwriteDictionary(Dictionary<CompanionPhase, List<string>> dictionary, List<CompanionScriptPerPhase> list)
    {
        for (int i = list.Count-1;i>=0;i--)
        {
            if (!dictionary.ContainsKey(list[i].phase))
            {
                Debug.Log("add");
                dictionary.Add(list[i].phase,list[i].lines);
                Debug.Log(_phaseScriptDictionary[list[i].phase]);
            }
            else
            {
                Debug.Log(i);
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
                        if (_textCount < _currenScript.Count)
                        {
                            textMeshProObject.text = _currenScript[_textCount++];
                        }
                        else
                        {
                            _textCount = 0;
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
        textBox.SetActive(true);
        _textCount = 0;
        textMeshProObject.text = _currenScript[_textCount++];
        SetPhase(phase);
        uiManager.SetMenuStatus(false);
    }
    
    void SetPhase(CompanionPhase phase)
    {
        _currentPhase = phase;

        if (_phaseScriptDictionary.TryGetValue(phase, out List<string> lines))
        {
            _currenScript = lines;
        }
    }

}
