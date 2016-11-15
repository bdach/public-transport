using Moq;
using NUnit.Framework;
using ReactiveUI;

namespace PublicTransport.Tests.Client
{
    public abstract class RoutableViewModelTest
    {
        protected Mock<IScreen> Screen = new Mock<IScreen>();
        protected RoutingState Router = new RoutingState();

        [SetUp]
        public void RouterSetUp()
        {
            Screen.Setup(s => s.Router).Returns(Router);
        }
    }
}
