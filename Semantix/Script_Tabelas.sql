CREATE TABLE Cliente(
	Cod INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(255) NOT NULL
);

CREATE TABLE Endereco(
	Cod INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	CEP VARCHAR(8) NULL,
	Numero VARCHAR(10) NULL,
	Cod_Cliente INT FOREIGN KEY REFERENCES Cliente(Cod)
);

CREATE TABLE Telefone(
	Cod INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Numero VARCHAR(20) NULL,
	Cod_Cliente INT FOREIGN KEY REFERENCES Cliente(Cod)
);