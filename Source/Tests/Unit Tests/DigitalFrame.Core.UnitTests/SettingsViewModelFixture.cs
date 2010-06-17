using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Tests.Core;
using Microsoft.Practices.Composite.Events;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Core.UnitTests
{
    [TestFixture]
    public class SettingsViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Subscribe_To_LoadSettingsEvent()
        {
            var loadSettingsEvent = MockEventWithExpectation<LoadSettingsEvent>();
            MockEvent<SaveSettingsEvent>();

            CreateSUT();

            EventAggregator.VerifyAllExpectations();
        }

        [Test]
        public void Should_Subscribe_To_SaveSettingsEvent()
        {
            MockEvent<LoadSettingsEvent>();
            var saveSettingsEvent = MockEventWithExpectation<SaveSettingsEvent>();

            CreateSUT();

            EventAggregator.VerifyAllExpectations();
        }

        private TestSettingsViewModel CreateSUT()
        {
            return new TestSettingsViewModel(Stub<IRepository<TestSettings>>(), EventAggregator);
        }

    }

    public class TestSettingsViewModel : SettingsViewModel<TestSettings>
    {
        public TestSettingsViewModel(IRepository<TestSettings> repository, IEventAggregator eventAggregator) : base(repository, eventAggregator)
        {
        }

        protected override void OnSaveSettings(object obj)
        {
            
        }

        protected override void OnLoadSettings(object obj)
        {
        }
    }

    public class TestSettings
    {
        
    }
}
