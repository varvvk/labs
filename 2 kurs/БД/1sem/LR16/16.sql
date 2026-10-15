

--1
select * from CLIENTS

SELECT 
    CLIENTS.Registration_number [Registration_number],
    CLIENTS.Ownership_type [Ownership_type],
    CLIENTS.Firm_name [Firm_name],
    CLIENTS.Address [Address],
    CLIENTS.Phone_number [Phone_number],
    CLIENTS.Contact_person [Contact_person]
FROM CLIENTS [CLIENTS]
WHERE CLIENTS.Ownership_type = 'ООО'
FOR XML PATH('Firm'), ROOT('Firms')


--2

SELECT LOANS.Loan_type_name [Name], CONTRACTS.Amount, CONTRACTS.Issue_date
FROM LOANS
JOIN CONTRACTS
ON LOANS.Loan_number = CONTRACTS.Loan_number
WHERE Amount > 800000
FOR XML AUTO,
ROOT('Loans_list'), ELEMENTS;


--3
DECLARE @NewLoansXML NVARCHAR(MAX) = 
N'<NewLoans>
    <Loan Loan_number="1616" Loan_type_name="Ипотека на строительство жилого дома" Rate="4.500" />
    <Loan Loan_number="161616" Loan_type_name="Кредит на покупку автомобиля" Rate="3.800" />
    <Loan Loan_number="161" Loan_type_name="Ипотека под залог коммерческой недвижимости" Rate="5.200" />
</NewLoans>';

DECLARE @docHandle INT;

EXEC sp_xml_preparedocument @docHandle OUTPUT, @NewLoansXML;

INSERT INTO LOANS (Loan_number, Loan_type_name, Rate)
SELECT * FROM OPENXML(@docHandle, '/NewLoans/Loan', 1)
WITH (
    Loan_number INT '@Loan_number',
    Loan_type_name NVARCHAR(255) '@Loan_type_name',
    Rate DECIMAL(10, 3) '@Rate'
);

EXEC sp_xml_removedocument @docHandle;

SELECT * FROM LOANS;

DELETE FROM LOANS WHERE Loan_number = '1616';
DELETE FROM LOANS WHERE Loan_number = '161616';
DELETE FROM LOANS WHERE Loan_number = '161';
 select * from LOANS


 --4
ALTER TABLE CLIENTS
ADD AddressXML XML;

INSERT INTO CLIENTS (Registration_number, Ownership_type, Firm_name, Address, Phone_number, Contact_person, AddressXML)
VALUES 
(1011, 'ООО', 'XML Клиент', 
    'ул. Ленина, д. 25, Минск',
    '+375 29 777-88-99', 
    'Екатерина Новик',
    '<Address>
        <Street>ул. Ленина</Street>
        <Building>25</Building>
        <City>Минск</City>
    </Address>');

UPDATE CLIENTS 
SET AddressXML = '<Address>
        <Street>ул. Ленина</Street>
        <Building>25</Building>
        <City>Los Angeles</City>
    </Address>' 
	WHERE Registration_number = 1011

SELECT 
    Registration_number,
    Firm_name,
    Address,
    AddressXML.query('/Address/Street') AS StreetXML,
    AddressXML.value('(/Address/Street)[1]', 'NVARCHAR(100)') AS Street,
    AddressXML.value('(/Address/Building)[1]', 'NVARCHAR(50)') AS Building,
    AddressXML.value('(/Address/City)[1]', 'NVARCHAR(50)') AS City
FROM CLIENTS;


 --5
 select * from LOANS

 CREATE XML SCHEMA COLLECTION Loan AS 
N'<?xml version="1.0" encoding="utf-16" ?>
<xs:schema attributeFormDefault="unqualified" 
           elementFormDefault="qualified"
           xmlns:xs="http://www.w3.org/2001/XMLSchema">
    <xs:element name="Loan">  
        <xs:complexType>
            <xs:sequence>
                <xs:element name="Номер_кредита" type="xs:int" />
                
                <xs:element name="Название" type="xs:string" />
                
                <xs:element name="Ставка">
                    <xs:simpleType>
                        <xs:restriction base="xs:decimal">
                            <xs:minInclusive value="0.01" />
                            <xs:maxInclusive value="100.00" />
                        </xs:restriction>
                    </xs:simpleType>
                </xs:element>
            </xs:sequence>
        </xs:complexType>
    </xs:element>
</xs:schema>';

ALTER TABLE LOANS
ADD RateXML XML(CONTENT Loan);

--(ставка слишком велика)
BEGIN TRY
    INSERT INTO LOANS (Rate)
    VALUES (N'<Loan>
                <Номер_кредита>102</Номер_кредита>
                <Название>Потребительский</Название>
                <Ставка>150.00</Ставка>
            </Loan>');
END TRY
BEGIN CATCH
    PRINT 'Ошибка: Неверное значение ставки.';
END CATCH;


INSERT INTO LOANS (Rate)
VALUES (N'<Loan>
            <Номер_кредита>103</Номер_кредита>
            <Название>Автокредит</Название>
            <Ставка>15.50</Ставка>
        </Loan>');


--ставка отрицательная
BEGIN TRY
    UPDATE LOANS
    SET Rate = N'<Кредит>
                    <Номер кредита>103</Номер кредита>
                    <Название>Автокредит</Название>
                    <Ставка>-5.00</Ставка>
                </Кредит>'
    WHERE Rate.exist('/Кредит[Номер кредита=103]') = 1;
END TRY
BEGIN CATCH
    PRINT 'Ошибка: Неверное значение ставки.';
END CATCH;
