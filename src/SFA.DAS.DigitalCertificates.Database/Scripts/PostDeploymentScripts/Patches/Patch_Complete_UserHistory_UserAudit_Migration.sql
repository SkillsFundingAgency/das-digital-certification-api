SET XACT_ABORT ON;
GO

IF EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Patch_Start_UserHistory_UserAudit_Migration'
)
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Patch_Complete_UserHistory_UserAudit_Migration'
)
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

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
            [Id],
            [GovUkIdentifier],
            [EmailAddress],
            [PhoneNumber],
            [LastLoginAt],
            [IsLocked],
            [CreatedAt],
            [Action],
            [ActionedAt]
        FROM [dbo].[UserAuditMigration];

        ------------------------------------------------------------
        -- Mark patch as applied
        ------------------------------------------------------------
        
        INSERT INTO [dbo].[PatchLog] ([PatchName])
        VALUES ('Patch_Complete_UserHistory_UserAudit_Migration');
        
        DROP TABLE [dbo].[UserAuditMigration];

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO