﻿/*==============================================================*/
/* Хп. Выводит данные всех работников                           */
/*==============================================================*/
CREATE PROCEDURE select_employee;1
AS
	SELECT *
	FROM [dbo].[Employee]
GO

/*==============================================================*/
/* Хп. Выводит данные работника по ключу                        */
/*==============================================================*/
CREATE PROCEDURE select_employee;2
(@username nvarchar (50))
AS
	SELECT * 
	FROM [dbo].[Employee]
	WHERE Employee.Username = @username
	
GO

/*==============================================================*/
/* Хп. Выводит Username всех работников                         */
/*==============================================================*/
CREATE PROCEDURE select_employee;3
AS
	SELECT Username
	FROM Employee
GO

/***************************************************************************************************************/

/*==============================================================*/
/* Хп. Добавляет нового адресата в таблицу                      */
/*==============================================================*/
CREATE PROCEDURE insert_recipient
(
	@recipientUsername nvarchar(50),
	@messageId int,
	@deleted bit,
	@viewed bit
)
AS
BEGIN
	IF NOT EXISTS
	(
		SELECT * 
		FROM Recipient
		WHERE 
			RecipientUsername = @recipientUsername
			AND MessageId = @messageId
	)
	BEGIN		
		INSERT INTO Recipient
		(
			RecipientUsername,
			MessageId,
			Deleted,
			Viewed
		) 
		VALUES
		(
			@recipientUsername,
			@messageId,
			@deleted,
			@viewed
		)
	END
END
GO

/*==============================================================*/
/* Выводит все сообщения адресата                               */
/*==============================================================*/
CREATE PROCEDURE [dbo].[select_recipient];1
	(@recipientUsername nvarchar(50),
	@deleted bit,
	@viewed bit)
AS
	SET NOCOUNT ON 
	SELECT *
	FROM Recipient
	WHERE RecipientUserName = @recipientUsername
	AND Deleted = @deleted
	AND Viewed = @viewed 
GO

/*==============================================================*/
/* Выводит всех адресатов сообщения                             */
/*==============================================================*/
CREATE PROCEDURE [dbo].[select_recipient];2
	(@messageId int)	
AS
	SET NOCOUNT ON
	SELECT *
	FROM Recipient
	WHERE MessageId = @messageId
GO

/*==============================================================*/
/* Обновляет поле viewed                                        */
/*==============================================================*/
CREATE PROCEDURE update_recipient;1
(@recipientUsername nvarchar(50),
@messageId int,
@viewed bit)
AS
	UPDATE Recipient
	SET Viewed = @viewed
	WHERE 	RecipientUsername = @recipientUsername
	AND MessageId = @messageId
GO

/*==============================================================*/
/* Обновляет поле deleted                                       */
/*==============================================================*/
CREATE PROCEDURE update_recipient;2
(@recipientUsername nvarchar(50),
@messageId int,
@deleted bit)
AS
	UPDATE Recipient
	SET Deleted = @deleted
	WHERE 	RecipientUsername = @recipientUsername
	AND MessageId = @messageId
GO

/*==============================================================*/
/* Полностью удаляет получателя                                 */
/*==============================================================*/
CREATE PROCEDURE delete_recipient;1
(@recipientUsername nvarchar(50),
@messageId int)
AS
	DELETE Recipient
	WHERE RecipientUsername = @recipientUsername
	AND MessageId = @messageId
GO

/***************************************************************************************************************/

/*==============================================================*/
/* Выводит сообщение по его идентификатору                      */
/*==============================================================*/
CREATE PROCEDURE select_message;1
(@Id int)
AS
	SELECT *
	FROM Message
	WHERE Id = @Id
GO

/*==============================================================*/
/* Выводит сообщения по отправителю                             */
/*==============================================================*/
CREATE PROCEDURE select_message;2
(@senderUsername nvarchar(50),
@deleted bit)
AS
	SELECT *
	FROM Message
	WHERE SenderUsername = @senderUsername
	AND Deleted = @deleted
GO

/*==============================================================*/
/* Выводит Id сообщений по отправителю                          */
/*==============================================================*/
CREATE PROCEDURE select_message;3
(@senderUsername nvarchar(50),
@deleted bit)
AS
	SELECT Id
	FROM Message
	WHERE SenderUsername = @senderUsername
	AND Deleted = @deleted
GO

/*==============================================================*/
/* Обновляет LastUpdate письма                                  */
/*==============================================================*/
CREATE PROCEDURE update_message;1
(@id int,
@lastUpdate datetime)
AS
	UPDATE Message
	SET LastUpdate = @lastUpdate
	WHERE Id = @id
GO

/*==============================================================*/
/* Обновляет Deleted письма                                     */
/*==============================================================*/
CREATE PROCEDURE update_message;2
(@id int,
@deleted bit)
AS
	UPDATE Message
	SET Deleted = @deleted
	WHERE Id = @id
GO

/*==============================================================*/
/* Хп. Добавляет новое письмо в таблицу                         */
/*==============================================================*/
CREATE procedure insert_message
(
	@title	nvarchar(100),
	@date	datetime,
	@content nvarchar(1000),
	@senderUsername	nvarchar(50),
	@deleted bit,
	@lastUpdate datetime,
	@id int output	
)
as
	insert into Message(
	Title,
	[Date],
	Content,
	SenderUsername,
	Deleted,
	LastUpdate
	)
	values(
	@title,
	@date,
	@content,
	@senderUsername,
	@deleted,
	@lastUpdate
	)
	select @id = Id
	from Message 
	where Title = @title 
	AND [Date] = @date
	AND SenderUsername = @senderUsername
	AND Deleted = @deleted
	AND Content = @content
	AND LastUpdate = @lastUpdate
GO
  
/*==============================================================*/
/* Полностью удаляет письмо                                     */
/*==============================================================*/
CREATE PROCEDURE delete_message;1
(@id int)
AS
	DELETE Message
	WHERE Id = @id
GO


