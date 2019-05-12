using System;
using UnityEngine;

public static class MathUtility
{
    #region Bresenhams

    // Author: Jason Morley (Source: http://www.morleydev.co.uk/blog/2010/11/18/generic-bresenhams-line-algorithm-in-visual-basic-net/
    public static void Swap<T>(ref T lhs, ref T rhs) { T temp; temp = lhs; lhs = rhs; rhs = temp; }

    /// <summary>
    /// The plot function delegate
    /// </summary>
    /// <param name="x">The x co-ord being plotted</param>
    /// <param name="y">The y co-ord being plotted</param>
    /// <returns>True to continue, false to stop the algorithm</returns>
    public delegate bool PlotFunction(int x, int y);

    /// <summary>
    /// Plot the line from (x0, y0) to (x1, y10
    /// </summary>
    /// <param name="x0">The start x</param>
    /// <param name="y0">The start y</param>
    /// <param name="x1">The end x</param>
    /// <param name="y1">The end y</param>
    /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
    public static void Line(int x0, int y0, int x1, int y1, PlotFunction plot)
    {
        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep) { Swap<int>(ref x0, ref y0); Swap<int>(ref x1, ref y1); }
        if (x0 > x1) { Swap<int>(ref x0, ref x1); Swap<int>(ref y0, ref y1); }
        int dX = (x1 - x0), dY = Math.Abs(y1 - y0), err = (dX / 2), ystep = (y0 < y1 ? 1 : -1), y = y0;

        for (int x = x0; x <= x1; ++x)
        {
            if (!(steep ? plot(y, x) : plot(x, y))) return;
            err = err - dY;
            if (err < 0) { y += ystep; err += dX; }
        }


    }

    #endregion

    /// <summary>
    /// Given two Points, Gives a Third Point at a Distance from the First Point
    /// </summary>
    /// <param name="a">First Point</param>
    /// <param name="b">Second Point</param>
    /// <param name="distance">Distance from the First point to the Extrapolated point</param>
    /// <returns>The Exrtrapolated Point</returns>
    public static Vector3 Extrapolate(Vector3 a, Vector3 b, ushort distance)
    {
        Vector3 result = new Vector3();

        float length = Vector3.Distance(a, b);
        float unitSlopeX = (b.x - a.x) / length;
        float unitSlopeY = (b.y - a.y) / length;
        float unitSlopeZ = (b.z - a.z) / length;

        result.x = a.x + unitSlopeX * distance;
        result.y = a.y + unitSlopeY * distance;
        result.z = a.z + unitSlopeZ * distance;

        return result;
    }

    /// <summary>
    /// Angle between Two Points in Radians
    /// </summary>
    /// <param name="vec1"></param>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static float AngleInRadians(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    /// <summary>
    /// Angle between Two Points in Degrees
    /// </summary>
    /// <param name="vec1"></param>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static float AngleInDegrees(Vector3 vec1, Vector3 vec2)
    {
        float angle = AngleInRadians(vec1, vec2) * 180 / Mathf.PI;
        return angle < 0 ? angle + 360 : angle;
    }

}
