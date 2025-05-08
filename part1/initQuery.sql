CREATE DATABASE OnlineShop;
GO

USE OnlineShop;
GO

CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100) UNIQUE
);

CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Price DECIMAL(10, 2),
    Stock INT
);

CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY,
    CustomerId INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    PricePerItem DECIMAL(10, 2),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE ProductCategories (
    CategoryId INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100)
);

CREATE TABLE ProductCategoryLinks (
    ProductId INT,
    CategoryId INT,
    PRIMARY KEY (ProductId, CategoryId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (CategoryId) REFERENCES ProductCategories(CategoryId)
);


CREATE PROCEDURE CreateOrder
    @CustomerId INT,
    @ProductId INT,
    @Quantity INT
AS
BEGIN
    DECLARE @Price DECIMAL(10,2);

    SELECT @Price = Price FROM Products WHERE ProductId = @ProductId;

    IF @Price IS NULL
    BEGIN
        RAISERROR('Product not found', 16, 1);
        RETURN;
    END

    BEGIN TRANSACTION;

    INSERT INTO Orders (CustomerId)
    VALUES (@CustomerId);

    DECLARE @OrderId INT = SCOPE_IDENTITY();

    INSERT INTO OrderItems (OrderId, ProductId, Quantity, PricePerItem)
    VALUES (@OrderId, @ProductId, @Quantity, @Price);

    UPDATE Products
    SET Stock = Stock - @Quantity
    WHERE ProductId = @ProductId;

    COMMIT;
END;
GO

CREATE FUNCTION fn_GetCustomerTotalAmount (@CustomerId INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @Sum DECIMAL(10,2);
    SELECT @Sum = SUM(oi.Quantity * oi.PricePerItem)
    FROM Orders o
    INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
    WHERE o.CustomerId = @CustomerId;

    RETURN ISNULL(@Sum, 0);
END;
GO

CREATE FUNCTION fn_GetCustomerOrdersDetailed (@CustomerId INT)
RETURNS TABLE
AS
RETURN
    SELECT 
        o.OrderId,
        o.OrderDate,
        SUM(oi.Quantity * oi.PricePerItem) AS OrderTotal
    FROM Orders o
    INNER JOIN OrderItems oi ON o.OrderId = oi.OrderId
    WHERE o.CustomerId = @CustomerId
    GROUP BY o.OrderId, o.OrderDate;
GO

CREATE TRIGGER trg_DecreaseStockOnInsert
ON OrderItems
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.Stock = p.Stock - i.Quantity
    FROM Products p
    INNER JOIN inserted i ON i.ProductId = p.ProductId;
END;
GO

CREATE TRIGGER trg_CleanupProductCategoryLinks
ON Products
AFTER DELETE
AS
BEGIN
    DELETE pcl
    FROM ProductCategoryLinks pcl
    INNER JOIN deleted d ON pcl.ProductId = d.ProductId;
END;
GO

DECLARE @CustomerId INT, @CustomerName NVARCHAR(100), @TotalAmount DECIMAL(10,2);

DECLARE customer_cursor CURSOR FOR
SELECT c.CustomerId, c.Name
FROM Customers c;

OPEN customer_cursor;
FETCH NEXT FROM customer_cursor INTO @CustomerId, @CustomerName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @TotalAmount = dbo.fn_GetCustomerTotalAmount(@CustomerId);

    PRINT 'Customer: ' + @CustomerName + ' | Total Spent: ' + CAST(@TotalAmount AS NVARCHAR);

    FETCH NEXT FROM customer_cursor INTO @CustomerId, @CustomerName;
END;

CLOSE customer_cursor;
DEALLOCATE customer_cursor;
GO
