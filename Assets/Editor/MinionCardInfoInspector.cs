using Cards;
using UnityEditor;

#if UNITY_EDITOR
namespace Spline
{
    [CustomEditor(typeof(MinionCardInfo))]
    public class MinionCardInfoInspector : UnityEditor.Editor
    {
        SerializedProperty m_infoProp;
        private MinionCardInfo myScript;
        void OnEnable()
        {
            myScript = (MinionCardInfo)target;
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            DrawDefaultInspector();
            
            if (EditorGUI.EndChangeCheck())
            {
                myScript.UpdateInfo(myScript.Info);
                Undo.RecordObject(myScript, "Change in values");
                EditorUtility.SetDirty(myScript);
            }   
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif