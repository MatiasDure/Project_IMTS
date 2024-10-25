using UnityEngine;

[System.Serializable]
public class Range
{
    public float Min;
    public float Max;

    public float GetRandomValueWithinRange() => Random.Range(Min, Max);
}
