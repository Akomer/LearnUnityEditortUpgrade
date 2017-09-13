using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Star)), CanEditMultipleObjects]
public class StarInspector : Editor
{
    private const string propertyNameCenter = "center";
    private const string propertyNamePoints = "points";
    private const string propertyNameFrequancy = "frequency";

    private static Vector3 pointSnap = Vector3.one * 0.1f;

    private SerializedProperty points;
    private SerializedProperty frequency;

    private void OnEnable()
    {
        points = serializedObject.FindProperty(propertyNamePoints);
        frequency = serializedObject.FindProperty(propertyNameFrequancy);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty(propertyNameCenter));
        EditorList.Show(points, EditorListOption.All);
        EditorGUILayout.PropertyField(frequency);

        if (!serializedObject.isEditingMultipleObjects)
        {
            var totalPoints = frequency.intValue * points.arraySize;
            if (totalPoints < 3)
            {
                EditorGUILayout.HelpBox("At least three points are needed", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox("There are " + totalPoints + " points", MessageType.Info);
            }
        }

        if (serializedObject.ApplyModifiedProperties() || UndoRedoHappend())
        {
            for (var i = 0; i < targets.Length; i++)
            {
                var target = targets[i] as Star;
                if (target != null && NotPrefab(target))
                {
                    target.UpdateMesh();
                }
            }
        }
    }

    private void OnSceneGUI()
    {
        var star = target as Star;
        var starTransform = star.transform;

        var angle = -360f / (star.frequency * star.points.Length);
        for(var i = 0; i < star.points.Length; i++)
        {
            var rotation = Quaternion.Euler(0f, 0f, angle * i);
            var oldPoint = starTransform.TransformPoint(rotation * star.points[i].position);
            var newPoint = Handles.FreeMoveHandle(oldPoint, Quaternion.identity, 0.02f, pointSnap, Handles.DotHandleCap);
            if(newPoint != oldPoint)
            {
                Undo.RecordObject(star, "Move point");
                EditorUtility.SetDirty(star);
                star.points[i].position = Quaternion.Inverse(rotation) * starTransform.InverseTransformPoint(newPoint);
                star.UpdateMesh();
            }
        }
    }

    private bool NotPrefab(Star target)
    {
        return PrefabUtility.GetPrefabType(target) != PrefabType.Prefab;
    }

    private static bool UndoRedoHappend()
    {
        return Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "UndoRedoPerformed";
    }
}
