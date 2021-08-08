using UnityEditor;
using UnityEngine;
using Utility.Bezier;

#if UNITY_EDITOR
namespace Spline
{
    [CustomEditor(typeof(BezierController))]
    public class BezierControllerInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BezierController myScript = (BezierController)target;
            if (GUILayout.Button("Generane Straight line"))
            {
                myScript.GenerateStraightLine();
            }
            if (GUILayout.Button("Refresh"))
            {
                if (!myScript.splineParticleWalker)
                {
                    myScript.OnEnable();
                }
                myScript.splineParticleWalker.Setup();
            }
        }
    }
}
#endif