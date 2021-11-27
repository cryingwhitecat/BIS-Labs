namespace Core.Abstractions
{
    public interface IHashingAlgorithm
    {
        byte[] GetHash(byte[] input);
    }
}
