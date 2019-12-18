using UnityEngine;
using System.Collections;
using Heyzap;

public class HeyZapAdsController : MonoBehaviour
{
	public static HeyZapAdsController instance;

	private MenuController menuConrtoller;
	private string HeyZapPublisherID = "c0d047674442aff18af093539748d8ae";
	public static bool AdChecker;

	void Awake ()
	{
		AdChecker = false;
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		menuConrtoller = GameObject.Find ("EventSystem").GetComponent<MenuController> ();
		HeyzapAds.Start (HeyZapPublisherID, HeyzapAds.FLAG_NO_OPTIONS);
	}

	// Use this for initialization
	void Start ()
	{
		HZIncentivizedAd.Fetch ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void ShowInterstitialAd ()
	{
		if (HZInterstitialAd.IsAvailable ()) {
			HZInterstitialAd.Show ();
		} else {
			UnityAdsController.instance.ShowUnityinterstitialAd ();
		}
	}

	public void ShowIncentivezedAd ()
	{
		if (HZIncentivizedAd.IsAvailable () && AdChecker) {
			AdChecker = false;
			HZIncentivizedAd.Show ();
		} else if (AdChecker) {
			AdChecker = false;
			HZIncentivizedAd.Fetch ();
			UnityAdsController.instance.ShowUnityRewardedAd ();
		}
		ShowIncentiveAds ();
	}

	private void ShowIncentiveAds ()
	{
		HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag) {

			if (adState.Equals ("fetch_failed")) {
				//TELL USER TO CHECK CONNECTION
				HZIncentivizedAd.Fetch ();
			}
			if (adState.Equals ("incentivized_result_complete")) {
				// Give reward to the player
				RewardToPlayer ();
				HZIncentivizedAd.Fetch ();
			}
			if (adState.Equals ("incentivized_result_incomplete")) {
				// The user did not watch the entire video and should not be given a reward.
				HZIncentivizedAd.Fetch ();
			}
		};

		HZIncentivizedAd.SetDisplayListener (listener);
	}


	public void RewardToPlayer ()
	{
		menuConrtoller.Things [4].SetActive (false);
		Destroy (Instantiate (menuConrtoller.VideoRewardEffect), 3.5f);
		Invoke ("GetStartPanel", 3.5f);
		menuConrtoller.IncreaseDiamond ();
	}

	private void GetStartPanel ()
	{
		menuConrtoller.Restart ();   
		menuConrtoller.PlayButton.interactable = true;
		menuConrtoller.Things [0].SetActive (true);
		menuConrtoller.Things [0].GetComponent<Animator> ().SetBool ("mainmenu", false);
	}
}
