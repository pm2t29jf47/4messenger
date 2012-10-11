/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2012-09-22 15:59:13                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('Employee')
            and   type = 'U')
   drop table Employee
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Message')
            and   type = 'U')
   drop table Message
go

/*==============================================================*/
/* Table: Employee                                              */
/*==============================================================*/
create table Employee (
   Employee_Id          int                  not null identity,
   Name                 varchar(100)         not null,
   constraint PK_EMPLOYEE primary key nonclustered (Employee_Id)
)
go

/*==============================================================*/
/* Table: Message                                               */
/*==============================================================*/
create table Message (
   Message_Id           int                  not null identity,
   Title                varchar(100)         not null,
   Date                 date                 not null,
   Recipient            int                  not null,
   Sender               int                  not null,
   Content              varchar(1000)        not null,
   constraint PK_MESSAGE primary key nonclustered (Message_Id)
)
go

/*==============================================================*/
/* Сотрудники                                                   */
/*==============================================================*/

insert into Employee values('Иванов Иван Иванович')
insert into Employee values('Петров Петр Петрович')
insert into Employee values('Сидоров Сидр Сидорович')
go

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
@recipient_Id	int,
@sender_Id	int,
@content varchar(1000))
as
insert into Message values(
@title,
@date,
@recipient_Id,
@sender_Id,
@content
)
go
