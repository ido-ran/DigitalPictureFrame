using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Module.Weather.ViewModels;
using DigitalFrame.Service.Core;
using DigitalFrame.Service.Core.Weather;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Module.Weather.UnitTests
{
    [TestFixture]
    public class WeatherViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Load_Settings_From_Repository_Upon_Construction()
        {
            var repository = Mock<IRepository<WeatherSettings>>();

            repository.Expect(r => r.Load()).Return(new WeatherSettings()).Repeat.Once();

            CreateSUT(repository);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void Should_Retrieve_Weather_Upon_Construction()
        {
            var weatherService = Mock<IWeatherService>();

            weatherService.Expect(s => s.GetLatestWeatherReport(Arg<string>.Is.Anything, Arg<UnitsSystems>.Is.Anything)).Repeat.Once();

            CreateSUT(weatherService);

            weatherService.VerifyAllExpectations();
        }

        [Test]
        public void Should_Subscribe_To_SettingsChangedEvent()
        {
            MockEventWithExpectation<SettingsChangedEvent<WeatherSettings>>();

            CreateSUT();

            EventAggregator.VerifyAllExpectations();
        }
        
        [Test]
        public void Should_Retrieve_Weather_When_SettingsChangedEvent_Received()
        {
            const string ZIP_CODE = "90210";

            var settingsChangedEvent = CreateEvent<SettingsChangedEvent<WeatherSettings>>();
            var service = Mock<IWeatherService>();

            var weatherSettings = new WeatherSettings {ZipCode = ZIP_CODE};

            service.Expect(s => s.GetLatestWeatherReport(Arg<string>.Is.Equal(ZIP_CODE), Arg<UnitsSystems>.Is.Equal(UnitsSystems.Imperial))).Repeat.Once();

            CreateSUT(service);

            settingsChangedEvent.Publish(weatherSettings);

            service.VerifyAllExpectations();
        }

        private WeatherViewModel CreateSUT()
        {
            IRepository<WeatherSettings> repository = StubRepository<WeatherSettings>();

            return CreateSUT(repository, Stub<IWeatherService>());
        }

        private WeatherViewModel CreateSUT(IRepository<WeatherSettings> repository)
        {
            return CreateSUT(repository, Stub<IWeatherService>());
        }

        private WeatherViewModel CreateSUT(IWeatherService service)
        {
            IRepository<WeatherSettings> repository = StubRepository<WeatherSettings>();

            return CreateSUT(repository, service);
        }

        private WeatherViewModel CreateSUT(IRepository<WeatherSettings> repository, IWeatherService service)
        {
            return new WeatherViewModel(repository, service, EventAggregator);
        }
    }
}
