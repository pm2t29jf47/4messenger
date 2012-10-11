/*==============================================================*/
/* ��. ������� ������ ���� ����������                           */
/*==============================================================*/
create procedure select_employees
as
select * from [dbo].[Employee]
go

/*==============================================================*/
/* ��. ������� ��� ������                                       */
/*==============================================================*/
create procedure select_messages
as
select	Message.Message_Id,
		Message.Title,
		Message.Date,
		E1.Name as 'Recipient',
		E2.Name as 'Sender',
		Message.Content
from	Message
inner join	Employee E1 on Message.Recipient = E1.Employee_Id
inner join 	Employee E2 on Message.Sender = E2.Employee_Id
go

/*==============================================================*/
/* ��. ��������� ����� ������ � �������                         */
/*==============================================================*/
alter procedure insert_message
(
	@title	varchar(100),
	@date	datetime,
	@senderId	int,
	@content varchar(1000),
	@deleteBySender bit,
	@id int output	
)
as
	insert into Message values(
	@title,
	@date,
	@content,
	@senderId,
	0
	)
	select @id = MessageId
	from Message 
	where Title = @title 
	AND [Date] = @date
	AND SenderId = @senderId
	AND Content = @content
go  
