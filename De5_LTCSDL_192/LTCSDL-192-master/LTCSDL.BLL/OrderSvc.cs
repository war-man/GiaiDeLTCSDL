using System;

namespace LTCSDL.BLL
{
    using DAL;
    using DAL.Models;
    using LTCSDL.Common.BLL;

    public class OrdersSvc : GenericSvc<OrdersRep, Orders>
    {
        public object TimKiemOrderTheoCompanyNameVaEmployeeName(int size, int page, string companyName, string employeeName)
        {
            return _rep.TimKiemOrderTheoCompanyNameVaEmployeeName(size, page, companyName, employeeName);
        }

        public object DanhSachDonHangTrongNgay_LinQ(int size, int page, DateTime date)
        {
            return _rep.DanhSachDonHangTrongNgay_LinQ(size, page, date);
        }

        public object SoLuongHangCanGiaoTrongKhoangThoiGian_LinQ(DateTime dateFrom, DateTime dateTo)
        {
            return _rep.SoLuongHangCanGiaoTrongKhoangThoiGian_LinQ(dateFrom, dateTo);
        }
    }
}
