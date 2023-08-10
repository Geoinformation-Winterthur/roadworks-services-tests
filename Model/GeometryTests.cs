using Xunit;
using roadwork_portal_service.Model;

namespace roadworks_portal_service_tests.Model;

public class GeometryTests
{
    [Fact]
    public void TestEmptyRoadWorkPolygon()
    {
        RoadworkPolygon roadWorkPolygon = new RoadworkPolygon();
        Assert.Equal(roadWorkPolygon.coordinates.Length, 0);
    }
}