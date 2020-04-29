//
// LocalizedGUIText.cs
//

using UnityEngine;
using System.Collections;

namespace GGTools.SmartLocalization
{
	public class LocalizedGUIText : MonoBehaviour 
	{
		public string localizedKey = "INSERT_KEY_HERE";
	
		void Start () 
		{
			//Subscribe to the change language event
			LanguageManager languageManager = LanguageManager.Instance;
			languageManager.OnChangeLanguage += OnChangeLanguage;
		
			//Run the method one first time
			OnChangeLanguage(languageManager);
		}

		void OnDestroy()
		{
			if(LanguageManager.HasInstance)
			{
				LanguageManager.Instance.OnChangeLanguage -= OnChangeLanguage;
			}
		}
	
		void OnChangeLanguage(LanguageManager languageManager)
		{
			//Initialize all your language specific variables here

#pragma warning disable CS0618 // Тип или член устарел
			GetComponent<GUIText>().text = LanguageManager.Instance.GetTextValue(localizedKey);
#pragma warning restore CS0618 // Тип или член устарел
		}
	}
}
