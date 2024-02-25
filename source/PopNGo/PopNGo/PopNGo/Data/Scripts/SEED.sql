USE [PopNGoDB];

-- Insert into PG_User
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('user1');
INSERT INTO [PG_User] ([ASPNETUserID]) VALUES ('user2');

-- Get the IDs of the users we just inserted
DECLARE @user1ID int = (SELECT [ID] FROM [PG_User] WHERE [ASPNETUserID] = 'user1');
DECLARE @user2ID int = (SELECT [ID] FROM [PG_User] WHERE [ASPNETUserID] = 'user2');

-- Insert into Event
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation]) 
VALUES ('event1', '2022-01-01T00:00:00', 'Event 1', 'Description 1', 'Location 1');
INSERT INTO [Event] ([ApiEventID], [EventDate], [EventName], [EventDescription], [EventLocation]) 
VALUES ('event2', '2022-01-02T00:00:00', 'Event 2', 'Description 2', 'Location 2');

-- Get the IDs of the events we just inserted
DECLARE @event1ID int = (SELECT [ID] FROM [Event] WHERE [ApiEventID] = 'event1');
DECLARE @event2ID int = (SELECT [ID] FROM [Event] WHERE [ApiEventID] = 'event2');

-- Insert into FavoriteEvents
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (@user1ID, @event1ID);
INSERT INTO [FavoriteEvents] ([UserID], [EventID]) VALUES (@user2ID, @event2ID);

-- Insert into EventHistory
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (@user1ID, @event1ID, '2022-01-01T00:00:00');
INSERT INTO [EventHistory] ([UserID], [EventID], [ViewedDate]) VALUES (@user2ID, @event2ID, '2022-01-02T00:00:00');