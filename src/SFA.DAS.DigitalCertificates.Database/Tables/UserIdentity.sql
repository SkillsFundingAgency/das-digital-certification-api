CREATE TABLE [dbo].[UserIdentity]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATE NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [ValidSince] DATETIME2 NULL,
    [ValidUntil] DATETIME2 NULL,
    CONSTRAINT FK_UserIdentity_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
);
GO

CREATE INDEX IX_UserIdentity_UserId ON [dbo].[UserIdentity]([UserId]);
GO
