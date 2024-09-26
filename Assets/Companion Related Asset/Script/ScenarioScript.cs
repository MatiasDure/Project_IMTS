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
    public List<ExpressionToText> expressionToTexts;
    public bool containQuestion;
    
    public string question;
    public string rightAnswer;
    public string wrongAnswer;

    public ExpressionToText respondToRightAnswer;
    public ExpressionToText respondToWrongAnswer;
}
