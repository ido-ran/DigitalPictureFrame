using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Module.Weather.ViewModels;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Module.Weather.UnitTests
{
    [TestFixture]
    public class WeatherSettingsViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Inherit_From_SettingsViewModel()
        {
            var viewModel = CreateSUT();

            Assert.IsInstanceOfType(typeof(SettingsViewModel<WeatherSettings>), viewModel);
        }

        [Test]
        public void Should_Request_Settings_From_Repository_When_LoadSettingsEvent_Received()
        {
            var loadSettingsEvent = CreateEvent<LoadSettingsEvent>();
            var repository = Mock<IRepository<WeatherSettings>>();

            repository.Expect(r => r.Load()).Return(new WeatherSettings());

            var viewModel = CreateSUT(repository);

            loadSettingsEvent.Publish(null);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void Should_Save_Settings_To_Repository_When_SaveSettingsEvent_Received()
        {
            const string ZIP_CODE = "90210";

            var saveSettingsEvent = CreateEvent<SaveSettingsEvent>();
            var repository = Mock<IRepository<WeatherSettings>>();

            repository.Expect(r => r.Save(Arg<WeatherSettings>.Matches(ws => ws.ZipCode == ZIP_CODE))).Repeat.Once();

            var viewModel = CreateSUT(repository);
            viewModel.ZipCode = ZIP_CODE;

            saveSettingsEvent.Publish(null);

            repository.VerifyAllExpectations();            
        }

        [Test]
        public void Should_Publish_SettingsChangedEvent_When_SaveSettingsEvent_Received()
        {
            const string ZIP_CODE = "90210";

            var settingsChangedEvent = MockEvent<SettingsChangedEvent<WeatherSettings>>();
            var saveSettingsEvent = CreateEvent<SaveSettingsEvent>();

            settingsChangedEvent.Expect(e => e.Publish(Arg<WeatherSettings>.Matches(ws => ws.ZipCode == ZIP_CODE))).Repeat.Once();

            var viewModel = CreateSUT();
            viewModel.ZipCode = ZIP_CODE;

            saveSettingsEvent.Publish(null);

            settingsChangedEvent.VerifyAllExpectations();
        }

        [Test]
        public void Should_Expose_ZipCode_Retrieved_From_Repository()
        {
            const string ZIP_CODE = "90210";

            var repository = Mock<IRepository<WeatherSettings>>();

            repository.Expect(r => r.Load()).Return(new WeatherSettings() { ZipCode = ZIP_CODE });

            var viewModel = CreateSUT(repository);
            viewModel.LoadSettings();

            Assert.AreEqual(ZIP_CODE, viewModel.ZipCode);
        }

        private WeatherSettingsViewModel CreateSUT()
        {
            return CreateSUT(Stub<IRepository<WeatherSettings>>());
        }

        private WeatherSettingsViewModel CreateSUT(IRepository<WeatherSettings> repository)
        {
            return new WeatherSettingsViewModel(repository, EventAggregator);
        }
    }
}
