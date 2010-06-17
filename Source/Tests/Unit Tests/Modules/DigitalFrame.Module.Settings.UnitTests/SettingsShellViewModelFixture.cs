using DigitalFrame.Infrastructure;
using DigitalFrame.Module.Settings.ViewModels;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Module.Settings.UnitTests
{
    [TestFixture]
    public class SettingsShellViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Subscribe_To_DisplaySettingsEvent()
        {
            MockEventWithExpectation<DisplaySettingsEvent>();
            
            CreateSUT();

            EventAggregator.VerifyAllExpectations();
        }

        [Test]
        public void Should_Set_IsVisible_To_True_When_DisplaySettingsEvent_Received()
        {
            var displaySettingsEvent = CreateEvent<DisplaySettingsEvent>();
            MockEvent<LoadSettingsEvent>();

            var viewModel = CreateSUT();

            displaySettingsEvent.Publish(null);

            Assert.IsTrue(viewModel.IsVisible);
        }

        [Test]
        public void Should_Publish_LoadSettingsEvent_When_IsVisible_Is_Set_To_True()
        {
            MockEvent<DisplaySettingsEvent>();
            var loadSettingsEvent = MockEvent<LoadSettingsEvent>();

            loadSettingsEvent.Expect(e => e.Publish(null)).Repeat.Once();

            var viewModel = CreateSUT();

            viewModel.IsVisible = true;

            loadSettingsEvent.VerifyAllExpectations();
        }

        [Test]
        public void Should_Not_Publish_LoadSettingsEvent_When_IsVisible_Is_Set_To_False()
        {
            MockEvent<DisplaySettingsEvent>();
            var loadSettingsEvent = MockEvent<LoadSettingsEvent>();

            // Should Only receive the Event one time, because we initially have to set IsVisible to true;
            loadSettingsEvent.Expect(e => e.Publish(null)).Repeat.Once();

            var viewModel = CreateSUT();

            viewModel.IsVisible = true;
            viewModel.IsVisible = false;

            loadSettingsEvent.VerifyAllExpectations();
        }

        [Test]
        public void Should_Publish_SaveSettingsEvent_When_SaveCommand_Executed()
        {
            MockEvent<DisplaySettingsEvent>();
            var saveSettingsEvent = MockEvent<SaveSettingsEvent>();

            saveSettingsEvent.Expect(e => e.Publish(null)).Repeat.Once();

            var viewModel = CreateSUT();

            viewModel.SaveCommand.Execute(null);

            saveSettingsEvent.VerifyAllExpectations();
        }

        [Test]
        public void Should_Set_IsVisible_To_False_When_CancelCommand_Executed()
        {
            MockEvent<DisplaySettingsEvent>();

            var viewModel = CreateSUT();

            viewModel.CancelCommand.Execute(null);

            Assert.IsFalse(viewModel.IsVisible);
        }

        private SettingsShellViewModel CreateSUT()
        {
            return new SettingsShellViewModel(EventAggregator);
        }
    }
}