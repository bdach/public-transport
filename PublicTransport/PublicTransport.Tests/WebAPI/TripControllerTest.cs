using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer.Filters;
using PublicTransport.Services.Repositories;
using PublicTransport.WebAPI.Controllers;
using PublicTransport.WebAPI.Models;

namespace PublicTransport.Tests.WebAPI
{
    [TestFixture]
    public class TripControllerTest
    {
        private Mock<ITripRepository> _tripRepository;
        private TripController _tripController;

        [SetUp]
        public void SetUp()
        {
            _tripRepository = new Mock<ITripRepository>();
            _tripController = new TripController(_tripRepository.Object);
        }

        [Test]
        public void Search()
        {
            // given
            var searchFilter = new RouteSearchFilter {OriginStopIdFilter = 5, DestinationStopIdFilter = 10};
            var results = new List<Tuple<Trip, StopTime, StopTime>>
            {
                new Tuple<Trip, StopTime, StopTime>
                (
                    new Trip {Route = new Route(), Service = new Calendar()},
                    new StopTime {Stop = new Stop()},
                    new StopTime {Stop = new Stop()}
                )
            };
            _tripRepository.Setup(tr => tr.FindTrips(searchFilter)).Returns(results);
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(searchFilter.GetType(), searchFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var result = _tripController.Search(request, searchFilter) as OkNegotiatedContentResult<List<TripInfo>>;
            // then
            result.Should().NotBeNull();
            result.Content.Count().ShouldBeEquivalentTo(1);
        }

        [Test]
        public void Search_InvalidParameter()
        {
            // given
            var searchFilter = new RouteSearchFilter();
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(searchFilter.GetType(), searchFilter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var result = _tripController.Search(request, searchFilter);
            // then
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [Test]
        public void MapData()
        {
            // given
            var filter = new TripSegmentFilter { TripId = 5, OriginSequenceNumber = 4, DestinationSequenceNumber = 8 };
            var results = new List<StopTime>
            {
                new StopTime {Shape = new Shape {Identifier = "a"} },
                new StopTime {Shape = new Shape {Identifier = "b"} },
                new StopTime {Shape = new Shape {Identifier = "c"} }
            };
            _tripRepository.Setup(tr => tr.GetTripSegment(filter)).Returns(results);
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(filter.GetType(), filter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var result = _tripController.MapData(request, filter) as OkNegotiatedContentResult<List<MapMarker>>;
            // then
            result.Should().NotBeNull();
            result.Content.Count().ShouldBeEquivalentTo(3);
        }

        [Test]
        public void MapData_InvalidParameter()
        {
            // given
            var filter = new TripSegmentFilter { TripId = 5, OriginSequenceNumber = 12, DestinationSequenceNumber = 8 };
            // when
            var request = new HttpRequestMessage
            {
                Content = new ObjectContent(filter.GetType(), filter, new JsonMediaTypeFormatter()),
                Method = HttpMethod.Post
            };
            var result = _tripController.MapData(request, filter);
            // then
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }
    }
}