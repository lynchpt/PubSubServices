﻿using PubSubServices.Model.PubSub;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubSubServices.Data.MessageSource.Interfaces
{
    public interface IOutgoingMessageSource
    {
        IList<OutgoingPubSubMessageDescription> GetOutgoingMessages();
    }
}
