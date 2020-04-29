// CreateLanguageWindow.cs
//

namespace GGTools.SmartLocalization.Editor
{
using UnityEditor;
using UnityEngine;

public class CreateLanguageWindow : EditorWindow
{
	[SerializeField]
	string englishName  = null;
	[SerializeField]
	string languageCode = null;
	[SerializeField]
	string nativeName	= null;
	[SerializeField]
	bool isRightToLeft	= false;

	bool	showHelpMessage = false;
	string  helpMessage		= null;
	MessageType helpMessageType = MessageType.Info;
	SmartLocalizationWindow parentWindow = null;

	void OnGUI()
	{
		if(LocalizationWindowUtility.ShouldShowWindow())
		{
			GUILayout.Label ("Создать новую информацию о культуре", EditorStyles.boldLabel);

			languageCode = EditorGUILayout.TextField("Код языка", languageCode);
			if(languageCode != null)
				languageCode = languageCode.RemoveWhitespace();

			englishName = EditorGUILayout.TextField("Английское имя", englishName);
			nativeName = EditorGUILayout.TextField("Родное имя", nativeName);
			isRightToLeft = EditorGUILayout.Toggle("Справа налево", isRightToLeft);

			if(GUILayout.Button("Создать"))
			{
				SmartCultureInfo newInfo = new SmartCultureInfo();
				newInfo.languageCode = languageCode;
				newInfo.englishName = englishName.Trim();
				newInfo.nativeName = nativeName.Trim();
				newInfo.isRightToLeft = isRightToLeft;

				SmartCultureInfoCollection allCultures = SmartCultureInfoEx.Deserialize(LocalizationWorkspace.CultureInfoCollectionFilePath());
				if(!allCultures.IsCultureInCollection(newInfo))
				{
					allCultures.AddCultureInfo(newInfo);
					allCultures.Serialize(LocalizationWorkspace.CultureInfoCollectionFilePath());
					LanguageHandlerEditor.CheckAndSaveAvailableLanguages(allCultures);

					showHelpMessage = true;
					helpMessageType = MessageType.Info;
					helpMessage = string.Format("Язык успешно создан!\n Код языка: {0}\n Английское имя:{1}\n Родное имя:{2}\n Справа налево:{3}",
												newInfo.languageCode, newInfo.englishName, newInfo.nativeName, newInfo.isRightToLeft);
												
					if(parentWindow != null)
					{
						parentWindow.InitializeCultureCollections(true);
					}
					
					this.Close();
				}
				else
				{
					SmartCultureInfo conflictingCulture = allCultures.FindCulture(newInfo);
					string conflictingVariable = null;

					if(conflictingCulture.languageCode.ToLower() == newInfo.languageCode.ToLower())
					{
						conflictingVariable = "Language Code:" + newInfo.languageCode;
					}
					else if(conflictingCulture.englishName.ToLower() == newInfo.englishName.ToLower())
					{
						conflictingVariable = "English Name:" + newInfo.englishName;
					}

					showHelpMessage = true;
					helpMessageType = MessageType.Error;
					helpMessage = string.Format("Не удалось создать язык!\n Конфликтующая переменная - {0}\n\n",
												conflictingVariable);

					helpMessage += string.Format("Конфликтующая культура \n Код языка: {0}\n Английское имя:{1}\n Родное имя:{2}",
												conflictingCulture.languageCode, conflictingCulture.englishName, conflictingCulture.nativeName);
				}
			}

			if(showHelpMessage)
			{
				EditorGUILayout.HelpBox(helpMessage, helpMessageType);
			}
		}
	}


#region Show Window
	public static CreateLanguageWindow ShowWindow(SmartLocalizationWindow parentWindow)
	{
		CreateLanguageWindow thisWindow = (CreateLanguageWindow)EditorWindow.GetWindow<CreateLanguageWindow>("Новый язык"); //New Language
			thisWindow.parentWindow = parentWindow;
		return thisWindow;
	}
#endregion
	}
}