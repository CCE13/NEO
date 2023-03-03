using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class ScoreDisplay : MonoBehaviour,ISaveable
{
    [Header("REQUIRED!!")]
    public TMP_Text scorePoints;
    public TMP_Text highScorePoints;
    [SerializeField] private CollectableCollecter collectableCollecter;

    [Space(20)]
    public float comboTimer;
    public List<EnemyType> Enemy;

    [Header("PopUp")]
    public GameObject comboPopUp;
    public TMP_Text addPoints;
    public float animationDuration;
    public float countUpDuration;
    public int currentScore
    {
        get;
        private set;
    }
    public int highScore
    {
        get;
        private set;
    }

    private TextMesh _comboPopUpText;
    private GameObject _player => FindObjectOfType<PlayerManager>().gameObject;
    private ComboCalculator _comboCalculator = new ComboCalculator();

    private int _pointsToBeAdded;
    private bool _timerCountdown;
    private float _startTimer;
    private int _targetScore;
    private string Name => "Score";


    private void Awake()
    {
        _comboPopUpText = comboPopUp.transform.Find("Text").GetComponent<TextMesh>();
    }
    private void Start()
    {
        _startTimer = comboTimer;
        collectableCollecter.ValueChanged += OnKillEnemy;
        _targetScore = currentScore;
        scorePoints.text = currentScore.ToString();
    }

    private void OnDestroy()
    {
        collectableCollecter.ValueChanged -= OnKillEnemy;
    }

    private void Update()
    {
        TimerCountdown();
    }
    private void ShowScore(int score)
    {
        ResetTimer();
        int comboMultiplier = _comboCalculator.currentMultiplier;
        int scoreToShow = CalculateScoreToShow(score);
        _targetScore += scoreToShow;
        ShowPointsPopUp(scoreToShow, comboMultiplier);
        CalculatePointsToAdd(scoreToShow);
        StartTimer();
    }

    private int CalculateScoreToShow(int scoreForCalculation)
    {
        int ScoreToAdd = _comboCalculator.ScoreCalulator(scoreForCalculation);
        return ScoreToAdd;
    }

    #region ScoreTextSetting

    private IEnumerator SetScoreText(int targetScore)
    {

        int stepAmount = Mathf.CeilToInt((targetScore - currentScore) / (60 * countUpDuration));
        if (currentScore < targetScore)
        {
            while (currentScore < targetScore)
            {
                currentScore += stepAmount;
                if (currentScore > targetScore)
                {
                    currentScore = targetScore;
                }
                scorePoints.text = (currentScore).ToString();
                yield return null;
            }
        }
        
    }

    public void SetHighscore(string chapter)
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt($"{chapter}Highscore", highScore);
            highScorePoints.text = highScore.ToString();
        }

    }

    #endregion ScoreTextSetting

    #region Timer

    private void StartTimer()
    {
        _timerCountdown = true;
    }

    private void ResetTimer()
    {
        comboTimer = _startTimer;
    }

    private void TimerCountdown()
    {
        var stillCounting = comboTimer > 0;
        var stoppedCounting = comboTimer <= 0;
        if (!_timerCountdown) return;
        if (stillCounting)
        {
            comboTimer -= Time.unscaledDeltaTime;
        }
        if (stoppedCounting)
        {
            _timerCountdown = false;
            int cachedPoints = _pointsToBeAdded;
            _pointsToBeAdded = 0;
            StartCoroutine(ShowPointsToAdd(cachedPoints));
            _comboCalculator.ResetMultiplier();
        }
    }

    #endregion Timer

    private void ShowPointsPopUp(int pointsGained, int comboMultiplier)
    {
        if (comboMultiplier < 2)
        {
            _comboPopUpText.text = $"+ {pointsGained}";
        }
        else
        {
            _comboPopUpText.text = $"X {comboMultiplier} COMBO\n+ {pointsGained}";
        }
        GameObject scorePopUpText = Instantiate(comboPopUp, _player.transform.position, Quaternion.identity);
        Destroy(scorePopUpText, 2f);
    }

    private void CalculatePointsToAdd(int PointsToAdd)
    {
        _pointsToBeAdded += PointsToAdd;
    }
    private IEnumerator ShowPointsToAdd(int pointsToBeAdded)
    {
        addPoints.text = $"+ {pointsToBeAdded} ";
        TMP_Text scoreToAddText = Instantiate(addPoints, transform);
        if (PauseMenuController.S_isPaused)
        {
            scoreToAddText.gameObject.SetActive(false);
        }
        else
        {
            scoreToAddText.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(animationDuration);
        Destroy(scoreToAddText);
        StartCoroutine(SetScoreText(pointsToBeAdded + currentScore));
    }

    private void OnKillEnemy()
    {
        foreach (EnemyType enemy in Enemy)
        {
            if (collectableCollecter.CountOf(enemy) > 0)
            {
                ShowScore(enemy.pointsWorth);
                collectableCollecter.Remove(enemy);
            }
        }
    }

    public void Save()    
    {
        ScoreData data = new ScoreData();
        data.scoreSaved = currentScore;
        SavingManager.Save(data, Name);
    }

    public void Load()
    {
        ScoreData data = SavingManager.Load<ScoreData>(Name);
        if (SavingManager.SaveExists(Name))
        {
            currentScore = data.scoreSaved;
            scorePoints.text = currentScore.ToString();
        }

        
    }

    [Serializable]
    public class ScoreData
    {
        public int scoreSaved;
    }
}