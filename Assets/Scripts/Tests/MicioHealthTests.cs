using NUnit.Framework;

public class MicioHealthTests
{
    [Test]
    public void AddLife_WhenBelowMax_IncreaseCurrentLives()
    {
        var health = new HealthSystem(initialLives: 2, maxLives: 9);

        health.AddLife(1);

        Assert.AreEqual(3, health.CurrentLives, "Жизнь должна добавиться!"); 
        Assert.AreEqual(0, health.BonusCoins, "Бонус не должен начисляться до достижения лимита!");
    }

    [Test]
    public void AddLife_WhenAtMax_IncreasesBonusCoinsInsteadOfLives()
    {
        var health = new HealthSystem(initialLives: 9, maxLives: 9);

        health.AddLife(1);

        Assert.AreEqual(9, health.CurrentLives, "Количество жизней не должно превышать 9!");
        Assert.AreEqual(1, health.BonusCoins, "При полном HP должен начислиться бонус!");
    }

    [Test]
    public void TakeDamage_DecreasesLives_AndDoesNotGoBelowZero()
    {
        var health = new HealthSystem(initialLives: 2, maxLives: 9);

        health.TakeDamage(5);

        Assert.AreEqual(0, health.CurrentLives, "Здоровье не должно опускаться ниже нуля!");
    }
}
