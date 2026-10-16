USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Enrolments
(
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    CategoryID INT NOT NULL,

    ParticipantID INT NOT NULL,

    EnrolmentDate DATETIME NOT NULL
        DEFAULT GETDATE(),

    RaceNumber VARCHAR(20) NOT NULL,

    PaymentStatus VARCHAR(20) NOT NULL
        DEFAULT 'Pending',

    CONSTRAINT CK_Enrolments_PaymentStatus
        CHECK (PaymentStatus IN
        ('Pending', 'Paid', 'Refunded', 'Cancelled')),

    CONSTRAINT UQ_Enrolments_RaceNumber
        UNIQUE (EventID, RaceNumber)
);
GO