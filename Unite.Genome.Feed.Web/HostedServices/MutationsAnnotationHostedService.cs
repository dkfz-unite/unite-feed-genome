﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Unite.Genome.Feed.Web.Configuration.Options;
using Unite.Genome.Feed.Web.Handlers;

namespace Unite.Genome.Feed.Web.HostedServices
{
    public class MutationsAnnotationHostedService : BackgroundService
    {
        private readonly MutationsAnnotationOptions _options;
        private readonly MutationsAnnotationHandler _handler;
        private readonly ILogger _logger;


        public MutationsAnnotationHostedService(
            MutationsAnnotationOptions options,
            MutationsAnnotationHandler handler,
            ILogger<MutationsAnnotationHostedService> logger)
        {
            _options = options;
            _handler = handler;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SSM annotation service started");

            stoppingToken.Register(() => _logger.LogInformation("SSM annotation service stopped"));

            // Delay 5 seconds to let the web api start working
            await Task.Delay(5000, stoppingToken);

            try
            {
                _handler.Prepare();
            }
            catch (Exception exception)
            {
                LogError(exception);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _handler.Handle(_options.BucketSize);
                }
                catch (Exception exception)
                {
                    LogError(exception);
                }
                finally
                {
                    await Task.Delay(_options.Interval, stoppingToken);
                }
            }
        }

        private void LogError(Exception exception)
        {
            _logger.LogError(exception.Message);

            if (exception.InnerException != null)
            {
                _logger.LogError(exception.InnerException.Message);
            }
        }
    }
}
