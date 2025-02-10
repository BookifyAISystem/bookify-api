using bookify_data.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
	public partial interface ICacheService
	{
		T Get<T>(string key);
		T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, CacheTimes minutesToCache) where T : class;
		/// <summary>
		/// Cache objects for a specified amount of time
		/// </summary>
		/// <param name="key">The cache key</param>
		/// <param name="data">Object / Data to cache</param>
		/// <param name="minutesToCache">How many minutes to cache them for</param>
		void Set(string key, object data, CacheTimes minutesToCache);
		bool IsSet(string key);
		void Invalidate(string key);
		void Clear();
		void ClearStartsWith(string keyStartsWith);
		void ClearStartsWith(List<string> keysStartsWith);
	}
	public partial class CacheService : ICacheService
	{
		#region Long Cache

		private static ObjectCache Cache => System.Runtime.Caching.MemoryCache.Default;

		private static IDictionaryEnumerator GetCacheToEnumerate()
		{
			return (IDictionaryEnumerator)((IEnumerable)Cache).GetEnumerator();
		}

		public T Get<T>(string key)
		{
			var objectToReturn = Cache[key];
			if (objectToReturn != null)
			{
				if (objectToReturn is T)
				{
					return (T)objectToReturn;
				}
				try
				{
					return (T)Convert.ChangeType(objectToReturn, typeof(T));
				}
				catch (InvalidCastException)
				{
					return default(T);
				}
			}
			return default(T);
		}

		/// <summary>
		///     Cache objects for a specified amount of time
		/// </summary>
		/// <param name="key">The cache key</param>
		/// <param name="data">Object / Data to cache</param>
		/// <param name="minutesToCache">How many minutes to cache them for</param>
		public void Set(string key, object data, CacheTimes minutesToCache)
		{
			var policy = new CacheItemPolicy
			{
				AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes((int)minutesToCache)
			};
			Cache.Remove(key);
			Cache.Add(new CacheItem(key, data), policy);
		}

		public bool IsSet(string key)
		{
			return Cache[key] != null;
		}

		public void Invalidate(string key)
		{
			Cache.Remove(key);
		}

		public void Clear()
		{
			var keys = new List<string>();
			var enumerator = GetCacheToEnumerate();

			while (enumerator.MoveNext())
			{
				keys.Add(enumerator.Key.ToString());
			}

			foreach (var t in keys)
			{
				Cache.Remove(t);
			}
		}

		public void ClearStartsWith(string keyStartsWith)
		{
			var keys = new List<string>();
			var enumerator = GetCacheToEnumerate();

			while (enumerator.MoveNext())
			{
				keys.Add(enumerator.Key.ToString());
			}

			foreach (var t in keys.Where(x => x.StartsWith(keyStartsWith)))
			{
				Cache.Remove(t);
			}
		}

		public void ClearStartsWith(List<string> keysStartsWith)
		{
			var keys = new List<string>();
			var enumerator = GetCacheToEnumerate();

			while (enumerator.MoveNext())
			{
				keys.Add(enumerator.Key.ToString());
			}
			foreach (var keyStartsWith in keysStartsWith)
			{
				var startsWith = keyStartsWith;
				foreach (var t in keys.Where(x => x.StartsWith(startsWith)))
				{
					Cache.Remove(t);
				}
			}
		}

		public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, CacheTimes minutesToCache) where T : class
		{
			T item = Cache.Get(cacheKey) as T;
			if (item == null)
			{
				item = getItemCallback();
				var policy = new CacheItemPolicy
				{
					AbsoluteExpiration = DateTime.UtcNow + TimeSpan.FromMinutes((int)minutesToCache)
				};
				Cache.Add(cacheKey, item, policy);
			}

			return item;
		}
		#endregion

	}
}
