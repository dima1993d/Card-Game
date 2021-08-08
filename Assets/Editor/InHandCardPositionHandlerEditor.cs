using Cards;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace CardGame.Editor
{
    [CustomEditor(typeof(InHandCardPositionHandler))]
    public class InHandCardPositionHandlerEditor : UnityEditor.Editor
    {
        ArcHandle m_ArcHandle = new ArcHandle();
        ArcHandle m_ArcHandle2 = new ArcHandle();
        private InHandCardPositionHandler m_target;
        protected virtual void OnEnable()
        {
            m_target = (InHandCardPositionHandler)target;
            m_ArcHandle.SetColorWithRadiusHandle(Color.white, 0.1f);
        }
        
        
        protected virtual void OnSceneGUI()
        {
            // copy the target object's data to the handle
            m_ArcHandle.angle = m_target.arcAngle;
            m_ArcHandle.radius = m_target.arcRadius;
            m_ArcHandle2.angle = -m_target.arcAngle;
            m_ArcHandle2.radius = m_target.arcRadius;

            // set the handle matrix so that angle extends upward from target's facing direction along ground
            Vector3 handleDirection = m_target.transform.up;
            Vector3 handleNormal = Vector3.Cross(handleDirection, Vector3.up);
            Matrix4x4 handleMatrix = Matrix4x4.TRS(
                m_target.transform.position + m_target.arcOffset,
                Quaternion.LookRotation(handleDirection, handleNormal),
                Vector3.one
            );

            
            
            
            using (new Handles.DrawingScope(handleMatrix))
            {
                // draw the handle
                EditorGUI.BeginChangeCheck();
                m_ArcHandle.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    // record the target object before setting new values so changes can be undone/redone
                    Undo.RecordObject(m_target, "Change Projectile Properties");

                    // copy the handle's updated data back to the target object
                    m_target.arcAngle = m_ArcHandle.angle;
                    m_ArcHandle2.angle = -m_ArcHandle.angle;
                    m_target.arcRadius = m_ArcHandle.radius;
                }
            } 
            using (new Handles.DrawingScope(handleMatrix))
            {
                // draw the handle
                EditorGUI.BeginChangeCheck();
                m_ArcHandle2.DrawHandle();
                if (EditorGUI.EndChangeCheck())
                {
                    // record the target object before setting new values so changes can be undone/redone
                    Undo.RecordObject(m_target, "Change Projectile Properties");

                    // copy the handle's updated data back to the target object
                    m_target.arcAngle = -m_ArcHandle2.angle;
                    m_ArcHandle.angle = -m_ArcHandle2.angle;

                    //m_target.arcRadius = m_ArcHandle2.radius;
                }
            }
        }
    }
}
