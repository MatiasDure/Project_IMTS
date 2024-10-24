using UnityEngine;

public static class MathHelper 
{
    public static bool AreVectorApproximatelyEqual(Vector3 a, Vector3 b, float tolerance = 0.4f)
    {   
        //calculate difference in each component then check
        return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance && Mathf.Abs(a.z - b.z) < tolerance;
    }
    public static bool AreQuaternionsApproximatelyEqual(Quaternion q1, Quaternion q2, float tolerance = 0.04f)
    {
        // Calculate the difference in each component
        float wDiff = Mathf.Abs(q1.w - q2.w);
        float xDiff = Mathf.Abs(q1.x - q2.x);
        float yDiff = Mathf.Abs(q1.y - q2.y);
        float zDiff = Mathf.Abs(q1.z - q2.z);
        
        // Check if all differences are within the specified epsilon
        return wDiff < tolerance && xDiff < tolerance && yDiff < tolerance && zDiff < tolerance;
    }
}
