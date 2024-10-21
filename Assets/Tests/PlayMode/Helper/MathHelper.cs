using UnityEngine;

public static class MathHelper 
{
    public static bool IsApproximatelyEqual(Vector3 a, Vector3 b, float tolerance = 0.4f)
    {
        return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance && Mathf.Abs(a.z - b.z) < tolerance;
    }
}
