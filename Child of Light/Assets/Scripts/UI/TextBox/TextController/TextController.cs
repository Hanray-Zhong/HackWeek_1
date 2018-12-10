using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextController : MonoBehaviour {
	[System.Serializable]
	public struct Images{
		public int key;
		public Sprite image;
	}
	[System.Serializable]
	public struct LiPainted{
		public int key;
		public Sprite image;
	}

	[Header("UI")]
	public List<GameObject> texts;
	public Images[] images;
	public GameObject BackGround;
	public LiPainted[] liPainteds;
	public GameObject LiPainted_obj;
	public GameObject BlackBGPrefab;
	public GameObject NextTextController;
	[Header("Game Process")]
	public GameObject Player;
	public GameObject EnemyInitializer;
	public GameObject BossInitializer;
	public bool NeedTimeOUT;
	public bool NeedRecoverTime;
	[Header("")]

	private float Button_A;
	private int timeRecorder = 1;
	private GameObject currnetText;

	private void Start() {
		currnetText = Instantiate(texts[0], gameObject.GetComponent<RectTransform>());
		ChangeLiPainted();
		if (NeedTimeOUT) {
			TimeOut();
		}
	}

	private void Update() {
		HandleConfiguration();
		ChangeText();
	}


	void HandleConfiguration () {
		Button_A = Input.GetAxisRaw( "Button_A" );
	}

	void ChangeText () {
		if (currnetText != null) {
			if ( Button_A >= 0.5 && currnetText.GetComponent<CanvasGroup>().alpha >= 1) {
				
				currnetText.GetComponent<GradualChange>().CanClose = true;
			}
			if (currnetText.GetComponent<GradualChange>().CanClose && currnetText.GetComponent<CanvasGroup>().alpha <= 0) {
				texts.RemoveAt(0);
				Destroy(currnetText);
				if (texts.Count > 0) {
					timeRecorder++;
					ChangeBackground();
					ChangeLiPainted();
					currnetText = Instantiate(texts[0], gameObject.GetComponent<RectTransform>());
				}
				else
					currnetText = null;
			}
		}
		else {
			GetComponent<RectTransform>().parent.GetComponent<GradualChange>().CanClose = true;
			RecoverGameObject(Player);
			RecoverGameObject(EnemyInitializer);
			RecoverGameObject(BossInitializer);
			if (NeedRecoverTime) {
				TimeRecover();
			}
			ActiveNextTextController(NextTextController);
			StartCoroutine(DestroySelf());
		}
	}

	void ChangeBackground() {
		if (images.Length == 0) {
			return;
		}
		foreach (var item in images) {
			if (item.key == timeRecorder && BackGround.GetComponent<Image>().sprite != item.image) {
				Instantiate(BlackBGPrefab, gameObject.GetComponent<RectTransform>().parent.parent);
				StartCoroutine(ChangeBG(item));
			}
		}
	}

	void ChangeLiPainted() {
		if (liPainteds.Length == 0) {
			return;
		}
		foreach (var item in liPainteds) {
			if (item.key == timeRecorder && LiPainted_obj.GetComponent<Image>().sprite != item.image) {
				LiPainted_obj.GetComponent<Image>().sprite = item.image;
			}
		}
	}

	void ProhibitGameObject(GameObject obj) {
		if (obj != null) {
			if (obj.GetComponent<EnemyInitializer>() == true || obj.GetComponent<BossInitializer>() == true) {
				obj.SetActive(false);
			}
			if (obj.GetComponent<PlayerController>() == true) {
				obj.GetComponent<PlayerController>().enabled = false;
				obj.GetComponent<Lighting>().enabled = false;
			}
		}
	}
	void RecoverGameObject(GameObject obj) {
		if (obj != null) {
			if (obj.GetComponent<EnemyInitializer>() == true || obj.GetComponent<BossInitializer>() == true) {
				obj.SetActive(true);
			}
			if (obj.GetComponent<PlayerController>() == true) {
				obj.GetComponent<PlayerController>().enabled = true;
				obj.GetComponent<Lighting>().enabled = true;
			}
		}
	}
	void TimeOut() {
		Time.timeScale = 0;
	}
	void TimeRecover() {
		Time.timeScale = 1;
	}
	void ActiveNextTextController(GameObject NextTextController) {
		if (NextTextController != null)
			NextTextController.SetActive(true);
	}

	IEnumerator ChangeBG(Images item) {
		yield return new WaitForSeconds(1f);
		BackGround.GetComponent<Image>().sprite = item.image;
	}
	IEnumerator DestroySelf() {
		yield return new WaitForSeconds(2f);
		Destroy(gameObject.GetComponent<RectTransform>().parent.gameObject);
	}
}

