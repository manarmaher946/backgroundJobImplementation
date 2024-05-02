
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;
using IDatabase = StackExchange.Redis.IDatabase;

namespace backgroundImplementation.DistributedChashing
{
    public class Distributed : IDistributed
    {
        IDatabase data;

        public Distributed()
        {
            var Redis = ConnectionMultiplexer.Connect("localhost:6379");
            data = Redis.GetDatabase();
        }

        public T GetData<T>(string Key)
        {
           var item=data.StringGet(Key);
            if (!string.IsNullOrEmpty(item))
            {
               return JsonSerializer.Deserialize<T>(item);
            }
            return default;
        }

        public object RemoveData(string Key)
        {
            var exist=data.KeyExists(Key);
            if (exist)
            {
                return data.KeyDelete(Key);
            }
            return false;
        }

        public bool SetData<T>(string Key, T Value, DateTimeOffset Expiredate)
        {
            var Time=Expiredate.DateTime.Subtract(DateTime.Now);
            var Setdate=data.StringSet(Key,JsonSerializer.Serialize(Value), Time);
            return Setdate;

        }


    }
}
