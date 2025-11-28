CREATE TABLE [dbo].[SharingAccess]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [SharingId] UNIQUEIDENTIFIER NOT NULL,
    [AccessedAt] DATETIME2 NOT NULL,
    CONSTRAINT FK_SharingAccess_Sharing FOREIGN KEY ([SharingId]) REFERENCES [dbo].[Sharing]([Id])
);
GO

CREATE INDEX IX_SharingAccess_SharingId ON [dbo].[SharingAccess]([SharingId]);
GO
