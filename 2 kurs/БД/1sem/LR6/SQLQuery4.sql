SELECT * FROM CLIENTS
SELECT * FROM CONTRACTS
SELECT * FROM LOANS

--1
SELECT L.Loan_type_name,
MAX(C.Amount)[MAX AMOUNT],
MIN(C.Amount)[MIN AMOUNT],
AVG(C.Amount)[AVG AMOUNT],
SUM(C.Amount)[SUM AMOUNT],
COUNT(*)[COUNT OF LOANS]
FROM LOANS AS L
INNER JOIN CONTRACTS AS C
ON L.Loan_number = C.Loan_number
GROUP BY L.Loan_type_name

--2
SELECT *
FROM (
	SELECT 
		CASE
		WHEN [RATE] BETWEEN 0 AND 5 THEN 'Rate < 5'
		WHEN [RATE] BETWEEN 5 AND 7 THEN 'Rate from 5 to 7'
		ELSE 'Rate > 7'
		END as Rate_Category,
		COUNT(*) AS Loan_Count
	FROM LOANS
	GROUP BY
        CASE 
        WHEN [RATE] BETWEEN 0 AND 5 THEN 'Rate < 5'
		WHEN [RATE] BETWEEN 5 AND 7 THEN 'Rate from 5 to 7'
		ELSE 'Rate > 7'
        END
) AS LoanRates
ORDER BY 
    CASE 
        WHEN Rate_Category = 'Rate < 5' THEN 3
        WHEN Rate_Category = 'Rate from 5 to 7' THEN 2
        WHEN Rate_Category = 'Rate > 7' THEN 1
        ELSE 0 
    END;

--3
SELECT * FROM CLIENTS
SELECT * FROM CONTRACTS
SELECT * FROM LOANS

SELECT CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	round(cast(LOANS.Rate as decimal(6,5)), 0)[Rate],
	AVG(CONTRACTS.Amount) [AVG AMOUNT]
FROM CLIENTS
INNER JOIN CONTRACTS
ON CLIENTS.Registration_number = CONTRACTS.Registration_number
INNER JOIN LOANS
ON CONTRACTS.Loan_number = LOANS.Loan_number
GROUP BY  CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	LOANS.Rate

--4
SELECT * FROM CONTRACTS

SELECT CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	round(cast(LOANS.Rate as decimal(6,5)), 0)[Rate],
	AVG(CONTRACTS.Amount) [AVG AMOUNT]
FROM CLIENTS
INNER JOIN CONTRACTS
ON CLIENTS.Registration_number = CONTRACTS.Registration_number
INNER JOIN LOANS
ON CONTRACTS.Loan_number = LOANS.Loan_number
WHERE CLIENTS.Firm_name like ' реатив%'
OR CLIENTS.Firm_name LIKE 'јвто%'
GROUP BY  CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	LOANS.Rate

--5
SELECT CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	SUM(CONTRACTS.Amount)[SUM],
	AVG(CONTRACTS.Amount) [AVG AMOUNT]
FROM CLIENTS
INNER JOIN CONTRACTS
ON CLIENTS.Registration_number = CONTRACTS.Registration_number
INNER JOIN LOANS
ON CONTRACTS.Loan_number = LOANS.Loan_number
WHERE CLIENTS.Firm_name like ' реатив%'
GROUP BY  CLIENTS.Firm_name,
	LOANS.Loan_type_name,
	LOANS.Rate

--6
SELECT * FROM CONTRACTS
SELECT * FROM LOANS

SELECT LOANS.Loan_type_name, 
    COUNT(*) AS ContractCount
FROM CONTRACTS
INNER JOIN LOANS 
ON CONTRACTS.Loan_number = LOANS.Loan_number
WHERE CONTRACTS.Amount > 700000
GROUP BY LOANS.Loan_type_name
HAVING COUNT(*) > 1
ORDER BY ContractCount DESC


--средн€€ сумма которую вз€ли по разным видам кредитов

SELECT LOANS.Loan_type_name,
	AVG(C.Amount) [—редн€€ сумма]
FROM LOANS
INNER JOIN CONTRACTS AS C
ON LOANS.Loan_number = C.Loan_number
GROUP BY LOANS.Loan_type_name