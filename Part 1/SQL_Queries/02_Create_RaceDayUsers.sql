USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.RaceDayUsers
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,

    FirstName VARCHAR(50) NOT NULL,

    LastName VARCHAR(50) NOT NULL,

    Email VARCHAR(100) NOT NULL UNIQUE,

    PasswordHash VARCHAR(255) NOT NULL,

    Role VARCHAR(20) NOT NULL,

    PhoneNumber VARCHAR(20) NULL,

    CreatedAt DATETIME NOT NULL
        DEFAULT GETDATE(),

    CONSTRAINT CK_RaceDayUsers_Role
        CHECK (Role IN ('Organiser', 'Participant'))
);
GO