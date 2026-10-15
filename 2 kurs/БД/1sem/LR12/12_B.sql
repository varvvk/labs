
--4
----------B--------
BEGIN TRANSACTION;
SELECT @@SPID; 
INSERT LOANS VALUES ('1212', 'Ипотека', 5.5);
UPDATE LOANS SET Loan_type_name = 'Ипотекаааа' WHERE Loan_type_name = 'Ипотека';
-------------t1---------------------
-------------t2---------------------
ROLLBACK; 


--5
--B--
BEGIN TRANSACTION;
------------t1--------------
SELECT @@SPID; 
UPDATE LOANS SET Loan_number = '101' WHERE Loan_number = '10';
commit
-------------t2---------------------


--6
--- B ---
begin transaction
-------------------------- t1 --------------------
INSERT LOANS VALUES ('789012', 'Микро-Бизнес Банк', 1.500);
commit
-------------------------- t2 --------------------


--7

--- B ---
BEGIN TRANSACTION;
delete LOANS where Loan_type_name = 'Кредит наличными';
INSERT LOANS VALUES (7777, 'Кредит наличными', 1.500);
UPDATE LOANS SET Loan_type_name = 'Кредит наличными' WHERE Rate = 1.500;
SELECT Loan_type_name FROM LOANS WHERE Rate = 1.500;
-------------------------- t1 --------------------
COMMIT;
SELECT Loan_type_name FROM LOANS WHERE Rate = 1.500;
-------------------------- t2 --------------------
