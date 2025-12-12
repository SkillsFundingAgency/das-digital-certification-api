/* 
   All system versioning tables must be explictly created and modified as the DACPAC does not correctly automatically sychronize added columns.
   
   The system versioning is turned off then the DACPAC is allowed to add columns and then the system versioning is turned back on.

   Ensure that any added columns have an explicitly named constraint and that the same constraint is added to both the main table and the system versioning table.
*/
ALTER TABLE [dbo].[User]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserHistory])
);

ALTER TABLE [dbo].[UserAuthorisation]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[UserAuthorisationHistory])
);

ALTER TABLE [dbo].[Sharing]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingHistory])
);

ALTER TABLE [dbo].[SharingAccess]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingAccessHistory])
);

ALTER TABLE [dbo].[SharingEmail]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingEmailHistory])
);

ALTER TABLE [dbo].[SharingEmailAccess]
SET (
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[SharingEmailAccessHistory])
);
