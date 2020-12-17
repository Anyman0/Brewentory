using Brewentory.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brewentory.Data
{
    public class LoginDatabase
    {
        readonly SQLiteAsyncConnection lDatabase;

        public LoginDatabase(string dbPath)
        {
            lDatabase = new SQLiteAsyncConnection(dbPath);
            lDatabase.CreateTableAsync<LoginModel>().Wait();
        }

        public Task<List<LoginModel>> GetData()
        {

            return lDatabase.Table<LoginModel>().ToListAsync();

        }


        public Task<int> SaveItemAsync(LoginModel model)
        {

            return lDatabase.InsertAsync(model);

        }

        public Task<int> DeleteItemAsync(LoginModel model)
        {
            //return lDatabase.DropTableAsync<LoginDBModel>();
            return lDatabase.DeleteAllAsync<LoginModel>();
        }
    }
}
