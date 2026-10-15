select * from LOANS

--1

DECLARE LTypeName CURSOR FOR
	SELECT Loan_type_name from LOANS;

DECLARE @name nvarchar(100), @t nvarchar(500)='';

OPEN LTypeName;
FETCH LTypeName into @name;
print 'Список кредитов';
while @@FETCH_STATUS = 0
	begin
		set @t = RTRIM(@name) + ', ' + @t
		FETCH next from LTypeName into @name;
	end;
print @t;
CLOSE LTypeName;
deallocate LTypeName;

GO

--2
DECLARE loanCursor CURSOR LOCAL FOR SELECT Loan_number, Loan_type_name FROM LOANS;

DECLARE @loanNumber nvarchar(20), @loanType nvarchar(50);
OPEN loanCursor;
FETCH loanCursor INTO @loanNumber, @loanType;
PRINT 'Loans:';
WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'ID: ' + @loanNumber + ', Название: ' + @loanType;
    FETCH NEXT FROM loanCursor INTO @loanNumber, @loanType;
END;
go

DECLARE @loanNumber nvarchar(20), @loanType nvarchar(50);
FETCH loanCursor INTO @loanNumber, @loanType;
PRINT 'Loans:';
WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'ID: ' + @loanNumber + ', Название: ' + @loanType;
    FETCH NEXT FROM loanCursor INTO @loanNumber, @loanType;
END;
go
CLOSE loanCursor;
DEALLOCATE loanCursor;

--GLOBAL
DECLARE loanCursor CURSOR GLOBAL FOR SELECT Loan_number, Loan_type_name FROM LOANS;

DECLARE @gloanNumber nvarchar(20), @gloanType nvarchar(50);
OPEN loanCursor;
FETCH loanCursor INTO @gloanNumber, @gloanType;
PRINT 'Loans:';
WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'ID: ' + @gloanNumber + ', Название: ' + @gloanType;
    FETCH NEXT FROM loanCursor INTO @gloanNumber, @gloanType;
END;
go

DECLARE @gloanNumber nvarchar(20), @gloanType nvarchar(50);
FETCH loanCursor INTO @gloanNumber, @gloanType;
PRINT 'Loans:';
WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'ID: ' + @gloanNumber + ', Название: ' + @gloanType;
    FETCH NEXT FROM loanCursor INTO @gloanNumber, @gloanType;
END;

CLOSE loanCursor;
DEALLOCATE loanCursor;
go


--3
select * from CONTRACTS

DECLARE @contractID nvarchar(20), @loanNumber nvarchar(20), @registrationNumber nvarchar(20), @amount nvarchar(20), @issueDate nvarchar(20), @returnDate nvarchar(20);

DECLARE contractCursor CURSOR LOCAL STATIC FOR 
SELECT Contract_id, Loan_number, Registration_number, Amount, Issue_date, Return_date FROM CONTRACTS; 

OPEN contractCursor;

UPDATE CONTRACTS SET Amount = '60000' WHERE Contract_id = '1';
DELETE FROM Contracts WHERE Contract_id = '2';

FETCH NEXT FROM contractCursor INTO @contractID, @loanNumber, @registrationNumber, @amount, @issueDate, @returnDate;

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Contract ID: ' + @contractID + ', Loan Number: ' + @loanNumber + ', Registration Number: ' + @registrationNumber + ', Amount: ' + @amount + ', Issue Date: ' + @issueDate + ', Return Date: ' + @returnDate;
    FETCH NEXT FROM contractCursor INTO @contractID, @loanNumber, @registrationNumber, @amount, @issueDate, @returnDate;
END;

CLOSE contractCursor;
DEALLOCATE contractCursor;
go

--DYNAMIC
DECLARE @contractID nvarchar(20), @loanNumber nvarchar(20), @registrationNumber nvarchar(20), @amount nvarchar(20), @issueDate nvarchar(20), @returnDate nvarchar(20);

DECLARE contractCursor CURSOR LOCAL DYNAMIC FOR 
SELECT Contract_id, Loan_number, Registration_number, Amount, Issue_date, Return_date FROM CONTRACTS; 

OPEN contractCursor;

UPDATE CONTRACTS SET Amount = '60000' WHERE Contract_id = '1';
DELETE FROM Contracts WHERE Contract_id = '2';

FETCH NEXT FROM contractCursor INTO @contractID, @loanNumber, @registrationNumber, @amount, @issueDate, @returnDate;

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Contract ID: ' + @contractID + ', Loan Number: ' + @loanNumber + ', Registration Number: ' + @registrationNumber + ', Amount: ' + @amount + ', Issue Date: ' + @issueDate + ', Return Date: ' + @returnDate;
    FETCH NEXT FROM contractCursor INTO @contractID, @loanNumber, @registrationNumber, @amount, @issueDate, @returnDate;
END;

CLOSE contractCursor;
DEALLOCATE contractCursor;
go

--4
select * from LOANS

DECLARE @Loan_number int, 
        @Loan_type_name varchar(50)

DECLARE loan_cursor CURSOR LOCAL SCROLL 
FOR
SELECT Loan_number, Loan_type_name
FROM LOANS;

OPEN loan_cursor;

FETCH FIRST FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH NEXT FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH PRIOR FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH LAST FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH ABSOLUTE 4 FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH ABSOLUTE -4 FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH RELATIVE 1 FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

FETCH RELATIVE -1 FROM loan_cursor INTO @Loan_number, @Loan_type_name
PRINT cast(@Loan_number as varchar(10)) + ' ' + RTRIM(@Loan_type_name)

CLOSE loan_cursor;
DEALLOCATE loan_cursor;
go

--5
select * from CONTRACTS
DECLARE @Contract_id INT, @Loan_number INT, @Amount INT;

DECLARE ContractCursor CURSOR LOCAL SCROLL FOR
SELECT Contract_id, Loan_number, Amount
FROM CONTRACTS
FOR UPDATE;

OPEN ContractCursor;

FETCH ContractCursor INTO @Contract_id, @Loan_number, @Amount;
DELETE CONTRACTS WHERE CURRENT OF ContractCursor;

FETCH Last from ContractCursor INTO @Contract_id, @Loan_number, @Amount;
UPDATE CONTRACTS SET Contract_id = @Contract_id + 1 WHERE CURRENT OF ContractCursor;

CLOSE ContractCursor;
DEALLOCATE ContractCursor;
go


--6
CREATE TABLE #Contracts (
    Contract_id INT,
    Loan_number INT,
    Registration_number INT,
    Amount BIGINT,
    Issue_date DATE,
    Return_date DATE
);

INSERT INTO #Contracts (Contract_id, Loan_number, Registration_number, Amount, Issue_date, Return_date)
VALUES
    (2, 2, 1002, 222222, '2023-02-02', '2032-02-02'),
    (3, 3, 1003, 3000000, '2023-03-10', '2043-03-10'),
    (4, 4, 1004, 800000, '2023-04-01', '2026-04-01'),
    (5, 5, 1005, 150000, '2023-05-15', '2025-05-15'),
    (6, 1, 1001, 200000, '2023-07-17', '2026-07-17'),
    (7, 3, 1005, 57000000, '2023-10-19', '2050-02-17'),
    (8, 3, 1003, 600000, '2023-04-01', '2027-04-01'),
    (9, 3, 1002, 866660, '2023-05-15', '2028-04-01'),
    (10, 1, 1003, 1000000, '2023-09-11', '2028-09-11'),
    (11, 2, 1002, 536829, '2024-01-01', '2030-01-01'),
    (12, 4, 1005, 200000, '2025-01-13', '2030-04-18'),
    (14, 5, 1004, 300000, '2025-02-18', '2031-08-06');

SELECT * FROM #Contracts;
DROP TABLE #Contracts;

CREATE TABLE #LoanTypes (
    Loan_number INT,
    Loan_type_name NVARCHAR(255),
    Rate DECIMAL(5, 4)
);
INSERT INTO #LoanTypes (Loan_number, Loan_type_name, Rate)
VALUES
    (1, N'Ипотека на коммерческую недвижимость', 5.7500),
    (2, N'Ипотека на складские и производственные помещения', 3.4500),
    (3, N'Ипотека на покупку земельных участков', 4.2000),
    (4, N'Ипотека на строительство коммерческой недвижимости', 6.0000),
    (5, N'Ипотека под залог уже имеющейся недвижимости', 2.9000),
    (6, N'Неиспользующийся кредит', 6.8000),
    (7, N'Ипотека на покупку земельных участков', 5.6000),
    (8, N'Ипотека на коммерческую недвижимость', 8.3000),
    (9, N'Новый кредит', 5.0000);

SELECT * FROM #LoanTypes;
DROP TABLE #LoanTypes;



--контракты с процентной ставкой ниже 4
DECLARE @Contract_id INT, @Rate DECIMAL(5, 4), @Amount BIGINT;

DECLARE Cur_Delete CURSOR FOR
SELECT C.Contract_id, LT.Rate, C.Amount
FROM #Contracts C
JOIN #LoanTypes LT ON C.Loan_number = LT.Loan_number
WHERE LT.Rate < 4.0
FOR UPDATE

OPEN Cur_Delete;

FETCH Cur_Delete INTO @Contract_id, @Rate, @Amount;
DELETE  #Contracts WHERE CURRENT OF Cur_Delete;

CLOSE Cur_Delete;
DEALLOCATE Cur_Delete;
go


--для конкретного типа кредита увеличивается ставка на 1.
DECLARE @Loan_number INT,@Loan_type_name nvarchar(50), @Rate DECIMAL(5, 4);

DECLARE Cur_Update CURSOR FOR
SELECT Loan_number, Loan_type_name, Rate
FROM #LoanTypes
WHERE Loan_number = 1
FOR UPDATE

OPEN Cur_Update;

FETCH Cur_Update INTO @Loan_number, @Loan_type_name, @Rate
UPDATE #LoanTypes SET Rate = @Rate + 1
    WHERE Loan_number = @Loan_number;

CLOSE Cur_Update;
DEALLOCATE Cur_Update;

