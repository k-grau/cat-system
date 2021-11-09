using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace CatApi.Models
{
    public class DataHandler
    {


        // private string connectionString = "Server=localhost;Database=CatDB;User ID=SA;Password=hashTAGplugga?1;MultipleActiveResultSets=true; ApplicationIntent=ReadWrite";
        private string connectionString = "";

        public DataHandler(IConfiguration configuration) {
            connectionString = configuration.GetValue<string>("ConnectionStrings:DbConnectionString");
        }

       public List<CatDetails> GetCats(out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();
           
            
            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT Katt.Id, Katt.Namn, Katt.Fodd, Katt.Farg, Katt.Sort FROM Katt;";
           


            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

           
            SqlDataReader reader = null;

            List<CatDetails> CatList = new List<CatDetails>();
            errormsg = "";

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    CatDetails Cat = new CatDetails();
                    Cat.Namn = reader["Namn"].ToString();
                    Cat.Fodd = Convert.ToInt16(reader["Fodd"]);
                    Cat.Farg = reader["Farg"].ToString();
                    Cat.Sort = reader["Sort"].ToString();
                    Cat.Id = Convert.ToInt16(reader["Id"]);

                    CatList.Add(Cat);
                }
                reader.Close();
                return CatList;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
        

        public Lifestyle GetCatLifestyle(out string errormsg, int id)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT Levnadsform.Id, Levnadsform.Liv" 
            + " FROM Levnadsform, Katt WHERE Katt.Lever_som = Levnadsform.Id AND Katt.Id = @id;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            SqlDataReader reader = null;

            Lifestyle ls = new Lifestyle();
            errormsg = "";

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    ls.Livsstil_Id = Convert.ToInt16(reader["Id"]);
                    ls.Beskrivning = reader["Liv"].ToString();
                    
                }
                reader.Close();
                return ls;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }




    public CatDetails GetCat(int id, out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;

            String sqlstring = "SELECT Katt.Id, Katt.Namn, Katt.Fodd, Katt.Farg, Katt.Sort, Levnadsform.Liv " 
            + "FROM Katt INNER JOIN Levnadsform on Katt.Lever_som = Levnadsform.Id WHERE Katt.Id = @id;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            SqlDataReader reader = null;
            errormsg = "";
            CatDetails Cat = new CatDetails();


            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    
                    Cat.Namn = reader["Namn"].ToString();
                    Cat.Fodd = Convert.ToInt16(reader["Fodd"]);
                    Cat.Farg = reader["Farg"].ToString();
                    Cat.Sort = reader["Sort"].ToString();
                    Cat.Id = Convert.ToInt16(reader["Id"]);

                }
                reader.Close();
                if(Cat.Id < 1) 
                {
                    errormsg = "No such post found";
                    return null;
                }
                return Cat;
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }



        public int EditCat(CatDetails cd, int id, out string errormsg)
        {
            if(cd.Id != id) {
                errormsg = "Entry did not match route. Database was not updated.";
                return 0;
            }

            SqlConnection dbConnection = new SqlConnection();
            string sqlstring = "";

            dbConnection.ConnectionString = connectionString;

            sqlstring = "UPDATE Katt SET Namn=@namn, Fodd=@fodd, Farg=@farg, Sort=@sort, Lever_som=@lever_som WHERE Id=@id";
            
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = cd.Id;
            dbCommand.Parameters.Add("namn", SqlDbType.VarChar, 30).Value = cd.Namn;
            dbCommand.Parameters.Add("fodd", SqlDbType.Int).Value = cd.Fodd;
            dbCommand.Parameters.Add("farg", SqlDbType.VarChar, 30).Value = cd.Farg;
            dbCommand.Parameters.Add("sort", SqlDbType.VarChar, 30).Value = cd.Sort;
            dbCommand.Parameters.Add("lever_som", SqlDbType.Int).Value = Convert.ToInt32(cd.Livsstil.Livsstil_Id);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed: could not save to database";}
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }


         public int DeleteCat(int id, out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();
            string sqlstring = "";

            dbConnection.ConnectionString = connectionString;
            if (id > 0)
            {
                sqlstring = "DELETE FROM Katt WHERE Id=@id";
            }
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Could not delete from database."; }
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }


     public List<Habits> GetHabits(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT * FROM Ovana";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            SqlDataReader reader = null;
            List<Habits> HabitList = new List<Habits>();
            errormsg = "";

            try
            {
                dbConnection.Open();
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                   Habits ch = new Habits();
                   ch.Id = Convert.ToInt16(reader["Id"]);
                    ch.Beteende = reader["Beteende"].ToString();
                    

                    HabitList.Add(ch);
                }
                reader.Close();
                return HabitList;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }



        public List<CatHabits> GetCatHabits(out string errormsg, int id)
        {
            SqlConnection dbConnection = new SqlConnection();


            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT Ovana.Beteende, Ovana.Id " +
                             "FROM((Katt INNER JOIN HarOvana ON Katt.Id = HarOvana.Katt)" +
                             "INNER JOIN Ovana ON HarOvana.Ovana = Ovana.Id) WHERE Katt.Id = @id";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("id", SqlDbType.Int).Value = id;

            SqlDataReader reader = null;

            List<CatHabits> CatHabitList = new List<CatHabits>();
            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();



                while (reader.Read())
                {
                    CatHabits ch = new CatHabits();
                    ch.Ovana_Id = Convert.ToInt16(reader["Id"]);
                    // ch.Namn = reader["Namn"].ToString();
                    // ch.Sort = reader["Sort"].ToString();
                    ch.Beteende = reader["Beteende"].ToString();


                    CatHabitList.Add(ch);
                }
                reader.Close();
                return CatHabitList;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }


         public List<Lifestyle> GetLifestyle(out string errormsg)
        {
            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "SELECT * FROM Levnadsform";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);



            SqlDataReader reader = null;


            List<Lifestyle> LifeStyleList = new List<Lifestyle>();
            errormsg = "";

            try
            {
                dbConnection.Open();

                reader = dbCommand.ExecuteReader();



                while (reader.Read())
                {
                    Lifestyle ls = new Lifestyle();
                    ls.Livsstil_Id = Convert.ToInt16(reader["Id"]);
                    ls.Beskrivning = reader["Liv"].ToString();

                    LifeStyleList.Add(ls);
                }
                reader.Close();
                return LifeStyleList;

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }


        public int InsertCat(CatDetails cd, out string errormsg)
        {

            SqlConnection dbConnection = new SqlConnection();

            dbConnection.ConnectionString = connectionString;
            String sqlstring = "INSERT INTO Katt (Namn, Fodd, Farg, Sort, Lever_som) VALUES(@namn,@fodd,@farg,@sort,@lever_som)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("namn", SqlDbType.VarChar, 30).Value = cd.Namn;
            dbCommand.Parameters.Add("fodd", SqlDbType.Int).Value = cd.Fodd;
            dbCommand.Parameters.Add("farg", SqlDbType.VarChar, 30).Value = cd.Farg;
            dbCommand.Parameters.Add("sort", SqlDbType.VarChar, 30).Value = cd.Sort;
            dbCommand.Parameters.Add("lever_som", SqlDbType.Int).Value = Convert.ToInt32(cd.Livsstil.Livsstil_Id);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1) { errormsg = ""; }
                else { errormsg = "Failed: could not save to database";}
                return (i);
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }
        }

    }
}