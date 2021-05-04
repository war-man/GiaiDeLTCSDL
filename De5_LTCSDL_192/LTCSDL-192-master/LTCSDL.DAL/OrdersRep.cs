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

    public class OrdersRep : GenericRep<NorthwindContext, Orders>
    {
        //Đề 5:
        //Câu 2: Hàm gọi strore câu 1c => nhập vào từ khóa để tìm Order theo CompanyName, Employee xuất ra danh sách Order tìm thấy được, có phân trang
        public object TimKiemOrderTheoCompanyNameVaEmployeeName(int size, int page, string companyName, string employeeName)
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
                cmd.CommandText = "TimKiemOrderTheoCompanyNameVaEmployeeName";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@size", size);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@companyName", companyName);
                cmd.Parameters.AddWithValue("@employeeName", employeeName);
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

        //Đề 5:
        //Câu 4: viết hàm bằng LinQ => nhập ngày tháng năm xuất ra danh sách các đơn hàng và tên khách hàng, địa chỉ cần giao trong ngày, có phân trang
        public object DanhSachDonHangTrongNgay_LinQ(int size, int page, DateTime date)
        {
            var ds = All.Join(this.Context.Customers, a => a.CustomerId, b => b.CustomerId, (a, b) => new
            {
                a.OrderId,
                b.CompanyName,
                a.ShipAddress,
                a.ShippedDate
            }).Where(o => o.ShippedDate == date)
            .Select(o => new { o.OrderId, o.CompanyName, o.ShipAddress });

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

        //Đề 5:
        //Câu 5: viết hàm bằng LinQ => nhập ngày bắt đầu, kết thúc tính số lượng hàng hóa cần giao trong từng ngày trong khoảng thời gian đó
        public object SoLuongHangCanGiaoTrongKhoangThoiGian_LinQ(DateTime dateFrom, DateTime dateTo)
        {
            var res = All.Join(this.Context.OrderDetails, a => a.OrderId, b => b.OrderId, (a, b) => new
            {
                a.ShippedDate,
                b.Quantity
            }).Where(o => o.ShippedDate >= dateFrom && o.ShippedDate <= dateTo)
            .GroupBy(o => new { o.ShippedDate })
            .Select(gr => new
            {
                Ngay = gr.Key.ShippedDate,
                SoLuongHangCanGiao = gr.Sum(o => o.Quantity)
            });

            return res;
        }
    }
}
