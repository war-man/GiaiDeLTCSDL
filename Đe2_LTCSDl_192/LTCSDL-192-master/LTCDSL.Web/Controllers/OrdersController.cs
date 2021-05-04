﻿using System;
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

        [HttpPost("get-ds-don-hang-trong-khoang-thoi-gian")]
        public IActionResult GetDSDonHangTrongKhoangThoiGian([FromBody] OrdersReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.GetDSDonHangTrongKhoangThoiGian(req.size, req.page, req.dateF, req.dateT);
            return Ok(res);
        }

        [HttpPost("get-chi-tiet-don-hang")]
        public IActionResult GetChiTietDonHang([FromBody] SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.GetChiTietDonHang(req.Id);
            return Ok(res);
        }

        [HttpPost("get-ds-don-hang-trong-khoang-thoi-gian-linq")]
        public IActionResult GetDSDonHangTrongKhoangThoiGian_LinQ([FromBody] OrdersReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.GetDSDonHangTrongKhoangThoiGian_LinQ(req.size, req.page, req.dateF, req.dateT);
            return Ok(res);
        }

        [HttpPost("get-chi-tiet-don-hang-linq")]
        public IActionResult GetChiTietDonHang_LinQ([FromBody] SimpleReq req)
        {
            var res = new SingleRsp();
            res.Data = _svc.GetChiTietDonHang_LinQ(req.Id);
            return Ok(res);
        }
    }
}
