// GGTeam
// LocalizedText.cs
//

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace GGTools.SmartLocalization
{
	[RequireComponent(typeof(Text))]
	public class LocalizedText : MonoBehaviour 
	{
		public string localizedKey = "INSERT_KEY_HERE";

		Text text = null;

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
			if (text == null) text = GetComponent<Text>();
			text.text = LanguageManager.Instance.GetTextValue(localizedKey);
		}
	}
}
