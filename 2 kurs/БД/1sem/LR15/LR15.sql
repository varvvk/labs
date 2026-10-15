
--1

CREATE TABLE TR_LOANS (
    ID INT IDENTITY, 
    STMT VARCHAR(20) CHECK (STMT IN ('INS', 'DEL', 'UPD')),
    TRNAME VARCHAR(50),
    CC VARCHAR(300)
);

go

create trigger TR_LOANS_INS on LOANS after insert 
as
declare @code int, @name nvarchar(70), @rate decimal, @all nvarchar(300) = ''
print 'Операция INSERT'
set @code = (select Loan_number from inserted)
set @name = (select Loan_type_name from inserted)
set @rate = (select Rate from inserted)
set  @all = cast(@code as nvarchar) + ' ' + @name + ' ' + cast(@rate as nvarchar(20));
insert into TR_LOANS(STMT, TRNAME, CC) values ('INS','TR_LOANS_INS', @all)
return;

insert into LOANS(Loan_number, Loan_type_name, Rate) values (20, 'Кредит 1', 10)
select * from TR_LOANS;

go

--2

CREATE TRIGGER TR_LOANS_DEL ON LOANS AFTER DELETE 
AS
declare @code int, @name nvarchar(70), @rate decimal, @all nvarchar(300) = ''
PRINT 'Операция DELETE';
SELECT @code = Loan_number, @name = Loan_type_name, @rate = Rate 
FROM deleted;
set  @all = cast(@code as nvarchar) + ' ' + @name + ' ' + cast(@rate as nvarchar(20));
insert into TR_LOANS(STMT, TRNAME, CC) values ('DEL','TR_LOANS_DEL', @all)
RETURN;

DELETE FROM LOANS WHERE Loan_number = 20;
SELECT * FROM TR_LOANS;

go

--3
CREATE TRIGGER TR_LOANS_UPD ON LOANS AFTER UPDATE 
AS
declare @code int, @name nvarchar(70), @rate decimal, @all nvarchar(300) = ''
PRINT 'Операция UPDATE';
SELECT @code = Loan_number, @name = Loan_type_name, @rate = Rate  FROM inserted;
SELECT @code = Loan_number, @name = Loan_type_name, @rate = Rate FROM deleted;
set  @all = cast(@code as nvarchar) + ' ' + @name + ' ' + cast(@rate as nvarchar(20));
insert into TR_LOANS(STMT, TRNAME, CC) values ('UPD','TR_LOANS_UPD', @all)
RETURN;

UPDATE LOANS SET Loan_type_name = 'Новое название кредита' WHERE Loan_number = 20;
SELECT * FROM TR_LOANS;

go

--4
CREATE TRIGGER TR_LOANS_TRIG ON LOANS 
AFTER INSERT, DELETE, UPDATE
AS
DECLARE @code INT, @name NVARCHAR(70), @rate DECIMAL, @all NVARCHAR(300);
DECLARE @ins INT = (SELECT COUNT(*) FROM inserted),
        @del INT = (SELECT COUNT(*) FROM deleted);

IF @ins > 0 AND @del = 0
BEGIN
    PRINT 'Событие: INSERT';
    SET @code = (SELECT Loan_number FROM inserted);
    SET @name = (SELECT Loan_type_name FROM inserted);
    SET @rate = (SELECT Rate FROM inserted);
    SET @all = CAST(@code AS NVARCHAR) + ' ' + @name + ' ' + CAST(@rate AS NVARCHAR(20));
    INSERT INTO TR_LOANS(STMT, TRNAME, CC) VALUES('INS', 'TR_LOANS_INS(4)', @all);
END

ELSE IF @ins = 0 AND @del > 0
BEGIN
    PRINT 'Событие: DELETE';
    SET @code = (SELECT Loan_number FROM deleted);
    SET @name = (SELECT Loan_type_name FROM deleted);
    SET @rate = (SELECT Rate FROM deleted);
    SET @all = CAST(@code AS NVARCHAR) + ' ' + @name + ' ' + CAST(@rate AS NVARCHAR(20));
    INSERT INTO TR_LOANS(STMT, TRNAME, CC) VALUES('DEL', 'TR_LOANS_DEL(4)', @all);
END

ELSE IF @ins > 0 AND @del > 0
BEGIN
    PRINT 'Событие: UPDATE';
    SET @code = (SELECT Loan_number FROM inserted);
    SET @name = (SELECT Loan_type_name FROM inserted);
    SET @rate = (SELECT Rate FROM inserted);
    SET @all = CAST(@code AS NVARCHAR) + ' ' + @name + ' ' + CAST(@rate AS NVARCHAR(20));
    
    SET @code = (SELECT Loan_number FROM deleted);
    SET @name = (SELECT Loan_type_name FROM deleted);
    SET @rate = (SELECT Rate FROM deleted);
    SET @all = CAST(@code AS NVARCHAR) + ' ' + @name + ' ' + CAST(@rate AS NVARCHAR(20)) + ' ' + @all;
    
    INSERT INTO TR_LOANS(STMT, TRNAME, CC) VALUES('UPD', 'TR_LOANS_UPD(4)', @all);
END

RETURN;

insert into LOANS(Loan_number, Loan_type_name, Rate) values (40, 'Кредит 40', 10);
UPDATE LOANS SET Loan_type_name = 'Новое название кредита' WHERE Loan_number = 40;
DELETE FROM LOANS WHERE Loan_number = 40;

SELECT * FROM TR_LOANS;

drop trigger TR_LOANS_TRIG;

drop trigger  TR_LOANS_INS;
drop trigger  TR_LOANS_UPD;
drop trigger  TR_LOANS_DEL;

drop table TR_LOANS
go

--5
alter table LOANS add constraint Rate check(Rate >= 0)

UPDATE LOANS SET Rate = -10 WHERE Loan_number = 40;

SELECT * FROM TR_LOANS;
GO

--6
CREATE TRIGGER TR_LOANS_DEL1 ON LOANS AFTER DELETE AS
BEGIN
	PRINT 'Триггер 1';
	DECLARE @a NVARCHAR(50) = (SELECT Loan_number FROM deleted);
	INSERT INTO TR_LOANS VALUES ('DEL', 'TR_LOANS_DEL1', @a);
END;
GO

CREATE TRIGGER TR_LOANS_DEL2 ON LOANS AFTER DELETE AS
BEGIN
	PRINT 'Триггер 2';
	DECLARE @a NVARCHAR(50) = (SELECT Loan_number FROM deleted);
	INSERT INTO TR_LOANS VALUES ('DEL', 'TR_LOANS_DEL2', @a);
END;
GO

CREATE TRIGGER TR_LOANS_DEL3 ON LOANS AFTER DELETE AS
BEGIN
	PRINT 'Триггер 3';
	DECLARE @a NVARCHAR(50) = (SELECT Loan_number FROM deleted);
	INSERT INTO TR_LOANS VALUES ('DEL', 'TR_LOANS_DEL3', @a);
END;
GO

EXEC SP_SETTRIGGERORDER @triggername = 'TR_LOANS_DEL1', @order = 'None', @stmttype = 'DELETE';
EXEC SP_SETTRIGGERORDER @triggername = 'TR_LOANS_DEL2', @order = 'Last', @stmttype = 'DELETE';
EXEC SP_SETTRIGGERORDER @triggername = 'TR_LOANS_DEL3', @order = 'First', @stmttype = 'DELETE';

INSERT INTO LOANS (Loan_number, Loan_type_name, Rate) VALUES (123456, 'Кредит 1111111', 10);
DELETE FROM LOANS WHERE Loan_number = 123456;

SELECT * FROM TR_LOANS;

SELECT t.name, e.type_desc
FROM sys.triggers t
JOIN sys.trigger_events e ON t.object_id = e.object_id
WHERE OBJECT_NAME(t.parent_id) = 'LOANS' AND e.type_desc = 'DELETE';

DROP TRIGGER TR_LOANS_DEL1;
DROP TRIGGER TR_LOANS_DEL2;
DROP TRIGGER TR_LOANS_DEL3;
go

--7
CREATE TRIGGER LOANS_TRAN ON LOANS AFTER INSERT, DELETE, UPDATE 
AS
BEGIN
	DECLARE @count INT = (SELECT COUNT(*) FROM LOANS);
	IF (@count > 3)
	BEGIN
		RAISERROR('Количество записей превышает допустимый предел для триггера', 10, 1);
		ROLLBACK;
	END;
	RETURN;
END;

INSERT INTO LOANS (Loan_number, Loan_type_name, Rate) VALUES (123456, 'Кредит 1', 10);

DROP TRIGGER LOANS_TRAN;
go

--8
CREATE TRIGGER LOANS_INSTEAD_OF ON LOANS INSTEAD OF DELETE AS
BEGIN
	RAISERROR(N'Удаление запрещено!', 10, 1);
	RETURN;
END;

insert into LOANS(Loan_number, Loan_type_name, Rate) values (60, 'Кредит 60', 10);

DELETE FROM LOANS WHERE Loan_number = 60;

DROP TRIGGER LOANS_INSTEAD_OF;
go

--9
CREATE TRIGGER DDL_CLIENTS ON DATABASE
	FOR DDL_DATABASE_LEVEL_EVENTS AS
BEGIN
	DECLARE @eventType VARCHAR(50), @objectName VARCHAR(50), @objectType VARCHAR(50);
	SET @eventType = EVENTDATA().value('(/EVENT_INSTANCE/EventType)[1]','VARCHAR(50)');
	SET @objectName = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]','VARCHAR(50)');
	SET @objectType = EVENTDATA().value('(/EVENT_INSTANCE/ObjectType)[1]','VARCHAR(50)');

	IF @objectName = 'CLIENTS' 
	BEGIN
		PRINT 'Тип события: ' + @eventType;
		PRINT 'Имя объекта: ' + @objectName;
		PRINT 'Тип объекта: ' + @objectType;
		RAISERROR('Операции с таблицей Виды_кредитов запрещены!', 10, 1);
		ROLLBACK;
	END;
END;

alter table CLIENTS drop column Contact_person

DROP TRIGGER DDL_CLIENTS ON DATABASE;
