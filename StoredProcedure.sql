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
ALTER PROCEDURE insert_message
(
	@title	varchar(100),
	@date	datetime,
	@senderId	int,
	@content varchar(1000),
	@deleteBySender bit,
	@id int output	
)
AS
BEGIN
	INSERT INTO Message 
	VALUES
	(
		@title,
		@date,
		@content,
		@senderId,
		0
	)
	
	--@@SCOPE_I
	
	--------------именить----------------
	SELECT @id = MessageId
	FROM Message 
	WHERE Title = @title 
	AND [Date] = @date
	AND SenderId = @senderId
	AND Content = @content
	--------------------------------------
END
GO  

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