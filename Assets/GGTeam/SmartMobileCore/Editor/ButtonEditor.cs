using System;
using System.Collections;
using System.Collections.Generic;
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
//            Texture2D tex = new Texture2D(128, 24);
//            GUIStyle style = new GUIStyle();
            //GUI.backgroundColor = Color.yellow;
//            style.normal.background = tex;
//            style.alignment = TextAnchor.MiddleCenter;


            var buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.margin = new RectOffset(buttonStyle.margin.right + 160, 0, buttonStyle.margin.top, buttonStyle.margin.bottom);


            foreach (var target in targets)
            {
                var items = target.GetType().GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == typeof(EditorButtonAttribute)));
                //if (items == null) continue;
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        if (item == null) continue;
                        var attribute = item.GetCustomAttribute<EditorButtonAttribute>();   //(EditorButtonAttribute)

                        //if (GUILayout.Button(attribute.text)) item.Invoke(target, null);
                        //if (GUILayout.Button(attribute.text, style)) item.Invoke(target, null);
                        if (GUILayout.Button(attribute.text, buttonStyle)) item.Invoke(target, null);
                    }
                }
            }
            base.OnInspectorGUI();
        }
    }
}

