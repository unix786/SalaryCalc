CREATE TABLE Parameter
(
	MinimumWage int NOT NULL
)
GO
CREATE TRIGGER t_SingleRow ON Parameter
FOR INSERT, DELETE
AS
IF (SELECT COUNT(*) FROM Parameter) <> 1
	THROW 50000, 'There must be exactly one row', 0