USE [K_MyBase]
GO

/****** Object:  StoredProcedure [dbo].[PLOANS]    Script Date: 20.12.2024 13:43:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--2
ALTER PROCEDURE PLOANS @p VARCHAR(50) = NULL, @c INT OUTPUT  
AS
BEGIN
    DECLARE @k INT = (select COUNT(*) FROM LOANS);
	print 'Параметры: ' + @p + ' ' + cast(@c as varchar(50))
    SELECT * FROM LOANS 
	where Loan_type_name = @p
    set @c = @@RowCount
	return @k
END;
GO

--3
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE PLOANS @p VARCHAR(100) = NULL 
AS
BEGIN
    DECLARE @k INT = (select COUNT(*) FROM LOANS);
    SELECT * FROM LOANS
    WHERE Loan_type_name = @p;
	PRINT 'Общее количество строк в таблице LOANS: ' + CAST(@k AS VARCHAR(10));
END;
GO





