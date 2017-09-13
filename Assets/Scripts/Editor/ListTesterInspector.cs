using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ListTester)), CanEditMultipleObjects]
public class ListTesterInspector : Editor {

    private const string propertyNameIntegers = "integers";
    private const string propertyNameVectors = "vectors";
    private const string propertyNameColorPoints = "colorPoints";
    private const string propertyNameObjects = "objects";
    private const string propertyNameNotAList = "notAList";
    

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorList.Show(serializedObject.FindProperty(propertyNameIntegers), EditorListOption.All);
        EditorList.Show(serializedObject.FindProperty(propertyNameVectors));
        EditorList.Show(serializedObject.FindProperty(propertyNameColorPoints), EditorListOption.All);
        EditorList.Show(serializedObject.FindProperty(propertyNameObjects), EditorListOption.All);
        EditorList.Show(serializedObject.FindProperty(propertyNameNotAList), EditorListOption.All);

        serializedObject.ApplyModifiedProperties();
    }
}
