﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Application.Interfaces
{
    public interface IMessageProducer
    {
        void SendMessage<T>(string queueName, T message);
    }
}
