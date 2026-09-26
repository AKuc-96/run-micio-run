using System.Collections;
using NUnit.Framework;
using UnityEngine; 
using UnityEngine.TestTools;

public class SpawnerTests
{
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
}
