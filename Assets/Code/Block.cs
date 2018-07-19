using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public int Number;
	public string Usage = "unused";
	GameMaster Canvas;

	void Start () {
		this.GetComponent<UnityEngine.UI.RawImage>().color = new Color(214 / 255f, 156 / 255f, 66 / 255f); //Neutral
		Canvas = GameObject.Find("Canvas").GetComponent<GameMaster>();
	}

	public void Block_Check () {
		if (Canvas.Game_Active == "true" && Usage == "unused") {
			if (Canvas.Path[Canvas.Position] == Number) {
				Canvas.Position++;
				this.GetComponent<UnityEngine.UI.RawImage>().color = new Color(119 / 255f, 174 / 255f, 128 / 255f); //Green
				Usage = "used";
				Canvas.Score += 1;
				if (Canvas.Position == Canvas.Path.Length) {
					StartCoroutine(Canvas.Cycle());
				}
			}else if (Canvas.Path[Canvas.Position] != Number) {
				if (Canvas.Score > PlayerPrefs.GetInt("Highscore")) {
					PlayerPrefs.SetInt("Highscore", Canvas.Score);
				}
				Canvas.Game_Active = "false";
				this.GetComponent<UnityEngine.UI.RawImage>().color = new Color(133 / 255f, 42 / 255f, 2 / 255f); //Red
			}
		}
	}
}
