// ================================
// Free license: CC BY Murnik Roman
// Module
// ================================

#if UNITY_PURCHASING
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GGTeam.SmartMobileCore.Modules.IAP
{
#if UNITY_EDITOR
    [CustomEditor(typeof(IAPModule))]
    [CanEditMultipleObjects]
    public class IAPModuleEditor : Editor
    {
        private static readonly string[] excludedFields = new string[] { "m_Script" };
        private static readonly string[] restoreExcludedFields = new string[] { "m_Script", "removeAppCaption", "onPurchaseComplete", "onPurchaseFailed", "titleText", "descriptionText", "priceText" };

        private const string kNoProduct = "<Нет>";

        private List<string> m_ValidIDs = new List<string>();
        private SerializedProperty m_ProductIDProperty;
        private SerializedProperty m_AppCaption;

        public void OnEnable()
        {
            m_ProductIDProperty = serializedObject.FindProperty("noAdsProductId");
            m_AppCaption = serializedObject.FindProperty("removeAppCaption");
        }

        public override void OnInspectorGUI()
        {
            IAPModule iap = (IAPModule)target;
            var catalog = iap.allProducts;

            if (catalog.Count > 0)
            {
                EditorGUILayout.LabelField(new GUIContent("NoAds ID:", "Select a product from the IAP catalog"));

                m_ValidIDs.Clear();
                m_ValidIDs.Add(kNoProduct);

                foreach (var product in catalog)
                {
                    m_ValidIDs.Add(product.id);
                }

                int currentIndex = string.IsNullOrEmpty(iap.noAdsProductId) ? 0 : m_ValidIDs.IndexOf(iap.noAdsProductId);
                int newIndex = EditorGUILayout.Popup(currentIndex, m_ValidIDs.ToArray());
                if (newIndex > 0 && newIndex < m_ValidIDs.Count)
                {
                    m_ProductIDProperty.stringValue = m_ValidIDs[newIndex];
                }
                else
                {
                    m_ProductIDProperty.stringValue = string.Empty;
                }
                EditorGUILayout.HelpBox(new GUIContent("Id продукта, который отключает рекламу"));
                EditorGUILayout.Space();

                EditorGUILayout.PropertyField(m_AppCaption);
                EditorGUILayout.HelpBox(new GUIContent("Корректировка заголовка, для правильного отображения названия продукта. Product(GameName)->Product"));
                EditorGUILayout.Space();

                EditorGUILayout.LabelField(new GUIContent("Каталог продуктов:"));

            }
            else
            {
                EditorGUILayout.HelpBox(new GUIContent("Добавьте все продукты из магазинов GooglePlay и AppStore"));
            }

            DrawPropertiesExcluding(serializedObject, catalog.Count == 0 ? restoreExcludedFields : excludedFields);

            serializedObject.ApplyModifiedProperties();

        }


    }
#endif
}
#endif