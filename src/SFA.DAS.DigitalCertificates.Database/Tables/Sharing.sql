CREATE TABLE [dbo].[Sharing]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [CertificateId] UNIQUEIDENTIFIER NOT NULL,
    [CertificateType] VARCHAR(20) NOT NULL,
    [CourseName] VARCHAR(MAX) NOT NULL,
    [LinkCode] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [ExpiryTime] DATETIME2 NOT NULL,
    [Status] VARCHAR(20) NOT NULL DEFAULT 'Live',
    CONSTRAINT FK_Sharing_User FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
);
GO

CREATE INDEX IX_Sharing_UserId ON [dbo].[Sharing]([UserId]);
GO

CREATE UNIQUE INDEX UX_Sharing_LinkCode ON [dbo].[Sharing]([LinkCode]);
GO
