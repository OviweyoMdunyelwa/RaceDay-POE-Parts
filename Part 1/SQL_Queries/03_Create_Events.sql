USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Events
(
    EventID INT IDENTITY(1,1) PRIMARY KEY,

    OrganiserID INT NOT NULL,

    EventName VARCHAR(100) NOT NULL,

    Description VARCHAR(500) NULL,

    EventDate DATETIME NOT NULL,

    Location VARCHAR(150) NOT NULL,

    RegistrationDeadline DATETIME NOT NULL,

    Status VARCHAR(20) NOT NULL DEFAULT 'Upcoming',

    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT CK_Events_Status
        CHECK (Status IN
        ('Upcoming', 'Open', 'Closed', 'Completed', 'Cancelled'))
);
GO