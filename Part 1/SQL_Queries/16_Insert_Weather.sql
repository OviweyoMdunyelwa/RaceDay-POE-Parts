USE RaceDay_PoE_Final;
GO

INSERT INTO dbo.Weather
(
    EventID,
    ForecastDate,
    Temperature,
    Conditions,
    WindSpeed,
    Humidity
)
VALUES
(
    3,
    '2026-10-10',
    22.50,
    'Sunny',
    12.00,
    45.00
),
(
    4,
    '2026-11-07',
    25.00,
    'Partly Cloudy',
    18.00,
    62.00
),
(
    5,
    '2026-12-05',
    20.50,
    'Clear',
    10.00,
    50.00
);
GO