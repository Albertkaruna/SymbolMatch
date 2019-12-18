using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq.Expressions;

public class PlayerController : MonoBehaviour
{
	public Transform[] Gens;
	public Transform[] X, Y;
	public GameObject Eff1, Eff2;
	private Vector2 fp, lp;
	private float dragDistance = Screen.height * 0.05f;
	[HideInInspector]
	public bool CanInvoke;
	private bool CanProceed;
	private int[] arr;
	private MenuController MenuC;
	private float MovingTimeX, MovingTimeY;
	private bool IsSwiped;
	[HideInInspector]
	public int SymCount, AchievementsCount;

	void Awake ()
	{

		MenuC = GameObject.Find ("EventSystem").GetComponent<MenuController> ();
		AchievementsCount = 0;
	}

	// Use this for initialization
	void Start ()
	{
		IsSwiped = false;
		MovingTimeX = 2.2f;
		MovingTimeY = 2f;
		CanInvoke = true;
		SymCount = 8;
		if (PlayerPrefs.HasKey ("Achievements"))
			AchievementsCount = PlayerPrefs.GetInt ("Achievements");

		transform.position = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			fp = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			lp = Input.mousePosition;
			float xDistance = Mathf.Abs (lp.x - fp.x);
			float yDistance = Mathf.Abs (lp.y - fp.y);

			if (xDistance > dragDistance || yDistance > dragDistance) {
				IsSwiped = true;
				Invoke ("SwipeEnd", 0.3f);
				if (xDistance > yDistance && CanInvoke) {
					if (lp.x > fp.x) {
						iTween.MoveTo (gameObject, iTween.Hash ("x", 6f, "easetype", iTween.EaseType.linear, "time", 0.15f));
						iTween.MoveTo (gameObject, iTween.Hash ("x", 0f, "easetype", iTween.EaseType.linear, "time", 0.01f, "delay", 0.15f));
					} else {
						iTween.MoveTo (gameObject, iTween.Hash ("x", -6f, "easetype", iTween.EaseType.linear, "time", 0.15f));
						iTween.MoveTo (gameObject, iTween.Hash ("x", 0f, "easetype", iTween.EaseType.linear, "time", 0.01f, "delay", 0.15f));
					}
				} else if (CanInvoke) {
					if (lp.y > fp.y) {
						iTween.MoveTo (gameObject, iTween.Hash ("y", 3.5f, "easetype", iTween.EaseType.linear, "time", 0.15f));
						iTween.MoveTo (gameObject, iTween.Hash ("y", 0f, "easetype", iTween.EaseType.linear, "time", 0.011f, "delay", 0.15f));
					} else {
						iTween.MoveTo (gameObject, iTween.Hash ("y", -3.5f, "easetype", iTween.EaseType.linear, "time", 0.15f));
						iTween.MoveTo (gameObject, iTween.Hash ("y", 0f, "easetype", iTween.EaseType.linear, "time", 0.01f, "delay", 0.15f));
					}
				}
			}
		}
	}


	void SwipeEnd ()
	{
		IsSwiped = false;
	}

	public void SymbolsGenerator ()
	{
		int Dir = 0;
		GenerateUniqueRandom (SymCount, 0, SymCount);

		int CentreSymbol = Random.Range (0, 4);
		GameObject SG = Instantiate (X [arr [CentreSymbol]].gameObject, transform.position, Quaternion.identity) as GameObject;
		iTween.ScaleFrom (SG, Vector3.zero, 0.7f);
		SG.transform.parent = transform;
	
		if (Dir == 0) {
			GameObject G = Instantiate (Y [arr [Dir]].gameObject, Gens [Dir].position, Quaternion.identity) as GameObject;
			if (Dir == CentreSymbol) {
				G.tag = "Symbol";
			}
			iTween.MoveTo (G, iTween.Hash ("x", 0, "time", MovingTimeX, "easetype", iTween.EaseType.linear));
			Dir++;
		}
		if (Dir == 1) {
			GameObject G = Instantiate (Y [arr [Dir]].gameObject, Gens [Dir].position, Quaternion.identity) as GameObject;
			if (Dir == CentreSymbol) {
				G.tag = "Symbol";
			}
			iTween.MoveTo (G, iTween.Hash ("x", 0, "time", MovingTimeX, "easetype", iTween.EaseType.linear));
			Dir++;
		}
		if (Dir == 2) {
			GameObject G = Instantiate (Y [arr [Dir]].gameObject, Gens [Dir].position, Quaternion.identity) as GameObject;
			if (Dir == CentreSymbol) {
				G.tag = "Symbol";
			}
			iTween.MoveTo (G, iTween.Hash ("y", 0, "time", MovingTimeY, "easetype", iTween.EaseType.linear));
			Dir++;
		}
		if (Dir == 3) {
			GameObject G = Instantiate (Y [arr [Dir]].gameObject, Gens [Dir].position, Quaternion.identity) as GameObject;
			if (Dir == CentreSymbol) {
				G.tag = "Symbol";
			}
			iTween.MoveTo (G, iTween.Hash ("y", 0, "time", MovingTimeY, "easetype", iTween.EaseType.linear));
			Dir++;
		}
	}

	public int[] GenerateUniqueRandom (int amount, int min, int max)
	{
		arr = new int[amount];
		for (int i = 0; i < amount; i++) {
			bool done = false;
			while (!done) {
				int num = Random.Range (min, max);
				int j = 0;
				for (j = 0; j < i; j++) {
					if (num == arr [j]) {
						break;    
					}
				}
				if (j == i) {
					arr [i] = num;
					done = true;
				}
			}
		}
		return arr;
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Y" || col.gameObject.tag == "Symbol") {
			GameObject[] DG = GameObject.FindGameObjectsWithTag ("Y");
			foreach (GameObject d in DG) {
				Destroy (d);
			}
			Destroy (GameObject.FindGameObjectWithTag ("Symbol"));
			if (col.gameObject.tag == "Symbol") {
				if (IsSwiped) {
					MenuC.Score++;
					MenuC.TrueOnce ();
					MenuC.SoundEffect.PlayOneShot (MenuC.One);
					Destroy (Instantiate (Eff1, transform.position, Quaternion.identity)as GameObject, 2f);
				} else {
					MenuC.SoundEffect.PlayOneShot (MenuC.Two);
					Destroy (Instantiate (Eff2, transform.position, Quaternion.identity)as GameObject, 2f);
					MenuC.VidAdCount++;
					CanInvoke = false;
					Invoke ("GetRestartPanel", 0.5f);
				}
				AchievementUnlock ();
				MenuC.ScoreTxt.text = MenuC.Score.ToString ();
			} else if (col.gameObject.tag == "Y") {
				MenuC.SoundEffect.PlayOneShot (MenuC.Two);
				Destroy (Instantiate (Eff2, transform.position, Quaternion.identity)as GameObject, 2f);

				MenuC.VidAdCount++;
				CanInvoke = false;

				if (MenuC.Score > MenuC.HScorePP) {
					PlayerPrefs.SetInt ("HScore", MenuC.Score);
					MenuC.NewScoreTxt.text = "New : " + MenuC.Score.ToString ();
					MenuC.HighScoreTxt1.text = MenuC.Score.ToString ();
					MenuC.HighScoreTxt2.text = "New Record!";
				} else {
					MenuC.NewScoreTxt.text = "New : " + MenuC.Score.ToString ();
					MenuC.HighScoreTxt2.text = "Best : " + PlayerPrefs.GetInt ("HScore");
				}
				Invoke ("GetRestartPanel", 0.5f);
			}
			if (transform.childCount > 0) {
				Destroy (transform.GetChild (0).gameObject);
			}
			if (CanInvoke)
				Invoke ("SymbolsGenerator", 0.3f);
			else
				CancelInvoke ("SymbolsGenerator");
		}
	}


	public void AchievementUnlock ()
	{
		
		if (MenuController.PermScore >= 100 && MenuController.PermScore < 200 && AchievementsCount == 0) {
			AchievementsCount = 1;
			PlayerPrefs.SetInt ("Achievements", AchievementsCount);


			MenuC.UnlockEffect ();
		} else if (MenuController.PermScore >= 200 && MenuController.PermScore < 300 && AchievementsCount == 1) {
			AchievementsCount = 2;
			PlayerPrefs.SetInt ("Achievements", AchievementsCount);

            
			MenuC.UnlockEffect ();
		} else if (MenuController.PermScore >= 300 && MenuController.PermScore < 400 && AchievementsCount == 2) {
			AchievementsCount = 3;
			PlayerPrefs.SetInt ("Achievements", AchievementsCount);

            
			MenuC.UnlockEffect ();
		} else if (MenuController.PermScore >= 400 && MenuController.PermScore < 500 && AchievementsCount == 3) {
			AchievementsCount = 4;
			PlayerPrefs.SetInt ("Achievements", AchievementsCount);

			MenuC.UnlockEffect ();
		} else if (MenuController.PermScore >= 500 && MenuController.PermScore < 600 && AchievementsCount == 4) {
			AchievementsCount = 5;
			PlayerPrefs.SetInt ("Achievements", AchievementsCount);

			MenuC.UnlockEffect ();
		}
	}

	void GetRestartPanel ()
	{
		MenuC.InterstitialAdCount++;
		if (MenuC.InterstitialAdCount == 4) {
			
			HeyZapAdsController.instance.ShowInterstitialAd ();
			MenuC.InterstitialAdCount = 0;
		}
		if (MenuC.VidAdCount == 6) {
			MenuC.VidAdCount = 0;
			MenuC.AdButton.SetActive (true);
			MenuC.AdButton.GetComponent<Button> ().interactable = true;
		} else {
			MenuC.AdButton.SetActive (false);
		}
		MenuC.Things [4].SetActive (true);
		MenuC.Things [4].GetComponent<Animator> ().SetBool ("restart", false);
	}
}
