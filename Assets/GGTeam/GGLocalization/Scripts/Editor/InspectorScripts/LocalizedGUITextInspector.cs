//
// LocalizedGUITextInspector.cs
//

namespace GGTools.SmartLocalization.Editor
{

using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor class for a localized GUIText
/// </summary>
[CustomEditor(typeof(LocalizedGUIText))]
public class LocalizedGUITextInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedGUIText textObject = ((LocalizedGUIText)target);
		if(textObject != null)
		{
			selectedKey = textObject.localizedKey;
		}
	}
	
	/// <summary>
	/// Override of the OnInspectorGUI method
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.STRING);
		
		if(!Application.isPlaying && GUILayout.Button("������������ ����", GUILayout.Width(130)))
		{
			LocalizedGUIText textObject = ((LocalizedGUIText)target);		
			textObject.localizedKey = selectedKey;
		}
	}
}
}
