using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
	public UnityEngine.UI.Text Score_Text;
	public UnityEngine.UI.Text HighScore_Text;

	public GameObject Block;

	public int Score = 0;

	float Canvas_Width;
	int Buffer = 10;

	public int[] Path;
	public int Position = 0;

	public string Game_Active = "true";
	int Level = 2;
	int Count = 1;

	void Start () {
		HighScore_Text.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
		Canvas_Width = this.GetComponent<RectTransform>().sizeDelta.x;
		Generate_Path(Count, Level * Level);
		Generate_Blocks(Level * Level);
		StartCoroutine(Hint());
	}
	

	void Update () {
		Score_Text.text = "Score: " + Score;
		if (Input.GetMouseButtonDown(0) && Game_Active == "false") {
			SceneManager.LoadScene("Game");
		}
	}

	public IEnumerator Cycle () {
		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < Path.Length; i++) {
			GameObject.Find("Block #" + Path[i]).GetComponent<Block>().Usage = "unused";
			GameObject.Find("Block #" + Path[i]).GetComponent<UnityEngine.UI.RawImage>().color = new Color(214 / 255f, 156 / 255f, 66 / 255f); //Neutral
		}
		if (Count == Level * Level) {
			for (int i = 0; i < Level * Level; i++) {
				Destroy(GameObject.Find("Block #" + i));
			}
			Level++;
			Count = 1;
			Generate_Path(Count, Level * Level);
			Generate_Blocks(Level * Level);
		}else {
			Count++;
			Generate_Path(Count, Level * Level);
		}
		Position = 0;
		StartCoroutine(Hint());
	}

	IEnumerator Hint () {
		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < Path.Length; i++) {
			GameObject.Find("Block #" + Path[i]).GetComponent<UnityEngine.UI.RawImage>().color = new Color(119 / 255f, 174 / 255f, 128 / 255f); //Green
			if (Count != 1) {
				yield return new WaitForSeconds(1f);
			}
		}
		yield return new WaitForSeconds(1);
		for (int i = 0; i < Path.Length; i++) {
			GameObject.Find("Block #" + Path[i]).GetComponent<UnityEngine.UI.RawImage>().color = new Color(214 / 255f, 156 / 255f, 66 / 255f); //Neutral
		}
		Game_Active = "true";
	}

	void Generate_Blocks (int Quantity) {
		Position = 0;
		float Block_Size = 0;
		for (int a = 0; a < Mathf.Sqrt(Quantity); a++) { //X pos
			for (int b = 0; b < Mathf.Sqrt(Quantity); b++) { //Y pos
				GameObject Clone = Instantiate(Block);
				Clone.transform.SetParent(GameObject.Find("Canvas").transform);
				Block_Size = Canvas_Width / Mathf.Sqrt(Quantity);
				Clone.GetComponent<RectTransform>().sizeDelta = new Vector2(Block_Size - Buffer, Block_Size - Buffer); //gucciiii
				float x = 0 - (Canvas_Width / 2f) + (Block_Size / 2) + (Canvas_Width / Mathf.Sqrt(Quantity) * a); //no no no
				float y = 0 - (Canvas_Width / 2f) + (Block_Size / 2) + (Canvas_Width / Mathf.Sqrt(Quantity) * b);
				Clone.transform.localPosition = new Vector2(x, y);
				Clone.GetComponent<Block>().Number = Mathf.RoundToInt(a + (Mathf.Abs(Mathf.Sqrt(Quantity) - b - 1) * Mathf.Sqrt(Quantity)));
				Clone.name = "Block #" + Clone.GetComponent<Block>().Number;
			}
		}
		Score_Text.rectTransform.sizeDelta = new Vector2(Canvas_Width, (this.GetComponent<RectTransform>().sizeDelta.y - (Block_Size * Level)) / 2f);
		HighScore_Text.rectTransform.sizeDelta = new Vector2(Canvas_Width, (this.GetComponent<RectTransform>().sizeDelta.y - (Block_Size * Level)) / 2f);
		Score_Text.rectTransform.localPosition = new Vector3(0, ((Score_Text.rectTransform.sizeDelta.y + (Block_Size * Level)) / 2f), 0);
		HighScore_Text.rectTransform.localPosition = new Vector3(0, ((HighScore_Text.rectTransform.sizeDelta.y + (Block_Size * Level)) / 2f) * -1, 0);
	}

	void Generate_Path (int Length, int Size) { //Works 100%
		Game_Active = "working";
		Path = new int[Length];
		for (int i = 0; i < Length; i++) {
			Path[i] = Random.Range(0, Size);
			while (1 > 0) {
				int Temp = Path[i];
				for (int a = 0; a < i; a++) {
					if (Path[i] == Path[a]) {
						Path[i] = Random.Range(0, Size);
						a = -1;
					}
				}
				if (Path[i] == Temp) {
					break;
				}
			}
		}
	}
}
