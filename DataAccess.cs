using ÖFMSluträkningUI.DbModel;
using ÖFMSluträkningUI.UIDataModel;

using System.Data;
using System.Data.SqlClient;

using Dapper;

/*
    DataAccess.cs

    Executes stored procedures which either fetches lists for categories or inserts data into the SQL-server
*/

namespace ÖFMSluträkningUI {
    public class DataAccess {

        public bool TestDBConnection() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                try { conn.Open(); }
                catch(Exception) { return false; }

                return true;
            }
        }

        public List<TransactionType> GetSqlSpTransactionList() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<TransactionType>("EXEC spOfmGetTransactionTypeList").ToList();
            }
        }

        public List<MainCategory> GetSqlSpMainCategory() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<MainCategory>("EXEC spOfmGetMainCategory").ToList();
            }
        }

        public List<SubCategory> GetSqlSpSubCategory() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<SubCategory>("EXEC spOfmGetSubCategory").ToList();
            }
        }

        public List<DetailCategory> GetSqlSpDetailCategory() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<DetailCategory>("EXEC spOfmGetDetailCategory").ToList();
            }
        }

        public List<SubCategory> GetSqlSpSubCategoryByMainCatId(int m_id) {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<SubCategory>("EXEC dbo.spOfmGetSubCatListByMainCatId @Id = " + m_id).ToList();
            }
        }

        public List<DetailCategory> GetSqlSpDetailCategoryBySubCatId(int m_id) {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Query<DetailCategory>("EXEC dbo.spOfmGetDetailCatListBySubCatId @Id = " + m_id).ToList();
            }
        }

        public int SqlSpDeleteTransactionTypeList() {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.ExecuteScalar<int>("EXEC spOfmDeleteTransactionTypeList");
            }
        }

        public int SqlSpOfmInsertTransactionListDataRow(List<Grid.TransactionType> tList) {

            using (IDbConnection conn = new SqlConnection(Helper.GetCnnVal("DB"))) {

                conn.Open();

                return conn.Execute("EXEC spOfmInsertTransactionTypeListDataRow @tText, @hId, @uId, @dId", new { tText = tList[0].Value1, hId = tList[0].Value2, uId = tList[0].Value3, dId = tList[0].Value4 });
            }
        }
    }
}
