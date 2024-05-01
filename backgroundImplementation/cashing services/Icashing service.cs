namespace backgroundImplementation.cashing_services
{
    public interface Icashing_service
    {
        T GetData <T>(string Key);
        bool SetData <T>(string Key, T Value,DateTimeOffset Expiredate);
        object RemoveData(string Key);
    }
}
