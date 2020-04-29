// ================================
// Free license: CC BY Murnik Roman
// ================================

using System;
using System.Collections;
// ================================
// Free license: CC BY Murnik Roman
// ================================

using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    [CustomEditor(typeof(object), true, isFallback = false)]
    [CanEditMultipleObjects]
    public class ButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = new RectOffset(buttonStyle.margin.right + 160, 0, buttonStyle.margin.top, buttonStyle.margin.bottom);

            foreach (var target in targets)
            {
                var items = target.GetType().GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == typeof(EditorButtonAttribute)));
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (item == null) continue;
                        var attribute = item.GetCustomAttribute<EditorButtonAttribute>();   //(EditorButtonAttribute)
                        if (GUILayout.Button(attribute.text, buttonStyle)) item.Invoke(target, null);
                    }
                }
            }
            base.OnInspectorGUI();
        }
    }
}

