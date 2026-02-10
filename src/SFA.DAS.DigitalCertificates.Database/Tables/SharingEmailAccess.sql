CREATE TABLE [dbo].[SharingEmailAccess]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [SharingEmailId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT FK_SharingEmailAccess_SharingEmail FOREIGN KEY ([SharingEmailId]) REFERENCES [dbo].[SharingEmail]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingEmailAccessHistory]));
GO

CREATE INDEX IX_SharingEmailAccess_SharingEmailId ON [dbo].[SharingEmailAccess]([SharingEmailId]);
GO
