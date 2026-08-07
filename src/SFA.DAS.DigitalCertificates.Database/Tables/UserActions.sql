CREATE TABLE [dbo].[UserActions]
(
    [Id] BIGINT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ActionTypeId] INT NOT NULL,
    [ActionCode] VARCHAR(50) NULL,
    [ActionTime] DATETIME2 NOT NULL,
    [FamilyName] VARCHAR(255) NOT NULL,
    [GivenNames] VARCHAR(255) NOT NULL,
    [CertificateId] UNIQUEIDENTIFIER NULL,
    [CertificateType] VARCHAR(20) NULL,
    [CourseName] VARCHAR(1000) NULL,
    CONSTRAINT [FK_UserActions_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]),
    CONSTRAINT [FK_UserActions_ActionType] FOREIGN KEY ([ActionTypeId]) REFERENCES [dbo].[ActionType]([Id])
)
GO

CREATE INDEX IX_UserActions_UserId ON [dbo].[UserActions]([UserId]) INCLUDE ([Id],[ActionTypeId],[ActionCode]);
GO

CREATE UNIQUE INDEX UX_UserActions_ActionCode ON [dbo].[UserActions]([ActionCode]);
GO
