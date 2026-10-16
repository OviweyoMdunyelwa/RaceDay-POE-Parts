USE RaceDay_PoE_Final;
GO


/* =========================================================
   EVENTS → RACEDAYUSERS
   One organiser can manage many events
   ========================================================= */

ALTER TABLE dbo.Events
ADD CONSTRAINT FK_Events_Organiser
FOREIGN KEY (OrganiserID)
REFERENCES dbo.RaceDayUsers(UserID);
GO


/* =========================================================
   CATEGORIES → EVENTS
   One event can have many categories
   ========================================================= */

ALTER TABLE dbo.Categories
ADD CONSTRAINT FK_Categories_Event
FOREIGN KEY (EventID)
REFERENCES dbo.Events(EventID);
GO


/* =========================================================
   ENROLMENTS → EVENTS
   One event can have many enrolments
   ========================================================= */

ALTER TABLE dbo.Enrolments
ADD CONSTRAINT FK_Enrolments_Event
FOREIGN KEY (EventID)
REFERENCES dbo.Events(EventID);
GO


/* =========================================================
   ENROLMENTS → CATEGORIES
   One category can have many enrolments
   ========================================================= */

ALTER TABLE dbo.Enrolments
ADD CONSTRAINT FK_Enrolments_Category
FOREIGN KEY (CategoryID)
REFERENCES dbo.Categories(CategoryID);
GO


/* =========================================================
   ENROLMENTS → RACEDAYUSERS
   One participant can have many enrolments
   ========================================================= */

ALTER TABLE dbo.Enrolments
ADD CONSTRAINT FK_Enrolments_Participant
FOREIGN KEY (ParticipantID)
REFERENCES dbo.RaceDayUsers(UserID);
GO


/* =========================================================
   RESULTS → ENROLMENTS
   One enrolment can have one result
   ========================================================= */

ALTER TABLE dbo.Results
ADD CONSTRAINT FK_Results_Enrolment
FOREIGN KEY (EnrolmentID)
REFERENCES dbo.Enrolments(EnrolmentID);
GO


/* =========================================================
   ROUTES → EVENTS
   An event can have routes
   ========================================================= */

ALTER TABLE dbo.Routes
ADD CONSTRAINT FK_Routes_Event
FOREIGN KEY (EventID)
REFERENCES dbo.Events(EventID);
GO


/* =========================================================
   WEATHER → EVENTS
   An event can have weather records
   ========================================================= */

ALTER TABLE dbo.Weather
ADD CONSTRAINT FK_Weather_Event
FOREIGN KEY (EventID)
REFERENCES dbo.Events(EventID);
GO