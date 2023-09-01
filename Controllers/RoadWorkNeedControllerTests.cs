using Xunit;
using Moq;
using roadwork_portal_service.Model;
using roadwork_portal_service.DAO;

namespace roadworks_portal_service_tests.Controllers;

public class RoadWorkNeedControllerTests
{
    private const bool isDryRun = true;

    [Fact]
    public void DryRunOfAddNeedsShouldTriggerErrorMessage()
    {
        Mock roadWorkNeedFeatureMock = new Mock<RoadWorkNeedFeature>();
        RoadWorkNeedFeature mockedRoadWorkNeedFeature = (RoadWorkNeedFeature)roadWorkNeedFeatureMock.Object;
        mockedRoadWorkNeedFeature.properties.uuid = "18f08e1f-e6f5-413c-b336-d981bc563728";
        mockedRoadWorkNeedFeature.properties.description = "Test description";
        mockedRoadWorkNeedFeature.properties.kind.code = "buslanes";

        mockedRoadWorkNeedFeature.geometry.coordinates = new RoadworkCoordinate[5];
        mockedRoadWorkNeedFeature.geometry.coordinates[0] = new RoadworkCoordinate(2696199.5273826, 1263808.51504176);
        mockedRoadWorkNeedFeature.geometry.coordinates[1] = new RoadworkCoordinate(2696208.05078157, 1263784.83893354);
        mockedRoadWorkNeedFeature.geometry.coordinates[2] = new RoadworkCoordinate(2696317.43440156, 1263837.3998938);
        mockedRoadWorkNeedFeature.geometry.coordinates[3] = new RoadworkCoordinate(2696309.38452477, 1263861.54952418);
        mockedRoadWorkNeedFeature.geometry.coordinates[4] = mockedRoadWorkNeedFeature.geometry.coordinates[0];

        Mock configurationMock = new Mock<ConfigurationData>();
        ConfigurationData mockedConfiguration = (ConfigurationData)configurationMock.Object;
        mockedConfiguration.minAreaSize = 10;
        mockedConfiguration.maxAreaSize = 100000;

        RoadWorkNeedDAO roadWorkNeedDAO = new(isDryRun);
        RoadWorkNeedFeature roadWorkNeedResult = roadWorkNeedDAO.Insert(mockedRoadWorkNeedFeature, mockedConfiguration);

        Assert.NotNull(roadWorkNeedResult);
        if (roadWorkNeedResult != null)
        {
            // result should be SSP-25 error since the
            // roadwork need object is complete
            // but the isDryRun attribute is set to true:
            Assert.Equal("SSP-25", roadWorkNeedResult.errorMessage);
        }

    }

    [Fact]
    public void InsertNeedShouldThrowErrorWhenRoadWorkNeedIsEmpty()
    {
        Mock configurationMock = new Mock<ConfigurationData>();
        ConfigurationData mockedConfiguration = (ConfigurationData)configurationMock.Object;

        RoadWorkNeedDAO roadWorkNeedDAO = new(isDryRun);
        RoadWorkNeedFeature roadWorkNeedResult = roadWorkNeedDAO.Insert(null, mockedConfiguration);

        Assert.NotNull(roadWorkNeedResult);
        Assert.NotNull(roadWorkNeedResult.errorMessage);
        
        // result should be SSP-22 error since the
        // roadwork need is null:
        Assert.Equal("SSP-22", roadWorkNeedResult.errorMessage);

        Mock roadWorkNeedFeatureMock = new Mock<RoadWorkNeedFeature>();
        RoadWorkNeedFeature mockedRoadWorkNeedFeature = (RoadWorkNeedFeature)roadWorkNeedFeatureMock.Object;

        roadWorkNeedResult = roadWorkNeedDAO.Insert(mockedRoadWorkNeedFeature, mockedConfiguration);

        Assert.NotNull(roadWorkNeedResult);
        if (roadWorkNeedResult != null)
        {
            // result should be SSP-23 error since the
            // roadwork need description is mandatory:
            Assert.Equal("SSP-23", roadWorkNeedResult.errorMessage);
        }

    }

}
