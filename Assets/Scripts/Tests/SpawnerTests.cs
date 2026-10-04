using System.Collections;
using NUnit.Framework;
using UnityEngine; 
using UnityEngine.TestTools;

public class SpawnerTests
{

    private GameObject _spawnerObject;
    private Spawner _spawner;

    [SetUp]

    public void SetUp()
    {
        _spawnerObject = new GameObject("TestSpawner");
        _spawner = _spawnerObject.AddComponent<Spawner>();
    }

    [TearDown]

    public void TearDown()
    {
        if (_spawnerObject != null)
        {
            Object.DestroyImmediate(_spawnerObject);
        }
    }

    [UnityTest]
    public IEnumerator Spawner_ShouldNotSpawnObstacles_DuringVictoryPhase()
    {
        GameObject spawnerObject = new GameObject("TestSpawner");
        Spawner spawner = spawnerObject.AddComponent<Spawner>();

        PhaseConfig victoryConfig = ScriptableObject.CreateInstance<PhaseConfig>();
        victoryConfig.InitForTest(0f, false);

        spawner.HandlePhaseChanged(victoryConfig);

        yield return new WaitForSeconds (1f);

        Obstacle[] spawnedObstacles = Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None);

        Assert.AreEqual(0, spawnedObstacles.Length, "Препятствия не должны спавниться при spawnInterval <= 0!");

        Object.Destroy(spawnerObject); 

    }

    [Test]
    public void HandlePhaseChanged_ResetTimeAndExtraLifeFlag()
    {
        PhaseConfig config = ScriptableObject.CreateInstance<PhaseConfig>();
        config.InitForTest(2f, false);

        _spawner.HandlePhaseChanged(config);

        Assert.AreEqual(0f, _spawner.PhaseTimer, "Таймер фазы должен обнуляться при смене фазы!");
        Assert.IsFalse(_spawner.IsExtraLifeSpawned, "Флаг спавна жизни должен сбрасываться в false!");
    }

    [Test]
    public void Update_SpawnsExtraLife_WhenFiveSecondsRemainInNonBossPhase()
    {
        PhaseConfig config = ScriptableObject.CreateInstance<PhaseConfig>();
        config.InitForTest(interval: 2f, isBoss: false, phaseIsLong: 10f);

        _spawner.HandlePhaseChanged(config);

        _spawner.Tick(4.9f);
        Assert.IsFalse(_spawner.IsExtraLifeSpawned, "Жизнь НЕ должна спавниться раньше, чем за 5 секунд до конца фазы!");

        _spawner.Tick(0.2f);

        Assert.IsTrue(_spawner.IsExtraLifeSpawned, "Жизнь ДОЛЖНА заспавниться за 5 секунд до конца фазы!");
    }
}
