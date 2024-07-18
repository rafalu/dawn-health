namespace Dawnhealth.Antigravity.DomainServices;

public interface IRandomNumberGenerator
{
    /// <summary>
    /// Generate a random number
    /// </summary>
    /// <param name="length">The length of the random number</param>
    /// <returns>A random number</returns>
    int Generate(int length);
}
