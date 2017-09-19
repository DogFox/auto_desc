
  use auto76;

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
								   name varchar (max)  ,
								   part_number  varchar(max), 
								   sup_price float,
								   price float
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
								   [date] datetime
								   )
								   
  drop table if exists dbo.part_order;
  create table dbo.part_order( 
								   order_id   int  NOT NULL PRIMARY KEY,
								   part_id int  ,
								   sup_id  int  , 
								   )