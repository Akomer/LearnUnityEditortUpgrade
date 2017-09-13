﻿using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : PropertyDrawer
{
    const string propertyNamePosition = "position";
    const string propertyNameColor = "color";

    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        var oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);
        var contentPosition = EditorGUI.PrefixLabel(position, label);
        if (position.height > 16f)
        {
            position.height = 16f;
            EditorGUI.indentLevel += 1;
            contentPosition = EditorGUI.IndentedRect(position);
            contentPosition.y += 18;
        }
        contentPosition.width *= 0.75f;
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative(propertyNamePosition), GUIContent.none);
        contentPosition.x += contentPosition.width;
        contentPosition.width /= 3;
        var colorLabel = new GUIContent("C");
        EditorGUIUtility.labelWidth = 14f;
        EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative(propertyNameColor), colorLabel);
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = oldIndentLevel;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (label != GUIContent.none && Screen.width < 333) ? (16f + 18f) : 16f;
    }

}
