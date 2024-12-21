using System;
using UnityEngine.Assertions;

namespace Utility
{
    public sealed class DIContainer
    {
        public static IDisposable Inject<T>(T instance)
        {
            return Cache<T>.Register(instance);
        }

        public static T Get<T>()
        {
            return Cache<T>.Get();
        }

        sealed class Cache<T> : IDisposable
        {
            private static Cache<T> s_cache;
            private readonly T _instance;

            public Cache(T instance)
            {
                _instance = instance;
            }

            public static IDisposable Register(T instance)
            {
                Assert.IsNull(s_cache);
                return s_cache = new Cache<T>(instance);
            }

            public static T Get()
            {
                Assert.IsNotNull(s_cache);
                return s_cache._instance;
            }

            public void Dispose()
            {
                s_cache?.Dispose();
                s_cache = null;
            }
        }
    }
}