using DigitalFrame.Core.Interfaces;
using DigitalFrame.Module.Time.ViewModels;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Module.Time.UnitTests
{
    [TestFixture]
    public class TimeViewModelFixture : FixtureBase
    {
        private static TimeViewModel CreateSUT()
        {
            return new TimeViewModel();    
        }
    }
}
