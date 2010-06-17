using DigitalFrame.Infrastructure;
using DigitalFrame.Tests.Core;
using DigitalFrame.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.UnitTests
{
    [TestFixture]
    public class ShellViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Publish_DisplaySettingsEvent_When_SettingsCommand_Executed()
        {
            var displaySettingsEvent = MockEvent<DisplaySettingsEvent>();

            displaySettingsEvent.Expect(dse => dse.Publish(Arg<object>.Is.Anything));

            var viewModel = CreateSUT();

            viewModel.SettingsCommand.Execute(null);

            displaySettingsEvent.VerifyAllExpectations();
        }

        private ShellViewModel CreateSUT()
        {
            return new ShellViewModel(EventAggregator);
        }
    }
}
