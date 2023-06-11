using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;

public class DatabaseBuilder : MonoBehaviour {
    public string DatabaseName;
    protected string databasePath;
    protected SqliteConnection Connection => new SqliteConnection($"Data Source = {this.databasePath};");

    private void Awake() {
        if (string.IsNullOrEmpty(this.DatabaseName)) {
            Debug.LogError("DatabaseName está vazio!");
            return;
        }

        CreateDatabaseFileIfNotExists();
        // CopyDatabaseFileIfNotExists();
        /* try
         {

         CreateTableTipo();
         CreateTableInimigo();
         }
         catch(Exception e)
         {
             Debug.LogError(e.Message);
         } */

        CreateTableTipo();
        CreateTableInimigo();
        InsertDataTipo("ADistancia", 3, "Ataca de longa distancia");
        InsertDataTipo("CorpoACorpoNormal", 2, "Ataca diretamente, dano médio");
        InsertDataTipo("CorpoACorpoForte", 4, "Ataca diretamente, dano forte");
        InsertDataInimigo("Minhoca", 2, 1, 2);
        InsertDataInimigo("Arqueiro", 3, 3, 1);
        InsertDataInimigo("Touro", 4, 2, 3);
        Debug.Log("PESQUISA POR MEIO DA CHAVE PRIMARIA");
        DisplayTipoPK(1);
        DisplayInimigoPK(1);
        Debug.Log("INNER JOIN COM AS TABELAS INIMIGO E TIPO");
        DisplayTipoInimigo();
        Debug.Log("LEFT JOIN COM AS TABELAS INIMIGO E TIPO");
        DisplayTipoInimigoLEFTJOIN();






    }

    private void CreateDatabaseFileIfNotExists() {
        this.databasePath = Path.Combine(Application.persistentDataPath, this.DatabaseName);
        if (!File.Exists(this.databasePath)) {
            SqliteConnection.CreateFile(this.databasePath);
            Debug.Log($"Database path: {this.databasePath}");
        }
    }



    protected void CreateTableTipo() {
        using (var conn = Connection) {
            var commandText = $"CREATE TABLE IF NOT EXISTS Tipo"
                              + $"("
                              + $" Id INTEGER PRIMARY KEY,"
                              + $" Nome TEXT NOT NULL,"
                              + $" Dano INTEGER NOT NULL,"
                              + $" Descricao TEXT NOT NULL"
                              + $");";
            conn.Open();
            using (var command = conn.CreateCommand()) {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
               // Debug.LogError("Commando de Encerramento!");
            }
        }
    }






    protected void CreateTableInimigo() {
        using (var conn = Connection) {

            var commandText = $"CREATE TABLE IF NOT EXISTS Inimigo"
                              + $"("
                              + $" Id INTEGER PRIMARY KEY,"
                              + $" Nome TEXT NOT NULL,"
                              + $" Vida INTEGER NOT NULL,"
                              + $" Velocidade INTEGER NOT NULL,"

                              + $" TipoId INTEGER NOT NULL,"
                              + $" FOREIGN KEY (TipoId) REFERENCES Tipo(Id) ON UPDATE CASCADE ON DELETE RESTRICT"
                              + $");";
            //using (var connection = Connection)

            conn.Open();
            using (var command = conn.CreateCommand()) {
                command.CommandText = commandText;
                command.ExecuteNonQuery();
              //  Debug.LogError("Encerra Inimigo");
            }
        }

    }

    protected void InsertDataTipo(string nome, int dano, string descricao) {
        var commandText = "INSERT INTO Tipo (Nome, Dano, Descricao) VALUES(@nome,@dano,@descricao);";

        using (var connection = Connection) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@dano", dano);
                command.Parameters.AddWithValue("@descricao", descricao);
                var result = command.ExecuteNonQuery();
                Debug.Log($"INSERT TIPO: {result.ToString()}");
            }
        }
    }


    protected void InsertDataInimigo(string nome, int vida, int velocidade, int tipoId) {
        var commandText = "INSERT INTO Inimigo (Nome, Vida, Velocidade, TipoId) VALUES(@nome, @vida, @velocidade, @tipoId);";

        using (var connection = Connection) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@vida", vida);
                command.Parameters.AddWithValue("@velocidade", velocidade);
                command.Parameters.AddWithValue("@tipoId", tipoId);
                var result = command.ExecuteNonQuery();
                Debug.Log($"INSERT INIMIGO: {result.ToString()}");
            }
        }
    }


    // Geral
    public void DisplayTipo() {
        using (var connection = Connection) {
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                command.CommandText = "Select nome,dano,descricao from tipo;";

                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Debug.Log("nome:" + reader["nome"] + "\tdano:" + reader["dano"] + "\tdescricao:" + reader["descricao"]);


                    }
                    reader.Close();
                }
                connection.Close();
            }
        }
    }

    public void DisplayInimigo() {
        using (var connection = Connection) {
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                command.CommandText = "Select nome,vida,velocidade,tipoid from inimigo;";

                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Debug.Log("nome:" + reader["nome"] + "\tvida:" + reader["vida"] + "\tvelocidade:" + reader["velocidade"]);


                    }
                    reader.Close();
                }
                connection.Close();
            }
        }
    }

    // pesquisa de uma linha com chave primária
    public void DisplayTipoPK(int id) {
        using (var connection = Connection) {
            //var result = "None";
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                command.CommandText = "Select id, nome,dano,descricao from tipo where Id=id;";

                using (IDataReader reader = command.ExecuteReader()) {

                    Debug.Log("\tid:" + reader["id"] + "\tnome:" + reader["nome"] + "\tdano:" + reader["dano"] + "\tdescricao:" + reader["descricao"]);


                }
                //reader.Close();

                connection.Close();
            }
        }
    }

    public void DisplayInimigoPK(int id) {
        using (var connection = Connection) {
            //var result = "None";
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                command.CommandText = "Select id,nome,vida,velocidade from inimigo where Id=id;";

                using (IDataReader reader = command.ExecuteReader()) {

                    Debug.Log("\tid:" + reader["id"] + "\tnome:" + reader["nome"] + "\tvida:" + reader["vida"] + "\tvelocidade:" + reader["velocidade"]);


                }
                //reader.Close();

                connection.Close();
            }
        }
    }


    //Eliminar um inimigo (personagem)

    // pesquisa de uma linha com chave primária
    protected int DeleteInimigoPK(int id) {
        using (var connection = Connection) {
            var commandText = "DELETE FROM inimigo WHERE Id = @id;";
            connection.Open();

            using (var command = connection.CreateCommand()) {


                {
                    command.CommandText = commandText;
                    command.Parameters.AddWithValue("@id", id);
                    return command.ExecuteNonQuery();


                }


            }
        }
    }

    //atualizar algum valor do inimigo (personagem)
    protected int UpdateDataInimigo(int id, string nome, int vida, int velocidade, int tipoId) {
        var commandText = "UPDATE Inimigo  SET " +
            "Nome = @nome," +
            "Vida = @vida," +
            "Velocidade = velocidade," +
            "TipoId = tipoId " +
            "WHERE Id=@id;";

        using (var connection = Connection) {
            connection.Open();
            using (var command = connection.CreateCommand()) {
                command.CommandText = commandText;
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@nome", nome);
                command.Parameters.AddWithValue("@vida", vida);
                command.Parameters.AddWithValue("@velocidade", velocidade);
                command.Parameters.AddWithValue("@tipoId", tipoId);
                return command.ExecuteNonQuery();
                //Debug.Log("UPDATE CHARACTER");
            }
        }
    }


    // inner join
    public void DisplayTipoInimigo() {
        using (var connection = Connection) {
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                command.CommandText = "Select tipo.nome,vida,velocidade,tipo.dano,tipo.descricao from tipo inner join inimigo on tipo.Id=inimigo.tipoId;";

                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        Debug.Log("nome:" + reader["nome"] + "\tvida" + reader["vida"] + "\tvelocidade:" + reader["velocidade"] + "\tdano:" + reader["dano"] + "\tdescricao:" + reader["descricao"]);


                    }
                    reader.Close();
                }
                connection.Close();
            }
        }
    }


    public void DisplayTipoInimigoLEFTJOIN() {
        using (var connection = Connection) {
            connection.Open();
            //apontar para um objeto chamado command para permitir db control
            using (var command = connection.CreateCommand()) {
                //command.CommandText = "Select tipo.nome,velocidade,tipo.dano,descricao from tipo LEFT JOIN inimigo on tipo.Id=inimigo.tipoId;";
                command.CommandText = "Select tipo.nome,velocidade,tipo.dano,descricao from tipo LEFT JOIN inimigo on tipo.Id=inimigo.tipoId;";
                using (IDataReader reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        //Debug.Log("nome:" + reader["nome"] + "\tvelocidade:" + reader["velocidade"] + "\tdano:" + reader["dano"] + "\tdescricao:" + reader["descricao"]);
                        Debug.Log("nome:" + reader["nome"] + "\tvelocidade:" + reader["velocidade"] + "\tdano:" + reader["dano"] + "\tdescricao:" + reader["descricao"]);

                    }
                    reader.Close();
                }
                connection.Close();
            }
        }
    }

}

