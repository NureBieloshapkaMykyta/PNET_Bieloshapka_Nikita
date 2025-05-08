INSERT INTO Customers (Name, Email) VALUES
('���� ��������', 'oleg.p@gmail.com'),
('����� ���������', 'iryna.kovalenko@gmail.com'),
('����� ������', 'serhiy.bondar@gmail.com');

INSERT INTO Products (Name, Price, Stock) VALUES
('������� Lenovo', 25000.00, 10),
('�������� Samsung', 18000.00, 15),
('��������� Sony', 3200.00, 30),
('��������� Philips', 4500.00, 8),
('����� "SQL ��� ����������"', 550.00, 50);

INSERT INTO ProductCategories (Name) VALUES
('����������'),
('����'),
('������� ������'),
('�����');

INSERT INTO ProductCategoryLinks (ProductId, CategoryId) VALUES
(1, 1),
(2, 1), 
(3, 1), 
(3, 2), 
(4, 3),
(5, 4);

EXEC CreateOrder @CustomerId = 1, @ProductId = 1, @Quantity = 2;

EXEC CreateOrder @CustomerId = 2, @ProductId = 5, @Quantity = 3;

EXEC CreateOrder @CustomerId = 3, @ProductId = 4, @Quantity = 1;

EXEC CreateOrder @CustomerId = 1, @ProductId = 3, @Quantity = 2;