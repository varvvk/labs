
--1
SELECT C.Loan_number, C.Registration_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
GROUP BY ROLLUP (C.Loan_number, C.Registration_number)

--2
SELECT C.Loan_number, C.Registration_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
GROUP BY CUBE (C.Loan_number, C.Registration_number)

--3
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1003
GROUP BY C.Loan_number
UNION
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1005 OR C.Registration_number = 1003
GROUP BY C.Loan_number

SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1003
GROUP BY C.Loan_number
UNION ALL
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1005 OR C.Registration_number = 1003
GROUP BY C.Loan_number

--4
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1003
GROUP BY C.Loan_number
INTERSECT
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1005 OR C.Registration_number = 1003
GROUP BY C.Loan_number

--5
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1003
GROUP BY C.Loan_number
EXCEPT
SELECT C.Loan_number, SUM(C.Amount)[Общая сумма]
FROM CONTRACTS as C
WHERE C.Registration_number = 1005 OR C.Registration_number = 1003
GROUP BY C.Loan_number
