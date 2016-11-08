using System;

namespace PublicTransport.Services.DataTransfer
{
    public interface IStopTimeFilter
    {
        int StopId { get; }
        int RouteId { get; }
        DateTime? Date { get; }
        TimeSpan? Time { get; }
    }
}