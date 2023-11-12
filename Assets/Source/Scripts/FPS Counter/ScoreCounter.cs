using UnityEngine;
using UnityEngine.UI;
using YG;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private Button but;
    [SerializeField] private Text _scoreText;
    
    private int _score;

    private void Awake()
    {
        but.onClick.AddListener(delegate { AddScoreADS(1); });
    }

    private void Start()
    {
        _score = PlayerPrefs.GetInt("Score", 0);

        UpdateScoreText();
    }

    public void AddScore(int value)
    {
        _score += value;

        PlayerPrefs.SetInt("Score", _score);

        YandexGame.NewLeaderboardScores("LiderBoardScore", _score);

        UpdateScoreText();
    }

    private void UpdateScoreText()  
    {
        _scoreText.text = _score.ToString();
    }

    private void Rewarded(int id)
    {
        if (id == 1)
        {
            AddScore(500);
        }
    }

    public void AddScoreADS(int id)
    {
        YandexGame.RewVideoShow(id);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }
}