using PolymorphicWebAPI.Domain.Entities;
using PolymorphicWebAPI.Service.Features.MessageBroker;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolymorphicWebAPI.Service.Features.Worker
{
    public class MQWorker: BackgroundService
    {
       
        private readonly MessageQueueOptions _messageQueueOptions;
        private readonly MQConsumerFactory _mQConsumerFactory;
        private readonly ILogger<MQWorker> _logger;
        public MQWorker(IMediator mediator, MessageQueueOptions messageQueueOptions, ILogger<MQWorker> logger)
        {
            IMediator _mediator = mediator;
            _messageQueueOptions = messageQueueOptions;
            _mQConsumerFactory = new MQConsumerFactory(_mediator, _messageQueueOptions);
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           
            try
            {
                await Task.Delay(10000, stoppingToken);
                while (!stoppingToken.IsCancellationRequested)
                {
                    await _mQConsumerFactory.Create(_messageQueueOptions.Provider).Consumer();

                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch(Exception ex)
            {

                _logger.LogError("{Error} occured", ex.ToString());
              

            }

            


        }

       
    }
}
