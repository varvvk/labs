
use K_MyBase
go
--1
CREATE FUNCTION COUNT_CREDITS_BY_NAME(@name nvarchar(70)) RETURNS int
AS BEGIN DECLARE @rc int = 0;
    SET @rc = ( SELECT COUNT(*) FROM CONTRACTS 
				join LOANS
				ON CONTRACTS.Loan_number = LOANS.Loan_number
				WHERE LOANS.Loan_type_name = @name);
    RETURN @rc;
END;

DECLARE @rc INT = dbo.COUNT_CREDITS_BY_NAME('Ипотека на коммерческую недвижимость');
PRINT 'Количество кредитов: ' + CAST(@rc AS nvarchar(10));

GO

ALTER FUNCTION COUNT_CREDITS_BY_NAME(@name nvarchar(70), @prof int) RETURNS int
AS BEGIN DECLARE @rc int = 0;
    SET @rc = ( SELECT COUNT(*) FROM CONTRACTS 
				join LOANS
				ON CONTRACTS.Loan_number = LOANS.Loan_number
				WHERE LOANS.Loan_type_name = @name and CONTRACTS.Amount = @prof);
    RETURN @rc;
END;

DECLARE @rc INT = dbo.COUNT_CREDITS_BY_NAME('Ипотека на коммерческую недвижимость', 200000);
PRINT 'Количество кредитов: ' + CAST(@rc AS nvarchar(10));

drop function COUNT_CREDITS_BY_NAME

GO


--2
CREATE FUNCTION FCLIENTS(@p nvarchar(10)) RETURNS char(300) 
AS
BEGIN 
DECLARE @tv nvarchar(70);  
DECLARE @t nvarchar(300) = 'Клиенты:';  

DECLARE KCLIENTS CURSOR LOCAL 
for SELECT Firm_name from CLIENTS where Ownership_type = @p;
OPEN KCLIENTS;	  
FETCH  KCLIENTS into @tv;   	 
WHILE @@fetch_status = 0                                     
	BEGIN 
		SET @t = @t + ', ' + rtrim(@tv);         
		FETCH NEXT FROM KCLIENTS into @tv; 
	END;    
RETURN @t;
END;  

SELECT distinct Ownership_type, dbo.FCLIENTS(Ownership_type) FROM CLIENTS;

select * from CLIENTS;
drop function FCLIENTS
GO


--3
CREATE FUNCTION FCONTLOAN(@loanNumber nvarchar(20), @clientID nvarchar(20)) RETURNS TABLE 
AS RETURN
SELECT c.Contract_id, l.Loan_type_name, l.Rate, c.Amount
FROM CONTRACTS c 
LEFT JOIN LOANS l 
ON c.Loan_number = l.Loan_number
WHERE l.Loan_number = ISNULL(@loanNumber, l.Loan_number) AND c.Registration_number = ISNULL(@clientID, c.Registration_number);

SELECT * FROM dbo.FCONTLOAN(NULL, NULL); 
SELECT * FROM dbo.FCONTLOAN('3', NULL); 
SELECT * FROM dbo.FCONTLOAN(NULL, '1003'); 
SELECT * FROM dbo.FCONTLOAN('3', '1003'); 

select * from CONTRACTS
drop function FCONTLOAN
  
GO


--4
CREATE FUNCTION FCONTRACTS(@loanNumber int) RETURNS INT
AS BEGIN
	DECLARE @rc int = (SELECT COUNT(*) FROM CONTRACTS WHERE Loan_number = ISNULL(@loanNumber, Loan_number));
	RETURN @rc;
END;

SELECT Loan_number, Loan_type_name, dbo.FCONTRACTS(Loan_number)[Количество_кредитов] FROM LOANS;
SELECT dbo.FCONTRACTS(NULL)[Всего_кредитов];

drop function FCONTRACTS

GO


----6
--CREATE FUNCTION CONTRACTS_REPORT(@c int) 
--RETURNS @loan_report TABLE 
--(
--    [Loan_Number] int, 
--	[Количество кредитов] int,
--	[Сумма контрактов] int,
--	[Первая дата выдачи] date
--)
--AS BEGIN
--    DECLARE cc CURSOR STATIC FOR 
--    SELECT DISTINCT Loan_number 
--    FROM CONTRACTS 
--    WHERE Amount > @c;

--    DECLARE @loan_number INT;

--    OPEN cc;  
--    FETCH cc INTO @loan_number;

--    WHILE @@FETCH_STATUS = 0
--    BEGIN
--        INSERT INTO @loan_report 
--        VALUES 
--        (
--            @loan_number,
--            (SELECT COUNT(Contract_id) FROM CONTRACTS WHERE Loan_number = @loan_number),
--			(SELECT SUM(Amount) FROM CONTRACTS WHERE Loan_number = @loan_number),
--            (SELECT MIN(Issue_date) FROM CONTRACTS WHERE Loan_number = @loan_number)
--        ); 
        
--        FETCH cc INTO @loan_number;  
--    END;

--    CLOSE cc;
--    DEALLOCATE cc;

--    RETURN;
--END;


CREATE FUNCTION GET_LOAN_COUNT(@loan_number INT)
RETURNS INT
AS
BEGIN
    RETURN (SELECT COUNT(Contract_id) FROM CONTRACTS WHERE Loan_number = @loan_number);
END;


CREATE FUNCTION GET_LOAN_SUM(@loan_number INT)
RETURNS INT
AS
BEGIN
    RETURN (SELECT SUM(Amount) FROM CONTRACTS WHERE Loan_number = @loan_number);
END;


CREATE FUNCTION GET_FIRST_ISSUE_DATE(@loan_number INT)
RETURNS DATE
AS
BEGIN
    RETURN (SELECT MIN(Issue_date) FROM CONTRACTS WHERE Loan_number = @loan_number);
END;


CREATE FUNCTION CONTRACTS_REPORT(@c INT) 
RETURNS @loan_report TABLE 
(
    [Loan_Number] INT, 
    [Количество кредитов] INT,
    [Сумма контрактов] INT,
    [Первая дата выдачи] DATE
)
AS
BEGIN
    DECLARE cc CURSOR STATIC FOR 
    SELECT DISTINCT Loan_number 
    FROM CONTRACTS 
    WHERE Amount > @c;

    DECLARE @loan_number INT;

    OPEN cc;  
    FETCH cc INTO @loan_number;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO @loan_report 
        VALUES 
        (
            @loan_number,
            dbo.GET_LOAN_COUNT(@loan_number),  
            dbo.GET_LOAN_SUM(@loan_number),   
            dbo.GET_FIRST_ISSUE_DATE(@loan_number)
        ); 
        
        FETCH cc INTO @loan_number;  
    END;

    CLOSE cc;
    DEALLOCATE cc;
    RETURN;
END;

SELECT * FROM dbo.CONTRACTS_REPORT(800000);

select * from CONTRACTS;
drop function CONTRACTS_REPORT;