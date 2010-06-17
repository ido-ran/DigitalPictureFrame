using DigitalFrame.Core.Interfaces;
using Microsoft.Practices.Composite.Events;
using NUnit.Framework;
using Rhino.Mocks;

namespace DigitalFrame.Tests.Core
{
    public class FixtureBase
    {
        protected IEventAggregator EventAggregator { get; set; }

        [SetUp]
        public void SetUp()
        {
            EventAggregator = Mock<IEventAggregator>();
        }

        protected static T Stub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        protected static T Mock<T>() where T : class
        {
            return MockRepository.GenerateMock<T>();
        }

        protected T MockEventWithExpectation<T>() where T : EventBase
        {
            var @event = Mock<T>();

            MockGetEvent(@event);

            return @event;
        }
        
        protected T MockEvent<T>() where T : EventBase
        {
            var @event = Mock<T>();

            StubGetEvent(@event);

            return @event;
        }

        protected T CreateEvent<T>() where T : EventBase, new()
        {
            var @event = new T();

            StubGetEvent(@event);

            return @event;
        }

        private void MockGetEvent<T>(T @event) where T : EventBase
        {
            EventAggregator.Expect(a => a.GetEvent<T>()).Return(@event);
        }

        private void StubGetEvent<T>(T @event) where T : EventBase
        {
            EventAggregator.Stub(a => a.GetEvent<T>()).Return(@event);
        }

        protected static IRepository<T> StubRepository<T>() where T : new()
        {
            var repository = Mock<IRepository<T>>();

            repository.Stub(r => r.Load()).Return(new T());
        
            return repository;
        }
    }
}
