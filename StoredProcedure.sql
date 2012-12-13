/*==============================================================*/
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

/***************************************************************************************************************/

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
	@id int output	
)
as
	insert into Message values(
	@title,
	@date,
	@content,
	@senderUsername,
	@deleted
	)
	select @id = Id
	from Message 
	where Title = @title 
	AND [Date] = @date
	AND SenderUsername = @senderUsername
	AND Deleted = @deleted
	AND Content = @content
GO

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

/***************************************************************************************************************/

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

/***************************************************************************************************************/

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
	