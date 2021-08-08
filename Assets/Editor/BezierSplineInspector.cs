using UnityEngine;
using UnityEditor;
using Utility.Bezier;

#if UNITY_EDITOR
[CustomEditor (typeof(BezierSpline))]
public class BezierSplineInspector : Editor {

	private const int lineSteps        = 10;
	private const float directionScale = 0.5f;
    private const int stepsPerCurve    = 10;
    private const float handleSize     = 0.04f;
    private const float pickSize       = 0.06f;
    private int selectedIndex          = -1;

	private BezierSpline spline;
	private Transform handleTransform;
	private Quaternion handleRotation;

    private bool loacalSpaceState = false;
	private void OnSceneGUI() {
		spline = target as BezierSpline;
        
        if (spline.points.Count < 4)
        {
            return;
        }
        loacalSpaceState = spline.localSpace;
        handleTransform = spline.transform;
		handleRotation = Tools.pivotRotation == PivotRotation.Local ? 
			handleTransform.rotation : Quaternion.identity;
		Vector3 p0 = ShowPoint (0);
        for (int i = 1; i < spline.ControlPointCount; i += 3) {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i+1);
            Vector3 p3 = ShowPoint(i+2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
            p0 = p3;
        }
        ShowDirections();
        //DrawPointIndex();
    }

    private void DrawPointIndex()
    {
         GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        for (int i = 0; i < spline.points.Count; i++)
        {
            Handles.Label(spline.points[i] + new Vector3(0, 0.3F, 0), "" + i, style);
        }
    }
	private void ShowDirections() {
		Handles.color = Color.green;
		Vector3 point = spline.GetPoint (0f);
		Handles.DrawLine (point, point + spline.GetDirection (0f) * directionScale);
        int steps = stepsPerCurve * spline.CurveCount;
		for (int i = 1; i <= lineSteps; i++) {
			point = spline.GetPoint (i / (float)lineSteps);
			Handles.DrawLine (point, point + spline.GetDirection (i / (float)lineSteps) * directionScale);
		}
	}

    private static Color[] modeColors = {
        Color.white,
        Color.yellow,
        Color.cyan
    };

    private Vector3 ShowPoint(int index) {
        //Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        Vector3 point = spline.GetControlPoint(index);
        if (spline.localSpace)
        {
            point = spline.transform.TransformPoint(point);
        }
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0) {
            size *= 2f;
        }
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap)) {
            selectedIndex = index;
            Repaint();
        }
        if (selectedIndex == index) {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                //spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point));
                if (spline.localSpace)
                {
                    point = spline.transform.InverseTransformPoint(point);
                }
                spline.SetControlPoint(index, point);
            }
        }
		return point;
	}

    private void ChangePointsSpace(bool local)
    {
        
        for (int i = 0; i < spline.points.Count; i++)
        {
            if (local)
            {
                spline.points[i] = spline.transform.InverseTransformPoint(spline.points[i]);
            }
            else
            {
                spline.points[i] = spline.transform.TransformPoint(spline.points[i]);
            }
            
        }
    }
    public override void OnInspectorGUI() {
        spline = target as BezierSpline;
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        /*
        if (loacalSpaceState != spline.localSpace)
        {
            loacalSpaceState = spline.localSpace;
            if (loacalSpaceState)
            {
                ChangePointsSpace(true);
            }
            else
            {
                ChangePointsSpace(false);
            }
        }*/
        //bool loop = EditorGUILayout.Toggle("Loop", spline.Loop);
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Toggle Loop");
            EditorUtility.SetDirty(spline);
            //spline.Loop = loop;
        }
        if (GUILayout.Button ("To Local Space")) {
            ChangePointsSpace(true);
        }
        if (GUILayout.Button ("To World Space")) {
            ChangePointsSpace(false);
        }
        if (selectedIndex >= 0 && selectedIndex < spline.ControlPointCount) {
            DrawSelectedPointInspector();
        }
		if (GUILayout.Button ("Add Point")) {
			Undo.RecordObject (spline, "Add Curve");
            spline.AddCurve(spline.points[selectedIndex] + (spline.points[selectedIndex] - spline.points[selectedIndex-3]).normalized * 2);
            selectedIndex += 3;
            EditorUtility.SetDirty (spline);
		}
        if (GUILayout.Button("Remove Selected Point"))
        {
            Undo.RecordObject(spline, "Remove Point");
            spline.DeletePoint(selectedIndex);
            EditorUtility.SetDirty(spline);
        }

    }

    private void DrawSelectedPointInspector() {
        GUILayout.Label("Selected Point");
        //EditorGUILayout.IntField(selectedIndex);

        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Positon", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
        EditorGUI.BeginChangeCheck();
        BezierControlPointMode mode = (BezierControlPointMode)EditorGUILayout.EnumPopup("Mode", spline.GetControlPointMode(selectedIndex));
        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(spline, "Change Point Mode");
            spline.SetControlPointMode(selectedIndex, mode);
            EditorUtility.SetDirty(spline);
        }
    }
#endif
}
