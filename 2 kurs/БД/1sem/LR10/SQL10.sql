exec SP_HELPINDEX  'LOANS'
exec SP_HELPINDEX 'CONTRACTS'
exec SP_HELPINDEX 'CLIENTS'
go

-- 1
create table #explre 
(	TIND int,
	TFIELD varchar(100)
);

DECLARE @i int = 0;
WHILE @i < 1000
	begin
INSERT #explre(TIND, TFIELD)
	values(floor(30000*rand()), REPLICATE('строка', 2) + CAST(FLOOR(1000 * RAND()) AS NVARCHAR(10)));
if(@i % 100 = 0)
	print @i;
SET @i = @i + 1;
end;

SELECT TIND, TFIELD
FROM #explre
WHERE TIND BETWEEN 100 AND 200
ORDER BY TIND;

CREATE clustered index #EXPLRE_CL on #EXPLRE(TIND asc)

go

--2
CREATE table #EX
(    TKEY int, 
      CC int identity(1, 1),
      TF varchar(100)
);
   
declare @i int = 0;
while   @i < 20000    
begin
    INSERT #EX(TKEY, TF) values(floor(30000*RAND()), replicate('строка ', 10));
    set @i = @i + 1; 
end;

CREATE index #EX_NONCLU on #EX(TKEY, CC)

SELECT * from  #EX where  TKEY > 1500 and  CC < 4500;
SELECT * from  #EX where  TKEY = 556 and  CC > 3
GO

--3
CREATE table #EX2
(    TKEY int, 
      CC int identity(1, 1),
      TF varchar(100)
);
    
declare @i int = 0;
while   @i < 20000    
begin
    INSERT #EX2(TKEY, TF) values(floor(30000*RAND()), replicate('строка ', 3));
    set @i = @i + 1; 
end;

SELECT CC from #EX2 where TKEY>15000 

CREATE index #EX_TKEY_X2 on #EX2(TKEY) INCLUDE (CC)

GO

--4
CREATE table #EX4
(    TKEY int, 
      CC int identity(1, 1),
      TF varchar(100)
);
   
declare @i int = 0;
while   @i < 20000    
begin
    INSERT #EX4(TKEY, TF) values(floor(30000*RAND()), replicate('строка ', 4));
    set @i = @i + 1; 
end;

SELECT TKEY from  #EX4 where TKEY between 5000 and 19999; 
SELECT TKEY from  #EX4 where TKEY>15000 and  TKEY < 20000  
SELECT TKEY from  #EX4 where TKEY=17000

CREATE  index #EX_WHERE on #EX4(TKEY) where (TKEY>=15000 and 
 TKEY < 20000); 
 
 GO

 --5
 CREATE table #EX5
(    TKEY int, 
      CC int identity(1, 1),
      TF varchar(100)
);
   
declare @i int = 0;
while   @i < 20000    
begin
    INSERT #EX5(TKEY, TF) values(floor(30000*RAND()), replicate('строка ', 4));
    set @i = @i + 1; 
end;

CREATE index #EX_TKEY ON #EX5(TKEY); 

SELECT name [Индекс], avg_fragmentation_in_percent [Фрагментация (%)]
FROM sys.dm_db_index_physical_stats(DB_ID(N'TEMPDB'), 
OBJECT_ID(N'#EX5'), NULL, NULL, NULL) ss  JOIN sys.indexes ii on ss.object_id = ii.object_id and ss.index_id = ii.index_id  WHERE name is not null;

 INSERT top(10000) #EX5(TKEY, TF) select TKEY, TF from #EX5;

 ALTER index #EX_TKEY on #EX5 reorganize;

 ALTER index #EX_TKEY on #EX5 rebuild with (online = off);

 DROP index #EX_TKEY on #EX5;

 --6

CREATE index #EX_TKEY2 on #EX5(TKEY) with (fillfactor = 65);

INSERT top(50)percent INTO #EX5(TKEY, TF) SELECT TKEY, TF  FROM #EX5;

