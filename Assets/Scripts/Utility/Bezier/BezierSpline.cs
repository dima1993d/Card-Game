using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Utility.Bezier
{
	
public enum BezierControlPointMode
{
    Free,
    Aligned,
    Mirrored
}
public class BezierSpline : MonoBehaviour {

    [SerializeField]
	public List<Vector3> points = new List<Vector3>();

    [SerializeField]
    private List<BezierControlPointMode> modes = new List<BezierControlPointMode>();
    [SerializeField]
    private float scaleHandles = 5;

    public bool localSpace = false;
    public int CurveCount {
        get {
            return (points.Count - 1) / 3;
        }
    }

    public int ControlPointCount {
        get {
            return points.Count;
        }
    }

    public Vector3 GetControlPoint (int index) {
        return points[index];
    }

    public BezierControlPointMode GetControlPointMode (int index) {
        return modes[(index + 1) / 3];
    }

    public void SetControlPoint (int index, Vector3 point) {
        if (index % 3 == 0) 
        {
            Vector3 delta = point - points[index];
            if (loop) 
            {
                if (index == 0) 
                {
                    points[1] += delta;
                    points[points.Count - 2] += delta;
                    points[points.Count - 1] += delta;
                }
                else if (index == points.Count - 1) 
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else 
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                    points[index] += delta;
                }
            }
            else {
                points[index] += delta;
                if (index > 0) 
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Count) 
                {
                    points[index + 1] += delta;
                }
            }
        }
        else
        {
            points[index] = point;
            EnforceMode(index);
        }
        
    }

    public void SetControlPointMode (int index, BezierControlPointMode mode) {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop) {
            if (modeIndex == 0)
                modes[modes.Count - 1] = mode;
            else if (modeIndex == modes.Count - 1)
                modes[0] = mode;
        }
        EnforceMode(index);
    }

    private void EnforceMode (int index) {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Count -1)) {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex) {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
                fixedIndex = points.Count - 2;
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Count) {
                enforcedIndex = 1;
            }
        }
        else {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Count) {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
                enforcedIndex = points.Count - 2;
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned) {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

	public Vector3 GetPoint (float t) {
        int i;
        if (t >= 1f) {
            t = 1f;
            i = points.Count - 4;
        }
        else {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
		//return transform.TransformPoint (Bezier.GetPoint (points [i], points [i+1], points [i+2], points [i+3], t));
        return Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t);
    }

    public Vector3[] GetPointsPositions(int splineIndex)
    {
        int Index = splineIndex * 3;
        return new[] { points[Index], points[Index + 1], points[Index + 2], points[Index + 3] };
    }

    public Vector3 GetVelocity (float t) {
        int i;
        if (t >= 1f) {
            t = 1f;
            i = points.Count - 4;
        }
        else {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
		return transform.TransformPoint (Bezier.GetFirstDerivative (points [i], points [i+1], points [i+2], points [i+3], t)) -
			transform.position;
	}

    public void Reset() {

        points.Clear();
        modes.Clear();
	}

	public Vector3 GetDirection (float t) {
		return GetVelocity (t).normalized;
	}

	
    public void DeletePoint(int num)
    {
        if (num > this.points.Count)
        {
            return;
        }
        int modeIndex = (num + 1) / 3;

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (num <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
                fixedIndex = points.Count - 2;
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Count)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Count)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
                enforcedIndex = points.Count - 2;
        }

        if (middleIndex < points.Count - 2)
        {
            points.RemoveRange(middleIndex - 1, 3);
        }
        else
        {
            points.Remove(points[points.Count - 1]);
            points.Remove(points[points.Count - 2]);
        }

        modes.Remove(modes[modeIndex]);
        if (points.Count - 1 < 4)
        {
            Reset();
            return;
        }
        FixModes(); 

        

        if (loop)
        {
            points[points.Count - 1] = points[0];
            modes[modes.Count - 1] = modes[0];
            EnforceMode(0);
        }
    }
    public void InsertCurve(Vector3 point, int num)
    {
        int index = num;
        if (index % 3 == 0)
        {
            index += 2;
        }
        else if (index % 3 == 1)
        {
            index++;
        }

        if (index < 0 || index > points.Count)
        {
            return;
        }
        if (points.Count == 0)
        {
            AddCurve(point);
            return;
        }
        if (points.Count == 1)
        {
            AddCurve(point);
            return;
        }
        if (index == 0)
        {
            points.Insert(0, point);
            points.Insert(1, points[0] - (points[0] - points[1]).normalized / scaleHandles);
            points.Insert(2, points[2] - (points[2] - points[0]).normalized / scaleHandles);

            modes[0] = BezierControlPointMode.Mirrored;
            modes[1] = BezierControlPointMode.Mirrored;
            modes.Insert(0, BezierControlPointMode.Free);
            modes.Insert(1, BezierControlPointMode.Free);
            for (int i = 0; i < modes.Count; i++)
            {
                EnforceMode(i);
            }
            return;
        }
        if (index == points.Count)
        {
            AddCurve(point);
            return;
        }

        List<Vector3> pointsList = new List<Vector3>();
        pointsList.Add((points[index] - points[index - 1]).normalized / scaleHandles + (points[index] - point));
        pointsList.Add(point);
        pointsList.Add((points[index] - points[index + 1]).normalized / scaleHandles + (points[index] - point));

        points.InsertRange(index, pointsList);
        modes.Insert(index, BezierControlPointMode.Mirrored);

        for (int i = 0; i < modes.Count; i++)
        {
            if (i == 0 || i == 1 || i == modes.Count - 2 || i == modes.Count - 1)
            {
                modes[i] = BezierControlPointMode.Free;
            }
            else
            {
                modes[i] = BezierControlPointMode.Mirrored;

            }
        }
        for (int i = 0; i < modes.Count; i++)
        {
            EnforceMode(i);
        }
    }
    public void AddCurve(Vector3 point)
    {
        if (localSpace)
        {
            point = transform.InverseTransformPoint(point);
        }
        if (points.Count == 0)
        {
            points.Add(point);
            modes.Add(BezierControlPointMode.Free);
            return;
        }

        if (points.Count == 1)
        {
            points.Add(points[0] - (points[0] - point).normalized / scaleHandles);
            points.Add(point - (point - points[0]).normalized / scaleHandles);
            points.Add(point);

            modes.Add(BezierControlPointMode.Free);
            return;
        }
        points.Add(points[points.Count -1] - (points[points.Count-1] - point).normalized / scaleHandles);
        points.Add(point - (point - points[points.Count - 2]).normalized / scaleHandles);
        points.Add(point);

        modes.Add(BezierControlPointMode.Free);

        if (loop)
        {
            points[points.Count - 1] = points[0];
            modes[modes.Count - 1] = modes[0];
            EnforceMode(0);
        }
    }
    public void FixModes()
    {
        for (int i = 0; i < modes.Count; i++)
        {
            if (i == 0 || i == modes.Count - 1)
            {
                modes[i] = BezierControlPointMode.Free;
            }
            else
            {
                modes[i] = BezierControlPointMode.Mirrored;
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            EnforceMode(i);
        }
    }
    [SerializeField]
    private bool loop;
    
    public bool Loop {
        get {
            return loop;
        }
        set {
            loop = value;
            if (value == true) {
                modes[modes.Count - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }
}

}