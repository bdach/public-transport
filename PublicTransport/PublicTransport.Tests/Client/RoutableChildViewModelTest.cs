using Moq;
using NUnit.Framework;
using ReactiveUI;

namespace PublicTransport.Tests.Client
{
    public abstract class RoutableChildViewModelTest : RoutableViewModelTest
    {
        [SetUp]
        public void ScreenStackSetUp()
        {
            var routableMock = new Mock<IRoutableViewModel>();
            Router.NavigationStack.Add(routableMock.Object);
        }
    }
}
