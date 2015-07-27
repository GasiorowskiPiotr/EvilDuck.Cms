using Microsoft.Framework.Caching.Memory;
using System;
using System.Threading;

namespace EvilDuck.Cms.Portal.Framework.Caching
{
    public abstract class CacheBase
    {
        private IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

        private string BuildUniqueStringKey(ICacheKey key)
        {
            return String.Format("{0};{1};{2}", GetType().FullName, key.GetType().FullName, key.GetStringKey());
        }

        private bool _useAbsoluteExpiration = true;
        private bool _useSlidingExpiration;

        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private TimeSpan _absoluteExpirationSpan = TimeSpan.FromHours(1);
        public TimeSpan AbsoluteExpirationSpan
        {
            get { return this._absoluteExpirationSpan; }
            set
            {
                this._useAbsoluteExpiration = true;
                this._useSlidingExpiration = false;
                this._absoluteExpirationSpan = value;
            }
        }

        private TimeSpan _slidingExpirationSpan = TimeSpan.Zero;
        public TimeSpan SlidingExpirationSpan
        {
            get { return this._slidingExpirationSpan; }
            set
            {
                this._useAbsoluteExpiration = false;
                this._useSlidingExpiration = true;
                this._slidingExpirationSpan = value;
            }
        }

        public DateTime AbsoluteExpiration
        {
            get { return DateTime.UtcNow + this.AbsoluteExpirationSpan; }
        }


        protected abstract void OnMiss(ICacheKey key, out object value);

        protected object Get(ICacheKey key)
        {

            try
            {
                _lock.EnterUpgradeableReadLock();

                object value;

                if(cache.TryGetValue(key.GetStringKey(), out value)) 
                {
                    return value;
                }

                OnMiss(key, out value);

                var cacheKey = BuildUniqueStringKey(key);

                var opts = new MemoryCacheEntryOptions();

                if (_useAbsoluteExpiration)
                {
                    opts.AbsoluteExpiration = AbsoluteExpiration;
                }
                else if (_useSlidingExpiration)
                {
                    opts.SlidingExpiration = SlidingExpirationSpan;
                }

                cache.Set(key.GetStringKey(), value, opts);

                return value;
            }
            finally
            {
                if(_lock.IsUpgradeableReadLockHeld)
                {
                    _lock.ExitUpgradeableReadLock();
                }
            }
        }

        protected void Remove(ICacheKey key)
        {
            
                Remove(BuildUniqueStringKey(key));
            
            
        }

        private void Remove(string key)
        {
            try
            {
                _lock.EnterWriteLock();
                cache.Remove(key);
            }
            finally
            {
                if (_lock.IsWriteLockHeld)
                {
                    _lock.ExitWriteLock();
                }
            }
        }
    }
}
