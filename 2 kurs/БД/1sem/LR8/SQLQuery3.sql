
--1
CREATE VIEW [LOANS_VIEW]
AS SELECT Loan_number [Номер кредита],
Loan_type_name [Кредит],
Rate [Ставка]
FROM LOANS
GO

SELECT * FROM LOANS_VIEW
GO

--2
CREATE VIEW [AMOUNT_OF_LOANS]
AS SELECT LOANS.Loan_type_name, COUNT(CONTRACTS.Amount) [AMOUNT]
FROM LOANS
INNER JOIN CONTRACTS
ON LOANS.Loan_number = CONTRACTS.Loan_number
GROUP BY LOANS.Loan_type_name
GO

SELECT * FROM [AMOUNT_OF_LOANS]
GO

--3
CREATE VIEW [LOANS_TYPE]
AS SELECT Loan_number [Номер кредита],
Loan_type_name [Кредит],
Rate [Ставка]
FROM LOANS
WHERE LOANS.Loan_type_name LIKE '%недвиж%'
GO

SELECT * FROM [LOANS_TYPE]
GO

--4
CREATE VIEW [Кредиты_недвижимости]
AS SELECT Loan_number [Номер кредита],
Loan_type_name [Кредит],
Rate [Ставка]
FROM LOANS
WHERE LOANS.Loan_type_name LIKE '%недвиж%' WITH CHECK OPTION
GO

INSERT Кредиты_недвижимости values (11, 'no loan', 6.92)
GO

--5
CREATE VIEW [CLIENTS_VIEW]
AS SELECT TOP(5) CL.Registration_number, CL.Ownership_type, CL.Firm_name
FROM CLIENTS AS CL
ORDER BY CL.Firm_name
GO

SELECT * FROM CLIENTS_VIEW
GO

--6
CREATE VIEW [AMOUNT_OF_LOANS_dbo] WITH SCHEMABINDING
AS SELECT LOANS.Loan_type_name, COUNT(CONTRACTS.Amount) [AMOUNT]
FROM dbo.LOANS
INNER JOIN dbo.CONTRACTS
ON LOANS.Loan_number = CONTRACTS.Loan_number
GROUP BY LOANS.Loan_type_name
GO

SELECT * FROM [AMOUNT_OF_LOANS_dbo]
GO

--представление сколько кредитов взяли в каждом году и каждом месяце

CREATE VIEW [Подсчет]
AS SELECT YEAR(C.Issue_date) [Год],
    MONTH(C.Issue_date) [Месяц],
    COUNT(C.Contract_id) [Количество]
FROM CONTRACTS AS C
WHERE YEAR(C.Issue_date)=2023 OR YEAR(C.Issue_date)=2024
GROUP BY ROLLUP(YEAR(C.Issue_date), MONTH(C.Issue_date))
GO

SELECT * FROM [Подсчет]
GO