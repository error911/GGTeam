//
//  EditorWindowUtility.cs
//

namespace GGTools.SmartLocalization.Editor
{

using UnityEngine;
using UnityEditor;

/// <summary>The different search types for a language</summary>
public enum LanguageSearchType
{
	/// <summary>Search by key</summary>
	KEY,
	/// <summary>Search by value</summary>
	VALUE,
}

/// <summary>The different sort types for a language</summary>
public enum LanguageSortType
{
	/// <summary>Sort by keys</summary>
	KEY,
	/// <summary> Sort by values </summary>
	VALUE,
	/// <summary> Sort by type</summary>
	TYPE,
}

/// <summary>
/// A Utility class with helper methods for editor windows using Smart Localization
/// </summary>
public static class LocalizationWindowUtility
{
	/// <summary>
	/// Returns true if the window should show, returns false if not.
	/// If false, it will draw an editor label that says the window is unavailable
	/// </summary>
	public static bool ShouldShowWindow(bool isAvailableInPlayMode = false)
	{
		if(Application.isPlaying && !isAvailableInPlayMode)
		{
			GUILayout.Label ("Окно Smart Localization недоступно в режиме воспроизведения", EditorStyles.boldLabel);
			if(LanguageManager.HasInstance)
			{
				if(GUILayout.Button("Перейти к окну перевода"))
				{
					TranslateLanguageWindow.ShowWindow(LanguageManager.Instance.GetCultureInfo(LanguageManager.Instance.CurrentlyLoadedCulture.languageCode), null);
				}
			}
			return false;
		}
		else if(!LocalizationWorkspace.Exists())
		{
			GUILayout.Label ("В проекте нет рабочей области локализации", EditorStyles.boldLabel);
			if(GUILayout.Button("Создать рабочую область локализации"))
			{
				if(LocalizationWorkspace.Create())
				{
					return true;
				}
			}

			return false;
		}
		else
		{
			return true;
		}
	}
}
} //namespace SmartLocalization.Editor