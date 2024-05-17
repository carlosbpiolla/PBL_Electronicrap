drop table [dbo].[register_sender_user]
drop table [dbo].[register_receiver_user]
drop table [dbo].[disposal_schedules]
drop table [dbo].[register_receiver_logins]
drop table [dbo].[register_sender_logins]

--TABELA USUARIO QUE VAI DAR O EQUIPAMENTO
CREATE TABLE register_sender_user 
(
	user_sender_id int primary key not null,
	username varchar(20) unique not null,
	full_name varchar(max) not null,
	cpf varchar(11) not null,
	phone_number varchar(11) not null,
	postal_code varchar(8) not null,
	address_street varchar(max) not null,
	address_number varchar(max) not null,
	address_complement varchar(max),
	address_city varchar(max) not null,
	address_state varchar(max) not null,
	email varchar(max) not null,
	created_date date not null 
);

--TABELA USUARIO QUE VAI RECEBER O EQUIPAMENTO
CREATE TABLE register_receiver_user 
(
	user_receiver_id int primary key not null,
	username varchar(20) unique not null,
	full_name varchar(max) not null,
	cpf varchar(11) not null,
	fantasy_name varchar(max) not null,
	cnpj varchar(14) not null,
	phone_number varchar(11) not null,
	postal_code varchar(8) not null,
	address_street varchar(max) not null,
	address_number varchar(max) not null,
	address_complement varchar(max),
	address_city varchar(max) not null,
	address_state varchar(max) not null,
	email varchar(max) not null,
	created_date date not null 
);

--TABELA AGENDAMENTOS
CREATE TABLE disposal_schedules
(
	schedule_id int primary key not null,
	device_type varchar(max) not null,
	device_conservation_state varchar(max) not null,
	schedule_date datetime not null,
	created_date datetime not null,
	fk_user_sender_id int foreign key references [dbo].[register_sender_user](user_sender_id) not null,
	fk_user_receiver_id int foreign key references register_receiver_user(user_receiver_id) not null
);

--TABELA GUARDA LOGINS USUARIOS QUE DAO EQUIPAMENTO
create table register_sender_logins
(
	sender_login_id int primary key not null,
	fk_sender_username varchar(20) foreign key references [dbo].[register_sender_user](username) not null,
	sender_password varchar(12) not null
);

--TABELA GUARDA LOGINS USUARIOS QUE RECEBEM EQUIPAMENTOS
create table register_receiver_logins
(
	receiver_login_id int primary key not null,
	fk_receiver_username varchar(20) foreign key references [dbo].[register_receiver_user](username) not null,
	receiver_password varchar(12) not null
);

