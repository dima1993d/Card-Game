using UnityEditor;
using UnityEngine;

namespace Utility.Variable
{
    [CreateAssetMenu(fileName = "New Float Variable", menuName = "Variable/FloatVariable", order = 51)]
    public class FloatVariable : ScriptableObject
    {
        public float Value;
        public void SetVariable(float var)
        {
            Value = var;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
