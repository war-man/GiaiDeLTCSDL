using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LTCSDL.Web.Controllers
{
    using BLL;
    using Common.Rsp;
    using LTCSDL.Common.Req;

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public OrdersController()
        {
            _svc = new OrdersSvc();
        }

        private readonly OrdersSvc _svc;

        [HttpPost("serach-order-theo-company-and-employee")]
        public IActionResult TimKiemOrderTheoCompanyNameVaEmployeeName([FromBody] OrdersReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.TimKiemOrderTheoCompanyNameVaEmployeeName(req.size, req.page, req.companyName, req.employeeName);
            return Ok(res);
        }

        [HttpPost("danh-sach-don-hang-trong-ngay")]
        public IActionResult DanhSachDonHangTrongNgay_LinQ([FromBody] OrdersReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.DanhSachDonHangTrongNgay_LinQ(req.size, req.page, req.dateF);
            return Ok(res);
        }

        [HttpPost("so-luong-hang-can-giao-trong-khoang-thoi-gian")]
        public IActionResult SoLuongHangCanGiaoTrongKhoangThoiGian_LinQ([FromBody] OrdersReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.SoLuongHangCanGiaoTrongKhoangThoiGian_LinQ(req.dateF, req.dateT);
            return Ok(res);
        }
    }
}