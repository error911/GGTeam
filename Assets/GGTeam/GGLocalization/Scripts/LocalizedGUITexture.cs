//
// LocalizedGUITexture.cs
//

namespace GGTools.SmartLocalization
{
	using UnityEngine;
	using System.Collections;

	public class LocalizedGUITexture : MonoBehaviour 
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

#pragma warning disable CS0618 // “ип или член устарел
			GetComponent<GUITexture>().texture = LanguageManager.Instance.GetTexture(localizedKey);
#pragma warning restore CS0618 // “ип или член устарел
		}
	}
}