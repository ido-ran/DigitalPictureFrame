using DigitalFrame.Core.Interfaces;
using DigitalFrame.Infrastructure;
using DigitalFrame.Tests.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Service.FileSystemImage.UnitTests
{
    [TestFixture]
    public class FileSystemImageServiceFixture : FixtureBase
    {
        [Test]
        public void Should_Load_Settings_From_Repository_Upon_Construction()
        {
            var repository = Mock<IRepository<FileSystemImageSettings>>();

            repository.Expect(r => r.Load()).Return(new FileSystemImageSettings()).Repeat.Once();

            CreateSUT(repository);

            repository.VerifyAllExpectations();
        }

        [Test]
        public void Should_Subscribe_To_SettingsChangedEvent()
        {
            MockEventWithExpectation<SettingsChangedEvent<FileSystemImageSettings>>();

            CreateSUT();

            EventAggregator.VerifyAllExpectations();
        }

        [Test]
        public void Should_Refresh_ImageList_When_SettingsChangedEvent_Received()
        {
            // Not Sure how to test this yet...
        }

        private FileSystemImageService CreateSUT()
        {
            IRepository<FileSystemImageSettings> repository = StubRepository<FileSystemImageSettings>();

            return CreateSUT(repository);
        }

        private FileSystemImageService CreateSUT(IRepository<FileSystemImageSettings> repository)
        {
            return new FileSystemImageService(repository, EventAggregator);
        }

    }
}
