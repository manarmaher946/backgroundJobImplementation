
using Newtonsoft.Json.Linq;
using System.Runtime.Caching;

namespace backgroundImplementation.cashing_services
{
    public class cashing_service : Icashing_service
    {
        private ObjectCache _MemoryCashe=MemoryCache.Default;

       

        public T GetData<T>(string Key)
        {
            try
            {
                T Item = (T)_MemoryCashe.Get(Key);
                return Item;
            }
            catch (Exception)
            {

                throw ;
            }
        }

        public object RemoveData(string Key)
        {
            var result = true;
            try
            {
                if (!string.IsNullOrEmpty(Key))
                {
                 var Resultremove= _MemoryCashe.Remove(Key);
                }
                else
                {
                    result = false;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SetData<T>(string Key, T Value, DateTimeOffset Expiredate)
        {
            var result = true;
            try
            {
                if (!string.IsNullOrEmpty(Key))
                {
                    _MemoryCashe.Set(Key, Value,Expiredate);
                }
                else
                {
                result =false;
                    
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
