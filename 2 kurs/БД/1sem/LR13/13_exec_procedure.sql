select * from LOANS

use K_MyBase
go

--1
CREATE PROCEDURE PLOANS
AS
BEGIN
    SELECT * FROM LOANS;
    DECLARE @RowCount INT = (select COUNT(*) FROM LOANS);
    return @RowCount
END;
go

DECLARE @RowCount int = 0;
exec @RowCount = PLOANS
print 'Количество строк:' + cast(@RowCount as nvarchar)
GO

DROP PROCEDURE PLOANS
go

--2
DECLARE @k INT = 0, @r INT = 0, @p VARCHAR(50);
SET @p = 'Ипотека на коммерческую недвижимость';

EXEC @k = PLOANS @p = @p, @c = @r OUTPUT;

PRINT 'кол-во кредитов всего = ' + CAST(@k AS VARCHAR(3));
PRINT 'кол-во кредитов, заказанных по типу "' + @p + '" = ' + CAST(@r AS VARCHAR(3));

go

--3
CREATE TABLE #LOANS (
    Loan_number INT,
    Loan_type_name NVARCHAR(100),
    Rate FLOAT
);

INSERT INTO #LOANS
EXEC PLOANS @p = 'Ипотека на коммерческую недвижимость';

INSERT INTO #LOANS
EXEC PLOANS @p = 'Ипотека на покупку земельных участков';

SELECT * FROM #LOANS;

DROP TABLE #LOANS;

go

--4
CREATE PROCEDURE PLOANS_INSERT
    @a INT,
    @n NVARCHAR(70),
    @c DECIMAL
AS declare @rc int = 1;
    BEGIN TRY
        INSERT INTO LOANS(Loan_number, Loan_type_name, Rate)
        VALUES (@a, @n, @c);
        RETURN @rc;
    END TRY
    BEGIN CATCH
        PRINT 'Номер: ' + CAST(ERROR_NUMBER() AS NVARCHAR(10));
        PRINT 'Уровень: ' + CAST(ERROR_SEVERITY() AS NVARCHAR(10));
        PRINT  'Сообщение: ' + ERROR_MESSAGE();
        RETURN -1;
    END CATCH
GO

DECLARE @rc INT;

EXEC @rc= PLOANS_INSERT
    @a = 10,
    @n = 'AA',
    @c = 7.45

PRINT 'Код' + cast(@rc as varchar);
GO

DROP PROCEDURE PLOANS_INSERT
go

--5
select * from LOANS
go

CREATE PROCEDURE PLOANS_REPORT @p NVARCHAR(70)
AS 
BEGIN
	BEGIN try
    DECLARE @rc INT = 0;
        DECLARE @tv NVARCHAR(70), @t NVARCHAR(MAX) = '';
        DECLARE LOAN_REPORT CURSOR FOR 
        SELECT Loan_type_name 
        FROM LOANS
		WHERE Loan_type_name = 'Ипотека на коммерческую недвижимость';
        
        IF NOT EXISTS (SELECT Loan_type_name FROM LOANS WHERE Loan_type_name = @p)
            RAISERROR('Ошибка', 11, 1);
        ELSE 
            OPEN LOAN_REPORT;
            FETCH NEXT FROM LOAN_REPORT INTO @tv;
            PRINT 'Список видов кредитов';
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @t = @tv + ', ' + @t;
                SET @rc = @rc + 1;
                FETCH NEXT FROM LOAN_REPORT INTO @tv;
            END;
			PRINT @t;
            CLOSE LOAN_REPORT;
            DEALLOCATE LOAN_REPORT;
            RETURN @rc;
    END TRY
	BEGIN CATCH
	PRINT 'Ошибка в параметрах.';
        IF ERROR_PROCEDURE() IS NOT NULL
            PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
        RETURN @rc;
	END CATCH
END;
GO

DECLARE @rc INT;
EXEC @rc = PLOANS_REPORT @p = 'Ипотека на коммерческую недвижимость';
PRINT 'Количество видов кредитов = ' + CAST(@rc AS VARCHAR(3));
go

DROP PROCEDURE PLOANS_REPORT
go

--6
SELECT * FROM CONTRACTS
SELECT * FROM LOANS
go

CREATE PROCEDURE PCONTRACTS_INSERTX 
	@a INT,
    @n NVARCHAR(70),
    @c DECIMAL,
	@ci INT,
    @rn INT,
	@am INT,
    @b DATE,
	@s DATE
AS 
    DECLARE @rc int = 1;
    BEGIN TRY
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
        BEGIN TRAN
        INSERT INTO CONTRACTS(Contract_id, Loan_number, Registration_number, Amount, Issue_date, Return_date)
        VALUES (@ci, @a, @rn, @am, @b, @s)
		exec @rc = PLOANS_INSERT @a, @n, @c
        COMMIT TRAN;
        RETURN @rc;
    END TRY
    BEGIN CATCH
        PRINT 'Номер ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(6));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        PRINT 'Уровень: ' + CAST(ERROR_SEVERITY() AS VARCHAR(6));
        PRINT 'Метка: ' + CAST(ERROR_STATE() AS VARCHAR(8));
        PRINT 'Номер строки: ' + CAST(ERROR_LINE() AS VARCHAR(8));
        IF ERROR_PROCEDURE() IS NOT NULL
            PRINT 'Имя процедуры: ' + ERROR_PROCEDURE();
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;
        RETURN -1;
    END CATCH;
GO

DECLARE @rc int;
EXEC @rc = PCONTRACTS_INSERTX @a = 9, @n = 'New Credit Type', @c = 7.45,
@ci = 10015, @rn = 1001, @am = 23000, @b= '2021-11-01', @s = '2022-11-23';
PRINT 'Код ошибки: ' + CAST(@rc AS VARCHAR(3));
go

drop PROCEDURE PCONTRACTS_INSERTX 
go

delete CONTRACTS where Contract_id = 10015
