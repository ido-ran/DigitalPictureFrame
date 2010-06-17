using System;
using DigitalFrame.Core;

namespace DigitalFrame.Service.Core.Calendar
{
    public class CalendarEvent : NotifyPropertyChangedBase
    {
        public DateTime StartTime { get; set; }
    }
}