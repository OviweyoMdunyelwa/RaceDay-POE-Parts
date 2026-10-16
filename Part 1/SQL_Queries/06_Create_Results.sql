USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Results
(
    ResultID INT IDENTITY(1,1) PRIMARY KEY,

    EnrolmentID INT NOT NULL,

    FinishTime TIME NULL,

    ChipTime TIME NULL,

    PositionOverall INT NULL,

    PositionCategory INT NULL,

    Pace DECIMAL(5,2) NULL,

    Status VARCHAR(20) NOT NULL
        DEFAULT 'Finished',

    CONSTRAINT UQ_Results_Enrolment
        UNIQUE (EnrolmentID),

    CONSTRAINT CK_Results_PositionOverall
        CHECK (PositionOverall IS NULL OR PositionOverall > 0),

    CONSTRAINT CK_Results_PositionCategory
        CHECK (PositionCategory IS NULL OR PositionCategory > 0),

    CONSTRAINT CK_Results_Pace
        CHECK (Pace IS NULL OR Pace > 0),

    CONSTRAINT CK_Results_Status
        CHECK (Status IN
        ('Finished', 'DNF', 'DNS', 'Disqualified'))
);
GO