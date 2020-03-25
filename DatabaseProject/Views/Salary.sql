CREATE VIEW Salary
AS
SELECT
	E.ID EmployeeID,
	ISNULL((SELECT MinimumWage FROM Parameter), 0) + ISNULL(S.Value, 0) * (100 + ISNULL(B.Value, 0)) / 100 Value
FROM
	Employee E
	LEFT JOIN Bonus B ON B.Score = CEILING(CAST(2*E.Rating + E.PrevRating1 + E.PrevRating2 as real)/40) -- Ratings should be stored as numbers 0 to 50.
	OUTER APPLY (
		SELECT TOP 1 *
		FROM BaseSalary S
		WHERE S.PositionID = E.PositionID AND S.Years <= E.YearsEmployed
		ORDER BY S.Years DESC
	) S