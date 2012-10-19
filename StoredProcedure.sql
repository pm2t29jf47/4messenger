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