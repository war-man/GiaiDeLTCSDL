using System;

namespace LTCSDL.BLL
{
    using DAL;
    using DAL.Models;
    using LTCSDL.Common.BLL;

    public class OrdersSvc : GenericSvc<OrdersRep, Orders>
    {
        public object GetDSDonHangTrongKhoangThoiGian(int size, int page, DateTime dateF, DateTime dateT)
        {
            return _rep.GetDSDonHangTrongKhoangThoiGian(size, page, dateF, dateT);
        }

        public object GetChiTietDonHang(int MaDH)
        {
            return _rep.GetChiTietDonHang(MaDH);
        }

        public object GetDSDonHangTrongKhoangThoiGian_LinQ(int size, int page, DateTime dateF, DateTime dateT)
        {
            return _rep.GetDSDonHangTrongKhoangThoiGian_LinQ(size, page, dateF, dateT);
        }

        public object GetChiTietDonHang_LinQ(int MaDH)
        {
            return _rep.GetChiTietDonHang_LinQ(MaDH);
        }

    }
}
