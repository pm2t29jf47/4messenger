/*==============================================================*/
/* Хп. Выводит данные всех работников                           */
/*==============================================================*/
CREATE PROCEDURE select_employees
AS
	SELECT * FROM [dbo].[Employee]
GO

/*==============================================================*/
/* Хп. Добавляет новое письмо в таблицу                         */
/*==============================================================*/
ALTER procedure insert_message
(
	@title	nvarchar(100),
	@date	datetime,
	@content nvarchar(1000),
	@senderUsername	nvarchar(50),
	@delete bit,
	@id int output	
)
as
	insert into Message values(
	@title,
	@date,
	@content,
	@senderUsername,
	@delete
	)
	select @id = Id
	from Message 
	where Title = @title 
	AND [Date] = @date
	AND SenderUsername = @senderUsername
	AND [Delete] = @delete
	AND Content = @content


/*==============================================================*/
/* Хп. Добавляет нового адресата в таблицу                      */
/*==============================================================*/
ALTER PROCEDURE insert_recipient
(
	@recipientUsername nvarchar(50),
	@messageId int,
	@delete bit
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
			AND [Delete] = @delete
	)
	BEGIN		
		INSERT INTO Recipient
		(
			RecipientUsername,
			MessageId,
			[Delete]
		) 
		VALUES
		(
			@recipientUsername,
			@messageId,
			@delete
		)
	END
END
GO

/*==============================================================*/
/* Выводит все сообщения адресата                               */
/*==============================================================*/
ALTER PROCEDURE [dbo].[select_recipient];1
	(@recipientUsername nvarchar(50))
AS
	SET NOCOUNT ON 
	SELECT *
	FROM Recipient
	WHERE RecipientUserName = @recipientUsername
GO

/*==============================================================*/
/* Выводит всех адресатов сообщения                             */
/*==============================================================*/
ALTER PROCEDURE [dbo].[select_recipient];2
	(@messageId int)	
AS
	SET NOCOUNT ON
	SELECT *
	FROM Recipient
	WHERE MessageId = @messageId
GO

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
