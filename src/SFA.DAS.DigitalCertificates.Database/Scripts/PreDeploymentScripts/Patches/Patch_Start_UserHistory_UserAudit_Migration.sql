SET XACT_ABORT ON;
GO

IF OBJECT_ID(N'[dbo].[PatchLog]', N'U') IS NOT NULL
BEGIN
    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[PatchLog]
        WHERE [PatchName] = 'Patch_Start_UserHistory_UserAudit_Migration'
    )
    BEGIN
        BEGIN TRY
            BEGIN TRANSACTION;

            /*
                Capture every version before changing either table.

                IsCurrent = 0: historical temporal version
                IsCurrent = 1: current version
            */
            SELECT
                [Id],
                [GovUkIdentifier],
                [EmailAddress],
                [PhoneNumber],
                [LastLoginAt],
                [IsLocked],
                [CreatedAt],
                [ValidFrom],
                [ValidTo],
                CAST(0 AS BIT) AS [IsCurrent]
            INTO #UserVersions
            FROM [dbo].[UserHistory]

            UNION ALL

            SELECT
                [Id],
                [GovUkIdentifier],
                [EmailAddress],
                [PhoneNumber],
                [LastLoginAt],
                [IsLocked],
                [CreatedAt],
                [ValidFrom],
                [ValidTo],
                CAST(1 AS BIT) AS [IsCurrent]
            FROM [dbo].[User];

            SELECT
                *,
                ROW_NUMBER() OVER
                (
                    PARTITION BY [Id]
                    ORDER BY [ValidFrom], [ValidTo], [IsCurrent]
                ) AS [VersionNumber],
                COUNT(*) OVER
                (
                    PARTITION BY [Id]
                ) AS [VersionCount],
                MAX(CONVERT(TINYINT, [IsCurrent])) OVER
                (
                    PARTITION BY [Id]
                ) AS [HasCurrentVersion]
            INTO #OrderedUserVersions
            FROM #UserVersions;

            -- Convert User back to an ordinary non-temporal table.
            ALTER TABLE [dbo].[User]
                DROP PERIOD FOR SYSTEM_TIME;

            ALTER TABLE [dbo].[User]
                DROP COLUMN [ValidFrom], [ValidTo];

            CREATE TABLE [dbo].[UserAuditMigration]
            (
                [AuditId] BIGINT IDENTITY(1, 1) NOT NULL CONSTRAINT [PK_UserAuditMigration] PRIMARY KEY,
                [Id] UNIQUEIDENTIFIER NULL,
                [GovUkIdentifier] VARCHAR(100) NULL,
                [EmailAddress] VARCHAR(100) NULL,
                [PhoneNumber] VARCHAR(20) NULL,
                [LastLoginAt] DATETIME2 NULL,
                [IsLocked] BIT NULL,
                [CreatedAt] DATETIME2 NULL,
                [Action] VARCHAR(10) NOT NULL,
                [ActionedAt] DATETIME2 NOT NULL,
                CONSTRAINT [CK_UserAuditMigration_Action] CHECK ([Action] IN ('Created', 'Updated', 'Deleted'))
            );

            /*
                The earliest version is the state when the user was created.
                Every subsequent version contains the values after an update.

                This also creates the requested Created history entry for rows
                that have only ever existed in the current User table.
            */
            INSERT INTO [dbo].[UserAuditMigration]
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
                CASE
                    WHEN [VersionNumber] = 1 THEN 'Created'
                    ELSE 'Updated'
                END,
                CASE
                    WHEN [VersionNumber] = 1 THEN [CreatedAt]
                    ELSE [ValidFrom]
                END
            FROM #OrderedUserVersions;

            /*
                No current version means that the user was deleted.
                The final temporal ValidTo value is the deletion time.
            */
            INSERT INTO [dbo].[UserAuditMigration]
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
                'Deleted',
                [ValidTo]
            FROM #OrderedUserVersions
            WHERE [VersionNumber] = [VersionCount]
              AND [HasCurrentVersion] = 0;

            INSERT INTO [dbo].[PatchLog] ([PatchName])
                VALUES ('Patch_Start_UserHistory_UserAudit_Migration');

            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;

            THROW;
        END CATCH;
    END;
END;
GO