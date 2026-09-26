using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI; 

    [Header("Gameplay Stats UI")]
    [SerializeField] private TextMeshProUGUI livesUI;
    [SerializeField] private TextMeshProUGUI bonusesUI;

    private GameManager gm; 
    private HealthSystem hs;

    private void OnEnable()
    {
        GameManager.onGameOver += ActivateGameOverUI;
        HealthSystem.onHealthChanged += UpdateLivesUI; 
        HealthSystem.onAddBonuses += UpdateBonusesUI;
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= ActivateGameOverUI; 
        HealthSystem.onHealthChanged -= UpdateLivesUI;
        HealthSystem.onAddBonuses -= UpdateBonusesUI;
    }
    private void Start()
    {
        gm = GameManager.Instance;
        // if (gm != null)
        // {
            //gm.onGameOver += ActivateGameOverUI;    
        // }
        gm = GameManager.Instance;

        hs = HealthSystem.Instance;
        if (hs != null)
        {
            UpdateLivesUI(hs.CurrentLives);
            UpdateBonusesUI(hs.BonusCoins);
        }
        hs = HealthSystem.Instance;
        if (hs != null)
        {
            UpdateLivesUI(hs.CurrentLives);
            UpdateBonusesUI(hs.BonusCoins);
        }
    }

    private void Update()
    {
        // Пока идет игра, обновляем плашку с очками
        if (gm != null && gm.isPlaying)
        {
            UpdateScoreUI(gm.PrettyScore());
        }
        
        // Пока идет игра, обновляем плашку с очками
        if (gm != null && gm.isPlaying)
        {
            UpdateScoreUI(gm.PrettyScore());
        }
    }

    private void OnDestroy()
    {
        
    }
    public void PlayButtonHandler()
    {
        gm.StartGame();
    }

    public void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);

        gameOverScoreUI.text = "Score: " + gm.PrettyScore();
        gameOverHighscoreUI.text = "Highscore: " + gm.PrettyHighscore();
    }

    public void UpdateScoreUI (string scoreText)
    {
        if (scoreUI != null)
        {
            scoreUI.text = scoreText;
        }
    }

    public void UpdateLivesUI (int currentLives)
    {
        if (livesUI != null)
        {
            livesUI.text = "Lives: " + currentLives;
        }
    } 

    public void UpdateBonusesUI (int bonusCoins)
    {
        if (bonusesUI != null)
        {
            bonusesUI.text = "Bonuses: " + bonusCoins;
        }
    }
}