 CREATE TABLE Sellers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Phone NVARCHAR(MAX) NOT NULL,
    ShopName NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(MAX) NOT NULL
);
CREATE TABLE Orders
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(MAX),
    Price INT,
    Size NVARCHAR(MAX),
    Color NVARCHAR(MAX),
    Quantity INT,
    BuyerName NVARCHAR(MAX),
    OrderDate DATETIME2,
    Status NVARCHAR(MAX)
);
 CREATE TABLE Ratings
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(MAX),
    Stars INT,
    UserName NVARCHAR(MAX),
    Review NVARCHAR(MAX),
    ReviewDate DATETIME2
);