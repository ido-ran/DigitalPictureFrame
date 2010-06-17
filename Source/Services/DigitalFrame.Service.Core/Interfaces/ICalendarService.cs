using System.Collections.Generic;
using DigitalFrame.Service.Core.Calendar;

namespace DigitalFrame.Service.Core
{
    public interface ICalendarService
    {
        bool Authorize(string userName, string password);
    }
}