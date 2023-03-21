using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanAoDoAn.Models;
using PagedList;
using PagedList.Mvc;

namespace WebQuanAoDoAn.Controllers
{
    public class WebQuanAoController : Controller
    {
        // GET: WebQuanAo
        DbWebQuanAoDataContext data = new DbWebQuanAoDataContext();
        // GET: WebQuanAo
        private List<SAN_PHAM> LaySanPhamMoi(int count)
        {
            return data.SAN_PHAMs.Take(count).ToList();
        }
        private List<SAN_PHAM> LayAllsp()
        {
            return data.SAN_PHAMs.ToList();
        }
        
        [HttpGet]
        public ActionResult Index(int? page)
        {

            var sanpham = LaySanPhamMoi(12);

            return View(sanpham);
        }
        
        public ActionResult ALLsanpham(int ? page, string TKTT="")
        {

            if (TKTT != "")
            {
                int pagesize = 12;
                int pagenum = (page ?? 1);
                var tkSDT = from ctd in data.SAN_PHAMs where ctd.TEN_SP.ToUpper().Contains(TKTT.ToUpper()) select ctd;
                return View(tkSDT.OrderByDescending(n => n.MA_SP).ToPagedList(pagenum, pagesize));
            }
            else
            {
                int Pagesize = 12;
                int pageNum = (page ?? 1);
                var sanpham = LayAllsp();
                return View(sanpham.ToPagedList(pageNum, Pagesize));
            }
        }
        [HttpGet]
        public ActionResult SLtk()
        {
            var QSCount = (from num in data.KHACH_HANGs select num).Count();
            return View(Session["SLTHANHVIEN"]= QSCount);
        }
        public ActionResult Loaispcon()
        {

            var loaisp = from sp in data.LOAI_SPs select sp;
            return PartialView(loaisp);
        }

        public ActionResult SptheoLoai(int id, int? page)
        {
            if (page == null) page = 1;
            var sanpham = from sp in data.SAN_PHAMs where sp.MA_LOAI == id select sp;
            int Pagesize = 3;
            int pageNum = (page ?? 1);
            return View(sanpham.ToPagedList(pageNum, Pagesize));


        }

        public ActionResult Detail(int id)
        {
            var sanpham = from sp in data.SAN_PHAMs where sp.MA_SP == id select sp;
            
            return View(sanpham.Single());


        }

        public ActionResult Size(int id)
        {
            var sizes = from size in data.KICH_THUOCs where size.MA_SP== id select size;

            return PartialView(sizes);


        }

        public ActionResult IMAGE(int id)
        {
            var images = from image in data.HINH_ANHs where image.MA_SP == id select image;

            return PartialView(images);

        }
    }
}