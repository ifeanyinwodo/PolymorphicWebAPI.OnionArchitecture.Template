/* Use below for postgresql.*/
CREATE TABLE IF NOT EXISTS EventStore(
    id UUID  NOT NULL,
    createdAt TIMESTAMP NOT NULL,
    sequence INT GENERATED ALWAYS AS IDENTITY NOT NULL,
    version INT NOT NULL,
    name VARCHAR(250) NOT NULL,
    aggregateId VARCHAR(250) NOT NULL,
    data TEXT NOT NULL
   
);

CREATE TABLE IF NOT EXISTS ItemCategoryStore(
    id UUID  NOT NULL,
    categoryName VARCHAR(250) NOT NULL,
    description VARCHAR NOT NULL,
    quantity BIGINT NOT NULL
);


/* Use below for mssqlserver.
IF Not EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'EventStore' AND xtype = N'U')
BEGIN
 CREATE TABLE EventStore(
    id UNIQUEIDENTIFIER  NOT NULL,
    createdAt DATETIME2(6) NOT NULL,
    sequence INT  IDENTITY(1,1) NOT NULL,
    version INT NOT NULL,
    name VARCHAR(250) NOT NULL,
    aggregateId VARCHAR(250) NOT NULL,
    data TEXT NOT NULL
    
	);
END

IF Not EXISTS(SELECT name FROM sys.sysobjects WHERE Name = N'ItemCategoryStore' AND xtype = N'U')
BEGIN
 CREATE TABLE  ItemCategoryStore(
    id  VARCHAR(Max)  NOT NULL,
    categoryName VARCHAR(250) NOT NULL,
    description VARCHAR NOT NULL,
    quantity BIGINT NOT NULL
);
END
*/
