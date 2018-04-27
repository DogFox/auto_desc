
  use auto76;
  
  drop table if exists dbo.Session;
  create table dbo.Session( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   login varchar (max)   ,
								   id_login  varchar(max) ,
								   time datetime  
								   )

  drop table if exists dbo.users;
  create table dbo.users( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   login varchar (max)   ,
								   password  varchar(max)  
								   )

								   
 
insert into dbo.users
(login, password )
values( 'sa', '290168' )

insert into dbo.users
(login, password )
values( 'ROMAN', '12345' )

insert into dbo.users
(login, password )
values( 'ALEXANDR', '12345' )

  drop table if exists dbo.customers;
  create table dbo.customers( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   name varchar (max)   ,
								   phone  varchar(max)  ,
								   addres  varchar(max) ,
								   price_level   int 
								   )
   
  drop table if exists dbo.parts;
  create table dbo.parts( 
								   id   int  NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   producer varchar (max)  ,
								   part_number  varchar(max), 
								   name varchar(max),
								   model varchar(max),
								   sup_price float,
								   ratio int,
								   count int,
								   code varchar(max),
								   sup_id int
								   )
								   
  drop table if exists dbo.parts_order;
  create table dbo.parts_order( 
								   id   int  NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   producer varchar (max)  ,
								   part_number  varchar(max), 
								   name varchar(max),
								   model varchar(max),
								   sup_price float,
								   ratio int,
								   count int,
								   code varchar(max),
								   sup_id int,
								   order_id int,
								   )

								   
  drop table if exists dbo.parts_import;
  create table dbo.parts_import( 
								   c1  varchar (max),
								   c2 varchar (max)  ,
								   c3  varchar(max), 
								   c4 varchar(max),
								   c5 float,
								   c6  varchar(max), 
								   c7 varchar(max),
								   c8 varchar(max),
								   c9 varchar(max)
								   )

  drop table if exists dbo.suppliers;
  create table dbo.suppliers( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   name varchar (max)   ,
								   phone  int            ,
								   full_name  varchar(max) ,
								   addres  varchar(max) ,
								   kpp  varchar(max) ,
								   inn  varchar(max)  
								   )
								   
  drop table if exists dbo.orders;
  create table dbo.orders( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   number varchar (max)  ,
								   cust_id  int ,
								   summ  float,
								   [count]  int ,
								   comment  varchar(max),
								   [status] int ,
								   [date] datetime,
								   author varchar(50)
								   )
								   
  drop table if exists dbo.part_order;
  create table dbo.part_order( 
								   order_id   int,
								   part_id int  ,
								   )

								   
alter table dbo.parts_order
add  price float


IF OBJECT_ID('dbo.order_view') IS NOT NULL
	drop view dbo.order_view
GO
CREATE view dbo.order_view as
SELECT        o.id, o.number, o.cust_id, o.comment, o.status, o.date, o.author, c.name, COUNT(po.id) AS count, SUM(po.price) AS price, SUM(po.price - po.sup_price) AS marge
FROM            dbo.orders AS o INNER JOIN
                         dbo.customers AS c ON c.id = o.cust_id INNER JOIN
                         dbo.parts_order AS po ON po.order_id = o.id
GROUP BY o.id, o.number, o.cust_id, o.comment, o.status, o.date, o.author, c.name