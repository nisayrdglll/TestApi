using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ServiceActivator
    {
        //TODO: Base Controller'da ki base() yapısını türetebilmek için.
        internal static IServiceProvider _serviceProvider = null;

        /// <summary>
        /// ServiceActivator'ı tüm serviceProvider ile yapılandırıyoruz
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Bu ServiceActivator'larıın kullanıldığı bir kapsam oluşturuyoruz
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }
    }
}
