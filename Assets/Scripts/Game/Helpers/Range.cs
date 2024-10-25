using UnityEngine;

[System.Serializable]
public class Range
{
    public Vector2 valuesRange;

    public static float GetRandomValueWithinRange(Vector2 range) => Random.Range(Mathf.Min(range.x, range.y), Mathf.Max(range.x, range.y));
    public static float GetRandomValueNegativeToPositive(float value) => Random.Range(-value, value);
}
