CREATE TABLE [dbo].[SharingAccessHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [SharingId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)