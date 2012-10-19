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
	@employeeId int,
	@messageId int,
	@deleteByRecipient bit
)
AS
BEGIN
	IF NOT EXISTS
	(
		SELECT * 
		FROM Recipient
		WHERE 
			EmployeeId = @employeeId
			AND MessageId = @messageId
			AND DeleteByRecipient = @deleteByRecipient
	)
	BEGIN		
		INSERT INTO Recipient
		(
			EmployeeId,
			MessageId,
			DeleteByRecipient
		) 
		VALUES
		(
			@employeeId,
			@messageId,
			@deleteByRecipient
		)
	END
END
GO