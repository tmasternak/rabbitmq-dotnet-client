﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using RabbitMQ.Client.Events;

namespace RabbitMQ.Client.Impl
{
    sealed class BasicCancel : Work
    {
        readonly string _consumerTag;

        public BasicCancel(IBasicConsumer consumer, string consumerTag) : base(consumer)
        {
            _consumerTag = consumerTag;
        }

        protected override async Task Execute(IModel model, IAsyncBasicConsumer consumer)
        {
            try
            {
                await consumer.HandleBasicCancel(_consumerTag).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                if (!(model is ModelBase modelBase))
                {
                    return;
                }

                var details = new Dictionary<string, object>
                {
                    {"consumer", consumer},
                    {"context",  "HandleBasicCancel"}
                };
                modelBase.OnCallbackException(CallbackExceptionEventArgs.Build(e, details));
            }
        }
    }
}
