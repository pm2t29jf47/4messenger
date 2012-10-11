/*==============================================================*/
/* Хп. Выводит данные всех работников                           */
/*==============================================================*/

create procedure select_employees
as
select * from [dbo].[Employee]
go

/*==============================================================*/
/* Хп. Выводит все письма                                       */
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
/* Хп. Добавляет новое письмо в таблицу                         */
/*==============================================================*/

create procedure insert_message(
@title	varchar(100),
@date	datetime,
@recipientId	int,
@senderId	int,
@content varchar(1000))
as
insert into Message values(
@title,
@date,
@recipientId,
@senderId,
@content
)
go
