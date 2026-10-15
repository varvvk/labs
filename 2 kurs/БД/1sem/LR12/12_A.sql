
--1
set nocount on
if exists (select * from  SYS.OBJECTS where OBJECT_ID = object_id (N'DBO.TAB') )--таблица TAB есть?
drop table TAB;
declare @c int, @flag char = 'r';	-- commit или rollback?
SET IMPLICIT_TRANSACTIONS  ON		-- включ. режим неявной транзакции
create table TAB (i int);				-- начало транзакции 
	insert TAB values (1),(2),(3),(4),(5);
	set @c = (select count(*) from TAB);
	print N'кол-во строк в TAB: ' + cast(@c as varchar(2));
	if @flag = 'c' commit -- фиксация
	else rollback; -- откат
SET IMPLICIT_TRANSACTIONS OFF -- выключ. режим неявной транзакции

if exists (select * from SYS.OBJECTS where OBJECT_ID= object_id(N'DBO.TAB')) 
print N'таблица TAB есть';
else print N'таблицы TAB нет'
go

--2
begin try
	begin tran		
		insert CLIENTS values (1010, 'ООО', 'клиент 1', 'г.Минск, ул. Белорусская, д.1', '+1234567890', 'Анна Добрая');
		insert CLIENTS values (1012, 'ООО', 'клиент2', 'г.Гродно, ул. Советская, д.10', '+9876543210', 'Иван Петров');
	commit tran;
end try
begin catch
	print N'ошибка:' + case
	when error_number() = 2627 and patindex('%Клиенты_PK%', error_message()) > 0
	then N'дублирование ID клиента'
	else N'неизвестная ошибка' + cast(error_number() as varchar(5)) + error_message()
	end;
	if @@TRANCOUNT > 0 rollback tran;
end catch
select * from CLIENTS
go

--3
DECLARE @point NVARCHAR(10);
BEGIN TRY
    BEGIN TRAN
		delete CLIENTS where Registration_number = 1011;
		set @point = 'p1'; save tran @point;
		insert CLIENTS values (1011, 'ООО', 'клиент 2', 'г.Минск, ул. Белорусская, д.1', '+1234567890', 'Анна Добрая', null);
		set @point = 'p2'; save tran @point;
		insert CLIENTS values (1011, 'ООО', 'клиент 1', 'г.Минск, ул. Белорусская, д.1', '+1234567890', 'Анна Добрая', null);
        COMMIT TRAN;
END TRY
BEGIN CATCH
    PRINT N'ошибка: ' + CASE 
        WHEN ERROR_NUMBER() = 2627 AND PATINDEX('%Кредиты_PK%', ERROR_MESSAGE()) > 0 
		THEN N'дублирование номера кредита'
        ELSE N'неизвестная ошибка: ' + CAST(ERROR_NUMBER() AS VARCHAR(5)) + ERROR_MESSAGE()
    END;
    IF @@TRANCOUNT > 0
    BEGIN
        PRINT N'контрольная точка: ' + @point;
        ROLLBACK TRAN @point;
        COMMIT TRAN; 
    END;
END CATCH;
SELECT * FROM CLIENTS
go

--4
----------------A------------------
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
BEGIN TRANSACTION;

----------------t1-----------------
SELECT @@SPID, 'insert LOANS' N'результат', * FROM LOANS WHERE Loan_type_name = 'Ипотекаааа';
SELECT @@SPID, 'update LOANS' AS 'результат', Loan_number, Loan_type_name, Rate FROM LOANS WHERE Loan_type_name = 'Ипотекаааа';
COMMIT;
---------------t2----------------------


--5
--А--
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;
SELECT count(*) from LOANS where Loan_number = '10';
--------------t1---------------
--------------t2---------------
SELECT 'update Кредиты' N'результат', count(*) from LOANS where Loan_number = '101';
COMMIT;


--6
select * from LOANS

--- A ---
set transaction isolation level REPEATABLE READ
begin transaction
select Loan_type_name from LOANS where Loan_type_name = N'Микро-Бизнес Банк';
-------------------------- t1 ------------------
-------------------------- t2 -----------------
select case
when Loan_type_name = N'Новый кредит' then 'insert Виды_кредитов' else ' '
end N'результат', Loan_type_name from LOANS where Loan_type_name = N'Микро-Бизнес Банк';
commit;

delete LOANS where Loan_number = 789012

--7
select * from LOANS

-- A ---
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

delete LOANS where Loan_type_name = 'Кредит наличными';
INSERT LOANS VALUES (7777, 'Кредит наличными', 1.500);
UPDATE LOANS SET Loan_type_name = 'Кредит наличными' WHERE Rate = 1.500;
SELECT Loan_type_name FROM LOANS WHERE Rate = 1.500;
-------------------------- t1 -----------------
SELECT Loan_type_name FROM LOANS WHERE Rate = 1.500;
-------------------------- t2 ------------------
COMMIT;

delete LOANS where Loan_number = 7777


--8
begin tran
	insert LOANS values (11111, 'bbb', 10.53);
	begin tran
		update LOANS set Loan_type_name = 'cc' where Loan_type_name = 'bb'
		commit;
	if @@TRANCOUNT > 0 rollback;
select 
	(select count(*) from dbo.LOANS where LOANS.Loan_type_name = 'bbb') N'1',
	(select count(*) from dbo.LOANS where LOANS.Loan_type_name = N'cc') N'2';

select * from LOANS