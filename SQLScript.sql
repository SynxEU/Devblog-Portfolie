------------------------------------------------
-- Drops old DB if it exists
------------------------------------------------
USE [master]
GO
IF EXISTS (SELECT * FROM sysdatabases WHERE name='DevBlog')
BEGIN
    ALTER DATABASE [DevBlog] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE DevBlog
END
GO

------------------------------------------------
-- New DB
------------------------------------------------
CREATE DATABASE DevBlog
GO
------------------------------------------------
-- Tables
------------------------------------------------
USE DevBlog

-- Drop if exists
DROP TABLE IF EXISTS PersonTable;
DROP TABLE IF EXISTS PostTable;
DROP TABLE IF EXISTS TagTable;
DROP TABLE IF EXISTS PostTagTable;

CREATE TABLE PersonTable (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [FirstName] NVARCHAR(255) NOT NULL,
    [LastName] NVARCHAR(255) NOT NULL,
    [Age] INT NOT NULL,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [Password] VARBINARY(80) NOT NULL,
    [City] NVARCHAR(255) NOT NULL,
    [PhoneNumber] NVARCHAR(16) NOT NULL,
    [LinkedIn] NVARCHAR(MAX) NULL,
    [Github] NVARCHAR(MAX) NULL,
	[Salt] VARBINARY(16) NOT NULL,
    [RegistrationDate] DATE DEFAULT GETDATE()
)

CREATE UNIQUE INDEX IX_Person_Id ON PersonTable(Id);

CREATE INDEX IX_Person_Email ON PersonTable(Email);

CREATE TABLE PostTable (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Title] NVARCHAR(255) NOT NULL,
    [AuthorId] UNIQUEIDENTIFIER NOT NULL,
    [Reference] NVARCHAR(MAX),
    [Type] BIT NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [CreateDate] DATE DEFAULT GETUTCDATE(),
	[LastUpdated] DATE DEFAULT GETUTCDATE(),
    FOREIGN KEY ([AuthorId]) REFERENCES PersonTable([Id])
)

CREATE INDEX IX_Post_IsDeleted ON PostTable(IsDeleted);

CREATE INDEX IX_Post_AuthorId ON PostTable(AuthorId);

CREATE TABLE TagTable (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE PostTagTable (
    [PostID] UNIQUEIDENTIFIER,
    [TagID] UNIQUEIDENTIFIER,
    FOREIGN KEY (PostID) REFERENCES PostTable(ID),
    FOREIGN KEY (TagID) REFERENCES TagTable(ID),
    PRIMARY KEY (PostID, TagID)
);

CREATE UNIQUE INDEX IX_PostTag_TagID ON PostTagTable(TagID);

GO
------------------------------------------------
-- Inserts
------------------------------------------------
INSERT INTO [TagTable] ([Id], [Name]) 
VALUES 
    (NEWID(), 'C'),
    (NEWID(), 'C#'),
    (NEWID(), 'Transact-SQL'),
    (NEWID(), 'Java'),
    (NEWID(), 'JavaScript'),
    (NEWID(), 'Python'),
    (NEWID(), 'PHP'),
    (NEWID(), 'Ruby'),
    (NEWID(), 'Go'),
    (NEWID(), 'Swift'),
    (NEWID(), 'Kotlin'),
    (NEWID(), 'HTML'),
    (NEWID(), 'CSS'),
    (NEWID(), 'TypeScript'),
    (NEWID(), 'VB.NET'),
    (NEWID(), 'PowerShell'),
    (NEWID(), 'Blazor'),
    (NEWID(), 'ASP.NET Core'),
    (NEWID(), 'Entity Framework'),
    (NEWID(), 'React'),
    (NEWID(), 'Vue.js'),
    (NEWID(), 'XAML'),
    (NEWID(), 'WPF'),
    (NEWID(), 'WinForms'),
    (NEWID(), 'Microsoft SQL'),
    (NEWID(), 'Visual Studio'),
    (NEWID(), 'Visual Studio Code'),
    (NEWID(), 'ASP.NET MVC'),
    (NEWID(), 'Razor Pages'),
    (NEWID(), 'NoSQL'),
    (NEWID(), 'MongoDB'),
    (NEWID(), 'Node.js'),
    (NEWID(), 'MAUI');
GO
------------------------------------------------
-- Views
------------------------------------------------

CREATE OR ALTER VIEW View_PostsWithAuthors
AS
	SELECT 
	    p.Id AS PostId, 
	    p.Title, 
	    p.Content, 
	    p.Reference, 
	    p.Type, 
	    p.CreateDate, 
	    p.LastUpdated, 
	    p.IsDeleted,
	    pers.Id AS AuthorId, 
		pers.Email
	FROM PostTable p
	JOIN PersonTable pers ON p.AuthorId = pers.Id;
GO

CREATE OR ALTER VIEW View_ShowAllPosts
AS
	SELECT * FROM PostTable
GO

CREATE OR ALTER VIEW View_PostsWithTags
AS
	SELECT 
		p.Id AS PostId, 
	    p.Title, 
	    p.Content, 
	    t.Name AS TagName
	FROM PostTable p
	JOIN PostTagTable pt ON p.Id = pt.PostID
	JOIN TagTable t ON pt.TagID = t.Id;
GO

CREATE OR ALTER VIEW View_ActivePosts
AS
	SELECT 
	    Id, Title, AuthorId, Content, CreateDate, LastUpdated 
	FROM PostTable
	WHERE IsDeleted = 0;
GO

CREATE OR ALTER VIEW View_BasicPersonInfo
AS
	SELECT 
		Id, FirstName, LastName, Age, Email, City, PhoneNumber, LinkedIn, Github, RegistrationDate
	FROM PersonTable;
GO


------------------------------------------------
-- Stored Procedures
------------------------------------------------

	------------------------------------------------
	-- Create
	------------------------------------------------

CREATE OR ALTER PROCEDURE sp_CreateUser
(@FName NVARCHAR(255), @LName NVARCHAR(255), @Age INT, @Mail NVARCHAR(255), @PhoneNumber NVARCHAR(16), @City NVARCHAR(255), @LinkedIn NVARCHAR(MAX), @Github NVARCHAR(MAX), @Password NVARCHAR(64))
AS
BEGIN
    DECLARE @Salt VARBINARY(16) = CAST(CRYPT_GEN_RANDOM(16) AS VARBINARY(16));
    DECLARE @HashedPassword VARBINARY(64) = HASHBYTES('SHA2_256', @Salt + CAST(@Password AS VARBINARY(64)));
    DECLARE @Date DATE = CONVERT(DATE, GETUTCDATE());
    INSERT INTO [PersonTable]
    ([FirstName], [LastName], [Age], [EMail], [PhoneNumber], [City], [LinkedIn], [Github], [Password], [Salt], [RegistrationDate]) 
    VALUES 
    (@FName, @LName, @Age, @Mail, @PhoneNumber, @City, @LinkedIn, @Github, @HashedPassword, @Salt, @Date)
END
GO

CREATE OR ALTER PROCEDURE sp_CreatePost
(@Title NVARCHAR(255), @AuthorId UNIQUEIDENTIFIER, @Reference NVARCHAR(MAX), @Type BIT, @Content NVARCHAR(MAX))
AS
BEGIN
    DECLARE @PostId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO PostTable ([Id], [Title], [AuthorId], [Reference], [Type], [Content], [IsDeleted], [CreateDate])
    VALUES (@PostId, @Title, @AuthorId, @Reference, @Type, @Content, 0, GETDATE());
    
    -- Add tags
  --  IF @Tags IS NOT NULL
  --  BEGIN
  --      DECLARE @TagList TABLE (Tag NVARCHAR(50));
		--INSERT INTO @TagList (Tag)
		--SELECT value FROM STRING_SPLIT(@Tags, ',');
  --      INSERT INTO PostTagTable ([PostId], [TagId])
  --      SELECT @PostId, 
		--t.[Id] FROM TagTable t WHERE t.[Name] 
		--IN (SELECT Tag FROM @TagList);
  --  END
END
GO

CREATE OR ALTER PROCEDURE sp_CreateTag
(@TagName NVARCHAR(100))
AS
BEGIN
    INSERT INTO [TagTable] ([Id], [Name]) 
    VALUES (NEWID(), @TagName);
END
GO

	------------------------------------------------
	-- Read
	------------------------------------------------

CREATE OR ALTER PROCEDURE sp_GetPostsByDeletion
(@IsDeleted BIT = NULL)
AS
BEGIN
    IF @IsDeleted IS NULL
    BEGIN
        SELECT * FROM PostTable;
    END
    ELSE
    BEGIN
        SELECT * FROM PostTable WHERE [IsDeleted] = @IsDeleted;
    END
END
GO

CREATE OR ALTER PROCEDURE sp_GetUserById
(@PersonID UNIQUEIDENTIFIER)
AS
BEGIN
    SELECT * FROM PersonTable WHERE Id = @PersonID;
END
GO

CREATE OR ALTER PROCEDURE sp_GetPostById
(@PostID UNIQUEIDENTIFIER)
AS
BEGIN
    SELECT * FROM PostTable WHERE Id = @PostID;
END
GO

CREATE OR ALTER PROCEDURE sp_GetTagById
(@TagID UNIQUEIDENTIFIER)
AS
BEGIN
    SELECT * FROM TagTable WHERE Id = @TagID;
END
GO

CREATE OR ALTER PROCEDURE sp_GetAllTags
AS
BEGIN
	SELECT * FROM TagTable
END
GO

CREATE OR ALTER PROCEDURE sp_GetAllPosts
(@IsDeleted BIT = NULL)
AS
BEGIN
    IF @IsDeleted IS NULL
    BEGIN
        SELECT * FROM View_ShowAllPosts;
    END
    ELSE
    BEGIN
        SELECT * FROM View_ShowAllPosts WHERE [IsDeleted] = @IsDeleted;
    END
END
GO

CREATE OR ALTER PROCEDURE sp_GetUserByMail
(@Mail NVARCHAR(255))
AS
BEGIN
	SELECT * FROM PersonTable WHERE [Email] = @Mail
END
GO

CREATE OR ALTER PROCEDURE sp_GetTagsForPost
(@PostID UNIQUEIDENTIFIER)
AS
BEGIN
	SELECT t.* 
        FROM TagTable t
        JOIN PostTagTable pt ON t.Id = pt.TagID
        WHERE pt.PostID = @PostID
END
GO

CREATE OR ALTER PROCEDURE sp_GetPostByIdAndDeletion
(@PostID UNIQUEIDENTIFIER, @IsDeleted BIT = NULL)
AS
BEGIN
    IF @IsDeleted IS NULL
    BEGIN
        SELECT * FROM PostTable WHERE Id = @PostID;
    END
    ELSE
    BEGIN
        SELECT * FROM PostTable WHERE Id = @PostID AND [IsDeleted] = @IsDeleted;
    END
END
GO

CREATE OR ALTER PROCEDURE sp_GetPostsByAuthorEmail
    @Email NVARCHAR(255)
AS
BEGIN
    SELECT 
        PostId, Title, Content, Reference, Type, CreateDate, LastUpdated, IsDeleted
    FROM View_PostsWithAuthors
    WHERE Email = @Email;
END
GO

CREATE OR ALTER PROCEDURE sp_GetPostsByAuthorId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SELECT 
        Id, Title, Content, Reference, Type, CreateDate, LastUpdated, IsDeleted
    FROM PostTable
    WHERE [AuthorId] = @Id;
END
GO

CREATE OR ALTER PROCEDURE sp_GetPostsByTag
    @TagName NVARCHAR(50)
AS
BEGIN
    SELECT 
        PostId, Title, Content 
    FROM View_PostsWithTags
    WHERE TagName = @TagName;
END
GO

CREATE OR ALTER PROCEDURE sp_GetAllActivePosts
AS
BEGIN
    SELECT * FROM View_ActivePosts;
END
GO

CREATE OR ALTER PROCEDURE sp_GetPersonBasicInfo
    @PersonID UNIQUEIDENTIFIER
AS
BEGIN
    SELECT * FROM View_BasicPersonInfo WHERE Id = @PersonID;
END
GO


	------------------------------------------------
	-- Update
	------------------------------------------------

CREATE OR ALTER PROCEDURE sp_UpdatePost
(@PostID UNIQUEIDENTIFIER, @Title NVARCHAR(255), @Content NVARCHAR(MAX), @Reference NVARCHAR(MAX))
AS
BEGIN
    UPDATE [PostTable]
    SET [Title] = @Title, [Content] = @Content, [LastUpdated] = GETUTCDATE(), [Reference] = @Reference
    WHERE [Id] = @PostID;
END
GO

CREATE OR ALTER PROCEDURE sp_UpdateTag
(@TagID UNIQUEIDENTIFIER, @NewTagName NVARCHAR(100))
AS
BEGIN
    UPDATE TagTable 
	SET [Name] = @NewTagName 
	WHERE Id = @TagID;
END
GO

CREATE OR ALTER PROCEDURE sp_UpdatePerson
(@PersonId UNIQUEIDENTIFIER, @FName NVARCHAR(255), @LName NVARCHAR(255), @Age INT, @Email NVARCHAR(255), @City NVARCHAR(255), @Number NVARCHAR(16), @LinkedIn NVARCHAR(255), @Github NVARCHAR(255))
AS
BEGIN
	UPDATE PersonTable
	SET [FirstName] = @FName, [LastName] = @LName, [Age] = @Age, [Email] = @Email, [City] = @City, [PhoneNumber] = @Number, [LinkedIn] = @LinkedIn, [Github] = @Github
	WHERE
	[Id] = @PersonId
END
GO
	
CREATE OR ALTER PROCEDURE sp_UpdatePersonPassword
(@Id UNIQUEIDENTIFIER, @Password NVARCHAR(64))
AS
BEGIN
    DECLARE @Salt VARBINARY(16) = CAST(CRYPT_GEN_RANDOM(16) AS VARBINARY(16));
    DECLARE @HashedPassword VARBINARY(64) = HASHBYTES('SHA2_256', @Salt + CAST(@Password AS VARBINARY(64)));
	UPDATE PersonTable
	SET [Salt] = @Salt, [Password] = @HashedPassword
	WHERE [ID] = @Id
END
GO

	------------------------------------------------
	-- Delete
	------------------------------------------------

CREATE OR ALTER PROCEDURE sp_SoftDeletePost
(@PostId UNIQUEIDENTIFIER)
AS
BEGIN
    UPDATE PostTable 
	SET [IsDeleted] = 1 
	WHERE [Id] = @PostId;
END
GO

CREATE OR ALTER PROCEDURE sp_DeletePost
(@PostId UNIQUEIDENTIFIER)
AS
BEGIN
    DELETE FROM PostTable 
	WHERE [Id] = @PostId 
	AND [IsDeleted] = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_DeletePerson
(@PersonId UNIQUEIDENTIFIER)
AS
BEGIN
    DELETE FROM PersonTable
	WHERE [Id] = @PersonId;
END
GO

CREATE OR ALTER PROCEDURE sp_DeleteTag
(@TagID UNIQUEIDENTIFIER)
AS
BEGIN
    DELETE FROM TagTable 
	WHERE Id = @TagID;
END
GO

	------------------------------------------------
	-- Extra
	------------------------------------------------

CREATE OR ALTER PROCEDURE sp_RemoveTagFromPost
(@PostID UNIQUEIDENTIFIER, @TagID UNIQUEIDENTIFIER)
AS
BEGIN
    DELETE FROM PostTagTable WHERE PostID = @PostID AND TagID = @TagID;
END
GO

CREATE OR ALTER PROCEDURE sp_AddTagsToPost
(@PostID UNIQUEIDENTIFIER, @TagID UNIQUEIDENTIFIER)
AS
BEGIN
	INSERT INTO 
	PostTagTable (PostID, TagID) 
	VALUES 
	(@PostID, @TagID)
END
GO

CREATE OR ALTER PROCEDURE sp_UserLogOn
(@Email NVARCHAR(255), @Password NVARCHAR(64))
AS
BEGIN
    DECLARE @StoredPassword VARBINARY(64);
    DECLARE @Salt VARBINARY(16);
    
    SELECT @StoredPassword = [Password], @Salt = [Salt] 
    FROM PersonTable 
    WHERE [EMail] = @Email;
    
    IF @StoredPassword IS NULL OR @Salt IS NULL
    BEGIN
        SELECT 'Login failed' AS Result;
        RETURN;
    END

    DECLARE @HashedInputPassword VARBINARY(64) = HASHBYTES('SHA2_256', @Salt + CAST(@Password AS VARBINARY(64)));

    IF @HashedInputPassword = @StoredPassword
	BEGIN
	    SELECT 'Login successful' AS Result;
		SELECT Id, FirstName, LastName, Age, City, PhoneNumber, LinkedIn, Github
		FROM PersonTable
		WHERE Email = @Email
	END
	ELSE
	BEGIN
		SELECT 'Login failed' AS Result;
	END

END
GO

CREATE OR ALTER PROCEDURE sp_RestoreDeletedPost
(@PostId UNIQUEIDENTIFIER)
AS
BEGIN
	UPDATE PostTable
	SET [IsDeleted] = 0
	WHERE [Id] = @PostId
END
GO
