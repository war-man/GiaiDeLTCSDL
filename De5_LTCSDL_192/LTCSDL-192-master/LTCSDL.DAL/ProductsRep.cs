using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LTCSDL.DAL
{
    using Models;
    using LTCSDL.Common.DAL;
    using LTCSDL.Common.BLL;
    using Microsoft.EntityFrameworkCore.Internal;
    using LTCSDL.Common.Req;

    public class ProductsRep : GenericRep<NorthwindContext, Products>
    {
        //Đề 5:
        //Câu 2: Hàm gọi store câu 1a => nhập ngày, xuất ra danh sách các product mà không còn tồn trong kho, có phân trang
        public object GetDanhSachProductKhongCoDonHangTrongNgay(int size, int page, DateTime date)
        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)Context.Database.GetDbConnection();
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = "DanhSachProductKhongCoDonHangTrongNgay";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@size", size);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@date", date);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            STT = row["STT"],
                            ProductID = row["ProductID"],
                            ProductName = row["ProductName"],
                            SupplierID = row["SupplierID"],
                            CategoryID = row["CategoryID"],
                            QuantityPerUnit = row["QuantityPerUnit"],
                            UnitPrice = row["UnitPrice"],
                            UnitsInStock = row["UnitsInStock"],
                            UnitsOnOrder = row["UnitsOnOrder"],
                            ReorderLevel = row["ReorderLevel"],
                            Discontinued = row["Discontinued"]
                        };
                        res.Add(x);
                    }
                }
            }
            catch (Exception e)
            {
                res = null;
            }
            return res;
        }

        //Đề 5:
        //Câu 3: Hàm thêm mới 1 record cho bảng Product
        public object ThemMoiProduct(InsertProductReq pro)
        {
            List<object> res = new List<object>();
            var cnn = (SqlConnection)Context.Database.GetDbConnection();
            if (cnn.State == ConnectionState.Closed)
                cnn.Open();
            try
            {
                string sql = "insert into [dbo].[Products] ([ProductName] ,[SupplierID] ,[CategoryID] ,[QuantityPerUnit] ,[UnitPrice] ,[UnitsInStock] ,[UnitsOnOrder] ,[ReorderLevel] ,[Discontinued])";
                sql = sql + "values ('" + pro.ProductName + "', '" + pro.SupplierId + "', '" + pro.CategoryId + "', '" + pro.QuantityPerUnit + "', '" + pro.UnitPrice + "', '" + pro.UnitsInStock + "', '" + pro.UnitsOnOrder + "', '" + pro.ReorderLevel + "', '" + pro.Discontinued + "')";
                sql = sql + "select * from [dbo].[Products] where ProductID = @@identity";
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            ProductID = row["ProductID"],
                            ProductName = row["ProductName"],
                            SupplierID = row["SupplierID"],
                            CategoryID = row["CategoryID"],
                            QuantityPerUnit = row["QuantityPerUnit"],
                            UnitPrice = row["UnitPrice"],
                            UnitsInStock = row["UnitsInStock"],
                            UnitsOnOrder = row["UnitsOnOrder"],
                            ReorderLevel = row["ReorderLevel"],
                            Discontinued = row["Discontinued"]
                        };
                        res.Add(x);
                    }
                }
            }
            catch (Exception e)
            {
                res = null;
            }
            return res;
        }
    }
}
