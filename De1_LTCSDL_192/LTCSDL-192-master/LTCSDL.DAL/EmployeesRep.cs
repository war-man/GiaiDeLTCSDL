using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LTCSDL.DAL
{
    using Models;
    using LTCSDL.Common.DAL;
    using Microsoft.VisualBasic;
    using LTCSDL.Common.Req;

    public class EmployeesRep : GenericRep<NorthwindContext, Employees>
    {
        //Đề 1:
        //Câu 2: Hàm gọi store câu 1a => nhập ngày tháng năm xuất ra danh sách nhân viên và doanh thu tương ứng trong ngày 
        public object GetDoanhThuNhanVienTrongNgay(DateTime date)
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
                cmd.CommandText = "DoanhThuNhanVienTrongNgay";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@date", date);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            EmployeeID = row["EmployeeID"],
                            FirstName = row["FirstName"],
                            LastName = row["LastName"],
                            DoanhThu = row["DoanhThu"]
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

        //Đề 1:
        //Câu 2: Hàm gọi store 1b => nhập thời gian bắt đầu và thời gian kết thúc, xuất ra danh sách nhân viên và doanh thu tương ứng trong khoảng thời gian đó
        public object GetDoanhThuNhanVienTrongKhoangThoiGian(DateTime dateBegin, DateTime dateEnd)
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
                cmd.CommandText = "DoanhThuNhanVienTrongKhoangThoiGian";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@dateBegin", dateBegin);
                cmd.Parameters.AddWithValue("@dateEnd", dateEnd);
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var x = new
                        {
                            EmployeeID = row["EmployeeID"],
                            FirstName = row["FirstName"],
                            LastName = row["LastName"],
                            DoanhThu = row["DoanhThu"]
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

        //Đề 1:
        ////Câu 3: Hàm gọi store câu 1a bằng LinQ => nhập ngày tháng năm xuất ra danh sách nhân viên và doanh thu tương ứng trong ngày 
        public object GetDoanhThuNhanVienTrongNgay_LinQ(DateTime date)
        {
            var res = All.Join(this.Context.Orders, a => a.EmployeeId, b => b.EmployeeId, (a, b) => new
            {
                a.EmployeeId,
                a.FirstName,
                a.LastName,
                b.OrderId,
                b.OrderDate
            }).Join(this.Context.OrderDetails, a => a.OrderId, c => c.OrderId, (a, c) => new
            {
                a.EmployeeId,
                a.FirstName,
                a.LastName,
                a.OrderDate,
                DoanhThu = c.Quantity * Convert.ToDouble(c.UnitPrice) * (1 - Convert.ToDouble(c.Discount))
            }).Where(emp => emp.OrderDate.Value.Day == date.Day && emp.OrderDate.Value.Month == date.Month &&
                            emp.OrderDate.Value.Year == date.Year)
            .GroupBy(emp => new { emp.EmployeeId, emp.FirstName, emp.LastName })
            .Select(group => new {
                group.Key.EmployeeId,
                group.Key.FirstName,
                group.Key.LastName,
                DoanhThu = Math.Round(group.Sum(emp => emp.DoanhThu), 2)
            });
            return res;

        }

        //Đề 1:
        //Câu 3: Hàm gọi store 1b => nhập thời gian bắt đầu và thời gian kết thúc, xuất ra danh sách nhân viên và doanh thu tương ứng trong khoảng thời gian đó
        public object GetDoanhThuNhanVienTrongKhoangThoiGian_LinQ(DateTime dateBegin, DateTime dateEnd)
        {
            var res = All.Join(this.Context.Orders, a => a.EmployeeId, b => b.EmployeeId, (a, b) => new
            {
                a.EmployeeId,
                a.FirstName,
                a.LastName,
                b.OrderId,
                b.OrderDate
            }).Join(this.Context.OrderDetails, a => a.OrderId, c => c.OrderId, (a, c) => new
            {
                a.EmployeeId,
                a.FirstName,
                a.LastName,
                a.OrderDate,
                DoanhThu = c.Quantity * Convert.ToDouble(c.UnitPrice) * (1 - c.Discount)
            }).Where(emp => emp.OrderDate >= dateBegin && emp.OrderDate <= dateEnd)
            .GroupBy(emp => new { emp.EmployeeId, emp.FirstName, emp.LastName })
            .Select(group => new {
                group.Key.EmployeeId,
                group.Key.FirstName,
                group.Key.LastName,
                DoanhThu = Math.Round(group.Sum(emp => emp.DoanhThu), 2)
            });
            return res;

        }
    }
}
