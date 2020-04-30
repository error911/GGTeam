#if (!UNITY_IPHONE && !UNITY_IOS && !UNITY_ANDROID) || (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnsupportedPlatformAgent : IronSourceIAgent
{
	string mes = "ADS> Неподдерживаемая платформа. Выполните Build Settings -> Switch Platform на ANDROID, IPHONE или IOS";    //Unsupported Platform
	public UnsupportedPlatformAgent ()
	{
		Debug.Log (mes);
	}
	
	#region IronSourceAgent implementation

	public void start ()
	{
		Debug.Log(mes);
	}

	//******************* Base API *******************//

	public void onApplicationPause (bool pause)
	{
		Debug.Log(mes);
	}
	
	public void setAge (int age)
	{
		Debug.Log(mes);
	}
	
	public void setGender (string gender)
	{
		Debug.Log(mes);
	}

	public void setMediationSegment (string segment)
	{
		Debug.Log(mes);
	}

	public string getAdvertiserId ()
	{
		Debug.Log(mes); ;
		return "";
	}
	
	public void validateIntegration ()
	{
		Debug.Log(mes);
	}
	
	public void shouldTrackNetworkState (bool track)
	{
		Debug.Log(mes);
	}

	public bool setDynamicUserId (string dynamicUserId)
	{
		Debug.Log(mes);
		return false;
	}

	public void setAdaptersDebug(bool enabled)
	{
		Debug.Log(mes);
	}

    public void setMetaData(string key, string value)
    {
		Debug.Log(mes);
	}

    //******************* SDK Init *******************//

    public void setUserId (string userId)
	{
		Debug.Log(mes);
	}

	public void init (string appKey)
	{
		Debug.Log(mes);
	}

	public void init (string appKey, params string[] adUnits)
	{
		Debug.Log(mes);
	}

	public void initISDemandOnly (string appKey, params string[] adUnits)
	{
		Debug.Log(mes);
	}

	//******************* RewardedVideo API *******************//
	
	public void showRewardedVideo ()
	{
		Debug.Log(mes);
	}

	public void showRewardedVideo (string placementName)
	{
		Debug.Log(mes);
	}
	
	public bool isRewardedVideoAvailable ()
	{
		Debug.Log(mes);
		return false;
	}

	public bool isRewardedVideoPlacementCapped (string placementName)
	{
		Debug.Log(mes);
		return true;
	}

	public IronSourcePlacement getPlacementInfo (string placementName)
	{
		Debug.Log(mes);
		return null;
	}

	public void setRewardedVideoServerParams(Dictionary<string, string> parameters) 
	{
		Debug.Log(mes);
	}

	public void clearRewardedVideoServerParams() 
	{
		Debug.Log(mes);
	}

	//******************* RewardedVideo DemandOnly API *******************//

	public void showISDemandOnlyRewardedVideo (string instanceId) 
	{
		Debug.Log(mes);
	}

	public void loadISDemandOnlyRewardedVideo (string instanceId)
	{
		Debug.Log(mes);
	}

	public bool isISDemandOnlyRewardedVideoAvailable (string instanceId)
	{
		Debug.Log(mes);
		return false;
	}

	//******************* Interstitial API *******************//

	public void loadInterstitial ()
	{
		Debug.Log(mes);
	}

	public void showInterstitial ()
	{
		Debug.Log(mes);
	}

	public void showInterstitial (string placementName)
	{
		Debug.Log(mes);
	}

	public bool isInterstitialReady ()
	{
		Debug.Log(mes);
		return false;
	}

	public bool isInterstitialPlacementCapped (string placementName)
	{
		Debug.Log(mes);
		return true;
	}

	//******************* Interstitial DemandOnly API *******************//

	public void loadISDemandOnlyInterstitial (string instanceId)
	{
		Debug.Log(mes);
	}

	public void showISDemandOnlyInterstitial (string instanceId)
	{
		Debug.Log(mes);
	}

	public bool isISDemandOnlyInterstitialReady (string instanceId)
	{
		Debug.Log(mes);
		return false;
	}

	//******************* Offerwall API *******************//
	
	public void showOfferwall ()
	{
		Debug.Log(mes);
	}

	public void showOfferwall (string placementName)
	{
		Debug.Log(mes);
	}
	
	public void getOfferwallCredits ()
	{
		Debug.Log(mes);
	}

	public bool isOfferwallAvailable ()
	{
		Debug.Log(mes);
		return false;
	}

	//******************* Banner API *******************//

	public void loadBanner (IronSourceBannerSize size, IronSourceBannerPosition position)
	{
		Debug.Log(mes);
	}
	
	public void loadBanner (IronSourceBannerSize size, IronSourceBannerPosition position, string placementName)
	{
		Debug.Log(mes);
	}
	
	public void destroyBanner()
	{
		Debug.Log(mes);
	}

	public void displayBanner()
	{
		Debug.Log(mes);
	}

	public void hideBanner()
	{
		Debug.Log(mes);
	}
	
	public bool isBannerPlacementCapped(string placementName)
	{
		Debug.Log(mes);
		return false;
	}

	public void setSegment(IronSourceSegment segment)
	{
		Debug.Log(mes);
	}

	public void setConsent(bool consent)
	{
		Debug.Log(mes);
	}

		
	#endregion
}

#endif
