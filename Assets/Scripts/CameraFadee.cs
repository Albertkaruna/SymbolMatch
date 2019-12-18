using UnityEngine;
using System.Collections;

public class CameraFadee : MonoBehaviour
{

	public Texture2D tek;
	public GameObject empty;

	void Awake()
	{
		MakeCameraFadeFrom();
	}

	public void MakeCameraFadeTo()
	{
		StartCoroutine(CameraFadeTo(0.1f));
	}

	IEnumerator CameraFadeTo(float t)
	{
		yield return new WaitForSeconds(t);
		iTween.CameraFadeAdd();
		iTween.CameraFadeSwap(tek);
		iTween.CameraFadeTo(iTween.Hash("amount", 1f, "time", 1f, "easetype", iTween.EaseType.linear));
	}

	public void MakeCameraFadeFrom()
	{
		StartCoroutine(CameraFadeFrom(0));
	}

	IEnumerator CameraFadeFrom(float t)
	{
		if (empty != null)
			empty.SetActive(true);
		yield return new WaitForSeconds(t);
		iTween.CameraFadeAdd();
		iTween.CameraFadeSwap(tek);
		iTween.CameraFadeFrom(iTween.Hash("amount", 1f, "time", 1f, "easetype", iTween.EaseType.linear));
		yield return new WaitForSeconds(0.5f);
		Destroy(empty);
	}


}
