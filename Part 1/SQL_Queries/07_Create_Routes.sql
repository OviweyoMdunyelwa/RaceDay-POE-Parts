USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Routes
(
    RouteID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    RouteName VARCHAR(100) NOT NULL,

    DistanceKM DECIMAL(5,2) NOT NULL,

    ElevationGain DECIMAL(7,2) NOT NULL,

    Description VARCHAR(500) NULL,

    MapURL VARCHAR(500) NULL,

    CONSTRAINT CK_Routes_Distance
        CHECK (DistanceKM > 0),

    CONSTRAINT CK_Routes_Elevation
        CHECK (ElevationGain >= 0)
);
GO