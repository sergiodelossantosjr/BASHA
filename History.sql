CREATE TABLE [dbo].[History]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Sensor] NVARCHAR(50) NULL, 
    [Temperature] DECIMAL NULL, 
    [Magnitude] DECIMAL NULL, 
    [CreatedOn] DATETIME NULL
)
