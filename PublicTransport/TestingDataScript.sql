-- Sample data script.
-- Clears database contents and populates the database with sample data for testing purposes.

USE [PublicTransport]
GO

-- Clears data from the database.

-- User data.
DELETE FROM [UserRoles]
DELETE FROM [Roles]
DELETE FROM [Users]

-- Connection data.
DELETE FROM [FareAttributes]
DELETE FROM [FareRules]
DELETE FROM [StopTimes]
DELETE FROM [Trips]
DELETE FROM [CalendarCalendarDates]
DELETE FROM [CalendarDates]
DELETE FROM [Calendars]
DELETE FROM [Routes]
DELETE FROM [Agencies]
DELETE FROM [Stops]
DELETE FROM [Zones]
DELETE FROM [Streets]
DELETE FROM [Cities]
GO

-- Resets identity column values.

-- User data.
DBCC CHECKIDENT ('[Roles]', RESEED, 0)
DBCC CHECKIDENT ('[Users]', RESEED, 0)

-- Connection data.
DBCC CHECKIDENT ('[FareAttributes]', RESEED, 0)
DBCC CHECKIDENT ('[FareRules]', RESEED, 0)
DBCC CHECKIDENT ('[StopTimes]', RESEED, 0)
DBCC CHECKIDENT ('[Trips]', RESEED, 0)
DBCC CHECKIDENT ('[CalendarDates]', RESEED, 0)
DBCC CHECKIDENT ('[Calendars]', RESEED, 0)
DBCC CHECKIDENT ('[Routes]', RESEED, 0)
DBCC CHECKIDENT ('[Agencies]', RESEED, 0)
DBCC CHECKIDENT ('[Stops]', RESEED, 0)
DBCC CHECKIDENT ('[Zones]', RESEED, 0)
DBCC CHECKIDENT ('[Streets]', RESEED, 0)
DBCC CHECKIDENT ('[Cities]', RESEED, 0)
GO

-- Users and roles.
INSERT INTO [Roles] 
	([Name])
VALUES 
	(0), -- Employee
	(1)  -- Administrator

INSERT INTO [Users]
	([UserName], [Password])
VALUES
	('root', 'root'),	 -- Passwords will be likely subject to changes here
	('employee', 'password'),
	('guest', 'guest')

INSERT INTO [UserRoles]
	([User_Id], [Role_Id])
VALUES
	(1, 1), (1, 2),		-- User root is an administrator.
	(2, 1)				-- User employee is an employee.
						-- User guest has no roles assigned.

	-- Connection data.
	INSERT INTO [Cities]
	([Name])
VALUES
	-- Intra-city example.
	('Warszawa'),				-- 1
	-- Inter-city example.
	('Zawiercie'),				-- 2
	('£azy'),					-- 3
	('Z¹bkowice Œl¹skie'),		-- 4
	('Sosnowiec'),				-- 5
	('Katowice'),				-- 6
	('Gliwice')					-- 7

INSERT INTO [Streets]
	([CityId], [Name])
VALUES
	-- Streets on the path of the E-1 ZTM line.
	(1, 'Sokola'),									-- 1
	(1, 'Wa³ Miedzeszyñski'),						-- 2
	(1, 'Saska'),									-- 3
	(1, 'Bora-Komorowskiego'),						-- 4
	-- Addresses of rail stations of the 101 PKP Intercity line.
	(2, '3 Maja'),									-- 5
	(3, 'Dworcowa'),								-- 6
	(4, 'Niepodleg³oœci'),							-- 7
	(5, '3 Maja'),									-- 8
	(6, 'Plac Oddzia³ów M³odzie¿y Powstañczej'),	-- 9
	(7, 'Bohaterów Getta Warszawskiego'),			-- 10
	-- Agency locations.
	(1, '¯elazna')									-- 11

INSERT INTO [Zones]
	([Name])
VALUES
	('Warszawa - ZTM - Strefa 1')

INSERT INTO [Stops]
	([Name], [StreetId], [ZoneId], [ParentStationId], [IsStation])
VALUES
	-- Warsaw bus stops.
	('Metro Stadion Narodowy', 1, 1, NULL, 0),			-- 1
	('Kryniczna', 2, 1, NULL, 0),						-- 2
	('Saska', 3, 1, NULL, 0),							-- 3
	('Afrykañska', 4, 1, NULL, 0),						-- 4
	('Wa³ Goc³awski', 4, 1, NULL, 0),					-- 5
	('Abrahama', 4, 1, NULL, 0),						-- 6
	('Bora-Komorowskiego', 4, 1, NULL, 0),				-- 7
	('Horbaczewskiego', 4, 1, NULL, 0),					-- 8
	('Goc³aw', 4, 1, NULL, 0),							-- 9
	-- Rail stops.
	('Zawiercie D.K.', 5, NULL, NULL, 0),				-- 10
	('£azy D.K.', 6, NULL, NULL, 0),					-- 11
	('D¹browa Górnicza Z¹bkowice', 7, NULL, NULL, 0),	-- 12
	('Sosnowiec G³ówny', 8, NULL, NULL, 0),				-- 13
	('Katowice', 9, NULL, NULL, 0),						-- 14
	('Gliwice', 10, NULL, NULL, 0)						-- 15

INSERT INTO [Agencies]
	([Name], [Phone], [Url], [Regon], [StreetId], [StreetNumber])
VALUES
	('Zarz¹d Transportu Miejskiego', '19 115', 'http://www.ztm.waw.pl', '012605780', 11, '61'),	-- 1
	('PKP Intercity', '19 757', 'http://www.intercity.pl', '017258024', 11, '59a')				-- 2

INSERT INTO [Routes]
	([AgencyId], [ShortName], [LongName], [RouteType])
VALUES
	(1, 'E-1', 'Goc³aw-Metro Stadion Narodowy', 3),		-- 1; route type 3 is a Bus
	(2, '101', 'Zawiercie-Gliwice', 2)					-- 2; route type 2 is Rail

INSERT INTO [Calendars]
	([StartDate], [EndDate], [Monday], [Tuesday], [Wednesday], [Thursday], [Friday], [Saturday], [Sunday])
VALUES
	('2016-01-01', '2017-12-31', 1, 1, 1, 1, 1, 0, 0),	-- Line does not run on weekends.
	('2016-10-29', '2017-10-29', 1, 1, 1, 1, 1, 1, 1)	-- Line runs every day.

INSERT INTO [CalendarDates]
	([Date], [ExceptionType])
VALUES
	('2016-01-01', 1)	-- Service does not run on November 1.

INSERT INTO [CalendarCalendarDates]
	([Calendar_Id], [CalendarDate_Id])
VALUES
	(1, 1)				-- Remove Nov 1 from the ZTM line.

INSERT INTO [Trips]
	([RouteId], [ServiceId], [Headsign], [ShortName], [Direction])
VALUES
	(1, 1, 'Goc³aw', NULL, 0),							-- 1
	(1, 1, 'Metro Stadion Narodowy', NULL, 1),			-- 2
	(2, 2, 'Gliwice', NULL, 0)							-- 3

INSERT INTO [StopTimes]
	([StopId], [TripId], [ArrivalTime], [DepartureTime], [StopSequence])
VALUES
	-- ZTM trip: one way.
	(1, 1, '09:01:00', '09:01:00', 1),
	(2, 1, '09:05:00', '09:05:00', 2),
	(3, 1, '09:07:00', '09:07:00', 3),
	(4, 1, '09:09:00', '09:07:00', 4),
	(5, 1, '09:10:00', '09:10:00', 5),
	(6, 1, '09:11:00', '09:11:00', 6),
	(7, 1, '09:13:00', '09:13:00', 7),
	(8, 1, '09:14:00', '09:14:00', 8),
	(9, 1, '09:15:00', '09:15:00', 9),
	-- ZTM trip: the other way.
	(9, 2, '15:03:00', '15:03:00', 1),
	(8, 2, '15:04:00', '15:04:00', 2),
	(7, 2, '15:05:00', '15:05:00', 3),
	(6, 2, '15:07:00', '15:07:00', 4),
	(5, 2, '15:08:00', '15:08:00', 5),
	(4, 2, '15:09:00', '15:09:00', 6),
	(3, 2, '15:13:00', '15:13:00', 7),
	(2, 2, '15:15:00', '15:15:00', 8),
	(1, 2, '15:19:00', '15:19:00', 9),
	-- Rail trip.
	(10, 3, '13:23:00', '13:24:00', 1),
	(11, 3, '13:28:00', '13:29:00', 2),
	(12, 3, '13:40:00', '13:41:00', 3),
	(13, 3, '13:58:00', '13:59:00', 4),
	(14, 3, '14:10:00', '14:21:00', 5),
	(15, 3, '14:29:00', '14:29:00', 6)

INSERT INTO [FareRules]
	([RouteId], [OriginId], [DestinationId])
VALUES
	(1, 1, 1)	-- 1; Rule for the ZTM route. Not modeling the rail route due to complexity.

INSERT INTO [FareAttributes]
	([FareRuleId], [Price], [Transfers], [TransferDuration])
VALUES
	(1, 2.20, 0, 4500),	-- ZTM 75-minute ticket with unlimited transfers.
	(1, 1.70, 0, 1200)		-- ZTM 20-minute no-transfer ticket.
GO