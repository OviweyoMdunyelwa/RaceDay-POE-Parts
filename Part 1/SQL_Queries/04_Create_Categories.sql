USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Categories
(
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    CategoryName VARCHAR(100) NOT NULL,

    DistanceKM DECIMAL(5,2) NOT NULL,

    EntryFee DECIMAL(10,2) NOT NULL,

    AgeGroup VARCHAR(50) NULL,

    Description VARCHAR(300) NULL,

    CONSTRAINT CK_Categories_Distance
        CHECK (DistanceKM > 0),

    CONSTRAINT CK_Categories_EntryFee
        CHECK (EntryFee >= 0),

    CONSTRAINT UQ_Categories_Event_Name
        UNIQUE (EventID, CategoryName)
);
GO