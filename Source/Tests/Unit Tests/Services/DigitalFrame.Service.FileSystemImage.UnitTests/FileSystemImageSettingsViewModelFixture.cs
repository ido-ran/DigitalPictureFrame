using DigitalFrame.Core;
using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Service.FileSystemImage.ViewModels;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Service.FileSystemImage.UnitTests
{
    [TestFixture]
    public class FileSystemImageSettingsViewModelFixture : FixtureBase
    {
        [Test]
        public void Should_Inherit_From_SettingsViewModel()
        {
            var viewModel = CreateSUT();

            Assert.IsInstanceOfType(typeof(SettingsViewModel<FileSystemImageSettings>), viewModel);
        }

        [Test]
        public void Should_Request_Settings_From_Repository_When_LoadSettingsEvent_Received()
        {
            var loadSettingsEvent = CreateEvent<LoadSettingsEvent>();
            var repository = Mock<IRepository<FileSystemImageSettings>>();

            repository.Expect(r => r.Load()).Return(new FileSystemImageSettings());

            CreateSUT(repository);

            loadSettingsEvent.Publish(null);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void Should_Save_Settings_To_Repository_When_SaveSettingsEvent_Received()
        {
            const string PATH = @"C:\Pictures";

            var saveSettingsEvent = CreateEvent<SaveSettingsEvent>();
            var repository = Mock<IRepository<FileSystemImageSettings>>();

            repository.Expect(r => r.Save(Arg<FileSystemImageSettings>.Matches(ws => ws.Path == PATH))).Repeat.Once();

            var viewModel = CreateSUT(repository);
            viewModel.Path = PATH;

            saveSettingsEvent.Publish(null);

            repository.VerifyAllExpectations();            
        }

        [Test]
        public void Should_Publish_SettingsChangedEvent_When_SaveSettingsEvent_Received()
        {
            const string PATH = @"C:\Pictures";

            var settingsChangedEvent = MockEvent<SettingsChangedEvent<FileSystemImageSettings>>();
            var saveSettingsEvent = CreateEvent<SaveSettingsEvent>();

            settingsChangedEvent.Expect(e => e.Publish(Arg<FileSystemImageSettings>.Matches(ws => ws.Path == PATH))).Repeat.Once();

            var viewModel = CreateSUT();
            viewModel.Path = PATH;

            saveSettingsEvent.Publish(null);

            settingsChangedEvent.VerifyAllExpectations();
        }

        [Test]
        public void Should_Expose_Path_Retrieved_From_Repository()
        {
            const string PATH = @"C:\Pictures";

            var repository = Mock<IRepository<FileSystemImageSettings>>();

            repository.Expect(r => r.Load()).Return(new FileSystemImageSettings() { Path = PATH });

            var viewModel = CreateSUT(repository);
            viewModel.LoadSettings();

            Assert.AreEqual(PATH, viewModel.Path);
        }

        private FileSystemImageSettingsViewModel CreateSUT()
        {
            return CreateSUT(Stub<IRepository<FileSystemImageSettings>>());
        }

        private FileSystemImageSettingsViewModel CreateSUT(IRepository<FileSystemImageSettings> repository)
        {
            return new FileSystemImageSettingsViewModel(repository, EventAggregator);
        }
    }
}
