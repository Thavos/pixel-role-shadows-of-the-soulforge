using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AbstractGenerator), true)]
public class AbsGenEditor : Editor
{
    AbstractGenerator generator;

    private void Awake()
    {
        generator = (AbstractGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Dungeon"))
        {
            generator.SetRandom();
            generator.Generate();
        }
    }
}