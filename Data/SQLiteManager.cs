using System.Data;
using System.Data.SQLite;

using BlazorLibrary.Modelle;
using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Csv;

namespace BlazorLibrary.Data
{
	public class SQLiteManager
	{
		private static readonly string sqliteConString = $"Data Source={Path.Combine(Manager.MauiProgramActiveDirectory(), MauiProgram.Einstellungen.Sqlitedatabasename)}.sqlite;Version=3;";

		public static void SetupDatabase()
		{
			ExecuteQuery("CREATE TABLE IF NOT EXISTS spiele (id INT, name TEXT, beschreibung TEXT, bildlink TEXT, exepfad TEXT, sternetooltip TEXT,favorit INT, sterne INT, papierkorb INT, metacritic TEXT, estimatedprice TEXT, PRIMARY KEY(id))");
			ExecuteQuery("CREATE TABLE IF NOT EXISTS genre (id INT, name TEXT, PRIMARY KEY(id))");
			ExecuteQuery("CREATE TABLE IF NOT EXISTS spielgenre (spielid INT, genreid INT, PRIMARY KEY (spielid, genreid))");
		}

		private static SQLiteConnection HoleConnnection()
		{
			return new(sqliteConString);
		}

		private static void ExecuteQuery(string cmd)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = cmd;
					comm.ExecuteNonQuery();
				}
			}
		}

		public async Task CsvInsertGenreInDatabase(List<CsvGenre> genreListe)
		{
			foreach (CsvGenre genre in genreListe)
			{
				await CreateGenreInDatabase(genre.Name);
			}
		}

		public async Task CsvInsertSpieleInDatabase(List<CsvSpiel> spieleliste)
		{
			foreach (CsvSpiel spiel in spieleliste)
			{
				await CreateGameInDatabase(spiel.Name, spiel.Beschreibung, spiel.Bildlink, spiel.Exepfad, string.Empty, 0, 0, 0, string.Empty, string.Empty);
			}
		}

		public Task<int> CreateGameInDatabase(string name, string beschreibung = "", string bild = "", string exepfad = "", string sternetooltip = "", int favorit = 0, int sterne = 0, int papierkorb = 0, string metacritic = "", string estimatedprice = "")
		{
			Manager.RemoveNullValues(ref beschreibung, ref exepfad);
			Manager.AddExternalSource(name, ref bild, ref metacritic, ref estimatedprice);

			int spieldbid = Manager.RandomSpielNummer();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "INSERT INTO spiele (id, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice) VALUES(@Id, @Name, @Beschreibung, @Bild, @Exepfad, @Sternetooltip, @Favorit, @Sterne, @Papierkorb, @Metacritic, @Estimatedprice)";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Id", spieldbid));
					comm.Parameters.Add(new("@Name", name));

					comm.Parameters.Add(new("@Beschreibung", beschreibung));
					comm.Parameters.Add(new("@Bild", bild));

					comm.Parameters.Add(new("@Exepfad", exepfad));
					comm.Parameters.Add(new("@Sternetooltip", sternetooltip));

					comm.Parameters.Add(new("@Favorit", favorit));
					comm.Parameters.Add(new("@Sterne", sterne));

					comm.Parameters.Add(new("@Papierkorb", papierkorb));
					comm.Parameters.Add(new("@Metacritic", metacritic));

					comm.Parameters.Add(new("@Estimatedprice", estimatedprice));
					comm.ExecuteNonQueryAsync();
				}
			}
			return Task.FromResult(spieldbid);
		}

		public async Task UpdateSpielInDatabase(Spiel spiel)
		{
			string metacritic = "!";
			string estimatedprice = "!";

			string bild = spiel.Bildlink;
			Manager.AddExternalSource(spiel.Name, ref bild, ref metacritic, ref estimatedprice);

			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "UPDATE spiele SET name=@Name, beschreibung=@Beschreibung, bildlink=@Bildlink, exepfad=@Exepfad, metacritic=@Metacritic, estimatedprice=@Estimatedprice WHERE id=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Name", spiel.Name));
					comm.Parameters.Add(new("@Beschreibung", spiel.Beschreibung));

					comm.Parameters.Add(new("@Bildlink", bild));
					comm.Parameters.Add(new("@Exepfad", spiel.Exepfad));

					comm.Parameters.Add(new("@Metacritic", metacritic));
					comm.Parameters.Add(new("@Estimatedprice", estimatedprice));

					comm.Parameters.Add(new("@Id", spiel.Id));
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task UpdateSterneForGame(Spiel spiel)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "UPDATE spiele SET sterne=@Sterne, sternetooltip=@Sternetooltip WHERE id=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Sterne", spiel.Sterne));
					comm.Parameters.Add(new("@Sternetooltip", spiel.SterneTooltip));

					comm.Parameters.Add(new("@Id", spiel.Id));
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task SaveGenreOfGame(int spielid, int[] genreIds)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				foreach (int IdGenre in genreIds)
				{
					using (SQLiteCommand comm = con.CreateCommand())
					{
						comm.CommandText = "INSERT INTO spielgenre (spielid,genreid) VALUES(@Spielid,@Genreid)";
						comm.CommandType = CommandType.Text;

						comm.Parameters.Add(new("@Spielid", spielid));
						comm.Parameters.Add(new("@Genreid", IdGenre));

						await comm.ExecuteNonQueryAsync();
					}
				}
			}
		}

		public async Task<bool> SpielGenreCheck(int[] genreids)
		{
			bool spielNutztGenre = false;
			List<Spiel> AktuelleSpieleListe = await SpieleListeErhalten();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				foreach (Spiel spiel in AktuelleSpieleListe)
				{
					foreach (int IdGenre in genreids)
					{
						using (SQLiteCommand comm = con.CreateCommand())
						{
							comm.CommandText = "SELECT COUNT(*) FROM spielgenre WHERE spielid=@spielid AND genreid=@genreid;";
							comm.CommandType = CommandType.Text;

							comm.Parameters.Add(new("@spielid", spiel.Id));
							comm.Parameters.Add(new("@genreid", IdGenre));

							SQLiteDataReader r = comm.ExecuteReader();
							while (r.Read())
							{
								int count = r.GetInt32(0);
								if (count > 0)
								{
									spielNutztGenre = true;
								}
							}
						}
					}
				}
			}
			return spielNutztGenre;
		}

		public async Task SetSpielDeleteStatus(Spiel spiel, bool state)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					int papierkorbstatus = 0;
					if (state)
					{
						papierkorbstatus = 1;
					}
					comm.CommandText = "UPDATE spiele SET papierkorb=@Papierkorb WHERE id=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Papierkorb", papierkorbstatus));
					comm.Parameters.Add(new("@Id", spiel.Id));

					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteSpielCompletely(Spiel spiel)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "DELETE FROM spiele WHERE id=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Id", spiel.Id));
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task DeleteGenreInDatabase(int[] genreids)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				foreach (int IdGenre in genreids)
				{
					using (SQLiteCommand comm = con.CreateCommand())
					{
						comm.CommandText = "DELETE FROM genre WHERE id=@Id";
						comm.CommandType = CommandType.Text;

						comm.Parameters.Add(new("@Id", IdGenre));
						await comm.ExecuteNonQueryAsync();
					}
				}
			}
		}

		public async Task CreateGenreInDatabase(string name)
		{
			int genreid = Manager.RandomGenreNummer();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "INSERT INTO genre (id,name) VALUES (@genreid,@genrename)";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@genreid", genreid));
					comm.Parameters.Add(new("@genrename", name));

					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task RemoveAllGenreOfGame(int spielid)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "DELETE FROM spielgenre WHERE spielid=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Id", spielid));
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task SpielFavoritSetzen(int spielid, bool favorit)
		{
			int favwert = 0;
			if (favorit) { favwert = 1; }
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "UPDATE spiele SET favorit=@favwert WHERE id=@spielid";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@favwert", favwert));
					comm.Parameters.Add(new("@spielid", spielid));

					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public async Task RenameGenre(Genre genre)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "UPDATE genre SET name=@Name WHERE id=@Id";
					comm.CommandType = CommandType.Text;

					comm.Parameters.Add(new("@Name", genre.Name));
					comm.Parameters.Add(new("@Id", genre.Id));

					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public Genre BestimmtesGenreErhalten(int genreid)
		{
			string genrename = string.Empty;
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "SELECT name FROM genre WHERE id=@Id";
					comm.Parameters.Add(new("@Id", genreid));

					SQLiteDataReader r = comm.ExecuteReader();
					while (r.Read())
					{
						string name = (string)r["name"];
						genrename = name;
					}
				}
			}
			return new Genre(genreid, genrename);
		}

		public Task<Genre[]> AlleGenreErhalten()
		{
			List<Genre> genreList = new();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "SELECT id, name FROM genre";
					SQLiteDataReader r = comm.ExecuteReader();

					while (r.Read())
					{
						int id = (int)r["id"];
						string name = (string)r["name"];

						genreList.Add(new Genre(id, name));
					}
				}
			}
			return Task.FromResult(genreList.ToArray());
		}

		public string[] GenreVonSpielErhalten(int spielid)
		{
			List<String> genreList = new();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "SELECT genre.name FROM spielgenre JOIN spiele ON spielgenre.spielid = spiele.id JOIN genre ON spielgenre.genreid = genre.id WHERE spiele.id=@spielid";
					comm.Parameters.Add(new("@spielid", spielid));

					SQLiteDataReader r = comm.ExecuteReader();
					while (r.Read())
					{
						string genre = (string)r["name"];
						genreList.Add(genre);
					}
				}
			}
			return genreList.ToArray();
		}

		public Genre[] GenreArrayVonSpielErhalten(int spielid)
		{
			List<Genre> genreList = new();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = "SELECT genre.id, genre.name FROM spielgenre JOIN spiele ON spielgenre.spielid = spiele.id JOIN genre ON spielgenre.genreid = genre.id WHERE spiele.id=@spielid";
					comm.Parameters.Add(new("@spielid", spielid));

					SQLiteDataReader r = comm.ExecuteReader();
					while (r.Read())
					{
						int id = (int)r["id"];
						string name = (string)r["name"];

						genreList.Add(new Genre(id, name));
					}
				}
			}
			return genreList.ToArray();
		}

		public Task<Spiel> SpielErhalten(int SpielId)
		{
			Spiel spiel = null;
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand fmd = con.CreateCommand())
				{
					fmd.CommandText = "SELECT id, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice FROM spiele WHERE id=@SpielId";
					fmd.Parameters.Add(new("@spielid", SpielId));

					SQLiteDataReader r = fmd.ExecuteReader();
					while (r.Read())
					{
						int id = (int)r["id"];
						string name = (string)r["name"];

						string beschreibung = (string)r["beschreibung"];
						string bildlink = (string)r["bildlink"];

						string exepfad = (string)r["exepfad"];
						string sternetooltip = (string)r["sternetooltip"];

						int fav = (int)r["favorit"];
						int sterne = (int)r["sterne"];

						int papierkorb = (int)r["papierkorb"];
						string metacritic = (string)r["metacritic"];

						string estimatedprice = (string)r["estimatedprice"];
						Genre[] genrelist = GenreArrayVonSpielErhalten(id);

						spiel = new(id, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, papierkorb, metacritic, estimatedprice, genrelist);
					}
				}
			}
			return Task.FromResult(spiel);
		}

		public async Task<List<Spiel>> SpieleListeErhalten()
		{
			List<Spiel> spiele = new();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand fmd = con.CreateCommand())
				{
					fmd.CommandText = "SELECT id, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice FROM spiele";
					SQLiteDataReader r = fmd.ExecuteReader();

					while (r.Read())
					{
						int id = (int)r["id"];
						string name = (string)r["name"];

						string beschreibung = (string)r["beschreibung"];
						string bildlink = ConvertFromDBVal<string>(r["bildlink"]);

						string exepfad = (string)r["exepfad"];
						string sternetooltip = (string)r["sternetooltip"];

						int fav = (int)r["favorit"];
						int sterne = (int)r["sterne"];

						int papierkorb = (int)r["papierkorb"];
						string metacritic = (string)r["metacritic"];

						string estimatedprice = (string)r["estimatedprice"];
						Genre[] genrelist = GenreArrayVonSpielErhalten(id);

						spiele.Add(new Spiel(id, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, papierkorb, metacritic, estimatedprice, genrelist));
					}
				}
			}
			return spiele;
		}

		public async Task<List<Spiel>> GeloeschteSpieleListeErhalten()
		{
			List<Spiel> spiele = new();
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand fmd = con.CreateCommand())
				{
					fmd.CommandText = "SELECT id, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice FROM spiele WHERE papierkorb=1";
					SQLiteDataReader r = fmd.ExecuteReader();
					while (r.Read())
					{
						int id = (int)r["id"];
						string name = (string)r["name"];

						string beschreibung = (string)r["beschreibung"];
						string bildlink = (string)r["bildlink"];

						string exepfad = (string)r["exepfad"];
						string sternetooltip = (string)r["sternetooltip"];

						int fav = (int)r["favorit"];
						int sterne = (int)r["sterne"];

						int papierkorb = (int)r["papierkorb"];
						string metacritic = (string)r["metacritic"];

						string estimatedprice = (string)r["estimatedprice"];
						Genre[] genrelist = GenreArrayVonSpielErhalten(id);

						spiele.Add(new Spiel(id, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, papierkorb, metacritic, estimatedprice, genrelist));
					}
				}
			}
			return spiele;
		}

		public static async Task ExecuteQueryAsync(string cmd)
		{
			using (SQLiteConnection con = HoleConnnection())
			{
				con.Open();
				using (SQLiteCommand comm = con.CreateCommand())
				{
					comm.CommandText = cmd;
					await comm.ExecuteNonQueryAsync();
				}
			}
		}

		public static T ConvertFromDBVal<T>(object obj)
		{
			if (obj == null || obj == DBNull.Value)
			{
				return default(T); // returns the default value for the type
			}
			else
			{
				return (T)obj;
			}
		}
	}
}