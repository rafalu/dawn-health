namespace Dawnhealth.Antigravity.DomainServices;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    public int Generate(int length)
    {
        var random = new Random();
        var min = (int)Math.Pow(10, length - 1);
        var max = (int)Math.Pow(10, length) - 1;
        return random.Next(min, max);
    }
}
