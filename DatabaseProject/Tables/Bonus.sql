CREATE TABLE Bonus
(
	Score tinyint PRIMARY KEY, -- Satisfaction score. It's calculated from ratings in Salary view.
	Value tinyint NOT NULL -- Used as a percentage.
)