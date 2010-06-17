using System;

namespace DigitalFrame.Core
{
    public class EventArgs<TPayload> : EventArgs
    {
        public TPayload Payload { get; set; }
    }
}