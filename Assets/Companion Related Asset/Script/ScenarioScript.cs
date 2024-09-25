using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public enum CompanionPhase
{
    Introduction,
    Scenario1,
    Scenario2
}
[CreateAssetMenu(fileName = "Scenario script")]
public class ScenarioScript : ScriptableObject
{
    public CompanionPhase phase;
    public List<string> scriptLines;
    public bool containQuestion;
    
    public string question;
    public string rightAnswer;
    public string wrongAnswer;
}
