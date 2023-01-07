using System.Data;
using System.Data.SQLite;

using BlazorLibrary.Modelle;

using BlazorLibrary.Management;
using BlazorLibrary.Modelle.Csv;
using BlazorLibrary.Modelle.Nutzer;

namespace BlazorLibrary.Data
{
    public class SQLiteManager
    {
        public static void SetupDatabase()
        {
            ExecuteQuery("CREATE TABLE IF NOT EXISTS spiele (id INT, owner INT, name TEXT, beschreibung TEXT, bildlink TEXT, exepfad TEXT, sternetooltip TEXT,favorit INT, sterne INT, papierkorb INT, metacritic TEXT, estimatedprice TEXT, PRIMARY KEY(id))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS genre (id INT, owner INT, name TEXT, PRIMARY KEY(id))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS spielgenre (spielid INT, genreid INT, PRIMARY KEY (spielid, genreid))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS nutzer (id INT, name TEXT, pincode INT, PRIMARY KEY (id))");

            //Kommentare und Ids
            ExecuteQuery("CREATE TABLE IF NOT EXISTS kommentare (id INT, kommentartext TEXT, PRIMARY KEY (id))");
        }

        private static SQLiteConnection HoleConnnection()
        {
            return new($"Data Source={Path.Combine(Manager.MauiProgramActiveDirectory(), MauiProgram.Einstellungen.Sqlitedatabasename)}.sqlite;Version=3;");
        }

        private static void ExecuteQuery(string cmd)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = cmd;
                comm.ExecuteNonQuery();
            }
        }

        public async Task CsvInsertGenreInDatabase(List<CsvGenre> genreListe, LibraryUser nutzer)
        {
            foreach (CsvGenre genre in genreListe)
            {
                await CreateGenreInDatabase(genre.Name, nutzer);
            }
        }

        public async Task CsvInsertSpieleInDatabase(List<CsvSpiel> spieleliste, LibraryUser nutzer)
        {
            foreach (CsvSpiel spiel in spieleliste)
            {
                await CreateGameInDatabase(spiel.Name, nutzer, spiel.Beschreibung, spiel.Bildlink, spiel.Exepfad, string.Empty, 0, 0, 0, string.Empty, string.Empty);
            }
        }

        public async Task<int> CreateGameInDatabase(string name, LibraryUser nutzer, string beschreibung = "", string bild = "", string exepfad = "", string sternetooltip = "", int favorit = 0, int sterne = 0, int papierkorb = 0, string metacritic = "", string estimatedprice = "")
        {
            (string _beschreibung, string _exepfad) = Manager.RemoveNullValues(beschreibung, exepfad);

            beschreibung = _beschreibung;
            exepfad = _exepfad;

            (string _bild, string _metacriticvalue, string _estimatedprice) = await Manager.AddExternalSource(name);
            int spieldbid = Manager.RandomSpielNummer();

            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "INSERT INTO spiele (id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice) VALUES(@Id, @Owner, @Name, @Beschreibung, @Bild, @Exepfad, @Sternetooltip, @Favorit, @Sterne, @Papierkorb, @Metacritic, @Estimatedprice)";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@Id", spieldbid));
                    comm.Parameters.Add(new("@Owner", nutzer.Id));

                    comm.Parameters.Add(new("@Name", name));
                    comm.Parameters.Add(new("@Beschreibung", beschreibung));

                    comm.Parameters.Add(new("@Bild", _bild));
                    comm.Parameters.Add(new("@Exepfad", exepfad));

                    comm.Parameters.Add(new("@Sternetooltip", sternetooltip));
                    comm.Parameters.Add(new("@Favorit", favorit));

                    comm.Parameters.Add(new("@Sterne", sterne));
                    comm.Parameters.Add(new("@Papierkorb", papierkorb));

                    comm.Parameters.Add(new("@Metacritic", _metacriticvalue));
                    comm.Parameters.Add(new("@Estimatedprice", _estimatedprice));

                    await comm.ExecuteNonQueryAsync();
                }
            }
            return spieldbid;
        }

        public async Task UpdateSpielInDatabase(Spiel spiel)
        {
            string bild = spiel.Bildlink;
            (string _bild, string _metacriticvalue, string _estimatedprice) = await Manager.AddExternalSource(spiel.Name, bild);

            bild = _bild;
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "UPDATE spiele SET name=@Name, beschreibung=@Beschreibung, bildlink=@Bildlink, exepfad=@Exepfad, metacritic=@Metacritic, estimatedprice=@Estimatedprice WHERE id=@Id";
                comm.CommandType = CommandType.Text;

                comm.Parameters.Add(new("@Name", spiel.Name));
                comm.Parameters.Add(new("@Beschreibung", spiel.Beschreibung));

                comm.Parameters.Add(new("@Bildlink", bild));
                comm.Parameters.Add(new("@Exepfad", spiel.Exepfad));

                comm.Parameters.Add(new("@Metacritic", _metacriticvalue));
                comm.Parameters.Add(new("@Estimatedprice", _estimatedprice));

                comm.Parameters.Add(new("@Id", spiel.Id));
                await comm.ExecuteNonQueryAsync();
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

        public async Task SaveGenreOfGame(int spielid, IEnumerable<Genre> genreIds)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                foreach (Genre gr in genreIds)
                {
                    using SQLiteCommand comm = con.CreateCommand();
                    comm.CommandText = "INSERT INTO spielgenre (spielid,genreid) VALUES(@Spielid,@Genreid)";

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add(new("@Spielid", spielid));

                    comm.Parameters.Add(new("@Genreid", gr.Id));
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ErstelleNutzer(string nickname, int passcode)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    int RandomId = new Random().Next(99, 99999);
                    comm.CommandText = "INSERT INTO nutzer (id, name,pincode) VALUES (@id, @name,@pincode)";

                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add(new("@id", RandomId));

                    comm.Parameters.Add(new("@name", nickname));
                    comm.Parameters.Add(new("@pincode", passcode));

                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public Tuple<LibraryUser, UserCase> ErhalteNutzer(string nickname, int passcode)
        {
            Tuple<LibraryUser, UserCase> User = null;
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "SELECT id FROM nutzer WHERE name=@name AND pincode=@pincode;";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@name", nickname));
                    comm.Parameters.Add(new("@pincode", passcode));

                    SQLiteDataReader r = comm.ExecuteReader();
                    int ResultCounter = 0;

                    int UserId = 0;
                    while (r.Read())
                    {
                        ResultCounter++;
                        UserId = r.GetInt32(0);
                    }

                    LibraryUser user = null;
                    UserCase usercase;

                    if (ResultCounter == 0)
                    {
                        //Erstellbar
                        user = new(UserId, nickname, passcode);
                        usercase = UserCase.Createable;
                    }
                    else if (ResultCounter == 1)
                    {
                        //Login
                        user = new(UserId, nickname, passcode);
                        usercase = UserCase.Exists;
                    }
                    else
                    {
                        //Fehler
                        user = new(UserId, nickname, passcode);
                        usercase = UserCase.Error;
                    }

                    User = new Tuple<LibraryUser, UserCase>(user, usercase);
                }
            }
            return User;
        }

        public async Task<bool> SpielGenreCheck(IEnumerable<Genre> genreids, LibraryUser nutzer)
        {
            bool spielNutztGenre = false;
            List<Spiel> AktuelleSpieleListe = await SpieleListeErhalten(nutzer);

            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                foreach (Spiel spiel in AktuelleSpieleListe)
                {
                    foreach (Genre gr in genreids)
                    {
                        using (SQLiteCommand comm = con.CreateCommand())
                        {
                            comm.CommandText = "SELECT COUNT(*) FROM spielgenre WHERE spielid=@spielid AND genreid=@genreid;";
                            comm.CommandType = CommandType.Text;

                            comm.Parameters.Add(new("@spielid", spiel.Id));
                            comm.Parameters.Add(new("@genreid", gr.Id));

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

        private async Task DeleteUser(LibraryUser nutzer)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM nutzer WHERE id=@Id";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@Id", nutzer.Id));
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task DeleteAllUserrelatedGames(LibraryUser nutzer)
        {
            List<Spiel> UserGames = await SpieleListeErhalten(nutzer);
            foreach (Spiel sp in UserGames)
            {
                await RemoveAllGenreOfGame(sp.Id);
                await DeleteSpielCompletely(sp);
            }

            Genre[] UserGenres = await AlleGenreErhalten(nutzer);
            foreach (Genre gr in UserGenres)
            {
                await DeleteGenre(gr.Id);
            }
        }

        public async Task DeleteLibraryUser(LibraryUser nutzer)
        {
            await DeleteUser(nutzer);
            await DeleteAllUserrelatedGames(nutzer);
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

        public async Task DeleteGenre(int genreid)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "DELETE FROM genre WHERE id=@Id";
                comm.CommandType = CommandType.Text;

                comm.Parameters.Add(new("@Id", genreid));
                await comm.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteGenreInDatabase(IEnumerable<Genre> genreids)
        {
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                foreach (Genre gr in genreids)
                {
                    using (SQLiteCommand comm = con.CreateCommand())
                    {
                        comm.CommandText = "DELETE FROM genre WHERE id=@Id";
                        comm.CommandType = CommandType.Text;

                        comm.Parameters.Add(new("@Id", gr.Id));
                        await comm.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task CreateGenreInDatabase(string name, LibraryUser nutzer)
        {
            int genreid = Manager.RandomGenreNummer();
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "INSERT INTO genre (id, owner, name) VALUES (@genreid, @Owner, @genrename)";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@genreid", genreid));
                    comm.Parameters.Add(new("@Owner", nutzer.Id));

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
            Genre SearchedGenre = new(genreid, 0, string.Empty);
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "SELECT name, owner FROM genre WHERE id=@Id";
                    comm.Parameters.Add(new("@Id", SearchedGenre.Id));

                    SQLiteDataReader r = comm.ExecuteReader();
                    while (r.Read())
                    {
                        string name = (string)r["name"];
                        int owner = (int)r["owner"];

                        SearchedGenre.Name = name;
                        SearchedGenre.Owner = owner;
                    }
                }
            }
            return SearchedGenre;
        }

        public Task<Genre[]> AlleGenreErhalten(LibraryUser nutzer)
        {
            List<Genre> genreList = new();
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "SELECT id, owner, name FROM genre";
                    SQLiteDataReader r = comm.ExecuteReader();

                    while (r.Read())
                    {
                        int id = (int)r["id"];
                        int owner = (int)r["owner"];

                        string name = (string)r["name"];
                        if (owner == nutzer.Id)
                        {
                            genreList.Add(new Genre(id, owner, name));
                        }
                    }
                }
            }
            return Task.FromResult(genreList.ToArray());
        }

        public string[] GenreVonSpielErhalten(int spielid)
        {
            List<string> genreList = new();
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
                using SQLiteCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT genre.id, genre.owner, genre.name FROM spielgenre JOIN spiele ON spielgenre.spielid = spiele.id JOIN genre ON spielgenre.genreid = genre.id WHERE spiele.id=@spielid";
                comm.Parameters.Add(new("@spielid", spielid));

                SQLiteDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    int id = (int)r["id"];
                    int owner = (int)r["owner"];

                    string name = (string)r["name"];
                    genreList.Add(new Genre(id, owner, name));
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
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice FROM spiele WHERE id=@SpielId";
                fmd.Parameters.Add(new("@spielid", SpielId));

                SQLiteDataReader r = fmd.ExecuteReader();
                while (r.Read())
                {
                    int id = (int)r["id"];
                    int owner = (int)r["owner"];

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
                    spiel = new(id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, papierkorb, metacritic, estimatedprice, genrelist);
                }
            }
            return Task.FromResult(spiel);
        }

        public async Task<List<Spiel>> SpieleListeErhalten(LibraryUser nutzer)
        {
            List<Spiel> spiele = new();
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne, papierkorb, metacritic, estimatedprice FROM spiele";
                SQLiteDataReader r = fmd.ExecuteReader();

                while (r.Read())
                {
                    int id = (int)r["id"];
                    int owner = (int)r["owner"];

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

                    if (owner == nutzer.Id)
                    {
                        spiele.Add(new Spiel(id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, papierkorb, metacritic, estimatedprice, genrelist));
                    }
                }
            }
            return spiele;
        }

        public async Task<List<Spiel>> GeloeschteSpieleListeErhalten(LibraryUser nutzer)
        {
            List<Spiel> spiele = new();
            using (SQLiteConnection con = HoleConnnection())
            {
                con.Open();
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, favorit, sterne,  metacritic, estimatedprice FROM spiele WHERE papierkorb=1";
                SQLiteDataReader r = fmd.ExecuteReader();

                while (r.Read())
                {
                    int id = (int)r["id"];
                    int owner = (int)r["owner"];

                    string name = (string)r["name"];
                    string beschreibung = (string)r["beschreibung"];

                    string bildlink = (string)r["bildlink"];
                    string exepfad = (string)r["exepfad"];

                    string sternetooltip = (string)r["sternetooltip"];
                    int fav = (int)r["favorit"];

                    int sterne = (int)r["sterne"];

                    string metacritic = (string)r["metacritic"];
                    string estimatedprice = (string)r["estimatedprice"];

                    Genre[] genrelist = GenreArrayVonSpielErhalten(id);

                    if (owner == nutzer.Id)
                    {
                        spiele.Add(new Spiel(id, owner, name, beschreibung, bildlink, exepfad, sternetooltip, fav, sterne, 1, metacritic, estimatedprice, genrelist));
                    }
                }
            }
            return spiele;
        }

        public static async Task ExecuteQueryAsync(string cmd)
        {
            using SQLiteConnection con = HoleConnnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = cmd;

            await comm.ExecuteNonQueryAsync();
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