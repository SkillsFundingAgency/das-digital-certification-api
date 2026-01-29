CREATE TABLE [dbo].[SharingEmail]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [SharingId] UNIQUEIDENTIFIER NOT NULL,
    [EmailAddress] VARCHAR(254) NOT NULL,
    [EmailLinkCode] UNIQUEIDENTIFIER NOT NULL,
    [SentTime] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) GENERATED ALWAYS AS ROW START,
    [ValidTo] DATETIME2 (0) GENERATED ALWAYS AS ROW END,
    PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo),
    CONSTRAINT FK_SharingEmail_Sharing FOREIGN KEY ([SharingId]) REFERENCES [dbo].[Sharing]([Id])
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingEmailHistory]));
GO

CREATE INDEX IX_SharingEmail_SharingId ON [dbo].[SharingEmail]([SharingId]);
GO

CREATE UNIQUE INDEX UX_SharingEmail_EmailLinkCode ON [dbo].[SharingEmail]([EmailLinkCode]);
GO
