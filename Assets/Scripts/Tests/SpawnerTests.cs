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

    [Test]
    public void Tick_DoesNotSpawnExtraLife_DuringBossPhase()
    {
        PhaseConfig bossConfig = ScriptableObject.CreateInstance<PhaseConfig>();
        bossConfig.InitForTest(interval: 2f, isBoss: true, phaseIsLong: 10f);
        
        _spawner.HandlePhaseChanged(bossConfig);

        _spawner.Tick(9.9f);

        Assert.IsFalse(_spawner.IsExtraLifeSpawned, "Дополнительная жизнь НЕ должна спавниться на фазе босса!");
    }

    [Test]
    public void SpawnExtraLife_Spawns_WhenRandomIsWithinChance()
    {
        PhaseConfig config = ScriptableObject.CreateInstance<PhaseConfig>();
        config.InitForTest(interval: 2f, isBoss: false, phaseIsLong: 10f, weCanSpawnExtraLife: true);
        
        _spawner.HandlePhaseChanged(config);

        _spawner.SetRandomGeneratorForTest((min, max) => 10.0f);

        _spawner.Tick(2.1f);

        Assert.IsTrue(_spawner.IsExtraLifeSpawned, "Жизнь должна заспавниться при успешном броске рандома!");
    }

    [Test]
    public void SpawnExtraLife_DoesNotSpawn_WhenRandomExceedsChance()
    {
        PhaseConfig config = ScriptableObject.CreateInstance<PhaseConfig>();
        config.InitForTest(interval: 2f, isBoss: false, phaseIsLong: 10f);

        _spawner.HandlePhaseChanged(config);

        _spawner.SetRandomGeneratorForTest((min, max) => 50.0f);

        _spawner.Tick(2.1f);

        Assert.IsFalse(_spawner.IsExtraLifeSpawned, "Жизнь НЕ должна спавниться, если рандом больше шанса!");
    }

    [Test]
    public void HandlePhaseChnaged_NonBossPhase_SetsNegativeSpawnTimeDelay()
    {
        PhaseConfig nonBossPhase = ScriptableObject.CreateInstance<PhaseConfig>();
        nonBossPhase.InitForTest(interval: 2f, isBoss: false);

        _spawner.HandlePhaseChanged(nonBossPhase);

        _spawner.Tick(2.0f);

        Obstacle[] spawnedObstacles = Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        Assert.AreEqual(0, spawnedObstacles.Length, "Препятствия не должны спавниться во время 3-секундной задержки!");
    }
}
