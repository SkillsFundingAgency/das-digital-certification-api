CREATE TABLE [dbo].[UserAudit]
(
    [AuditId] BIGINT IDENTITY(1, 1) NOT NULL CONSTRAINT [PK_UserAudit] PRIMARY KEY,
    [Id] UNIQUEIDENTIFIER NULL,
    [GovUkIdentifier] VARCHAR(100) NULL,
    [EmailAddress] VARCHAR(100) NULL,
    [PhoneNumber] VARCHAR(20) NULL,
    [LastLoginAt] DATETIME2 NULL,
    [IsLocked] BIT NULL,
    [CreatedAt] DATETIME2 NULL,
    [Action] VARCHAR(10) NOT NULL,
    [ActionedAt] DATETIME2 NOT NULL,
    CONSTRAINT [CK_UserAudit_Action] CHECK ([Action] IN ('Created', 'Updated', 'Deleted'))
)
GO

CREATE INDEX [IX_UserAudit_Id_ActionedAt] ON [dbo].[UserAudit] ([Id], [ActionedAt]);
GO
