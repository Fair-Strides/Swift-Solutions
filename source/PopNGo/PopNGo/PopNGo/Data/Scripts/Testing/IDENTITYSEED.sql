INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES
    ('a3c1a4a4-4089-40cd-89a2-d4627e0afd71', 'Admin', 'ADMIN', NULL),
    ('b18506f4-5f71-4099-b5ca-3165c3a7eff6', 'User', 'USER', NULL);

INSERT INTO [AspNetUsers] (
    [Id]
    ,[UserName]
    ,[NormalizedUserName]
    ,[Email]
    ,[NormalizedEmail]
    ,[EmailConfirmed]
    ,[PasswordHash]
    ,[SecurityStamp]
    ,[ConcurrencyStamp]
    ,[PhoneNumber]
    ,[PhoneNumberConfirmed]
    ,[TwoFactorEnabled]
    ,[LockoutEnd]
    ,[LockoutEnabled]
    ,[AccessFailedCount]
    ,[FirstName]
    ,[LastName]
    ,[NotificationEmail]
    ,[NotifyWeekBefore]
    ,[NotifyDayBefore]
    ,[NotifyDayOf]
    ) VALUES
    ('2ce786bf-706b-4ba4-87b3-b3f0cbc6a1ad', 'Eric Evangelista', 'ERIC EVANGELISTA', 'eevangelista19@mail.wou.edu', 'EEVANGELISTA19@MAIL.WOU.EDU', 1, 'AQAAAAIAAYagAAAAEBf/sdMDkKjhjFP+T9VJRv8egQv9AkN3kIrO1tqncicuuEgVVxqamgVTfR6gMeoRkQ==', 'WLDXOK76T3XAJ3UQF7S75AH4QP4YYG5Z', '51a66d77-2aaf-4435-9279-7959dff2aa60', NULL, 0, 0, NULL, 1, 0, 'Eric', 'Evangelista', 'eevangelista19@mail.wou.edu', 0, 0, 0),
    ('6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5', 'Tristan Goucher', 'TRISTAN GOUCHER', 'tgoucher22@mail.wou.edu', 'TGOUCHER22@MAIL.WOU.EDU', 1, 'AQAAAAIAAYagAAAAEOSlAU8TUjRYEns49hhN4S8QxQKTjTePd9TNWr9nF1oRRzDVOHJZbUV6PvPwUK1q/Q==', 'H7RYDK32CUCVGEVJQBRYHZNY3EHP6YAG', '8db02396-139d-42c5-a39f-cdc10a7fbe4a', NULL, 0, 0, NULL, 1, 0, 'Tristan', 'Goucher', 'tgoucher22@mail.wou.edu', 0, 0, 0),
    ('bb60f066-da0e-46b5-83d0-4d73d306dc64', 'Cameron Mance', 'CAMERON MANCE', 'cmance19@mail.wou.edu', 'CMANCE19@MAIL.WOU.EDU', 1, 'AQAAAAIAAYagAAAAEOhoEiAY75rcV+fUw8U0cp2KS8vmO8K5Hy5UcLNuSHzvMvQsnrEigbVEnwIwlh9HQg==', 'T2YT3PDOWDLVLHYD5SN22JQQW7FHVM45', 'fe6120c7-8345-4733-878a-efade5dbb7af', NULL, 0, 0, NULL, 1, 0, 'Cameron', 'Mance', 'cmance19@mail.wou.edu', 0, 0, 0),
    ('c4081410-2b09-4992-803a-ee550c92c76a', 'Joshua Weiss', 'JOSHUA WEISS', 'jweiss19@mail.wou.edu', 'JWEISS19@MAIL.WOU.EDU', 1, 'AQAAAAIAAYagAAAAECVmq38aabWsMcu8NClfEOeMOpTMlVycSqoK60I6E3yVeent/0v4vYNk16yr1br0Cw==', 'CCTV4BRO6U54VGPUKA2SYXADEG27TWIY', '44236465-ad80-4fe4-be55-5ea74280ed16', NULL, 0, 0, NULL, 1, 0, 'Joshua', 'Weiss', 'jweiss19@mail.wou.edu', 0, 0, 0);

INSERT INTO AspNetUserRoles ([UserId], [RoleId]) VALUES
    ('2ce786bf-706b-4ba4-87b3-b3f0cbc6a1ad', 'b18506f4-5f71-4099-b5ca-3165c3a7eff6'),
    ('6b6c5d33-e6d7-4d18-a565-a657eaf7a8a5', 'b18506f4-5f71-4099-b5ca-3165c3a7eff6'),
    ('bb60f066-da0e-46b5-83d0-4d73d306dc64', 'b18506f4-5f71-4099-b5ca-3165c3a7eff6'),
    ('c4081410-2b09-4992-803a-ee550c92c76a', 'b18506f4-5f71-4099-b5ca-3165c3a7eff6');