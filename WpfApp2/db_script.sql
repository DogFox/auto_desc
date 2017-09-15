
  use auto76;

  drop table if exists dbo.customers;
  create table dbo.customers( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   name varchar (max)     NOT NULL,
								   phone  int              NOT NULL,
								   addres  varchar(max) ,
								   price_level   int 
								   )
   
  drop table if exists dbo.parts;
  create table dbo.parts( 
								   id   int  NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   name varchar (max)     NOT NULL,
								   part_number  varchar(max) NOT NULL, 
								   sup_price float,
								   price float
								   )

  drop table if exists dbo.suppliers;
  create table dbo.suppliers( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   name varchar (max)     NOT NULL,
								   phone  int              NOT NULL,
								   full_name  varchar(max) ,
								   addres  varchar(max) ,
								   kpp  varchar(max) ,
								   inn  varchar(max)  
								   )
								   
  drop table if exists dbo.orders;
  create table dbo.orders( 
								   id   int NOT NULL IDENTITY(1,1) PRIMARY KEY,
								   number varchar (max)     NOT NULL,
								   cust_id  int not null,
								   summ  float,
								   [count]  int ,
								   comment  varchar(max),
								   [status] int ,
								   [date] datetime
								   )
								   
  drop table if exists dbo.part_order;
  create table dbo.part_order( 
								   order_id   int  NOT NULL PRIMARY KEY,
								   part_id int     NOT NULL,
								   sup_id  int NOT NULL, 
								   )