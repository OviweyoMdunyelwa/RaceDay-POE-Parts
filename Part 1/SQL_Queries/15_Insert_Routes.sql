USE RaceDay_PoE_Final;
GO

INSERT INTO dbo.Routes
(
    EventID,
    RouteName,
    DistanceKM,
    ElevationGain,
    Description,
    MapURL
)
VALUES
(
    3,
    'Johannesburg City Loop',
    10.00,
    120.00,
    'Urban loop through central Johannesburg.',
    'https://maps.example.com/johannesburg-city-loop'
),
(
    4,
    'Durban Coastal Route',
    50.00,
    280.00,
    'Coastal cycling route with moderate elevation.',
    'https://maps.example.com/durban-coastal-route'
),
(
    5,
    'Cape Town Community Route',
    21.10,
    190.00,
    'Road route through selected Cape Town communities.',
    'https://maps.example.com/cape-town-community-route'
);
GO