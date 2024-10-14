﻿using Application.Events;
using Integration;

namespace Collector.Application.Events;

public class ConfirmationEmailReceivedEvent(Ulid receivedEmailId) : IEvent
{
    public Ulid ReceivedEmailId { get; } = receivedEmailId;
}