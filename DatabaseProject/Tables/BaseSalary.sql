CREATE TABLE BaseSalary
(
	PositionID Identity_t REFERENCES Position (ID),
	Years tinyint NOT NULL, -- Minimum years employed. Will select the highest.
	Value int NOT NULL,
	PRIMARY KEY (PositionID, Years)
)