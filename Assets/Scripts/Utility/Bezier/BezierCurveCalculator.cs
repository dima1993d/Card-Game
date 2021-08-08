using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=-mad4YPAn2U
//https://www.habrador.com/tutorials/interpolation/3-move-along-curve/
//Interpolation between 2 points with a Bezier Curve (cubic spline)
namespace Utility.Bezier
{
	[ExecuteInEditMode]
public static class BezierCurveCalculator
{
    //Easier to use ABCD for the positions of the points so they are the same as in the tutorial image
    //static Vector3 A, B, C, D;

    public static float TotalLength(BezierSpline bezierSpline, ref float[]lengthOfParts)
    {
        float totalLength = 0;
        for (int i = 0; i < bezierSpline.CurveCount; i++)
        {
            Vector3[] positions = bezierSpline.GetPointsPositions(i);
            float LengthOfThisPart = LengthOfOnePart(positions);
            totalLength += LengthOfThisPart;
            lengthOfParts[i] = LengthOfThisPart;
        }
        return totalLength;
    }
    public static float LengthOfOnePart(Vector3[] positions)
    {
        return GetLengthSimpsons(0f, 1f, positions[0], positions[1], positions[2], positions[3]);
    }

    public static Vector3 GetPosition(float lengthOfThisPart, float currentDistanceTraveledOnThisPart, Vector3[] positions)
    {
        //Use Newton–Raphsons method to find the t value from the start of the curve 
        //to the end of the distance we have
        float t = FindTValue(currentDistanceTraveledOnThisPart, lengthOfThisPart, positions[0], positions[1], positions[2], positions[3]);
        //Get the coordinate on the Bezier curve at this t value
        return DeCasteljausAlgorithm(t, positions[0], positions[1], positions[2], positions[3]);
    }

    static Vector3 DeCasteljausAlgorithmDerivative(float t, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 dU = t * t * (-3f * (A - 3f * (B - C) - D));

        dU += t * (6f * (A - 2f * B + C));

        dU += -3f * (A - B);

        return dU;
    }
    //The De Casteljau's Algorithm
    static Vector3 DeCasteljausAlgorithm(float t, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        //Linear interpolation = lerp = (1 - t) * A + t * B
        //Could use Vector3.Lerp(A, B, t)

        //To make it faster
        float oneMinusT = 1f - t;

        //Layer 1
        Vector3 Q = oneMinusT * A + t * B;
        Vector3 R = oneMinusT * B + t * C;
        Vector3 S = oneMinusT * C + t * D;

        //Layer 2
        Vector3 P = oneMinusT * Q + t * R;
        Vector3 T = oneMinusT * R + t * S;

        //Final interpolated position
        Vector3 U = oneMinusT * P + t * T;

        return U;
    }
    //Get and infinite small length from the derivative of the curve at position t
    static float GetArcLengthIntegrand(float t, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        //The derivative at this point (the velocity vector)
        Vector3 dPos = DeCasteljausAlgorithmDerivative(t,A,B,C,D);

        //This the how it looks like in the YouTube videos
        //float xx = dPos.x * dPos.x;
        //float yy = dPos.y * dPos.y;
        //float zz = dPos.z * dPos.z;

        //float integrand = Mathf.Sqrt(xx + yy + zz);

        //Same as above
        float integrand = dPos.magnitude;

        return integrand;
    }

    //Get the length of the curve between two t values with Simpson's rule
    static float GetLengthSimpsons(float tStart, float tEnd, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        //This is the resolution and has to be even
        int n = 20;

        //Now we need to divide the curve into sections
        float delta = (tEnd - tStart) / (float)n;

        //The main loop to calculate the length

        //Everything multiplied by 1
        float endPoints = GetArcLengthIntegrand(tStart, A, B, C, D) + GetArcLengthIntegrand(tEnd, A, B, C, D);

        //Everything multiplied by 4
        float x4 = 0f;
        for (int i = 1; i < n; i += 2)
        {
            float t = tStart + delta * i;

            x4 += GetArcLengthIntegrand(t, A, B, C, D);
        }

        //Everything multiplied by 2
        float x2 = 0f;
        for (int i = 2; i < n; i += 2)
        {
            float t = tStart + delta * i;

            x2 += GetArcLengthIntegrand(t, A, B, C, D);
        }

        //The final length
        float length = (delta / 3f) * (endPoints + 4f * x4 + 2f * x2);

        return length;
    }
    //Use Newton–Raphsons method to find the t value at the end of this distance d
    static float FindTValue(float d, float totalLength, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        //Need a start value to make the method start
        //Should obviously be between 0 and 1
        //We can say that a good starting point is the percentage of distance traveled
        //If this start value is not working you can use the Bisection Method to find a start value
        //https://en.wikipedia.org/wiki/Bisection_method
        float t = 0;
        if (d != 0)
        {
            t = d / totalLength;
        }
        else
        {
            t = 0.001F / totalLength;
        }
        

        //Need an error so we know when to stop the iteration
        float error = 0.001f;

        //We also need to avoid infinite loops
        int iterations = 0;

        while (true)
        {
            //Newton's method
            float tNext = t - ((GetLengthSimpsons(0f, t, A, B, C, D) - d) / GetArcLengthIntegrand(t, A, B, C, D));

            //Have we reached the desired accuracy?
            if (Mathf.Abs(tNext - t) < error)
            {
                break;
            }

            t = tNext;

            iterations += 1;

            if (iterations > 1000)
            {
                break;
            }
        }

        return t;
    }
}
}

