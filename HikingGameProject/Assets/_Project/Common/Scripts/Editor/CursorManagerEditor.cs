using UnityEngine;
using UnityEditor;

namespace HikingGame.Common
{
    [CustomEditor(typeof(CursorManager), editorForChildClasses: true)]
    public class CursorManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CursorManager cm = target as CursorManager;
            if (GUILayout.Button("Lock Cursor"))
            {
                cm.LockCursor();
            }
            if (GUILayout.Button("Unlock Cursor"))
            {
                cm.UnlockCursor();
            }
        }
    }
}
