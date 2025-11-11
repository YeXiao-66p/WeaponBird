using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level Level;

    Vector2 scrollPos;

    public override void OnInspectorGUI()
    {   
        base.OnInspectorGUI();
        Level = (Level)target;
        OnRulesGUI(Level);
    }

    private void OnRulesGUI(Level level)
    {
        GUILayout.Label("Rules");
        //GUILayout.BeginScrollView(scrollPos);
        GUILayout.BeginVertical();
        for(int i=0; i< level.Rules.Count; i++)
        {
            EditorGUILayout.ObjectField(level.Rules[i], typeof(Unit), true);

        }
        GUILayout.EndVertical();
        //GUILayout.EndScrollView(scrollPos);
        if(GUILayout.Button("Add Rule"))
        {
            level.Rules.Add(new SpawnRule());
        }
    }

}
#else
#endif