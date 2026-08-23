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

    private void Start()
    {
    gm = GameManager.Instance;
    if (gm != null)
    {
        gm.onGameOver.AddListener(ActivateGameOverUI);    
    }

    hs = HealthSystem.Instance;
    if (hs != null)
    {
        hs.onHealthChanged.AddListener(UpdateLivesUI);
        hs.onAddBonuses.AddListener(UpdateBonusesUI);

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
}

    private void OnDestroy()
    {
        if (gm != null)
        {
            gm.onGameOver.RemoveListener(ActivateGameOverUI);
        } 

        if (hs != null)
    {
        hs.onHealthChanged.RemoveListener(UpdateLivesUI);
        hs.onAddBonuses.RemoveListener(UpdateLivesUI);
    }
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
