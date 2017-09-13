using UnityEditor;
using System;
using UnityEngine;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLables = 4,
    Buttons = 8,
    Default = ListSize | ListLabel | ElementLables,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}

public static class EditorList
{
    private static GUIContent moveButtonContent = new GUIContent("\u21b4", "move down");
    private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
    private static GUIContent deleteButtonContent = new GUIContent("-", "remove");
    private static GUIContent addButtonContent = new GUIContent("+", "add elemenet");
    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is not array / list", MessageType.Error);
            return;
        }

        var showListLabel = (options & EditorListOption.ListLabel) != 0;
        var showListSize = (options & EditorListOption.ListSize) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            }
            if (size.hasMultipleDifferentValues)
            {
                EditorGUILayout.HelpBox("Not showing lists with diefferent sizes", MessageType.Info);
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    private static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        var showElementLabel = (options & EditorListOption.ElementLables) != 0;
        var showButtons = (options & EditorListOption.Buttons) != 0;

        for (var i = 0; i < list.arraySize; i++)
        {
            if (showButtons)
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabel)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
            else
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
            }
            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }

        }
        if (showButtons && list.arraySize == 0)
        {
            if (GUILayout.Button(addButtonContent))
            {
                list.arraySize += 1;
            }
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, miniButtonWidth))
        {
            var sizeBeforDelete = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == sizeBeforDelete)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}
