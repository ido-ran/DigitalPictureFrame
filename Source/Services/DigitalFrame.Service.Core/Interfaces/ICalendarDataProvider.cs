namespace DigitalFrame.Service.Core
{
    public interface ICalendarDataProvider
    {
        bool Authorize(string userName, string password);
    }
}