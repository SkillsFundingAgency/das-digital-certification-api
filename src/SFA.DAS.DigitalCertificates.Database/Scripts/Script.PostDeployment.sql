/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\PostDeploymentScripts\LookupData\SynchronizeLookupData.sql
:r .\PostDeploymentScripts\Patches\Patch_Complete_UserHistory_UserAudit_Migration.sql
:r .\PostDeploymentScripts\Patches\Patch_Drop_SystemVersioned_History_Tables.sql
