using System.Data;
using System.Data.SQLite;

using BlazorLibrary.Models;

using BlazorLibrary.Management;
using BlazorLibrary.Models.User;

namespace BlazorLibrary.Data
{
    public class SqliteDatabaseManager
    {
        #region Utils

        public static void SetupDatabase()
        {
            ExecuteQuery("CREATE TABLE IF NOT EXISTS games (id INT, owner INT, name TEXT, description TEXT, imageurl TEXT, executablepath TEXT, reviewtext TEXT,isfavourite INT, reviewstars INT, istrash INT, metacritic TEXT, roughprice TEXT, PRIMARY KEY(id))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS genre (id INT, owner INT, name TEXT, PRIMARY KEY(id))");

            ExecuteQuery("CREATE TABLE IF NOT EXISTS gamegenre (gameid INT, genreid INT, PRIMARY KEY (gameid, genreid))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS libraryuser (id INT, name TEXT, pincode INT, PRIMARY KEY (id))");

            ExecuteQuery("CREATE TABLE IF NOT EXISTS gamecomment (gameid INT, commentid INT, PRIMARY KEY (gameid, commentid))");
            ExecuteQuery("CREATE TABLE IF NOT EXISTS comments (id INT, textcomment TEXT, PRIMARY KEY (id))");
        }

        private static SQLiteConnection GetConnection()
        {
            return new($"Data Source={Path.Combine(Manager.GetExecutionDirectory(), MauiProgram.Settings.Databasename)}.sqlite;Version=3;");
        }

        private static void ExecuteQuery(string Command)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = Command;

            comm.ExecuteNonQuery();
        }

        public static async Task ExecuteQueryAsync(string Command)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = Command;

            await comm.ExecuteNonQueryAsync();
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        #endregion Utils

        #region Csv

        public async Task InsertCsvEntries(List<dynamic> ElementList, LibraryUser User)
        {
            foreach (dynamic CsvEl in ElementList)
            {
                if (CsvEl == typeof(LibraryGenre))
                {
                    await InsertGenreEntry(((LibraryGenre)CsvEl).Name, User);
                }
                else
                {
                    await InsertNewGame(((LibraryGame)CsvEl).Name, User, ((LibraryGame)CsvEl).Description, ((LibraryGame)CsvEl).ImageUrl, ((LibraryGame)CsvEl).ExecutablePath, string.Empty, 0, 0, 0, string.Empty, string.Empty);
                }
            }
        }

        #endregion Csv

        #region Comments

        public async Task<(string, bool, int)> GetComment(int GameId)
        {
            int CommentId = await GetCommentIdOfCommentedGame(GameId);

            if (CommentId != 0)
            {
                string CommentText = await GetCommentTextByCommentId(CommentId);
                bool Commented = true;

                if (string.IsNullOrEmpty(CommentText) || string.IsNullOrWhiteSpace(CommentText))
                {
                    Commented = false;
                }
                return (CommentText, Commented, CommentId);
            }
            return (string.Empty, false, 0);
        }

        public async Task<int> SaveComment(int GameId, string Comment)
        {
            int CommentId = await SaveCommentEntry(Comment);
            await SaveGameCommentEntry(GameId, CommentId);

            return CommentId;
        }

        public async Task UpdateCommentEntry((string Text, bool Gesetzt, int Id) Comment)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "UPDATE comments SET textcomment=@CommentText WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@CommentText", Comment.Text));

            comm.Parameters.Add(new("@Id", Comment.Id));
            await comm.ExecuteNonQueryAsync();
        }

        public async Task DeleteComment(int GameId, int CommentId)
        {
            await DeleteCommentEntry(CommentId);
            await DeleteGameCommentEntry(GameId, CommentId);
        }

        private async Task DeleteCommentEntry(int CommentId)
        {
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM comments WHERE id=@Id";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@Id", CommentId));
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task DeleteGameCommentEntry(int GameId, int CommentId)
        {
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using (SQLiteCommand comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM gamecomment WHERE gameid=@GameId AND commentid=@CommentId";
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.Add(new("@GameId", GameId));
                    comm.Parameters.Add(new("@CommentId", CommentId));

                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task SaveGameCommentEntry(int GameId, int CommentId)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "INSERT INTO gamecomment (gameid,commentid) VALUES(@GameId,@CommentId)";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@GameId", GameId));

            comm.Parameters.Add(new("@CommentId", CommentId));
            await comm.ExecuteNonQueryAsync();
        }

        private async Task<int> SaveCommentEntry(string CommentText)
        {
            int GenId = Manager.RandomNumber();
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "INSERT INTO comments (id, textcomment) VALUES(@Id, @CommentText)";
                comm.CommandType = CommandType.Text;

                comm.Parameters.Add(new("@Id", GenId));
                comm.Parameters.Add(new("@CommentText", CommentText));

                await comm.ExecuteNonQueryAsync();
            }
            return GenId;
        }

        private async Task<int> GetCommentIdOfCommentedGame(int GameId)
        {
            int CommentId = 0;
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "SELECT commentid FROM gamecomment WHERE gameid=@GameId";
                comm.Parameters.Add(new("@GameId", GameId));

                SQLiteDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    int KommentarId = (int)r["commentid"];
                    CommentId = KommentarId;
                }
            }
            return CommentId;
        }

        private async Task<string> GetCommentTextByCommentId(int CommentId)
        {
            string CommentText = string.Empty;
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "SELECT textcomment FROM comments WHERE id=@Id";
                comm.Parameters.Add(new("@Id", CommentId));

                SQLiteDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    CommentText = (string)r["textcomment"];
                }
            }
            return CommentText;
        }

        #endregion Comments

        #region Game

        public async Task ChangeDeletionStateOfGame(LibraryGame GameToUpdate, bool NewState)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            int NewTrashcanState = 0;

            if (NewState)
            {
                NewTrashcanState = 1;
            }

            comm.CommandText = "UPDATE games SET istrash=@IsTrash WHERE id=@Id";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Add(new("@IsTrash", NewTrashcanState));
            comm.Parameters.Add(new("@Id", GameToUpdate.Id));

            await comm.ExecuteNonQueryAsync();
        }

        public async Task DeleteGame(LibraryGame GameToUpdate)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "DELETE FROM games WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@Id", GameToUpdate.Id));

            await comm.ExecuteNonQueryAsync();
        }

        public async Task<int> InsertNewGame(string Name, LibraryUser User, string Description = "", string Imageurl = "", string Executablepath = "", string ReviewText = "", int IsFavourite = 0, int ReviewStars = 0, int IsTrash = 0, string Metacritic = "", string RoughPrice = "")
        {
            (string _description, string _executablepath) = Manager.ToBeSureRemoveNullValues(Description, Executablepath);

            Description = _description;
            Executablepath = _executablepath;

            (string _imageurl, string _metacritic, string _roughprice) = await Manager.AddExternalGamecardSources(Name);
            int spieldbid = Manager.RandomNumber();

            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "INSERT INTO games (id, owner, name, description, imageurl, executablepath, reviewtext, isfavourite, reviewstars, istrash, metacritic, roughprice) VALUES(@Id, @Owner, @Name, @Description, @Imageurl, @ExecutablePath, @ReviewText, @IsFavourite, @ReviewStars, @IsTrash, @Metacritic, @RoughPrice)";
                comm.CommandType = CommandType.Text;

                comm.Parameters.Add(new("@Id", spieldbid));
                comm.Parameters.Add(new("@Owner", User.Id));

                comm.Parameters.Add(new("@Name", Name));
                comm.Parameters.Add(new("@Description", Description));

                comm.Parameters.Add(new("@Imageurl", _imageurl));
                comm.Parameters.Add(new("@ExecutablePath", Executablepath));

                comm.Parameters.Add(new("@ReviewText", ReviewText));
                comm.Parameters.Add(new("@IsFavourite", IsFavourite));

                comm.Parameters.Add(new("@ReviewStars", ReviewStars));
                comm.Parameters.Add(new("@IsTrash", IsTrash));

                comm.Parameters.Add(new("@Metacritic", _metacritic));
                comm.Parameters.Add(new("@RoughPrice", _roughprice));

                await comm.ExecuteNonQueryAsync();
            }
            return spieldbid;
        }

        public async Task UpdateGame(LibraryGame GameToUpdate)
        {
            string imageurl = GameToUpdate.ImageUrl;
            (string _imageurl, string _metacritic, string _roughprice) = await Manager.AddExternalGamecardSources(GameToUpdate.Name, imageurl);

            imageurl = _imageurl;
            using SQLiteConnection con = GetConnection();

            con.Open();
            using SQLiteCommand comm = con.CreateCommand();

            comm.CommandText = "UPDATE games SET name=@Name, description=@Description, imageurl=@Imageurl, executablepath=@ExecutablePath, metacritic=@Metacritic, roughprice=@RoughPrice WHERE id=@Id";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Add(new("@Name", GameToUpdate.Name));
            comm.Parameters.Add(new("@Description", GameToUpdate.Description));

            comm.Parameters.Add(new("@Imageurl", imageurl));
            comm.Parameters.Add(new("@ExecutablePath", GameToUpdate.ExecutablePath));

            comm.Parameters.Add(new("@Metacritic", _metacritic));
            comm.Parameters.Add(new("@RoughPrice", _roughprice));

            comm.Parameters.Add(new("@Id", GameToUpdate.Id));
            await comm.ExecuteNonQueryAsync();
        }

        public async Task UpdateGameReview(LibraryGame GameToUpdate)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "UPDATE games SET reviewstars=@ReviewStars, reviewtext=@ReviewText WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@ReviewStars", GameToUpdate.ReviewStars));

            comm.Parameters.Add(new("@ReviewText", GameToUpdate.ReviewText));
            comm.Parameters.Add(new("@Id", GameToUpdate.Id));

            await comm.ExecuteNonQueryAsync();
        }

        public async Task ChangeFavouriteStateOfGame(int GameId, bool IsFavourite)
        {
            int IsFavouriteValue = 0;
            if (IsFavourite)
            {
                IsFavouriteValue = 1;
            }

            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "UPDATE games SET isfavourite=@IsFav WHERE id=@GameId";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@IsFav", IsFavouriteValue));

            comm.Parameters.Add(new("@GameId", GameId));
            await comm.ExecuteNonQueryAsync();
        }

        public Task<LibraryGame> GetGameById(int GameId)
        {
            LibraryGame GameToReturn = null;
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, description, imageurl, executablepath, reviewtext, isfavourite, reviewstars, istrash, metacritic, roughprice FROM games WHERE id=@GameId";
                fmd.Parameters.Add(new("@GameId", GameId));

                SQLiteDataReader r = fmd.ExecuteReader();
                while (r.Read())
                {
                    int Id = (int)r["id"];
                    int Owner = (int)r["owner"];

                    string Name = (string)r["name"];
                    string Description = (string)r["description"];

                    string ImageUrl = (string)r["imageurl"];
                    string ExecutablePath = (string)r["executablepath"];

                    string ReviewText = (string)r["reviewtext"];
                    int IsFavourite = (int)r["isfavourite"];

                    int ReviewStars = (int)r["reviewstars"];
                    int IsTrash = (int)r["istrash"];

                    string Metacritic = (string)r["metacritic"];
                    string RoughPrice = (string)r["roughprice"];

                    LibraryGenre[] GamesGenrelist = GetGenresFromGame(Id);
                    GameToReturn = new(Id, Owner, Name, Description, ImageUrl, ExecutablePath, ReviewText, IsFavourite, ReviewStars, IsTrash, Metacritic, RoughPrice, genrelist: GamesGenrelist);
                }
            }
            return Task.FromResult(GameToReturn);
        }

        public async Task<List<LibraryGame>> GetLibraryUsersGamelist(LibraryUser User)
        {
            List<LibraryGame> Gamelist = new();
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, description, imageurl, executablepath, reviewtext, isfavourite, reviewstars, istrash, metacritic, roughprice FROM games";
                SQLiteDataReader r = fmd.ExecuteReader();

                while (r.Read())
                {
                    int Id = (int)r["id"];
                    int Owner = (int)r["owner"];

                    string Name = (string)r["name"];
                    string Description = (string)r["description"];

                    string ImageUrl = (string)r["imageurl"];
                    string ExecutablePath = (string)r["executablepath"];

                    string ReviewText = (string)r["reviewtext"];
                    int IsFavourite = (int)r["isfavourite"];

                    int ReviewStars = (int)r["reviewstars"];
                    int IsTrash = (int)r["istrash"];

                    string Metacritic = (string)r["metacritic"];
                    string RoughPrice = (string)r["roughprice"];

                    LibraryGenre[] GamesGenrelist = GetGenresFromGame(Id);
                    if (Owner == User.Id)
                    {
                        Gamelist.Add(new LibraryGame(Id, Owner, Name, Description, ImageUrl, ExecutablePath, ReviewText, IsFavourite, ReviewStars, IsTrash, Metacritic, RoughPrice, genrelist: GamesGenrelist));
                    }
                }
            }

            if (Gamelist.Count > 0)
            {
                foreach (LibraryGame g in Gamelist)
                {
                    g.Comment = await GetComment(g.Id);
                }
            }

            return Gamelist;
        }

        public async Task<List<LibraryGame>> GetLibraryUsersDeletedGamelist(LibraryUser User)
        {
            List<LibraryGame> DeletedGamelist = new();
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand fmd = con.CreateCommand();

                fmd.CommandText = "SELECT id, owner, name, description, imageurl, executablepath, reviewtext, isfavourite, reviewstars, istrash, metacritic, roughprice FROM games WHERE istrash=1";
                SQLiteDataReader r = fmd.ExecuteReader();

                while (r.Read())
                {
                    int Id = (int)r["id"];
                    int Owner = (int)r["owner"];

                    string Name = (string)r["name"];
                    string Description = (string)r["description"];

                    string ImageUrl = (string)r["imageurl"];
                    string ExecutablePath = (string)r["executablepath"];

                    string ReviewText = (string)r["reviewtext"];
                    int IsFavourite = (int)r["isfavourite"];

                    int ReviewStars = (int)r["reviewstars"];
                    int IsTrash = (int)r["istrash"];

                    string Metacritic = (string)r["metacritic"];
                    string RoughPrice = (string)r["roughprice"];

                    LibraryGenre[] GamesGenrelist = GetGenresFromGame(Id);
                    if (Owner == User.Id)
                    {
                        DeletedGamelist.Add(new LibraryGame(Id, Owner, Name, Description, ImageUrl, ExecutablePath, ReviewText, IsFavourite, ReviewStars, IsTrash, Metacritic, RoughPrice, genrelist: GamesGenrelist));
                    }
                }
            }
            return DeletedGamelist;
        }

        #endregion Game

        #region User

        public async Task CreateLibraryUser(string Nick, int Pass)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            int RandomId = Manager.RandomNumber();

            comm.CommandText = "INSERT INTO libraryuser (id, name,pincode) VALUES (@Id, @Name,@Pincode)";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Add(new("@Id", RandomId));
            comm.Parameters.Add(new("@Name", Nick));

            comm.Parameters.Add(new("@Pincode", Pass));
            await comm.ExecuteNonQueryAsync();
        }

        private async Task DeleteLibraryUserEntry(LibraryUser User)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "DELETE FROM libraryuser WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@Id", User.Id));

            await comm.ExecuteNonQueryAsync();
        }

        private async Task<List<LibraryGame>> DeleteLibraryUserRelatedGames(LibraryUser User)
        {
            List<LibraryGame> PersonalGames = await GetLibraryUsersGamelist(User);
            foreach (LibraryGame sp in PersonalGames)
            {
                await ClearGenresOfGame(sp.Id);
                await DeleteGame(sp);
            }

            LibraryGenre[] UserGenres = await GetUserrelatedGenreEntries(User);
            foreach (LibraryGenre gr in UserGenres)
            {
                await DeleteGenreEntry(gr.Id);
            }
            return PersonalGames.Where(x => x.Comment.Commented).ToList();
        }

        public async Task DeleteLibraryUser(LibraryUser User)
        {
            await DeleteLibraryUserEntry(User);
            List<LibraryGame> AllCommentedGames = await DeleteLibraryUserRelatedGames(User);

            foreach (LibraryGame sp in AllCommentedGames)
            {
                await DeleteComment(sp.Id, sp.Comment.Id);
            }
        }

        public Tuple<LibraryUser, LibraryUserAccountState> GetLibraryUserEntry(string Nick, int Pass)
        {
            Tuple<LibraryUser, LibraryUserAccountState> UserAndState = null;
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "SELECT id FROM libraryuser WHERE name=@Name AND pincode=@Pincode;";
                comm.CommandType = CommandType.Text;

                comm.Parameters.Add(new("@Name", Nick));
                comm.Parameters.Add(new("@Pincode", Pass));

                SQLiteDataReader r = comm.ExecuteReader();
                int ResultCounter = 0;

                int UserId = 0;
                while (r.Read())
                {
                    ResultCounter++;
                    UserId = r.GetInt32(0);
                }

                LibraryUser User = null;
                LibraryUserAccountState AccountState;

                if (ResultCounter == 0)
                {
                    User = new(UserId, Nick, Pass);
                    AccountState = LibraryUserAccountState.UserCanBeCreated;
                }
                else if (ResultCounter == 1)
                {
                    User = new(UserId, Nick, Pass);
                    AccountState = LibraryUserAccountState.UserAlreadyExists;
                }
                else
                {
                    User = new(UserId, Nick, Pass);
                    AccountState = LibraryUserAccountState.Error;
                }

                UserAndState = new Tuple<LibraryUser, LibraryUserAccountState>(User, AccountState);
            }
            return UserAndState;
        }

        #endregion User

        #region Genre

        public async Task RenameGenreEntry(LibraryGenre GenreToUpdate)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "UPDATE genre SET name=@Name WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@Name", GenreToUpdate.Name));

            comm.Parameters.Add(new("@Id", GenreToUpdate.Id));
            await comm.ExecuteNonQueryAsync();
        }

        public LibraryGenre GetGenreEntryById(int GenreId)
        {
            LibraryGenre GenreToReturn = new(GenreId, 0, string.Empty);
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "SELECT name, owner FROM genre WHERE id=@Id";
                comm.Parameters.Add(new("@Id", GenreToReturn.Id));

                SQLiteDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    string name = (string)r["name"];
                    int owner = (int)r["owner"];

                    GenreToReturn.Name = name;
                    GenreToReturn.LibraryUserId = owner;
                }
            }
            return GenreToReturn;
        }

        public Task<LibraryGenre[]> GetUserrelatedGenreEntries(LibraryUser User)
        {
            List<LibraryGenre> Genrelist = new();
            using (SQLiteConnection con = GetConnection())
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
                        if (owner == User.Id)
                        {
                            Genrelist.Add(new LibraryGenre(id, owner, name));
                        }
                    }
                }
            }
            return Task.FromResult(Genrelist.ToArray());
        }

        public LibraryGenre[] GetGenresFromGame(int GameId)
        {
            List<LibraryGenre> Genrelist = new();
            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                using SQLiteCommand comm = con.CreateCommand();

                comm.CommandText = "SELECT genre.id, genre.owner, genre.name FROM gamegenre JOIN games ON gamegenre.gameid = games.id JOIN genre ON gamegenre.genreid = genre.id WHERE games.id=@GameId";
                comm.Parameters.Add(new("@GameId", GameId));

                SQLiteDataReader r = comm.ExecuteReader();
                while (r.Read())
                {
                    int id = (int)r["id"];
                    int owner = (int)r["owner"];

                    string name = (string)r["name"];
                    Genrelist.Add(new LibraryGenre(id, owner, name));
                }
            }
            return Genrelist.ToArray();
        }

        /// <summary>
        /// Replace by more efficient approach, less foreach and SqlCommand placed better
        /// </summary>
        public async Task<bool> AreGenresUsedByGames(IEnumerable<LibraryGenre> Genrelist, LibraryUser User)
        {
            bool GameIsUsingGenres = false;
            List<LibraryGame> Gamelist = await GetLibraryUsersGamelist(User);

            using (SQLiteConnection con = GetConnection())
            {
                con.Open();
                foreach (LibraryGame spiel in Gamelist)
                {
                    foreach (LibraryGenre gr in Genrelist)
                    {
                        using SQLiteCommand comm = con.CreateCommand();
                        comm.CommandText = "SELECT COUNT(*) FROM gamegenre WHERE gameid=@GameId AND genreid=@GenreId;";

                        comm.CommandType = CommandType.Text;
                        comm.Parameters.Add(new("@GameId", spiel.Id));

                        comm.Parameters.Add(new("@GenreId", gr.Id));
                        SQLiteDataReader r = comm.ExecuteReader();

                        while (r.Read())
                        {
                            int Count = r.GetInt32(0);
                            if (Count > 0)
                            {
                                GameIsUsingGenres = true;
                            }
                        }
                    }
                }
            }
            return GameIsUsingGenres;
        }

        public async Task SaveGenresOfGame(int GaemId, IEnumerable<LibraryGenre> Genrelist)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            foreach (LibraryGenre GenreToAdd in Genrelist)
            {
                using SQLiteCommand comm = con.CreateCommand();
                comm.CommandText = "INSERT INTO gamegenre (gameid,genreid) VALUES(@GameId,@GenreId)";

                comm.CommandType = CommandType.Text;
                comm.Parameters.Add(new("@GameId", GaemId));

                comm.Parameters.Add(new("@GenreId", GenreToAdd.Id));
                await comm.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteGenreEntry(int GenreId)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "DELETE FROM genre WHERE id=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@Id", GenreId));

            await comm.ExecuteNonQueryAsync();
        }

        public async Task DeleteGenreInDatabase(IEnumerable<LibraryGenre> Genrelist)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            foreach (LibraryGenre gr in Genrelist)
            {
                using SQLiteCommand comm = con.CreateCommand();
                comm.CommandText = "DELETE FROM genre WHERE id=@Id";

                comm.CommandType = CommandType.Text;
                comm.Parameters.Add(new("@Id", gr.Id));

                await comm.ExecuteNonQueryAsync();
            }
        }

        public async Task InsertGenreEntry(string Name, LibraryUser User)
        {
            int NewGenreId = Manager.RandomNumber();
            using SQLiteConnection con = GetConnection();

            con.Open();
            using SQLiteCommand comm = con.CreateCommand();

            comm.CommandText = "INSERT INTO genre (id, owner, name) VALUES (@GenreId, @Owner, @GenreName)";
            comm.CommandType = CommandType.Text;

            comm.Parameters.Add(new("@GenreId", NewGenreId));
            comm.Parameters.Add(new("@Owner", User.Id));

            comm.Parameters.Add(new("@GenreName", Name));
            await comm.ExecuteNonQueryAsync();
        }

        public async Task ClearGenresOfGame(int GameId)
        {
            using SQLiteConnection con = GetConnection();
            con.Open();

            using SQLiteCommand comm = con.CreateCommand();
            comm.CommandText = "DELETE FROM gamegenre WHERE gameid=@Id";

            comm.CommandType = CommandType.Text;
            comm.Parameters.Add(new("@Id", GameId));

            await comm.ExecuteNonQueryAsync();
        }

        #endregion Genre
    }
}