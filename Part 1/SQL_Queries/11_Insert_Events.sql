USE RaceDay_PoE_Final;
GO

INSERT INTO dbo.Events
(
    OrganiserID,
    EventName,
    Description,
    EventDate,
    Location,
    RegistrationDeadline,
    Status,
    CreatedAt
)
VALUES
(
    1,
    'Johannesburg City Run',
    'A road running event through central Johannesburg.',
    '2026-10-10 06:00:00',
    'Johannesburg, Gauteng',
    '2026-10-01 23:59:59',
    'Open',
    GETDATE()
),
(
    2,
    'Durban Coastal Cycle',
    'A scenic cycling event along the Durban coastline.',
    '2026-11-07 06:30:00',
    'Durban, KwaZulu-Natal',
    '2026-10-28 23:59:59',
    'Open',
    GETDATE()
),
(
    1,
    'Cape Town Community Run',
    'A community road running and walking event.',
    '2026-12-05 07:00:00',
    'Cape Town, Western Cape',
    '2026-11-25 23:59:59',
    'Upcoming',
    GETDATE()
);
GO