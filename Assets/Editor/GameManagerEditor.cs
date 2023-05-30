using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameManager spell = (GameManager)target;
            
            if(GUILayout.Button("Call EndLevel")) {
                EventManager.InvokeFlagEvent(Flag.EndLevel);
            }
            if(GUILayout.Button("Call StartLevel")) {
                EventManager.InvokeFlagEvent(Flag.StartLevel);
            }
        
        }
    }
}
