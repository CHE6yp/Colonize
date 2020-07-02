using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor {

    public override void OnInspectorGUI()
    {
        Player myTarget = (Player)target;

        EditorGUILayout.LabelField("Player Name ", myTarget.playerName);
        EditorGUILayout.LabelField("Id ", myTarget.colorInt.ToString());
        
        string turn = (myTarget.turn) ? "It's his turn now!" : "Not his turn.";
        EditorGUILayout.LabelField(" ",turn);
        if (myTarget.turn)
        {
            string diceroll = (myTarget.rolledDice) ? "And the dices are rolled!" : "But the dices aren't rolled...";
            EditorGUILayout.LabelField(" ",diceroll);
        }

        EditorGUILayout.EnumPopup("Player Phase", myTarget.playerPhase);
        EditorGUILayout.Space();
        //EditorGUILayout.BeginFadeGroup(0);
        EditorGUILayout.LabelField("Wood ", myTarget.wealth[0].ToString());
        EditorGUILayout.LabelField("Sheep ", myTarget.wealth[1].ToString());
        EditorGUILayout.LabelField("Wheat ", myTarget.wealth[2].ToString());
        EditorGUILayout.LabelField("Brick ", myTarget.wealth[3].ToString());
        EditorGUILayout.LabelField("Stone ", myTarget.wealth[4].ToString());
        //EditorGUILayout.EndFadeGroup();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Win Points ", myTarget.winPoints.ToString());

        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}
