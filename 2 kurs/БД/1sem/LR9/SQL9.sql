
--1
DECLARE @ch char(5) = 'Hello', 
		@vch varchar(7) = 'World', 
		@dt datetime, 
		@t time, 
		@i int, 
		@si smallint, 
		@ti tinyint, 
		@n numeric(12, 5);

SET @dt = GETDATE()
SET @t = '17:41:00'
SET @i =1000
SELECT @si = 100, @ti = 1, @n = 1234567.11111
 
SELECT @ch ch, @vch vch, @dt dt, @t t, @i i;
PRINT @si
PRINT @ti
PRINT cast(@n as varchar(15))  
go

--2
DECLARE @amnt int= (SELECT SUM(CONTRACTS.Amount) FROM CONTRACTS ),
		@amntau int = (SELECT COUNT(*) FROM CONTRACTS),
		@_avg int = (SELECT AVG(CONTRACTS.Amount) FROM CONTRACTS)
DECLARE @minavg int = (SELECT COUNT(*) FROM CONTRACTS WHERE CONTRACTS.Amount < @_avg)
DECLARE	@perc real = (CAST(@minavg AS real)/ @amntau *100)

IF @amnt > 600000
BEGIN
SELECT	@amntau AS 'Количество контрактов', 
        @_avg AS 'Средняя сумма контрактов', 
        @minavg AS 'Сумма которых меньше средней', 
        @perc AS 'Процент'
END
ELSE
BEGIN
    PRINT 'Общая сумма контрактов составляет: ' + @amnt
END
go

--3
print 'Число обработанных строк: ' + cast(@@ROWCOUNT as varchar(10));
print 'Версия SQL Server: ' + cast(@@VERSION as varchar(200));
print 'Системный идентификатор процесса: ' + cast(@@SPID as varchar(10));
print 'Код последней ошибки: ' + cast(@@ERROR as varchar(30));
print 'Имя сервера: ' + cast(@@SERVERNAME as varchar(30));
print 'Уровень вложенности транзакции: ' + cast(@@trancount as varchar(30));
print 'Проверка результата считывания строк результирующего набора: ' + cast(@@FETCH_STATUS as varchar(30));
print 'Уровень вложенности текущей процедуры: ' + cast(@@NESTLEVEL as varchar(30));
go

--4
DECLARE @z float, @x int = 2, @t int = 1;

IF (@t > @x) SET @z = POWER((SIN(@t)), 2);
ELSE IF (@t < @x) SET @z = 4*(@t+@x);
ELSE SET @z = 1 - EXP(@x-2);

PRINT 'Z = ' + cast(@z as varchar(10))
go

SELECT 
    Contact_person,
    substring(Contact_person, 1, charindex(' ', Contact_person) - 1) + ' ' +
    upper(substring(Contact_person, charindex(' ', Contact_person) + 1, 1)) + '.' AS Formatted_Person
FROM CLIENTS;

go

DECLARE @dt date = getdate();
DECLARE @NextMStart DATE = DATEADD(MONTH, 1, @dt);
DECLARE @NextMEnd DATE = EOMONTH(@NextMStart);

SELECT 
    Contract_id,
    Loan_number,
    Registration_number,
	Issue_date
FROM CONTRACTS
WHERE Issue_date >= @NextMStart AND Issue_date <= @NextMEnd;
go
select * from CONTRACTS

SELECT 
    Registration_number,
    Loan_number,
    CASE DATEPART(WEEKDAY, Issue_date)
        WHEN 1 THEN 'Воскресенье'
        WHEN 2 THEN 'Понедельник'
        WHEN 3 THEN 'Вторник'
        WHEN 4 THEN 'Среда'
        WHEN 5 THEN 'Четверг'
        WHEN 6 THEN 'Пятница'
        WHEN 7 THEN 'Суббота'
    END AS Credit_Day
FROM CONTRACTS
WHERE Registration_number = 1001 AND Loan_number = 1;
go

--5
DECLARE @_avg float = (SELECT AVG(CONTRACTS.Amount) FROM CONTRACTS)
DECLARE @AboveAvg INT, @BelowAvg INT;

SELECT @AboveAvg = COUNT(*) 
FROM Contracts 
WHERE Amount > @_avg;

SELECT @BelowAvg = COUNT(*) 
FROM Contracts 
WHERE Amount < @_avg;

IF @AboveAvg > 0
    PRINT 'Количество контрактов на сумму больше средней: ' + CAST(@AboveAvg AS VARCHAR);
ELSE
    PRINT 'Нет контрактов на сумму больше средней.';

IF @BelowAvg > 0
    PRINT 'Количество контрактов на сумму меньше средней: ' + CAST(@BelowAvg AS VARCHAR);
ELSE
    PRINT 'Нет контрактов на сумму меньше средней.';
go

--6
SELECT 
    CASE 
        WHEN Amount < 1000000 THEN 'недорого'
        WHEN Amount BETWEEN 1000000 AND 3000000 THEN 'нормально'
        WHEN Amount > 3000000 THEN 'дорого'
        ELSE 'неизвестная категория'
    END AS Сумма_контракта,
    COUNT(*) AS Количество
FROM CONTRACTS
WHERE Loan_number = 1
GROUP BY 
    CASE 
        WHEN Amount < 1000000 THEN 'недорого'
        WHEN Amount BETWEEN 1000000 AND 3000000 THEN 'нормально'
        WHEN Amount > 3000000 THEN 'дорого'
        ELSE 'неизвестная категория'
    END;
go

--7
CREATE TABLE #example
(
	ID int,
	number_1 int,
	number_2 int,
);

DECLARE  @iter int = 1;
WHILE @iter <= 10
	begin
	INSERT #example(ID, number_1, number_2)
			values(rand() * 10, rand() * 20, rand() * 30);
	if (@iter = 10)
		print 'Таблица заполнена десятью числами'
	SET @iter = @iter + 1;
	end
select * from #example
drop table #example
go

--8
DECLARE @xx int = 5
print @xx + 1
print @xx + 2
RETURN
print @xx + 3
go

--9
select * from CLIENTS

BEGIN TRY
 UPDATE CLIENTS set Ownership_type = '12345678910' WHERE Registration_number = 1001
END TRY
BEGIN CATCH
print ERROR_NUMBER() 
print ERROR_MESSAGE()
print ERROR_LINE() 
print ERROR_PROCEDURE() 
print ERROR_SEVERITY()
print ERROR_STATE() 
END CATCH