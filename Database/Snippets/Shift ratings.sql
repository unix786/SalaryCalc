DECLARE @Year int = YEAR(GETDATE()) -- Assuming the server also sets YearsEmployed.

		UPDATE E
		SET
			E.YearsEmployed = E.YearsNow,
			E.PrevRating1 = E.Rating,
			E.PrevRating2 = CASE E.YearsNow - E.YearsEmployed
				WHEN 1 THEN E.PrevRating1
				ELSE E.Rating -- If skipped a year or more.
			END,
			Salary = NULL
		FROM
			(
				SELECT
					*,
					@Year - Employed as YearsNow
				FROM
					Employee
			) E
			INNER JOIN inserted I ON I.ID = E.ID
		WHERE
			E.YearsEmployed <> E.YearsNow