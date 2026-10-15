SELECT * FROM CLIENTS;
SELECT * FROM CONTRACTS;
SELECT * FROM LOANS;

--1
SELECT C.Registration_number, L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L, CONTRACTS AS C
WHERE L.Loan_number = C.Loan_number
and
C.Registration_number IN (SELECT Registration_number FROM CLIENTS 
									WHERE (Registration_number like '%5'))

--2
SELECT L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L
INNER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number
and
C.Registration_number IN (SELECT Registration_number FROM CLIENTS 
									WHERE (Registration_number like '%5'))

--3
SELECT L.Loan_type_name, C.Issue_date, C.Return_date
FROM LOANS AS L
INNER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number
INNER JOIN CLIENTS AS CL
ON C.Registration_number=CL.Registration_number
WHERE (CL.Registration_number like '%5')

--4
SELECT * FROM LOANS;
SELECT *
FROM LOANS AS L 
WHERE Rate = (SELECT TOP(1) LL.RATE FROM LOANS AS LL
				WHERE L.Loan_type_name=LL.Loan_type_name
				ORDER BY LL.Rate desc)
ORDER BY L.Rate DESC

--5
SELECT * FROM CONTRACTS;
SELECT * FROM LOANS;
SELECT L.Loan_number, L.Loan_type_name
FROM LOANS AS L
WHERE not exists (SELECT * FROM CONTRACTS AS C 
					WHERE L.Loan_number=C.Loan_number)

--6
SELECT * FROM LOANS;
SELECT TOP(1)
(SELECT avg(Rate) from LOANS
	WHERE Loan_type_name like '%недвижимость') [»потека на коммерческую недвижимость],
(SELECT avg(Rate) from LOANS
	WHERE Loan_type_name like '%участков') [»потека на покупку земельных участков]
FROM LOANS

--7
SELECT *
FROM LOANS
WHERE Rate >= all(SELECT Rate FROM LOANS
					WHERE Loan_type_name Like '%участков')

--8
SELECT * FROM LOANS;
SELECT *
FROM LOANS
WHERE Rate >= any (SELECT Rate FROM LOANS
					WHERE Loan_type_name Like '%участков')


--кредиты которые вз€ты в 1 день
SELECT * FROM CONTRACTS;
SELECT * FROM LOANS;

SELECT L.Loan_number, L.Loan_type_name, C.Issue_date
FROM LOANS AS L, CONTRACTS AS C
WHERE
C.Loan_number = L.Loan_number
AND
C.Issue_date IN (
    SELECT Issue_date
    FROM CONTRACTS
    GROUP BY Issue_date
    HAVING COUNT(Registration_number) > 1
) 
ORDER BY C.Issue_date
