create table Inquilino(
	ID_Inquilino varchar(100) not null,
	Tipo_Utilizador varchar(100) not null,
	Foto_perfil varchar(200) not null,
	Nome varchar(50) not null,
       Email varchar(50) not null,
	Data_de_Registo date not null default GETDATE (), 
       Data_Nascimento date not null,
       Username varchar(20) not null,
	Password varchar(200) not null,
	Estado int null,
       Numero_de_Telemovel varchar(25) not null,
       Cartao_de_Cidadao int not null,
       Primary key(ID_Inquilino),
       Unique(Email, Username),
       
	)

	create table Proprietario(
	ID_Proprietario varchar(100) not null,
	Tipo_Utilizador varchar(100) not null,
	Foto_perfil varchar(200) not null,
	Nome varchar(50) not null,
       Email varchar(50) not null,
	Data_de_Registo date not null default GETDATE (), 
       Data_Nascimento date not null,
       Username varchar(20) not null,
	Estado int null,
	Password varchar(200) not null,
       Numero_de_Telemovel varchar(25) not null,
       Cartao_de_Cidadao int not null,
       Primary key(ID_Proprietario),
       Unique(Email, Username),
      
	)

	create table Administrador(
	ID_Administrador varchar(100) not null,
	Tipo_Utilizador varchar(100) not null,
	Nome varchar(50) not null,
       Username varchar(20) not null,
	Password varchar(200) not null,
       Data_Nascimento date not null,
	Email varchar(50) not null,
       Cartao_de_Cidadao int not null,
       Primary key(ID_Administrador),
       Unique(Email, Username)
)

	create table Imovel(
       ID_Imovel int not null identity(1,1),
       Rua varchar(50)not null,
       Foto_principal varchar(100)not null,
       Codigo_Postal varchar(25)not null,
       N_Porta int not null,
       Estado int not null,
       Finalidade varchar(25) not null, 
       Data_de_Inscricao date not null default GETDATE (),
	Valor_aluguer money null,
	Valor_venda Money null,
	Descricao varchar(MAX) not null,
       Metros int not null,
       Latitude varchar(100) not null,
       Longitude varchar(100) not null,
       Garagem bit not null,
       Quantidade_de_Suits int not null,
       Quantidade_de_Quartos int not null,
       Quantidade_de_Casas_de_Banho int not null,
       Tipo_de_Imovel varchar(50) not null,
       N_vizualizacoes int not null,
       Primary key(ID_Imovel)
)
create table Fotos(
	ID_Foto int not null identity(1,1),
	Nome_Foto varchar(100) not null,
       Primary key(ID_Foto)
	)
create table Distrito(
	ID_Distrito int not null,
	Nome_distrito varchar(50) not null,
       Primary key(ID_Distrito)
	)
create table Concelho(
	ID_Concelho int not null,
	Nome_concelho varchar(50) not null,
       Primary key(ID_Concelho)
	)
create table PossuiFotos(
	ID_Foto int not null,
	ID_Imovel int not null,
	Primary key(ID_Foto),
       Foreign key(ID_Foto) references Fotos(ID_Foto),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
)
create table PossuiImovel(
	ID_Proprietario varchar(100) not null,
	ID_Imovel int not null,
	Primary key(ID_Imovel),
       Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
)

create table Situa_se(
	ID_Distrito int not null,
	ID_Concelho int not null,
       Primary key(ID_Concelho),
       Foreign key(ID_Concelho) references Concelho(ID_Concelho),
       Foreign key(ID_Distrito) references Distrito(ID_Distrito)
)
create table Encontra_se(
	ID_Imovel int not null,
	ID_Concelho int not null,
       Primary key(ID_Imovel),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
       Foreign key(ID_Concelho) references Concelho(ID_Concelho)
)
create table Aluga(
	Data_Aluguer_Inicio date not null default GETDATE (),
       Valor int not null,
	ID_Inquilino varchar(100) not null,
	ID_Proprietario varchar(100) not null,
	ID_Imovel int not null,
       Data_Aluguer_Final date not null,
       Primary key(Data_Aluguer_Inicio,ID_Inquilino, ID_Imovel),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
       Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino),
	
)
create table Compra(
       Data_de_compra date not null,
	ID_Inquilino varchar(100) not null,
	ID_Proprietario varchar(100) not null,
	ID_Imovel int not null,
       Valor int not null,
       Primary key(Data_de_compra,ID_Inquilino, ID_Imovel),
	Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino)
	
)

create table Comenta(
       Data_de_comentario datetime not null,
       ID_Inquilino varchar(100) not null,
       ID_Imovel int not null,
       Texto varchar(max) not null,
       Primary key(Data_de_comentario,ID_Inquilino,ID_Imovel),
       Foreign key(ID_Imovel) references Imovel(ID_Imovel),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino)
	
)
create table Denuncia(
	Data_denuncia datetime not null,
	ID_Inquilino varchar(100) not null,
       ID_Proprietario varchar(100) not null,
       Estado varchar(20) not null,
       Texto varchar(MAX) not null,
       Primary key(Data_denuncia, ID_Inquilino, ID_Proprietario),
       Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino),
	
)
create table DenunciaProp(
	Data_denuncia datetime not null,
	ID_Inquilino varchar(100) not null,
       ID_Proprietario varchar(100) not null,
       Estado varchar(20) not null,
       Texto varchar(MAX) not null,
       Primary key(Data_denuncia, ID_Inquilino, ID_Proprietario),
       Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino),
	
)

create table BloqueiaInqulino(
	Data_de_bloqueio smalldatetime not null default GETDATE(),
	ID_Inquilino varchar(100) not null,
	ID_Administrador varchar(100) not null,
       Motivo varchar(MAX) not null,
       Primary key(Data_de_bloqueio, ID_Inquilino),
       Foreign key(ID_Inquilino) references Inquilino(ID_Inquilino),
       Foreign key(ID_Administrador) references Administrador(ID_Administrador)
	
)
create table BloqueiaProprietario(
	Data_de_bloqueio smalldatetime not null default GETDATE(),
	ID_Proprietario varchar(100) not null,
	ID_Administrador varchar(100) not null,
       Motivo varchar(MAX) not null,
       Primary key(Data_de_bloqueio, ID_Proprietario),
       Foreign key(ID_Proprietario) references Proprietario(ID_Proprietario),
       Foreign key(ID_Administrador) references Administrador(ID_Administrador)
	
)

