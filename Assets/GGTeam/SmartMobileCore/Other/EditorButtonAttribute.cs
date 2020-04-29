using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGTeam.SmartMobileCore
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorButtonAttribute : Attribute
    {
        /// <summary>
        /// Button text
        /// </summary>
        public string text;

        /// <summary>
        /// Add button to Inspector
        /// </summary>
        /// <param name="text">Button text</param>
        public EditorButtonAttribute(string text)
        {
            this.text = text;
        }
    }
}
