//
// LocalizedGUITexture.cs
//

namespace GGTools.SmartLocalization
{
	using UnityEngine;
	using System.Collections;
    using UnityEngine.UI;

	[RequireComponent(typeof(Image))]
	public class LocalizedImage : MonoBehaviour 
	{
		public string localizedKey = "INSERT_KEY_HERE";

		Image image = null;
	
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
			if (image == null) image = GetComponent<Image>();
			image.material.mainTexture = LanguageManager.Instance.GetTexture(localizedKey);
			//image.overrideSprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height, new Vector2(0.5f, 0.5f));

		}
	}
}