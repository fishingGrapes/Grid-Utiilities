using UnityEngine;

public static class UnityExtensions
{

    /// <summary>
    /// Calculate the Rect given two points
    /// </summary>
    /// <param name="eventData"></param>
    public static void CalculateFromPoints(ref this Rect rect, Vector2 point1, Vector2 point2)
    {
        var min = Vector2.Min(point1, point2);
        var max = Vector2.Max(point1, point2);

        rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
    }

}
