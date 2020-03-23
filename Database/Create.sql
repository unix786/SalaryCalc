IF EXISTS (SELECT * FROM sys.databases WHERE name = 'MSSqlLocalDB')
BEGIN
	USE master
	ALTER DATABASE MSSqlLocalDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE MSSqlLocalDB
END

CREATE DATABASE MSSqlLocalDB
go

USE MSSqlLocalDB

CREATE TYPE Name_t FROM nvarchar(250) NOT NULL -- Should also add a minimum length check somewhere.
CREATE TYPE Year_t FROM smallint -- Year number
CREATE TYPE Identity_t FROM int
go

CREATE TABLE Position
(
	ID Identity_t IDENTITY PRIMARY KEY,
	Name Name_t UNIQUE
)

CREATE TABLE Employee
(
	ID Identity_t IDENTITY PRIMARY KEY,
	Name Name_t,
	Employed Year_t NOT NULL,
	PositionID Identity_t NOT NULL REFERENCES Position (ID),
	ManagerID Identity_t NULL REFERENCES Employee (ID),
	Rating tinyint NOT NULL DEFAULT 0, -- The last rating. Indirectly related to Bonus(Score). See Salary view.
	YearsEmployed tinyint NOT NULL DEFAULT 0, -- Relative year of the last rating.
	PrevRating1 tinyint NOT NULL DEFAULT 0, -- Next most recent rating (from last year).
	PrevRating2 tinyint NOT NULL DEFAULT 0,
	Salary int NULL -- Last computation result.
)

CREATE TABLE BaseSalary
(
	PositionID Identity_t REFERENCES Position (ID),
	Years tinyint NOT NULL, -- Minimum years employed. Will select the highest.
	Value int NOT NULL,
	PRIMARY KEY (PositionID, Years)
)

CREATE TABLE Bonus
(
	Score tinyint PRIMARY KEY, -- Satisfaction score. It's calculated from ratings in Salary view.
	Value tinyint NOT NULL -- Used as a percentage.
)

CREATE TABLE Parameter
(
	MinimumWage int NOT NULL
)
go

CREATE TRIGGER t_SingleRow ON Parameter
FOR INSERT, DELETE
AS
IF (SELECT COUNT(*) FROM Parameter) <> 1
	THROW 50000, 'There must be exactly one row', 0
go

CREATE VIEW Salary
AS
SELECT
	E.ID EmployeeID,
	ISNULL((SELECT MinimumWage FROM Parameter), 0) + ISNULL(S.Value, 0) * (100 + ISNULL(B.Value, 0)) / 100 Value
FROM
	Employee E
	LEFT JOIN Bonus B ON B.Score = CEILING((2*E.Rating + E.PrevRating1 + E.PrevRating2)/40) -- Ratings should be stored as numbers 0 to 50.
	OUTER APPLY (
		SELECT TOP 1 *
		FROM BaseSalary S
		WHERE S.PositionID = E.PositionID AND S.Years <= E.YearsEmployed
		ORDER BY S.Years DESC
	) S
go

CREATE PROCEDURE usp_RecalculateSalaries
(
	@WhereNull bit = 0 -- If 1, then will only update Salaries where they are not set.
)
AS
UPDATE E
SET E.Salary = S.Value
FROM
	Employee E
	INNER JOIN Salary S ON S.EmployeeID = E.ID
WHERE
	ISNULL(@WhereNull, 0) = 0 OR E.Salary IS NULL
go

CREATE TRIGGER t_RecalcSalary ON Employee
FOR INSERT, UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM deleted) AND (
		Update(Salary)
		OR NOT (Update(PositionID) OR Update(Rating) OR Update(YearsEmployed) OR Update(PrevRating1) OR Update(PrevRating2))
	)
		RETURN

	UPDATE E
	SET E.Salary = S.Value
	FROM
		Employee E
		INNER JOIN Salary S ON S.EmployeeID = E.ID
		INNER JOIN inserted I ON I.ID = E.ID
END