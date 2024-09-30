using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public enum CompanionPhase
{
    Introduction,
    Scenario1,
    Scenario2
}

[System.Serializable]
public class ExpressionToText
{
    public Material expression;
    public string text;
}

[CreateAssetMenu(fileName = "Scenario script")]
public class ScenarioScript : ScriptableObject
{   
    
    public CompanionPhase phase;
    [Header("Script")]
    public List<ExpressionToText> expressionToTexts;
    
    [Header("Auestion")]
    public bool containQuestion;
    public Material beeInQuestion;
    public string question;
    public Texture rightIcon;
    public List<Texture> wrongIcon;
    
    [Header("Material")]
    public Material hint;
    
    public Material beeWithItem;
    public Material wrongReaction;
    
    public Texture badge;
    
    

}
