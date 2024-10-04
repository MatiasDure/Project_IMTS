using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

[System.Serializable]
public class SpriteForButton
{
    public Texture icon;
    public Material sprite;
}

[CreateAssetMenu(fileName = "Scenario script")]
public class ScenarioScript : ScriptableObject
{   
    
    public CompanionPhase phase;
    [Header("Script")]
    public List<ExpressionToText> expressionToTexts;
    
    [Header("Question")]
    public bool containQuestion;
    public string question;

    public List<SpriteForButton> buttons;
    
    [Space]
    public Texture badge;
    
    

}
