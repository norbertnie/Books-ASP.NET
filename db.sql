CREATE TABLE books.dbo.Authors (
	AuthorID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	LastName varchar(255),
	FirstName varchar(255),
	DateofBirth date
);

CREATE TABLE books.dbo.Books (
	BookID int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	AuthorID int NOT NULL,
	Title varchar(255),
	ReleaseDate date
);

INSERT INTO books.dbo.Authors (LastName,FirstName) VALUES 
('Zeromski','Stefan')
,('Mickiewicz','Adam')
,('Slowacki','Juliusz')
,('Stanislaw','Prus')
,('Zofia','Nalkowska')
,('MUSIEROWICZ','MALGORZATA')
,('JURGIELEWICZOWA','IRENA')
,('Prus','Bolesław')
;

INSERT INTO books.dbo.Books (AuthorID,Title,ReleaseDate) VALUES 
('8','Faraon','1980')
,('1','Przedwiośnie','1980')
,('3','Kordian','1930')
,('2','Dziady','1930')
,('5','Granica','1930')
;


USE books
GO
CREATE VIEW AuthorsBooks AS
SELECT FirstName, LastName, COUNT(Title) AS NumberOfBooks
FROM books.dbo.Authors LEFT JOIN books.dbo.Books 
ON Authors.AuthorID = Books.AuthorID
GROUP BY FirstName, LastName