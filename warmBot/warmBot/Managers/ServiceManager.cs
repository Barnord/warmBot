using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace warmBot.Managers
{
    internal class ServiceManager
    {
        public static IServiceProvider Provider { get; private set; }

        public static void SetProvider(ServiceCollection collection) => Provider = collection.BuildServiceProvider();

        public static T GetService<T>() where T : new() => Provider.GetRequiredService<T>();
    }
}
