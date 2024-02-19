INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('User1');
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('User2');
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('User3');

INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (1, GETDATE(), 'Event1', GETDATE(), 'Event 1 Description', 'Event 1 Location', 'Name 1');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (1, GETDATE(), 'Event2', GETDATE(), 'Event 2 Description', 'Event 2 Location', 'Name 2');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (1, GETDATE(), 'Event3', GETDATE(), 'Event 3 Description', 'Event 3 Location', 'Name 3');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (1, GETDATE(), 'Event4', GETDATE(), 'Event 4 Description', 'Event 4 Location', 'Name 4');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (1, GETDATE(), 'Event5', GETDATE(), 'Event 5 Description', 'Event 5 Location', 'Name 5');

INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (2, GETDATE(), 'Event1', GETDATE(), 'Event 1 Description', 'Event 1 Location', 'Name 1');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (2, GETDATE(), 'Event2', GETDATE(), 'Event 2 Description', 'Event 2 Location', 'Name 2');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (2, GETDATE(), 'Event3', GETDATE(), 'Event 3 Description', 'Event 3 Location', 'Name 3');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (2, GETDATE(), 'Event4', GETDATE(), 'Event 4 Description', 'Event 4 Location', 'Name 4');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (2, GETDATE(), 'Event5', GETDATE(), 'Event 5 Description', 'Event 5 Location', 'Name 5');

INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (3, GETDATE(), 'Event1', GETDATE(), 'Event 1 Description', 'Event 1 Location', 'Name 1');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (3, GETDATE(), 'Event2', GETDATE(), 'Event 2 Description', 'Event 2 Location', 'Name 2');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (3, GETDATE(), 'Event3', GETDATE(), 'Event 3 Description', 'Event 3 Location', 'Name 3');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (3, GETDATE(), 'Event4', GETDATE(), 'Event 4 Description', 'Event 4 Location', 'Name 4');
INSERT INTO [EventHistory] ([UserID], [ViewedDate], [EventID], [EventDate], [EventDescription], [EventLocation], [EventName]) VALUES (3, GETDATE(), 'Event5', GETDATE(), 'Event 5 Description', 'Event 5 Location', 'Name 5');
