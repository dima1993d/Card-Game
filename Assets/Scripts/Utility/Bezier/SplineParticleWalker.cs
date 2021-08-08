using UnityEditor;
using UnityEngine;

namespace Utility.Bezier
{
    [ExecuteInEditMode,RequireComponent(typeof (ParticleSystem), typeof(BezierSpline))]
public class SplineParticleWalker : MonoBehaviour
{
    private BezierSpline spline;     //reference to Bezier curve
    private float[] lengthOfParts;  //Length of parts of Bezier curve
    private float TotalLength;      //Length of Bezier curve
    private float particleSpeed;    //Speed particle needs to be traweling at to go through all curve during its lifetime
    private ParticleSystem m_particleSystem;//reference to ParticleSystem
    private int[] particleSystemIndexs = new int[0];
    private ParticleSystem.Particle[] p;
    public Vector3[] AllPossiblePositions;
    public bool isSetup;
    [SerializeField]
    private float density = 1;

    private void OnEnable()
    {
        if (!m_particleSystem)
            m_particleSystem = GetComponent<ParticleSystem>();
        
        if (!spline)
            spline = GetComponent<BezierSpline>();
        if (AllPossiblePositions == null)
        {
            int numberOfPoints = (int)m_particleSystem.main.startLifetime.constant * 60;
            AllPossiblePositions = new Vector3[numberOfPoints];
            isSetup = false;
        }
    }
    public void Setup()
    {
        GetTotalLength();
        GetAllPossiblePositions();
        //var emission = m_particleSystem.emission;
        //emission.rateOverTime = density * TotalLength;
        isSetup = true;
#if UNITY_EDITOR

        PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject.transform);
        PrefabUtility.RecordPrefabInstancePropertyModifications(spline.transform);
#endif
    }
    public void GetTotalLength()
    {
        if (!spline.localSpace)
        {
            transform.position = spline.points[0];
        }
        
        p = new ParticleSystem.Particle[m_particleSystem.main.maxParticles];
        lengthOfParts = new float[spline.CurveCount];
        TotalLength = BezierCurveCalculator.TotalLength(spline, ref lengthOfParts);
        particleSpeed = TotalLength / m_particleSystem.main.startLifetime.constant;
    }

    public void GetAllPossiblePositions()
    {
        int numberOfPoints = (int) m_particleSystem.main.startLifetime.constant * 50;
        particleSystemIndexs = new int[m_particleSystem.main.maxParticles];
        AllPossiblePositions = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            float distanceParticleHasTraveled = (TotalLength / numberOfPoints) * i;
            int index = 0;
            float summOfParts = 0;
            for (int o = 0; o < spline.CurveCount; o++)
            {
                if (summOfParts + lengthOfParts[o] < distanceParticleHasTraveled)
                {
                    summOfParts += lengthOfParts[o];
                    index++;
                }
                else
                {
                    break;
                }
            }
            Vector3[] positions = spline.GetPointsPositions(index); //4 Points Positions of part of BezierSpline
            AllPossiblePositions[i] = BezierCurveCalculator.GetPosition(lengthOfParts[index], distanceParticleHasTraveled - summOfParts, positions);
        }
    }

    void Update()
    {
        if (spline.points.Count < 4)
        {
            return;
        }
        if (!Application.isPlaying )
        {
            //Setup();
        }
        if (isSetup)
        {
            if (!m_particleSystem.isPlaying)
            {
                m_particleSystem.Play();
            }
            MoveParticleOptimized();
        }
        else
        {
            m_particleSystem.Pause();
        }
    }
    void MoveParticleOptimized()
    {
        if (p == null)
        {
            Setup();
            return;
        }
        m_particleSystem.GetParticles(p);

        for (int i = 0; i < m_particleSystem.particleCount; i++)
        {
            float percentOfTheWay = (p[i].startLifetime - p[i].remainingLifetime) / p[i].startLifetime; // 0 - 1 
            int num = Mathf.CeilToInt((AllPossiblePositions.Length-1) * percentOfTheWay);

            p[i].position = AllPossiblePositions[num];
        }
        m_particleSystem.SetParticles(p, m_particleSystem.particleCount);
    }
}

}
