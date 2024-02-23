-- Seed data for PG_User table
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('user1');
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('user2');
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('user3');

-- Seed data for Event table
INSERT INTO [Event] ([EventID], [EventDate], [EventName], [EventDescription], [EventLocation]) VALUES ('event1', GETDATE(), 'Event 1', 'Event 1 Description', 'Location 1');
INSERT INTO [Event] ([EventID], [EventDate], [EventName], [EventDescription], [EventLocation]) VALUES ('event2', GETDATE(), 'Event 2', 'Event 2 Description', 'Location 2');
INSERT INTO [Event] ([EventID], [EventDate], [EventName], [EventDescription], [EventLocation]) VALUES ('event3', GETDATE(), 'Event 3', 'Event 3 Description', 'Location 3');

-- Seed data for EventHistory table
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (1, 1, GETDATE());
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (2, 2, GETDATE());
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (3, 3, GETDATE());
