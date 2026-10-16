USE RaceDay_PoE_Final;
GO

CREATE TABLE dbo.Weather
(
    WeatherID INT IDENTITY(1,1) PRIMARY KEY,

    EventID INT NOT NULL,

    ForecastDate DATE NOT NULL,

    Temperature DECIMAL(5,2) NOT NULL,

    Conditions VARCHAR(100) NOT NULL,

    WindSpeed DECIMAL(5,2) NOT NULL,

    Humidity DECIMAL(5,2) NOT NULL,

    CONSTRAINT CK_Weather_WindSpeed
        CHECK (WindSpeed >= 0),

    CONSTRAINT CK_Weather_Humidity
        CHECK (Humidity >= 0 AND Humidity <= 100)
);
GO