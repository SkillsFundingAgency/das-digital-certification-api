CREATE TABLE [dbo].[SharingEmailAccessHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [SharingEmailId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)