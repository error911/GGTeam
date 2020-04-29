// BulkUpdateWindow.cs
//
// Written by Niklas Borglund and Jakob Hillerström
//

namespace GGTools.SmartLocalization.Editor
{
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class BulkUpdateWindow : EditorWindow
{
	public enum BulkUpdateMethod
	{
		Import,
		Export,
	}

	static readonly string		exportFileName	= "Languages";
	static readonly string		csvFileEnding = ".csv";
	static readonly string		xlsFileEnding = ".xls";

	static readonly string[]	availableFileFormats = {".csv", ".xls"};

	[SerializeField]
	BulkUpdateMethod updateMedhod = BulkUpdateMethod.Export;
	CSVParser.Delimiter delimiter = CSVParser.Delimiter.COMMA;
	SmartLocalizationWindow parentWindow = null;
	int chosenFileFormat = 0;

	public void Initialize(BulkUpdateMethod updateMethod, SmartLocalizationWindow parentWindow)
	{
		this.updateMedhod = updateMethod;
		this.parentWindow = parentWindow;
	}

#region GUI Methods

	void OnGUI()
	{
		if(this.parentWindow == null)
		{
			this.Close();
		}

		if(LocalizationWindowUtility.ShouldShowWindow())
		{
			if(updateMedhod == BulkUpdateMethod.Import)
			{
				GUILayout.Label ("Импортировать все языки из одного файла", EditorStyles.boldLabel);
				ShowCommonGUI();
				ShowImportGUI();
			}
			else
			{
				GUILayout.Label ("Экспорт всех языков в один файл", EditorStyles.boldLabel);
				ShowCommonGUI();
				ShowExportGUI();
			}
		}
	}

	void ShowImportGUI()
	{
		if(GUILayout.Button("Импортировать"))
		{
			if(availableFileFormats[chosenFileFormat] == csvFileEnding)
			{
				string file = EditorUtility.OpenFilePanel("Выберите CSV файл.", "", "");
				if (file != null && file != "")
				{
					var values = CSVParser.Read(file, CSVParser.GetDelimiter(delimiter));
					if(values.Count > 0)
					{
						LanguageHandlerEditor.BulkUpdateLanguageFiles(values);
					}
				}
				this.Close();
			}
			else if(availableFileFormats[chosenFileFormat] == xlsFileEnding)
			{
				string file = EditorUtility.OpenFilePanel("Выберите XLS файл.", "", "");
				if (file != null && file != "")
				{
					var values = XLSExporter.Read(file);
					if(values.Count > 0)
					{
						LanguageHandlerEditor.BulkUpdateLanguageFiles(values);
					}
				}
				this.Close();
			}
			else
			{
				Debug.LogError("BulkUpdateWindow: Неподдерживаемый формат импорта!");
			}

			if(parentWindow.translateLanguageWindow != null)
			{
				parentWindow.translateLanguageWindow.ReloadLanguage();
			}
		}
	}

	void ShowExportGUI()
	{
		if(GUILayout.Button("Экспорт"))
		{
			string folderPath = EditorUtility.OpenFolderPanel("Выберите папку для сохранения.", "", "");
			if(availableFileFormats[chosenFileFormat] == csvFileEnding)
			{
				string fullPath = folderPath + "/" + exportFileName + csvFileEnding;
				CSVParser.Write(fullPath, CSVParser.GetDelimiter(delimiter),
					new List<string>(LanguageHandlerEditor.LoadLanguageFile(null, true).Keys), LanguageHandlerEditor.LoadAllLanguageFiles());

				Debug.Log("Экспортирован файл CSV в " + fullPath);
				this.Close();
			}
			else if(availableFileFormats[chosenFileFormat] == xlsFileEnding)
			{
				string fullPath = folderPath + "/" + exportFileName + xlsFileEnding;
				XLSExporter.Write(fullPath, "Languages",
					new List<string>(LanguageHandlerEditor.LoadLanguageFile(null, true).Keys), LanguageHandlerEditor.LoadAllLanguageFiles());

				Debug.Log("Экспортирован файл XLS в " + fullPath);
				this.Close();
			}
			else
			{
				Debug.LogError("BulkUpdateWindow: Неподдерживаемый формат экспорта!");
			}
		}
	}

	void ShowCommonGUI()
	{
		chosenFileFormat = EditorGUILayout.Popup("Формат файла", chosenFileFormat, availableFileFormats);

		if(availableFileFormats[chosenFileFormat] == csvFileEnding)
			ShowCSVOptions();
	}

	void ShowCSVOptions()
	{
		delimiter = (CSVParser.Delimiter)EditorGUILayout.EnumPopup("Разделитель", delimiter);
	}

#endregion

#region Show Windows
	public static BulkUpdateWindow ShowWindow(BulkUpdateMethod updateMethod, SmartLocalizationWindow parentWindow)
	{
		BulkUpdateWindow thisWindow = (BulkUpdateWindow)EditorWindow.GetWindow<BulkUpdateWindow>("Обновить языки"); //Update Languages
			thisWindow.Initialize(updateMethod, parentWindow);
		
		return thisWindow;
	}
#endregion
}
}