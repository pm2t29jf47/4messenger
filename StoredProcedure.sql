/*==============================================================*/
/* ��. ������� ������ ���� ����������                           */
/*==============================================================*/
create procedure select_employees
as
select * from [dbo].[Employee]
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
