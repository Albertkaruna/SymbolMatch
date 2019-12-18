using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	public AudioSource SoundEffect;
	public Sprite Sound1, Sound2;
	public AudioClip One, Two;
	public Text ScoreTxt, NewScoreTxt, HighScoreTxt1, HighScoreTxt2, DiamondScore;
	public GameObject[] Things;
	public RectTransform ScrollContent;
	public Sprite[] GroupIcons;
	public GameObject DiamondEff1, DiamondEff2, UnlockEff, VideoRewardEffect;
	public GameObject AdButton, SplashScreen;
	public Animator ExitAnim, MainMenuAnim;
	public Image SoundImage;
	[HideInInspector]
	public int Score, HScorePP, HScoreTemp, VidAdCount, ExitPressCount, InterstitialAdCount;
	private bool s, paused, Once1, Once2;
	private PlayerController Player;
	public Button PlayButton;
	public static int PermScore, Unlocked;
	private AudioListener audioListener;
	public Color[] clr;

	void Awake ()
	{
//		PlayerPrefs.DeleteAll ();
		PermScore = PlayerPrefs.GetInt ("PermScore");
		HScorePP = PlayerPrefs.GetInt ("HScore");
		Camera.main.backgroundColor = clr [Random.Range (0, 4)];
	}


	// Use this for initialization
	void Start ()
	{
		SplashScreen.SetActive (true);
		Invoke ("CloseSplash", 3f);
		s = true;
		paused = false;
		Score = 0;
		VidAdCount = 0;
		InterstitialAdCount = 0;
		Once1 = true;
		Once2 = true;
		HighScoreTxt1.text = HScorePP.ToString ();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		UnlockSets ();
		DiamondScore.text = PermScore.ToString ();
		audioListener = Camera.main.GetComponent<AudioListener> ();

		MusicSound ();
	}

	void CloseSplash ()
	{
		SplashScreen.SetActive (false);
		MainMenuAnim.enabled = true;
	}



	void MusicSound ()
	{
		if (PlayerPrefs.GetInt ("Sound") == 1) {
			SoundEffect.enabled = false;
			s = false;
			SoundImage.sprite = Sound2;
		}
	}

	// Grant user with the reward
	//			Destroy(Instantiate(MenuControl.VideoRewardEffect), 3.5f);
	//			MenuControl.IncreaseDiamond();
	//


	public void IncreaseDiamond ()
	{
		Invoke ("IncreaseNow", 3f);
	}

	void IncreaseNow ()
	{

		PlayerPrefs.SetInt ("PermScore", PermScore + 15);
		PermScore = PlayerPrefs.GetInt ("PermScore");
		DiamondScore.text = PermScore.ToString ();
	}

	// Update is called once per frame
	void Update ()
	{
		DiamondScore.text = PermScore.ToString ();
		if (Score > 9 && Score % 10 == 0 && Once1) { 
			Once1 = false;
			PlayerPrefs.SetInt ("PermScore", PermScore++);
			//Effect goes here
			Destroy (Instantiate (DiamondEff1, new Vector2 (2.1f, 2.5f), Quaternion.identity), 0.7f);
		}

		if (Score > 29 && Score % 30 == 0 && Once2) {
			Once2 = false;
			PlayerPrefs.SetInt ("PermScore", PermScore += 3);
			Destroy (Instantiate (DiamondEff2, new Vector2 (2.1f, 2.5f), Quaternion.identity), 0.7f);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ExitPressCount++;
			if (ExitPressCount >= 2) {
				Application.Quit ();
			}
			ExitAnim.SetBool ("exit", true);
			Invoke ("HideExitText", 2);
		}
	}

	void HideExitText ()
	{
		ExitPressCount = 0;
		ExitAnim.SetBool ("exit", false);
	}

	public void TrueOnce ()
	{
		if (Score % 10 == 1) {
			Once1 = true;
			Once2 = true;
		}
	}

	public void UnlockEffect ()
	{
		// Effect hoes here
		Destroy (Instantiate (UnlockEff), 6f);
		Player.CanInvoke = false;
		PlayerPrefs.SetInt ("Unlock", Unlocked += 1);
		UnlockSets ();
	}



	public void SoundBtn (Image b)
	{
//		PlayButtonClickSnd ();
		SoundEffect.enabled = !SoundEffect.enabled;
		if (s) {
			s = false;
			b.sprite = Sound2;
			PlayerPrefs.SetInt ("Sound", 1);
		} else if (!s) {
			s = true;
			b.sprite = Sound1;
			PlayerPrefs.SetInt ("Sound", 0);
		}
	}


	void UnlockSets ()
	{
		Unlocked = PlayerPrefs.GetInt ("Unlock");
		if (Unlocked == 1) {
			// First set unlocked and pause the game and show the effect
			ScrollContent.GetChild (1).GetComponent<Image> ().sprite = GroupIcons [0];
			Player.SymCount = 13;
		} else if (Unlocked == 2) {
			// Second set unlocked and pause the game and show the effect
			ScrollContent.GetChild (1).GetComponent<Image> ().sprite = GroupIcons [0];
			ScrollContent.GetChild (2).GetComponent<Image> ().sprite = GroupIcons [1];
			Player.SymCount = 18;
		} else if (Unlocked == 3) {
			// Third set unlocked and pause the game and show the effect
			ScrollContent.GetChild (1).GetComponent<Image> ().sprite = GroupIcons [0];
			ScrollContent.GetChild (2).GetComponent<Image> ().sprite = GroupIcons [1];
			ScrollContent.GetChild (3).GetComponent<Image> ().sprite = GroupIcons [2];
			Player.SymCount = 23;
		} else if (Unlocked == 4) {
			// Fourth set unlocked and pause the game and show the effect
			ScrollContent.GetChild (1).GetComponent<Image> ().sprite = GroupIcons [0];
			ScrollContent.GetChild (2).GetComponent<Image> ().sprite = GroupIcons [1];
			ScrollContent.GetChild (3).GetComponent<Image> ().sprite = GroupIcons [2];
			ScrollContent.GetChild (4).GetComponent<Image> ().sprite = GroupIcons [3];
			Player.SymCount = 28;
		} else if (Unlocked == 5) {
			// Fifth set unlocked and pause the game and show the effect
			ScrollContent.GetChild (1).GetComponent<Image> ().sprite = GroupIcons [0];
			ScrollContent.GetChild (2).GetComponent<Image> ().sprite = GroupIcons [1];
			ScrollContent.GetChild (3).GetComponent<Image> ().sprite = GroupIcons [2];
			ScrollContent.GetChild (4).GetComponent<Image> ().sprite = GroupIcons [3];
			ScrollContent.GetChild (5).GetComponent<Image> ().sprite = GroupIcons [4];
			Player.SymCount = 33;
		}
	}

	public void Restart ()
	{
		HScorePP = PlayerPrefs.GetInt ("HScore");
		HighScoreTxt1.text = HScorePP.ToString ();
		Score = 0;
		ScoreTxt.text = "0";
		if (Things [4].activeSelf)
			Things [4].GetComponent<Animator> ().SetBool ("restart", true);
	}

	IEnumerator GetScene (int l)
	{
		yield return new WaitForSeconds (0.5f);
		SceneManager.LoadSceneAsync (l);
	}


	public void Twitter ()
	{
		Application.OpenURL ("https://twitter.com/hyper_zeta");
	}

	public void Facebook ()
	{
		Application.OpenURL ("https://www.facebook.com/HyperZeta-522463037937722");
	}


	public void Play (Button b)
	{
		b.interactable = false;
		PlayButton = b;
		Things [0].GetComponent<Animator> ().SetBool ("mainmenu", true);
	}

	public void Sound ()
	{

	}

	public void Share ()
	{

	}

	public void SymbolGroupPanel (GameObject GroupPanel)
	{
		GroupPanel.GetComponent<Animator> ().SetBool ("scroll", true);
	}

	public void SymbolGroupExit (GameObject GroupPanel)
	{
		GroupPanel.GetComponent<Animator> ().SetBool ("scroll", false);
	}

	public void CallWaitAndDo (int w)
	{
		StartCoroutine (WaitAndDo (w));
	}

	IEnumerator WaitAndDo (int k)
	{
		if (k == 0) {
			Things [0].SetActive (false);
			yield return new WaitForSeconds (0.5f);
			Player.CanInvoke = true;
			Player.SymbolsGenerator ();
		} else if (k == 1) {
			PlayButton.interactable = true;
			Things [4].SetActive (false);
			Things [0].SetActive (true);
			Things [0].GetComponent<Animator> ().SetBool ("mainmenu", false);
		}
	}

	public void ShowRewardedADUI ()
	{
		HeyZapAdsController.AdChecker = true;
		HeyZapAdsController.instance.ShowIncentivezedAd ();
	}

}
