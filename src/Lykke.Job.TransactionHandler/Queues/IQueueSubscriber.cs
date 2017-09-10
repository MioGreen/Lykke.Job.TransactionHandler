using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Job.TransactionHandler.Queues
{
    public interface IQueueSubscriber : IDisposable
    {
        void Start();
        void Stop();
    }
}
