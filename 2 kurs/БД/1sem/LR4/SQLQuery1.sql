SELECT * FROM LOANS;
SELECT * FROM CONTRACTS;
SELECT * FROM CLIENTS;

SELECT C.Loan_number, L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L
INNER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number

SELECT C.Loan_number, L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L
INNER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number
AND L.Loan_type_name Like '%недвиж%'

SELECT * FROM CONTRACTS;
SELECT * FROM CLIENTS;

SELECT CL.Registration_number, CL.Firm_name, C.Issue_date, C.Return_date,
Case
When (C.Amount < 600000) then 'Небольшая'
When (C.Amount < 1500000) then 'Большая'
else 'Очень большая'
end [Сумма кредита]
FROM CLIENTS AS CL
INNER JOIN CONTRACTS AS C
ON CL.Registration_number = C.Registration_number

SELECT L.Loan_type_name, isnull(C.Registration_number, 0) AS [Registration number]
FROM LOANS AS L
LEFT OUTER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number

/*full outer*/
SELECT *
FROM LOANS AS L
FULL OUTER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number

SELECT *
FROM CONTRACTS AS C
FULL OUTER JOIN LOANS AS L
ON L.Loan_number = C.Loan_number

SELECT * FROM LOANS;
SELECT * FROM CONTRACTS;

SELECT *
FROM LOANS AS L
FULL OUTER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number
WHERE C.Loan_number IS NULL;

SELECT *
FROM LOANS AS L
FULL OUTER JOIN CONTRACTS AS C
ON  L.Loan_number = C.Loan_number
WHERE C.Loan_number IS NOT NULL

/*cross*/
SELECT C.Loan_number, L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L
CROSS JOIN CONTRACTS AS C
Where L.Loan_number = C.Loan_number
