use master;
GO

create database K_MyBase 
on primary
( name = N'K_MyBase_mdf', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_mdf.mdf', 
   size = 10240Kb, maxsize=UNLIMITED, filegrowth=1024Kb),
( name = N'K_MyBase_ndf', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_ndf.ndf', 
   size = 10240KB, maxsize=1Gb, filegrowth=25%),
filegroup FG1
( name = N'K_MyBase_fg1', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_fgq-1.ndf', 
   size = 10240Kb, maxsize=1Gb, filegrowth=25%),
filegroup FG2
( name = N'K_MyBase_fg2', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_fgq-2.ndf', 
   size = 10240Kb, maxsize=1Gb, filegrowth=25%),
filegroup FG3
( name = N'K_MyBase_fg3', filename = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_fgq-3.ndf', 
   size = 10240Kb, maxsize=1Gb, filegrowth=25%)
log on
( name = N'K_MyBase_log', filename=N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\K_MyBase_log.ldf',       
   size=10240Kb,  maxsize=2048Gb, filegrowth=10%)
GO

USE K_MyBase;
GO

CREATE TABLE LOANS (
	Loan_number int NOT NULL,
	Loan_type_name nvarchar(70) NOT NULL,
	Rate decimal(7,4) NOT NULL,
	CONSTRAINT PK_LOANS PRIMARY KEY (Loan_number)
) on FG1;
GO

CREATE TABLE CLIENTS (
	Registration_number int NOT NULL,
	Ownership_type nvarchar(10) NOT NULL,
	Firm_name nvarchar(100) NOT NULL,
	Address nvarchar(200) NOT NULL,
	Phone_number nvarchar(50) NOT NULL,
	
	CONSTRAINT PK_CLIENTS PRIMARY KEY (Registration_number)
) on FG2;
GO

CREATE TABLE CONTRACTS (
	Contract_id int NOT NULL,
	Loan_number int NOT NULL,
	Registration_number int NOT NULL,
	Amount int NOT NULL,
	Issue_date date NOT NULL,
	Return_date date NOT NULL,
	CONSTRAINT PK_CONTRACTS PRIMARY KEY (Contract_id),
	CONSTRAINT FK_LOANS FOREIGN KEY (Loan_number) REFERENCES LOANS(Loan_number),
	CONSTRAINT FK_CLIENTS FOREIGN KEY (Registration_number) REFERENCES CLIENTS(Registration_number)
) on FG3;
GO

ALTER TABLE CLIENTS ADD Contact_person nvarchar(30)
GO

ALTER TABLE CLIENTS ALTER COLUMN Contact_person nvarchar(30) NOT NULL
GO

INSERT INTO LOANS (Loan_number, Loan_type_name, Rate)
VALUES (1, 'Ипотека на коммерческую недвижимость', 5.7500),
       (2, 'Ипотека на складские и производственные помещения', 3.4500),
       (3, 'Ипотека на покупку земельных участков', 4.2000),
       (4, 'Ипотека на строительство коммерческой недвижимости', 6.0000),
       (5, 'Ипотека под залог уже имеющейся недвижимости', 2.9000);

INSERT INTO CLIENTS (Registration_number, Ownership_type, Firm_name, Address, Phone_number, Contact_person)
VALUES (1001, 'ООО', 'Тех Инновации', 'ул. Ленина, д. 10, Минск', '+375 29 474-55-33', 'Иван Иванов'),
       (1002, 'ЗАО', 'Зелёная Энергия', 'ул. Пушкина, д. 5, Минск', '+375 44 735-52-63', 'Мария Сидорова'),
       (1003, 'ООО', 'Креатив Студия', 'ул. Тверская, д. 15, Минск', '+375 33 856-38-45', 'Пётр Петров'),
       (1004, 'ИП', 'АвтоТехСервис', 'ул. Карла Маркса, д. 2, Минск', '+375 33 752-35-04', 'Анна Иванова'),
       (1005, 'АО', 'Техно Прогресс', 'ул. Советская, д. 8, Минск', '+375 29 634-54-82', 'Дмитрий Смирнов');
GO

INSERT INTO CONTRACTS (Contract_id, Loan_number, Registration_number, Amount, Issue_date, Return_date)
VALUES (1, 1, 1001, 500000, '2023-01-15', '2024-01-15'),
       (2, 2, 1002, 250000, '2023-02-01', '2023-12-01'),
       (3, 3, 1003, 3000000, '2023-03-10', '2043-03-10'),
       (4, 4, 1004, 800000, '2023-04-01', '2026-04-01'),
       (5, 5, 1005, 150000, '2023-05-15', '2025-05-15');
GO

SELECT * FROM LOANS;
SELECT Loan_number, Rate FROM LOANS;
UPDATE LOANS SET Rate = 6.75 Where Loan_number=1;
