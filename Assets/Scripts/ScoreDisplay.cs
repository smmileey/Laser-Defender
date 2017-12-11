using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    private Text _scoreDisplay;

	void Start () {
        _scoreDisplay = GetComponent<Text>();
        _scoreDisplay.text = ScoreKeeper.Score.ToString();
    }
}
