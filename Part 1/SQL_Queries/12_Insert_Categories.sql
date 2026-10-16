USE RaceDay_PoE_Final;
GO

INSERT INTO dbo.Categories
(
    EventID,
    CategoryName,
    DistanceKM,
    EntryFee,
    AgeGroup,
    Description
)
VALUES
(
    3,
    '10 KM Run',
    10.00,
    180.00,
    '16+',
    'Competitive 10 kilometre road race.'
),
(
    3,
    '5 KM Fun Run',
    5.00,
    100.00,
    'All Ages',
    'Short community fun run.'
),
(
    4,
    '50 KM Cycle',
    50.00,
    350.00,
    '18+',
    'Long-distance coastal cycling event.'
),
(
    4,
    '20 KM Cycle',
    20.00,
    220.00,
    '16+',
    'Shorter cycling category.'
),
(
    5,
    '21 KM Half Marathon',
    21.10,
    250.00,
    '18+',
    'Half marathon road race.'
),
(
    5,
    '10 KM Run',
    10.00,
    150.00,
    '16+',
    'Community 10 kilometre run.'
),
(
    5,
    '5 KM Walk',
    5.00,
    80.00,
    'All Ages',
    'Non-competitive community walk.'
);
GO