﻿using Hangfire.Annotations;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Jobs
{
    public static class HangfireServiceCollectionExtensions
    {

        public static IServiceCollection AddHangfire(
            [NotNull] this IServiceCollection services,
            [NotNull] Action<IGlobalConfiguration> configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            // ===== Configurable services =====

            services.TryAddSingletonChecked(_ => JobStorage.Current);
            services.TryAddSingletonChecked(_ => JobActivator.Current);         
            services.TryAddSingletonChecked<IJobFilterProvider>(_ => JobFilterProviders.Providers);


            // ===== Internal services =====

            // NOTE: these are not required to be checked, because they only depend on already checked configurables,
            //       are not accessed directly, and can't be affected by customizations made from configuration block.

            services.TryAddSingleton<IBackgroundJobFactory>(x => new BackgroundJobFactory(
                x.GetRequiredService<IJobFilterProvider>()));

            services.TryAddSingleton<IBackgroundJobStateChanger>(x => new BackgroundJobStateChanger(
                x.GetRequiredService<IJobFilterProvider>()));

            services.TryAddSingleton<IBackgroundJobPerformer>(x => new BackgroundJobPerformer(
                x.GetRequiredService<IJobFilterProvider>(),
                x.GetRequiredService<JobActivator>()));


            // ===== Client services =====

            // NOTE: these, on the other hand, need to be double-checked to be sure configuration block was executed, 
            //       in case of a client-only scenario with all configurables above replaced with custom implementations.

            services.TryAddSingletonChecked<IBackgroundJobClient>(x => new BackgroundJobClient(
                x.GetRequiredService<JobStorage>(),
                x.GetRequiredService<IBackgroundJobFactory>(),
                x.GetRequiredService<IBackgroundJobStateChanger>()));

            services.TryAddSingletonChecked<IRecurringJobManager>(x => new RecurringJobManager(
                x.GetRequiredService<JobStorage>(),
                x.GetRequiredService<IBackgroundJobFactory>()));


            // IGlobalConfiguration serves as a marker indicating that Hangfire's services 
            // were added to the service container (checked by IApplicationBuilder extensions).
            // 
            // Being a singleton, it also guarantees that the configuration callback will be 
            // executed just once upon initialization, so there's no need to double-check that.
            // 
            // It should never be replaced by another implementation !!!
            // AddSingleton() will throw an exception if it was already registered

            services.AddSingleton<IGlobalConfiguration>(serviceProvider =>
            {
                var configurationInstance = GlobalConfiguration.Configuration;

             
                configurationInstance.UseActivator(new HangfireActivator(serviceProvider));

                configuration(configurationInstance);

                return configurationInstance;
            });


            return services;
        }

        private static void TryAddSingletonChecked<T>(
            [NotNull] this IServiceCollection serviceCollection,
            [NotNull] Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            serviceCollection.TryAddSingleton<T>(serviceProvider =>
            {
                if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

                // ensure the configuration was performed
                serviceProvider.GetRequiredService<IGlobalConfiguration>();

                return implementationFactory(serviceProvider);
            });
        }

    }
}
