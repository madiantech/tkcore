using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System;
using System.Text;

namespace RedisTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RedisCache cache = new RedisCache(new RedisCacheOptions
            {
                Configuration = "192.168.2.21",
                InstanceName = "hello"
            });
            var s = "hello, world!";
            byte[] data = Encoding.UTF8.GetBytes(s);
            //cache.Set("key", data);

            data = cache.Get("key");
            Console.WriteLine(Encoding.UTF8.GetString(data));

            RedisCache cache1 = new RedisCache(new RedisCacheOptions
            {
                Configuration = "192.168.2.21",
                InstanceName = "world"
            });
            data = cache1.Get("key");
            s = "Delphi is Good";
            data = Encoding.UTF8.GetBytes(s);
            //cache1.Set("key", data);
            data = cache1.Get("key");
            Console.WriteLine(Encoding.UTF8.GetString(data));

            data = cache.Get("key");
            Console.WriteLine(Encoding.UTF8.GetString(data));

            Console.WriteLine("Hello World!");
        }
    }
}