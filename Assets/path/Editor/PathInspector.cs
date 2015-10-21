using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(com_path))]
public class PathInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("UpdatePath", EditorStyles.miniButtonLeft))
        {
            (this.target as com_path).UpdatePath();
        }
        if (GUILayout.Button("HidePath", EditorStyles.miniButtonLeft))
        {
            (this.target as com_path).HidePath();
        }
        if (GUILayout.Button("PathReset", EditorStyles.miniButtonLeft))
        {
            (this.target as com_path).Reset();
        }
        if (GUILayout.Button("PathAdd", EditorStyles.miniButtonLeft))
        {
            (this.target as com_path).GetPos(1.0f);
        }
		if (GUILayout.Button("ToggleShowCube", EditorStyles.miniButtonLeft))
        {
            (this.target as com_path).ToggleShowCube();
        }
    }
}


