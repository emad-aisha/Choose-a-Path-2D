// TODO: find a way to make custon GUI to make this easily useable
// later....
/*
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ColorManager {

    static ColorManager() {
        EditorApplication.hierarchyWindowItemByEntityIdOnGUI += Func;
    }

    static private void Func(EntityId entityId, Rect selectionRect) {
        Object obj = EditorUtility.EntityIdToObject(entityId);
        if (obj == null) return;

        Color backgroundColor = Color.white;
        Color textColor = Color.red;

        if (obj.name == "Main Camera") {
            backgroundColor = Color.red;
            textColor = Color.white;
        }


        if (backgroundColor != Color.white) {
            EditorGUI.DrawRect(selectionRect, backgroundColor);
        }

        if (textColor != Color.red) {
            GUIStyle test = new() {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() { textColor = textColor }
            };

            EditorGUI.LabelField(selectionRect, obj.name, test);
        }
    }

}

*/
