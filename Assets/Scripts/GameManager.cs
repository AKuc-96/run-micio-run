using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; } 
    public HealthSystem Health { get; private set; }

    private void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Health = new HealthSystem(initialLives, maxLives);
        HealthSystem.Instance = Health;
    }

    #endregion

    [Header("Health Settings")]
    [SerializeField] private int initialLives = 1;
    [SerializeField] private int maxLives = 9;

    [Header("Game Data & Score")]
    public float currentScore = 0f;
    public Data data;
    public bool isPlaying = false; 

    [Header("Events")]
    public UnityEvent onPlay = new();
    public UnityEvent onGameOver = new();

    private void Start()
    {
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<Data>(loadedData);
        }
        else
            data = new Data();
    }

    private void Update()
    {
        if (isPlaying)
            currentScore += Time.deltaTime;
    }

    public void StartGame()
    {
        isPlaying = true;
        currentScore = 0;

        if (Health != null)
        {
            Health.ResetLives(initialLives);
        }

        onPlay.Invoke();
    }

    public void GameOver()
    {
        if (data != null && data.highscore < currentScore)
        {
            data.highscore = currentScore;
            
            string saveString = JsonUtility.ToJson(data);
            SaveSystem.Save("save", saveString);
        }
        
        isPlaying = false;
        onGameOver.Invoke();
    }

    public string PrettyScore ()
    {
        return Mathf.RoundToInt(currentScore).ToString();
    }

    public string PrettyHighscore()
    {
        return Mathf.RoundToInt(data.highscore).ToString();
    }
}
