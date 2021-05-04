using System;
using System.Linq;

namespace LTCSDL.BLL
{
    using DAL;
    using DAL.Models;
    using LTCSDL.Common.BLL;
    using LTCSDL.Common.Req;

    public class ProductsSvc : GenericSvc<ProductsRep, Products>
    {
        public object GetDanhSachProductKhongCoDonHangTrongNgay(int size, int page, DateTime date)
        {
            return _rep.GetDanhSachProductKhongCoDonHangTrongNgay(size, page, date);
        }

        public object ThemMoiProduct(InsertProductReq req)
        {
            return _rep.ThemMoiProduct(req);
        }
    }
}
