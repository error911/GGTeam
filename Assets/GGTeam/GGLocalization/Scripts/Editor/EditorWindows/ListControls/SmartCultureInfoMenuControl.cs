
namespace GGTools.SmartLocalization.Editor
{
using UnityEngine;
using System.Collections;
using GGTools.SmartLocalization.ReorderableList;
using UnityEditor;

internal class SmartCultureInfoMenuControl : ReorderableListControl  
{
	static GUIContent commandTranslate = new GUIContent("Translate");
	static GUIContent commandUpdate = new GUIContent("Update");
	static GUIContent commandExport = new GUIContent("Export");

	public SmartCultureInfoMenuControl() : base(ReorderableListFlags.HideAddButton | ReorderableListFlags.DisableContextMenu){}

	//Nothing in here is used ATM, the context menu is disabled
	protected override void AddItemsToMenu(GenericMenu menu, int itemIndex, IReorderableListAdaptor adaptor) 
	{
		menu.AddItem(commandTranslate, false, defaultContextHandler, commandTranslate);
		menu.AddItem(commandUpdate, false, defaultContextHandler, commandUpdate);
		menu.AddItem(commandExport, false, defaultContextHandler, commandExport);
	}

	protected override bool HandleCommand(string commandName, int itemIndex,IReorderableListAdaptor adaptor) 
	{
		SmartCultureInfoListAdaptor smartAdaptor = adaptor as SmartCultureInfoListAdaptor;

		if(smartAdaptor == null)
			return false;

		switch (commandName) 
		{
			case "Translate":
				OnTranslateClick(smartAdaptor.GetCultureInfo(itemIndex));
				return true;
			case "Update":
				OnUpdateClick(smartAdaptor.GetCultureInfo(itemIndex));
				return true;
			case "Export":
				OnExportClick(smartAdaptor.GetCultureInfo(itemIndex));
				return true;
		}

		return false;
	}

	protected override void OnItemInserted(ItemInsertedEventArgs args)
	{
		//base.OnItemInserted(args);
	}

	protected override void OnItemRemoving(ItemRemovingEventArgs args)
	{
		SmartCultureInfoListAdaptor smartAdaptor = args.adaptor as SmartCultureInfoListAdaptor;

		if(smartAdaptor == null)
		{
			return;
		}

		SmartCultureInfo info = smartAdaptor.GetCultureInfo(args.itemIndex);

		if(EditorUtility.DisplayDialog("Удалить " + info.englishName + "?",
			"Вы уверены, что хотите удалить " + info.englishName + " и весь его контент из проекта? Вы не сможете отменить это действие.",
			"Да, удали это.", "Отмена"))
		{
			LanguageHandlerEditor.DeleteLanguage(info);
			base.OnItemRemoving(args);
		}
		else
		{
			args.Cancel = true;
		}
	}

	public void OnTranslateClick(SmartCultureInfo info)
	{
		Debug.Log("Translate: " + info.englishName);
	}

	public void OnUpdateClick(SmartCultureInfo info)
	{
		Debug.Log("Update: " + info.englishName);
	}

	public void OnExportClick(SmartCultureInfo info)
	{
		Debug.Log("Export: " + info.englishName);
	}
}
}
