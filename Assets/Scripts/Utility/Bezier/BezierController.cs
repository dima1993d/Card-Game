using UnityEngine;

namespace Utility.Bezier
{
    [ExecuteInEditMode, RequireComponent( typeof(BezierSpline), typeof(SplineParticleWalker))]
    public class BezierController : MonoBehaviour
    {
        [HideInInspector]
        public BezierSpline bezierSpline;
        [HideInInspector]
        public SplineParticleWalker splineParticleWalker;
        public Transform start, end;
        public void OnEnable()
        {
            if (!bezierSpline)
                bezierSpline = GetComponent<BezierSpline>();
            if (!splineParticleWalker)
                splineParticleWalker = GetComponent<SplineParticleWalker>();
        }
        public void GenerateStraightLine()
        {
            if (start && end)
            {
                bezierSpline.Reset();
                bezierSpline.AddCurve(start.position);
                bezierSpline.AddCurve(end.position);
                splineParticleWalker.Setup();
            }
            else
            {
                Debug.Log("Transforms are not setup to test Particles",gameObject);
            }
        }

        private void SavePositions(Transform start, Transform end)
        {
            this.start = start;
            this.end = end;
        }
    }

}


