using NUnit.Framework;

public class FrequencyTest
{
	Frequency frequency;

	[SetUp]
	public void Setup()
	{
		frequency = new Frequency();
		frequency.FrequencyAmount = 2;
	}

    [Test]
	public void Frequency_DecreaseFrequency_ShouldBeOneLessThanBefore()
	{
		uint initialFrequencyAmount = frequency.FrequencyAmount;
		frequency.DecreaseFrequency();

		Assert.AreEqual(initialFrequencyAmount - 1, frequency.FrequencyAmount);
	}

	[Test]
	public void Frequency_IsFrequencyOver_ShouldBeTrue()
	{
		frequency.DecreaseFrequency();
		frequency.DecreaseFrequency();
		
		Assert.IsTrue(frequency.IsFrequencyOver());
	}

	[Test]
	public void Frequency_IsFrequencyOver_ShouldBeFalse()
	{
		frequency.DecreaseFrequency();
		
		Assert.IsFalse(frequency.IsFrequencyOver());
	}
}
