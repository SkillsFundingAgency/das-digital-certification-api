CREATE TABLE [dbo].[SharingAccess]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [SharingId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT FK_SharingAccess_Sharing FOREIGN KEY ([SharingId]) REFERENCES [dbo].[Sharing]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingAccessHistory]));
GO

CREATE INDEX IX_SharingAccess_SharingId ON [dbo].[SharingAccess]([SharingId]);
GO
