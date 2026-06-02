CREATE TABLE [dbo].[UserActions]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ActionType] INT NOT NULL,
    [ActionCode] VARCHAR(50) NULL,
    [ActionTime] DATETIME2 NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [CertificateId] UNIQUEIDENTIFIER NULL,
    [CertificateType] VARCHAR(20) NULL,
    [CourseName] VARCHAR(1000) NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT [FK_UserActions_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
)

WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserActionsHistory]));
GO

CREATE INDEX IX_UserActions_UserId ON [dbo].[UserActions]([UserId]);
GO

CREATE UNIQUE INDEX UX_UserActions_ActionCode ON [dbo].[UserActions]([ActionCode]);
GO
