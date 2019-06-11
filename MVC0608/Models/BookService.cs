﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MVC0608.Models
{
    public class BookService
    {

        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }


        public List<Models.BookData> GetBookByCondtioin(Models.BookSearch arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BOOK_ID, BOOK_CLASS_NAME ,BOOK_NAME ,BOOK_BOUGHT_DATE ,CODE_NAME ,USER_ENAME
                           FROM [dbo].[BOOK_DATA] as da

                           left join [dbo].[BOOK_CLASS] as cl
                           on da.BOOK_CLASS_ID = cl.BOOK_CLASS_ID
                           left join [dbo].[BOOK_CODE] as co
                           on da.BOOK_STATUS = co.CODE_ID
                           left join [dbo].[MEMBER_M] as m
                           on da.BOOK_KEEPER = m.USER_ID

                           Where (da.BOOK_STATUS = co.CODE_ID) AND
                                 (da.BOOK_ID = @Book_ID OR @Book_ID='') AND
                                 (BOOK_NAME LIKE ('%' + @Book_Name + '%')or @Book_Name='') AND
                                 (UPPER(da.BOOK_CLASS_ID) LIKE UPPER('%' + @Book_Class_Name + '%')or @Book_Class_Name='') AND
                                 (BOOK_STATUS LIKE ('%' + @Book_Status + '%')or @Book_Status='') AND
                                 (BOOK_KEEPER LIKE ('%' + @Book_Keeper + '%')or @Book_Keeper='')";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Book_ID", arg.Book_ID == null ? string.Empty : arg.Book_ID));
                cmd.Parameters.Add(new SqlParameter("@Book_Name", arg.Book_Name == null ? string.Empty : arg.Book_Name));
                cmd.Parameters.Add(new SqlParameter("@Book_Class_Name", arg.Book_Class_Name == null ? string.Empty : arg.Book_Class_Name));
                cmd.Parameters.Add(new SqlParameter("@Book_Status", arg.Book_Status == null ? string.Empty : arg.Book_Status));
                cmd.Parameters.Add(new SqlParameter("@Book_Keeper", arg.Book_Keeper == null ? string.Empty : arg.Book_Keeper));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDataToList(dt);
        }

        public int Insert(Models.BookData book)
        {
            string sql = @" INSERT INTO [dbo].[BOOK_DATA]
                         (
        	                 BOOK_NAME, BOOK_AUTHOR, BOOK_PUBLISHER, BOOK_NOTE, BOOK_BOUGHT_DATE, BOOK_CLASS_ID , BOOK_STATUS, BOOK_KEEPER
                         )
                        VALUES
                        (
        	                 @Book_Name,@Book_Author, @Book_Publisher, @Book_Note, @Book_BoughtDate, @Book_Class_ID , @Book_Status, @Book_Keeper
                        )
                        Select SCOPE_IDENTITY()";
            int BookId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Book_Name", book.Book_Name));
                cmd.Parameters.Add(new SqlParameter("@Book_Author", book.Book_Author));
                cmd.Parameters.Add(new SqlParameter("@Book_Publisher", book.Book_Publisher));
                cmd.Parameters.Add(new SqlParameter("@Book_Note", book.Book_Note));
                cmd.Parameters.Add(new SqlParameter("@Book_BoughtDate", book.Book_BoughtDate));
                cmd.Parameters.Add(new SqlParameter("@Book_Class_ID", book.Book_Class_ID));
                cmd.Parameters.Add(new SqlParameter("@Book_Status", book.Book_Status));
                cmd.Parameters.Add(new SqlParameter("@Book_Keeper", book.Book_Keeper));
                BookId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return BookId;
        }

        public int Delete(string BookId)
        {

            string sql = "Delete FROM [dbo].[BOOK_DATA] Where Book_ID=@Book_ID";
            int BookID;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Book_ID", BookId));
                BookID = (int)(cmd.ExecuteNonQuery());
                conn.Close();
            }
            return BookID;
        }



        public List<Models.BookData> Book_Class()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT *
                           FROM [dbo].[BOOK_CLASS] as cl";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            List<Models.BookData> result = new List<BookData>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new BookData()
                {
                    Book_Class_ID = row["Book_Class_ID"].ToString(),
                    Book_Class_Name = row["BOOK_CLASS_NAME"].ToString()
                });
            }
            return result;
        }

        public List<Models.BookData> Book_Status()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT *
                           FROM [dbo].[BOOK_CODE] as co";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            List<Models.BookData> result = new List<BookData>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new BookData()
                {
                    Book_Status = row["CODE_ID"].ToString(),
                    Book_Status_Name = row["CODE_NAME"].ToString()
                });
            }
            return result;
        }

        public List<Models.BookData> Book_Keeper()
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT *
                           FROM [dbo].[MEMBER_M] as m";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            List<Models.BookData> result = new List<BookData>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new BookData()
                {
                    Book_Keeper = row["USER_ID"].ToString(),
                    Book_Keeper_EName = row["USER_ENAME"].ToString()
                });
            }
            return result;
        }


        private List<Models.BookData> MapBookDataToList(DataTable BookData)
        {
            List<Models.BookData> result = new List<BookData>();
            foreach (DataRow row in BookData.Rows)
            {
                result.Add(new BookData()
                {
                    Book_ID = row["Book_ID"].ToString(),
                    Book_Class_Name = row["BOOK_CLASS_NAME"].ToString(),
                    Book_Name = row["BOOK_NAME"].ToString(),
                    Book_BoughtDate = Convert.ToDateTime(row["BOOK_BOUGHT_DATE"]).ToString("yyyy/MM/dd"),
                    Book_Status_Name = row["CODE_NAME"].ToString(),
                    Book_Keeper_EName = row["USER_ENAME"].ToString()
                });
            }
            return result;
        }


    }
}