﻿using Application.Events;

namespace Collector.Application.Events;

public class ArticlesEmailReceivedEvent(Ulid receivedEmailId) : IEvent
{
    public Ulid ReceivedEmailId { get; } = receivedEmailId;
}