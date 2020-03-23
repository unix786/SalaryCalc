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
GO
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