namespace backgroundImplementation.DistributedChashing
{
    public interface IDistributed
    {
        T GetData<T>(string Key);
        bool SetData<T>(string Key, T Value, DateTimeOffset Expiredate);
        object RemoveData(string Key);
    }
}
