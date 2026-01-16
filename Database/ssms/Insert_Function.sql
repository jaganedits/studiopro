TRUNCATE TABLE [Function];
GO

SET IDENTITY_INSERT [Function] ON;
GO

INSERT INTO [Function]
(
 functionid, functionname, functionurl, menuicon, rellink,
 parentid, isexternal, screenorder, isapprovalneeded,
 status, createdby, createdon, modifiedby, modifiedon, mobilescreen
)
VALUES
/* =====================
   MAIN MODULES
===================== */
(1, N'Main',   N'~/Dashboard',   'pi pi-home',      'dashboard',  0, 1, 1, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(2, N'Master',      N'~/Master',      'pi pi-cog',       'master',     0, 1, 2, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(3, N'Admin',       N'~/Users',       'pi pi-user',      'users',      0, 1, 3, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(4, N'Billing', N'~/AssetTools',  'pi pi-briefcase', 'assettools', 0, 1, 4, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(5, N'Reports',     N'~/Maintenance', 'pi pi-compass',   'reports',    0, 1, 5, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(6, N'Records',    N'~/Records',    'pi pi-file',      'records',   0, 1, 6, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(7, N'Approval',    N'~/Approval',    'pi pi-file',      'approval',   0, 1, 7, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),


/* =====================
   USERS SCREENS
===================== */
(8, N'Users',            N'/home/vUsers',                 'pi pi-users',   '', 3, 1, 1, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(9, N'Role',             N'/home/cRoles',                  'pi pi-id-card', '', 3, 1, 2, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(10, N'Rolewise Mapping', N'/home/cRolewiseScreenMapping', 'pi pi-sitemap', '', 3, 1, 4, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(11, N'Change Password',  N'/home/cChangePassword',        'pi pi-lock',    '', 3, 1, 5, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL),
(12, N'Branch',  N'/home/cBranch',							'pi pi-lock',    '', 3, 1, 3, 0, 1, 1, GETDATE(), 1, GETDATE(), NULL)


GO
SET IDENTITY_INSERT [Function] OFF;
GO
