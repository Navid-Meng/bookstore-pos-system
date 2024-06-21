CREATE DATABASE POS_Book;
GO
USE POS_BOOK;
GO
CREATE TABLE tblPublisher (
    id INT PRIMARY KEY IDENTITY,
    publisher VARCHAR(MAX) NOT NULL
);
GO
CREATE TABLE tblGenre (
    id INT PRIMARY KEY IDENTITY,
    genre VARCHAR(MAX) NOT NULL
);
GO
CREATE TABLE tblBook (
    bookCode VARCHAR(50) PRIMARY KEY,
    bookTitle VARCHAR(MAX),
	bookAuthor VARCHAR(MAX),
    publisherID INT,
    genreID INT,
    price DECIMAL(18,2),
    qty INT,
    FOREIGN KEY (publisherID) REFERENCES tblPublisher(id),
    FOREIGN KEY (genreID) REFERENCES tblGenre(id)
);
GO 
CREATE TABLE tblStockIn (
    id INT PRIMARY KEY IDENTITY,
    refNo VARCHAR(50),
    bookCode VARCHAR(50) FOREIGN KEY REFERENCES tblBook(bookCode),
    qty INT,
    stockInDate DATETIME,
    stockInBy VARCHAR(100)
);
GO
CREATE TABLE tblUser (
  username VARCHAR(50) PRIMARY KEY, 
  password VARCHAR(50) not null, 
  role VARCHAR(250) not null, 
  name VARCHAR(250)not null,
  isActive BIT NOT NULL DEFAULT 1
);
GO
INSERT INTO tblUser(username, password, role, name) VALUES ('admin', 'admin', 'Admin', 'Administrator'); 
GO
CREATE TABLE tblCart (
    id INT PRIMARY KEY IDENTITY,
    transno VARCHAR(50) NULL,
    pid VARCHAR(50),
    price DECIMAL(18,2) NULL,
    qty INT NOT NULL,
    total DECIMAL(18,2) NULL,
    sdate DATE NOT NULL,
    status VARCHAR(50) NULL DEFAULT ('Pending'),
    cashierName VARCHAR(50) NULL
);
GO
CREATE TRIGGER dbo.ComputeTotal
ON dbo.tblCart
AFTER INSERT, DELETE, UPDATE
AS 
BEGIN 
    SET NOCOUNT ON;
    UPDATE tblCart SET total = price * qty;
END

