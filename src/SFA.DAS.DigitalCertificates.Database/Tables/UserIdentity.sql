CREATE TABLE [dbo].[UserIdentity]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [DateOfBirth] DATETIME2 NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [ValidSince] DATETIME2 NULL,
    [ValidUntil] DATETIME2 NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT FK_UserIdentity_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserIdentityHistory]));
GO

CREATE INDEX IX_UserIdentity_UserId ON [dbo].[UserIdentity]([UserId]);
GO
