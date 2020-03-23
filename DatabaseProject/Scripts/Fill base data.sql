IF EXISTS (SELECT * FROM Parameter)
	RETURN

INSERT Parameter (MinimumWage) VALUES (800)

INSERT Bonus (Score, Value)
VALUES
	(0, 0),
	(1, 1),
	(2, 2),
	(3, 7),
	(4, 15),
	(5, 20)

INSERT Position (Name) VALUES ('Technician')
INSERT BaseSalary (PositionID, Years, Value)
SELECT @@IDENTITY, *
FROM (
	VALUES
		(0, 1000),
		(3, 1200),
		(6, 1520)
) V(Y, V)

INSERT Position (Name) VALUES ('Sales')
INSERT BaseSalary (PositionID, Years, Value)
SELECT @@IDENTITY, *
FROM (
	VALUES
		(0, 1100),
		(3, 1400),
		(7, 1650)
) V(Y, V)
