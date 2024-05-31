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
address_district varchar(max) not null,
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
fantasy_name varchar(max) not null,
cnpj varchar(14) not null,
phone_number varchar(11) not null,
postal_code varchar(8) not null,
address_street varchar(max) not null,
address_number varchar(max) not null,
address_complement varchar(max),
address_district varchar(max) not null,
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

--Insert disposial
create procedure spInsert_disposal_schedules (
 @schedule_id int,
 @device_type varchar(max),
 @device_conservation_state varchar(max),
 @schedule_date datetime,
 @created_date datetime,
 @fk_user_sender_id int,
 @fk_user_receiver_id int
)
as
begin
 insert into [dbo].[disposal_schedules]
 (schedule_id, device_type, device_conservation_state, schedule_date, created_date,fk_user_sender_id, fk_user_receiver_id)
 values
 (@schedule_id, @device_type , @device_conservation_state, @schedule_date, @created_date, @fk_user_sender_id, @fk_user_receiver_id)
end

--Update Disposial
create procedure spUpdate_disposal_schedules (
 @schedule_id int,
 @device_type varchar(max),
 @device_conservation_state varchar(max),
 @schedule_date datetime,
 @created_date datetime,
 @fk_user_sender_id int,
 @fk_user_receiver_id int
)
as
begin
 update [dbo].[disposal_schedules] set
 device_type = @device_type,
device_conservation_state = @device_conservation_state,
schedule_date = @schedule_date,
created_date = @created_date,
fk_user_sender_id = @fk_user_sender_id,
fk_user_receiver_id = @fk_user_receiver_id
 where schedule_id = @schedule_id
end

--delete disposial
create procedure spDelete_disposal_schedules
(
 @schedule_id int
)
as
begin
 delete [dbo].[disposal_schedules] where schedule_id = @schedule_id
end


--INSERT LOGIN RECEIVER
create procedure spInsert_register_receiver_logins(
 @receiver_login_id int,
 @fk_receiver_username varchar(20),
 @receiver_password varchar(12)
)
as
begin
 insert into [dbo].[register_receiver_logins]
 (receiver_login_id, fk_receiver_username, receiver_password)
 values
 (@receiver_login_id, @fk_receiver_username, @receiver_password)
end

--UPDATE LOGIN RECEIVER (SÓ FAZ ALTERAÇÃO DA SENHA)
create procedure spUpdate_register_receiver_logins(
 @receiver_login_id int,
 @fk_receiver_username varchar(20),
 @receiver_password varchar(12)
)
as
begin
 update [dbo].[register_receiver_logins] set
 receiver_password = @receiver_password
 where fk_receiver_username = @fk_receiver_username
end

--delete login receiver
create procedure spDelete_register_receiver_logins
--O DELETE DAQUI DEVE SER FEITO QND O DELETE DO USUARIO FOR FEIOT NA TABELA REGISTER_RECEIVER_USER


--INSERT RECEIVER USER
alter procedure spInsert_register_receiver_user(
@user_receiver_id int,
@username varchar(20) ,
@full_name varchar(max) ,
@fantasy_name varchar(max),
@cnpj varchar(14) ,
@phone_number varchar(11),
@postal_code varchar(8) ,
@address_street varchar(max) ,
@address_number varchar(max) ,
@address_complement varchar(max),
@address_district varchar(max),
@address_city varchar(max),
@address_state varchar(max),
@email varchar(max),
@created_date date 
)
as
begin
	insert into [dbo].[register_receiver_user] (user_receiver_id, username, full_name, 
	fantasy_name, cnpj, phone_number, postal_code, address_street, address_number, 
	address_complement,address_district, address_city, address_state, email, created_date) 
	VALUES
	(@user_receiver_id, @username, @full_name, 
	@fantasy_name, @cnpj, @phone_number, @postal_code, @address_street, @address_number, 
	@address_complement, @address_district, @address_city, @address_state, @email, @created_date)
end

-- UPDATE RECEIVER USER
alter procedure spUpdate_register_receiver_user(
@user_receiver_id int,
@username varchar(20) ,
@full_name varchar(max) ,
@fantasy_name varchar(max),
@cnpj varchar(14) ,
@phone_number varchar(11),
@postal_code varchar(8) ,
@address_street varchar(max) ,
@address_number varchar(max) ,
@address_complement varchar(max),
@address_district varchar(max),
@address_city varchar(max),
@address_state varchar(max),
@email varchar(max),
@created_date date 
)
as
begin
	update [dbo].[register_receiver_user] set
	username  = @username,
	full_name  = @full_name,
	fantasy_name = @fantasy_name,
	cnpj = @cnpj,
	phone_number  = @phone_number,
	postal_code = @postal_code,
	address_street = @address_street,
	address_number = @address_number,
	address_complement = @address_complement,
	address_district = @address_district,
	address_city = @address_city,
	address_state = @address_state,
	email = @email
	where user_receiver_id = @user_receiver_id
end

--DELETE RECEIVER USER
create procedure spDelete_register_receiver_user(
 @user_receiver_id int
)
as
begin
 delete [dbo].[register_receiver_user] where user_receiver_id = @user_receiver_id
end



--INSERT LOGIN sender
create procedure spInsert_register_sender_logins(
 @sender_login_id int,
 @fk_sender_username varchar(20),
 @sender_password varchar(12)
)
as
begin
 insert into [dbo].[register_sender_logins]
 (sender_login_id, fk_sender_username, sender_password)
 values
 (@sender_login_id, @fk_sender_username, @sender_password)
end

--UPDATE LOGIN sender (SÓ FAZ ALTERAÇÃO DA SENHA)
create procedure spUpdate_register_sender_logins(
 @sender_login_id int,
 @fk_sender_username varchar(20),
 @sender_password varchar(12)
)
as
begin
 update [dbo].[register_sender_logins] set
 sender_password = @sender_password
 where fk_sender_username = @fk_sender_username
end

--delete login sender
create procedure spDelete_register_sender_logins
--O DELETE DAQUI DEVE SER FEITO QND O DELETE DO USUARIO FOR FEIOT NA TABELA REGISTER_sender_USER

--INSERT SENDER USER
alter procedure spInsert_register_sender_user (
@user_sender_id int,
@username varchar(20),
@full_name varchar(max),
@cpf varchar(11),
@phone_number varchar(11), 
@postal_code varchar(8),
@address_street varchar(max), 
@address_number varchar(max),
@address_complement varchar(max),
@address_district varchar(max),
@address_city varchar(max),
@address_state varchar(max),
@email varchar(max),
@created_date date
)
as
begin
 insert into [dbo].[register_sender_user] (user_sender_id,
username ,
full_name,
cpf ,
phone_number, 
postal_code,
address_street, 
address_number,
address_complement,
address_district,
address_city,
address_state,
email,
created_date
)
values (@user_sender_id,
@username ,
@full_name,
@cpf ,
@phone_number, 
@postal_code,
@address_street, 
@address_number,
@address_complement,
@address_district,
@address_city,
@address_state,
@email,
@created_date
)
end

--UPDATE SENDER USER
alter procedure spUpdate_register_sender_user(
@user_sender_id int,
@username varchar(20),
@full_name varchar(max),
@cpf varchar(11),
@phone_number varchar(11), 
@postal_code varchar(8),
@address_street varchar(max), 
@address_number varchar(max),
@address_complement varchar(max),
@address_district varchar(max),
@address_city varchar(max),
@address_state varchar(max),
@email varchar(max),
@created_date date
)
as
begin
 update [dbo].[register_sender_user] set
username  = @username,
full_name = @full_name,
cpf  = @cpf ,
phone_number = @phone_number,
postal_code = @postal_code,
address_street = @address_street,
address_number  = @address_number ,
address_complement = @address_complement,
address_district = @address_district,
address_city = @address_city,
address_state = @address_state,
email = @email,
created_date = @created_date
where user_sender_id = @user_sender_id
end

--DELETE SENDER USER
create procedure spDelete_register_sender_user(
 @user_sender_id int
)
as
begin
 delete [dbo].[register_sender_user] where user_sender_id = @user_sender_id
end


alter procedure spProximoId (@tabela varchar(max))
as
	begin
		if @tabela = 'register_sender_user'
		begin
			exec('select isnull(max(user_sender_id) +1, 1) as MAIOR from ' +@tabela)
		end
		else if @tabela ='register_receiver_user'
		begin
			exec('select isnull(max(user_receiver_id) +1, 1) as MAIOR from ' +@tabela)
		end
		else if @tabela ='register_receiver_logins'
		begin
			exec('select isnull(max(receiver_login_id) +1, 1) as MAIOR from ' +@tabela)
		end
		else if @tabela ='register_sender_logins'
		begin
			exec('select isnull(max(sender_login_id) +1, 1) as MAIOR from ' +@tabela)
		end
		else if @tabela ='disposal_schedules'
		begin
			exec('select isnull(max(schedule_id) +1, 1) as MAIOR from ' +@tabela)
		end
	end
	

--PROCEDURE CONSULTA SE USERNAME JA FOI USADO
alter procedure spConsulta_Username
(
    @username varchar(max),
    @tabela varchar(max)
)
as
begin
    declare @sql nvarchar(max);
    set @sql = 'select * from ' + quotename(@tabela) + ' where username = @username';
    
    print 'Generated SQL: ' + @sql; -- Debugging aid
    
    exec sp_executesql @sql, N'@username varchar(max)', @username;
end

--PROCEDURE CONSULTA SE CPF OU cnpj JA FOI USADO
create procedure spConsulta_CNPJ_CPF
(
 @documento varchar(max) ,
 @tabela varchar(max)
)
as
begin
 declare @sql varchar(max);

 if @tabela = 'register_sender_user'
 begin
 	set @sql = 'select * from ' + @tabela +
 	' where cpf = ' + @documento
 end
 else if @tabela = 'register_receiver_user'
 begin 
	set @sql = 'select * from ' + @tabela +
 	' where cnpj = ' + @documento
 end
 exec(@sql)
end

--CHECA A TENTATIVA DE LOGIN
ALTER PROCEDURE spConsulta_Login
(
    @username NVARCHAR(MAX),
    @password NVARCHAR(MAX),
    @tabela NVARCHAR(MAX)
)
AS
BEGIN
    DECLARE @sql NVARCHAR(MAX);

    IF @tabela = 'register_sender_logins'
    BEGIN
        SET @sql = 'SELECT * FROM ' + QUOTENAME(@tabela) +
                   ' WHERE fk_sender_username = @username AND sender_password = @password';
    END
    ELSE IF @tabela = 'register_receiver_logins'
    BEGIN
        SET @sql = 'SELECT * FROM ' + QUOTENAME(@tabela) +
                   ' WHERE fk_receiver_username = @username AND receiver_password = @password';
    END

    EXEC sp_executesql @sql, N'@username NVARCHAR(MAX), @password NVARCHAR(MAX)', @username, @password;
END


--tabela recebimentos
create table recebimentos_electronicrap (
	recebimento_id int primary key not null,
	quantidade_recebimento varchar(max) not null,
	descricao_recebimento varchar(max) not null,
	fk_user_sender_id int foreign key references [dbo].[register_sender_user](user_sender_id) not null,
	fk_user_receiver_id int foreign key references [dbo].[register_receiver_user](user_receiver_id) not null,
	fk_categoria_lixo int foreign key references [dbo].[categorias_lixo_electronicrap](categoria_id) not null
	
)

--procedure cria recebimentos
CREATE procedure spInsert_recebimentos_electronicrap(
@recebimento_id int,
@quantidade_recebimento varchar(max),
@descricao_recebimento varchar(max),
@fk_user_sender_id int ,
@fk_user_receiver_id int,
@fk_categoria_lixo int
)
as
begin
	insert into recebimentos_electronicrap (recebimento_id, quantidade_recebimento, descricao_recebimento, fk_user_sender_id, fk_user_receiver_id, fk_categoria_lixo) 
	VALUES
	(@recebimento_id, @quantidade_recebimento, @descricao_recebimento, @fk_user_sender_id, @fk_user_receiver_id, @fk_categoria_lixo)
end