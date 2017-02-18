using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IshServices.Models
{
    public class ExpiringCache<T>
    {
        
        public ExpiringCache(T item, TimeSpan cacheDuration)
        {
            Item = item;
            _cacheCreated = DateTime.Now;
            CacheDuration = cacheDuration;
        }

        public TimeSpan CacheDuration { get; private set; }
        private DateTime _cacheCreated;

        public T Item { get; private set; }

        public bool IsExpired()
        {
            return DateTime.Now.Subtract(_cacheCreated).TotalMilliseconds > CacheDuration.TotalMilliseconds;
        }

        public void Refresh(T item)
        {
            Item = item;
            _cacheCreated = DateTime.Now;
        }
    }
}