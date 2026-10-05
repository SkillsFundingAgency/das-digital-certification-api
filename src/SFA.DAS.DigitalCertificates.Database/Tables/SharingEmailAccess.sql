CREATE TABLE [dbo].[SharingEmailAccess]
(
    [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT [DF_SharingEmailAccess_Id] DEFAULT NEWID() PRIMARY KEY,
    [SharingEmailId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL CONSTRAINT [DF_SharingEmailAccess_AccessedAt] DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_SharingEmailAccess_SharingEmail FOREIGN KEY ([SharingEmailId]) REFERENCES [dbo].[SharingEmail]([Id])
)
GO

CREATE INDEX IX_SharingEmailAccess_SharingEmailId ON [dbo].[SharingEmailAccess]([SharingEmailId]);
GO
