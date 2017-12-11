using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {
    private Text _text;

    public static long Score { get; private set; }

    void Start()
    {
        _text = GetComponent<Text>();
        Reset();
    }

    public void AddScore(int points)
    {
        _text.text = (Score += points).ToString();
    }    

    public void Reset()
    {
        Score = 0;
    }
}
