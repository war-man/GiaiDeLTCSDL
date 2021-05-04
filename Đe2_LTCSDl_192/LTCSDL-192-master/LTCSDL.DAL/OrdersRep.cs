using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

//Đề 2:
namespace LTCSDL.DAL
{
    using Models;
    using LTCSDL.Common.DAL;

    public class OrdersRep : GenericRep<NorthwindContext, Orders>
    {
        //Câu 2:
        public object GetDSDonHangTrongKhoangThoiGian(int size, int page, DateTime dateF, DateTime dateT)
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
                cmd.CommandText = "DanhSachDonHangTrongKhoangThoiGian";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dateF", dateF);
                cmd.Parameters.AddWithValue("@dateT", dateT);
                cmd.Parameters.AddWithValue("@size", size);
                cmd.Parameters.AddWithValue("@page", page);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            STT = row["STT"],
                            OrderID = row["OrderID"],
                            CustomerID = row["CustomerID"],
                            EmployeeID = row["EmployeeID"],
                            OrderDate = row["OrderDate"],
                            RequiredDate = row["RequiredDate"],
                            ShippedDate = row["ShippedDate"],
                            ShipVia = row["ShipVia"],
                            Freight = row["Freight"],
                            ShipName = row["ShipName"],
                            ShipAddress = row["ShipAddress"],
                            ShipCity = row["ShipCity"],
                            ShipRegion = row["ShipRegion"],
                            ShipPostalCode = row["ShipPostalCode"],
                            ShipCountry = row["ShipCountry"]
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

        //Câu 2:
        public object GetChiTietDonHang(int MaDH)
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
                cmd.CommandText = "ChiTietDonHang";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaDH", MaDH);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            OrderID = row["OrderID"],
                            ProductID = row["ProductID"],
                            UnitPrice = row["UnitPrice"],
                            Quantity = row["Quantity"],
                            Discount = row["Discount"]
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

        //Câu 3:
        public object GetDSDonHangTrongKhoangThoiGian_LinQ(int size, int page, DateTime dateF, DateTime dateT)
        {
            var ds = All.Where(o => o.OrderDate >= dateF && o.OrderDate <= dateT);

            var offset = (page - 1) * size;
            var totalRecord = ds.Count();
            var totalPage = (totalRecord % size) == 0 ? (int)(totalRecord / size) : (int)((totalRecord / size) + 1);
            var data = ds.Skip(offset).Take(size).ToList();

            return new
            {
                Data = data,
                totalRecord = totalRecord,
                totalPage = totalPage,
                page = page,
                size = size
            };
        }

        //Câu 3:
        public object GetChiTietDonHang_LinQ(int MaDH)
        {
            var res = All.Join(this.Context.OrderDetails, a => a.OrderId, b => b.OrderId, (a, b) => new {
                a.OrderId,
                b.ProductId,
                b.UnitPrice,
                b.Quantity,
                b.Discount
            }).Where(o => o.OrderId == MaDH);
            return res;
        }
    }
}
