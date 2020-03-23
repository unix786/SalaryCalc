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