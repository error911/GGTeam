//
// LocalizedGUITextureInspector.cs
//

namespace GGTools.SmartLocalization.Editor
{

using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor class for a localized GUITexture
/// </summary>
[CustomEditor(typeof(LocalizedGUITexture))]
public class LocalizedGUITextureInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedGUITexture textObject = ((LocalizedGUITexture)target);
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
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.TEXTURE);
		
		if(!Application.isPlaying && GUILayout.Button("������������ ����", GUILayout.Width(130)))
		{
			LocalizedGUITexture textObject = ((LocalizedGUITexture)target);
			textObject.localizedKey = selectedKey;
		}
	}
}
}
