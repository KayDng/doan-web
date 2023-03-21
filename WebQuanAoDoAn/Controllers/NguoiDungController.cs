using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanAoDoAn.Models;
using System.Security.Cryptography;
using System.Text;
using PagedList;
using PagedList.Mvc;

namespace WebQuanAoDoAn.Controllers
{
    public class NguoiDungController : Controller
    {
        // mã hóa 
        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }

        // GET: NguoiDung
        DbWebQuanAoDataContext data = new DbWebQuanAoDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        //dangky
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult DangKy(FormCollection collection, KHACH_HANG kh)
        {
           

            var hoten = collection["hoten"];
            var mail = collection["email"];
            var SDT = collection["SDT"];
            var mk = GetMD5(collection["pwd-signup"]);
            var nlmk = collection["pwd-signup-again"];
            KHACH_HANG khS = data.KHACH_HANGs.FirstOrDefault(n => n.EMAIL == mail);
            if (khS == null)
            {
                kh.TEN_KH = hoten;
                kh.MK_DN = mk;
                kh.EMAIL = mail;
                kh.SDT = SDT;
                kh.TEN_DN = mail;
                data.KHACH_HANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Index", "WebQuanAo");
               
            }
            else
            {

                ViewBag.ThongBao = "Email đã có người đăng kí";
            }

            return this.Dangky();
        }
        //dang nhap
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult DangNhap(FormCollection collection)
        {
            var tendn = collection["txt-signin"];
            var matkhau =  collection["pwd"];        
            KHACH_HANG kh = data.KHACH_HANGs.SingleOrDefault(n => n.TEN_DN == tendn && n.MK_DN == GetMD5(matkhau));
            if (kh != null)
            {
               
                Session["TaiKhoan"] = kh;
                KHACH_HANG kh1 = (KHACH_HANG)Session["Taikhoan"];
                if (Session["TaiKhoan"] != null)
                {
                    Session["ten-khach-hang"] = kh1.TEN_KH;
                    Session["MaKH"] = kh1.MA_KH;
                }
                return RedirectToAction("Index", "WebQuanAo");
            }
            else
            {
                ViewBag.Thongbao = "Thông Tin Tài Khoản Không Đúng !";
            }
            return View();
        }
       
        public ActionResult Logout()
        {
            Session["TaiKhoan"]=null;//remove session
            return RedirectToAction("Index", "WebQuanAo");
            
        }

        public ActionResult TTHoaDonTv(int id, int? page)
        {
            int pagesize = 5;
            //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
            int pagenum = (page ?? 1);
            var DH = from dh in data.DON_DAT_HANGs where dh.MA_KH == id select dh;
            return View(DH.OrderByDescending(m=>m.MA_DON_HANG).ToPagedList(pagenum, pagesize));
        }
        public ActionResult TTctHoadon(int id, int? page)
        {
            int pagesize = 2;
            //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
            int pagenum = (page ?? 1);
            var DH = from dh in data.CT_DON_HANGs where dh.MA_DON_HANG== id select dh;
            return View(DH.OrderByDescending(m => m.MA_DON_HANG).ToPagedList(pagenum, pagesize));
        }

    }
}