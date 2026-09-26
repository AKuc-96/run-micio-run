using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTests
{
    private GameObject gmObject;
    private GameManager gameManager;

    [SetUp]
    public void SetUp()
    {
        gmObject = new GameObject("TestGameManager");
        gameManager = gmObject.AddComponent<GameManager>();

        gameManager.isPlaying = true;
    }

    [TearDown]
    public void TearDown()
    {
        if (gmObject != null)
        {
            Object.Destroy(gmObject);
        }
    }

    [UnityTest]
    public IEnumerator GameManager_ShouldStopTimer_WhenVictoryPhaseStarts()
    {
        yield return new WaitForSeconds(0.2f);

        float scoreBeforeVictory = float.Parse(gameManager.PrettyScore());

        PhaseConfig victoryConfig = ScriptableObject.CreateInstance<PhaseConfig>();
        victoryConfig.InitForTest(0f, false);

        gameManager.HandlePhaseChanged(victoryConfig);

        yield return new WaitForSeconds(0.5f);

        float scoreAfterVictory = float.Parse(gameManager.PrettyScore());

        Assert.AreEqual(scoreBeforeVictory, scoreAfterVictory, "Таймер/Score должен полностью остановиться при наступлении Victory!"); 
        Assert.IsFalse(gameManager.isPlaying, "Флаг isPlaying должен стать false при наступлении Victory!");
    }
}
