#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public static class TransformUtils
{
    [MenuItem("GameObject/Reset Transform", false, 100)] // GameObject
    [MenuItem("CONTEXT/Transform/Reset Transform", false, 10)] // Component
    [MenuItem("Tools/Reset Selected Transforms ^&r", false)] // Menu Item (Ctrl + Alt + R)
    public static void ResetTransform()
    {
        const int MAX_NAMES_IN_UNDO = 3;

        Transform[] transforms = Selection.transforms;

        if (transforms.Length == 0)
        {
            Debug.Log("No active transforms selected.");
            return;
        }

        StringBuilder undoLog = new();
        IEnumerable<string> displayedNames = transforms.Take(MAX_NAMES_IN_UNDO).Select(t => t.name);
        undoLog.AppendJoin(", ", displayedNames);

        if (transforms.Length > MAX_NAMES_IN_UNDO)
        {
            undoLog.Append($", and {transforms.Length - MAX_NAMES_IN_UNDO} more.");
        }

        Undo.RegisterCompleteObjectUndo(transforms, $"Reset Transform: {undoLog}");

        foreach (Transform tr in transforms)
        {
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }
    }
}

#endif