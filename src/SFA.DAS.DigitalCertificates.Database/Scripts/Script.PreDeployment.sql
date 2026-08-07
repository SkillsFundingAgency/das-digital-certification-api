/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r .\PreDeploymentScripts\DisableSystemVersioningTables.sql
:r .\PreDeploymentScripts\Patches\Delete_Duplicate_Reprint_ActionType.sql

-- temporarily run the action type lookup script before updating the database so that a foreign key can be added initially
-- this can be removed after all environments have been updated with the correct ActionType entries
:r .\PostDeploymentScripts\LookupData\ActionType.sql