//
// LocalizedAudioSourceInspector.cs
//

namespace GGTools.SmartLocalization.Editor
{

using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor class for a localized Audio Source
/// </summary>
[CustomEditor(typeof(LocalizedAudioSource))]
public class LocalizedAudioSourceInspector : Editor 
{
	private string selectedKey = null;
	
	void Awake()
	{
		LocalizedAudioSource audioObject = ((LocalizedAudioSource)target);
		if(audioObject != null)
		{
			selectedKey = audioObject.localizedKey;
		}
	}
	
	/// <summary>
	/// Override of the OnInspectorGUI method
	/// </summary>
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		
		selectedKey = LocalizedKeySelector.SelectKeyGUI(selectedKey, true, LocalizedObjectType.AUDIO);
		
		if(!Application.isPlaying && GUILayout.Button("������������ ����", GUILayout.Width(130)))
		{
			LocalizedAudioSource audioObject = ((LocalizedAudioSource)target);			
			audioObject.localizedKey = selectedKey;
		}
	}
}
}
