CREATE TRIGGER [dbo].[TR_User_Audit]
ON [dbo].[User]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Created rows, storing initial values
    INSERT INTO [dbo].[UserAudit]
    (
        [Id],
        [GovUkIdentifier],
        [EmailAddress],
        [PhoneNumber],
        [LastLoginAt],
        [IsLocked],
        [CreatedAt],
        [Action],
        [ActionedAt]
    )
    SELECT
        i.[Id],
        i.[GovUkIdentifier],
        i.[EmailAddress],
        i.[PhoneNumber],
        i.[LastLoginAt],
        i.[IsLocked],
        i.[CreatedAt],
        'Created',
        SYSUTCDATETIME()
    FROM inserted i
    LEFT JOIN deleted d ON d.[Id] = i.[Id]
    WHERE d.[Id] IS NULL;

    -- Updated rows, storing new values
    INSERT INTO [dbo].[UserAudit]
    (
        [Id],
        [GovUkIdentifier],
        [EmailAddress],
        [PhoneNumber],
        [LastLoginAt],
        [IsLocked],
        [CreatedAt],
        [Action],
        [ActionedAt]
    )
    SELECT
        i.[Id],
        i.[GovUkIdentifier],
        i.[EmailAddress],
        i.[PhoneNumber],
        i.[LastLoginAt],
        i.[IsLocked],
        i.[CreatedAt],
        'Updated',
        SYSUTCDATETIME()
    FROM inserted i
    INNER JOIN deleted d ON d.[Id] = i.[Id];

    -- Deleted rows, storing final values
    INSERT INTO [dbo].[UserAudit]
    (
        [Id],
        [GovUkIdentifier],
        [EmailAddress],
        [PhoneNumber],
        [LastLoginAt],
        [IsLocked],
        [CreatedAt],
        [Action],
        [ActionedAt]
    )
    SELECT
        d.[Id],
        d.[GovUkIdentifier],
        d.[EmailAddress],
        d.[PhoneNumber],
        d.[LastLoginAt],
        d.[IsLocked],
        d.[CreatedAt],
        'Deleted',
        SYSUTCDATETIME()
    FROM deleted d
    LEFT JOIN inserted i ON i.[Id] = d.[Id]
    WHERE i.[Id] IS NULL;
END;
GO