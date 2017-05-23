using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hex))]
public class HexEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = (Hex)target;

        Handles.color = Color.red;

        //Handles.Label(t.transform.position, "ARA");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("A");
        //Handles.Label(t.transform.position, "ARA");
        EditorGUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(Town))]
public class TownEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = (Town)target;

        Handles.color = Color.red;

        

        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("A");
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        foreach (Hex h in t.hexes)
        {
            Handles.Label(h.transform.position, "HEX", style);
        }

        foreach (Road h in t.roads)
        {
            Handles.Label(h.transform.position, "R", style);
        }
        //Handles.Label(t.transform.position, "A");
        EditorGUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(Road))]
public class RoadEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = (Road)target;

        Handles.color = Color.red;

        //Handles.Label(t.transform.position, "ARA");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("A");
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        
        foreach (Town h in t.towns)
        {
            Handles.Label(h.transform.position, "TOWN", style);
        }
        //Handles.Label(t.transform.position, "A");
        EditorGUILayout.EndHorizontal();
    }
}