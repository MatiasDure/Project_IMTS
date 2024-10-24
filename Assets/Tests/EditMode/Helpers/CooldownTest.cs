using NUnit.Framework;

public class CooldownTest
{
	Cooldown cooldown;

	[SetUp]
	public void Setup()
	{
		cooldown = new Cooldown();
	}

    [Test]
	public void Cooldown_StartCooldown_ShouldSetIsOnCooldownToTrue()
	{
		float cooldownTime = 5f;
		cooldown.StartCooldown(cooldownTime);

		Assert.IsTrue(cooldown.IsOnCooldown);
	}

	[Test]
	public void Cooldown_DecreaseCooldown_ShouldInvokeOnCooldownOver()
	{
		float cooldownTime = 5f;
		cooldown.StartCooldown(cooldownTime);

		bool isOnCooldownOverInvoked = false;
		cooldown.OnCooldownOver += () => isOnCooldownOverInvoked = true;

		Assert.IsTrue(cooldown.IsOnCooldown);

		cooldown.DecreaseCooldown(cooldownTime);

		Assert.IsTrue(isOnCooldownOverInvoked);
		Assert.IsFalse(cooldown.IsOnCooldown);
	}
}
