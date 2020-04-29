using UnityEngine;
using System.Collections;

namespace GGTools.SmartLocalization
{
	internal interface ILocalizedAssetLoader {
		T LoadAsset<T>(string assetKey, string languageCode) where T : UnityEngine.Object;
	}
}
