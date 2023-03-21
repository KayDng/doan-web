using mvcDangNhap.common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanAoDoAn.Models;

namespace WebQuanAoDoAn.Controllers
{
    public class GiohangController : Controller
    {
        DbWebQuanAoDataContext data = new DbWebQuanAoDataContext();
        // GET: Giohang
        public ActionResult Index()
        {
            return View();
        }
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listGiohang
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        //Them hang vao gio
        public ActionResult ThemGiohang(int id, string strURL)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra SP này tồn tại trong Session["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMasp == id);
            SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);
            if (sanpham == null)
            {
                
                    sanpham = new Giohang(id);
                    lstGiohang.Add(sanpham);
                
                return Redirect(strURL);
            }
            else
            {
                if (sanpham.iSoluong < sp.SL)
                {
                    sanpham.iSoluong++;
                }
                return Redirect(strURL);
            }
        }
        public ActionResult ThemGiohangALL(int id, string strURL, FormCollection f)
        {
            //Lay ra Session gio hang
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra SP này tồn tại trong Session["Giohang"] chưa?
            Giohang sanpham = lstGiohang.Find(n => n.iMasp == id);
            SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                if (sanpham.iSoluong <sp.SL && int.Parse(f["SL"].ToString()) <=sp.SL)
                {
                    sanpham.iSoluong = sanpham.iSoluong + int.Parse(f["SL"].ToString()) - 1;
                    lstGiohang.Add(sanpham);
                }
                else
                {
                    sanpham.iSoluong = sanpham.iSoluong - 1;
                }
               
                return Redirect(strURL);
            }
            else
            {
                
               
                if (sanpham.iSoluong < sp.SL && int.Parse(f["SL"].ToString()) <= sp.SL) 
                {
                    sanpham.iSoluong = sanpham.iSoluong + int.Parse(f["SL"].ToString());
                }
                else
                {
                    sanpham.iSoluong = sanpham.iSoluong - 1;
                }
                return Redirect(strURL);
            }
        }
        // Tong so luong
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }
        //Tong Tien
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //hien thi gio hang view
       
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            if (lstGiohang.Count == 0)
                return RedirectToAction("Index","WebQuanAo");
            else
                return View(lstGiohang);
        }
        //
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            return PartialView();
        }
        
        //Xoa Giohang
        public ActionResult Xoagiohang(int id)
        {
            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasp == id);
            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMasp == id);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index","WebQuanAo");
            }
            return RedirectToAction("GioHang");
        }
        // cap nhat gio hang
       
        public ActionResult CapnhatGiohang(int id, FormCollection f)
        {

            //Lay gio hang tu Session
            List<Giohang> lstGiohang = Laygiohang();
            //Kiem tra sach da co trong Session["Giohang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMasp == id);
            SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);

            //Neu ton tai thi cho sua Soluong
            if (sanpham != null)
            {
                if (sanpham.iSoluong < sp.SL && int.Parse(f["txtSoluong"].ToString()) <= sp.SL)
                {
                    
                    sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
                }
                else 
                {
                   
                        sanpham.iSoluong = sanpham.iSoluong-1;
                  
                }
               
            }
            return RedirectToAction("GioHang", "Giohang");
        }

        // dat hang
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
              return RedirectToAction("Dangnhap", "Nguoidung");
            if (Session["Giohang"] == null)
                return RedirectToAction("Index", "WebQuanAo");


            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            KHACH_HANG kh = (KHACH_HANG)Session["Taikhoan"];
            var khdata = data.KHACH_HANGs.First(m => m.MA_KH == kh.MA_KH);
            //var tenkh = collection["hoten"];
            var ngaydsinh = String.Format("{0:MM/dd/yyyy}", collection["date-time"]);
            var diachi = collection["dia-chia"];
            Session["SDTKH"] = collection["sdt"];
            khdata.MA_KH = kh.MA_KH;
            Session["diachi"] = diachi;
            Session["gmailNH"] = collection["gmail"];
            //khdata.TEN_KH = tenkh;
            khdata.NGAY_SINH = DateTime.Parse( ngaydsinh);
            //khdata.DIA_CHI = diachi;
            //khdata.SDT = sdt;
            UpdateModel(khdata);
            data.SubmitChanges();
            // cập nhạt tt trong sestion
            Session["Taikhoan"] = khdata;
           
            //Session["ten-khach-hang"] = khdata.TEN_KH;
            return RedirectToAction("tienhhanhdathang", "Giohang");
        }

        public ActionResult tienhhanhdathang()
        {
            ViewBag.Tongtien = TongTien();
            return View();
        }
        [HttpPost]
        //Xay dung chuc nang Dathang
        public ActionResult tienhhanhdathang(FormCollection collection)
        {
            List<Giohang> lstGiohang = Laygiohang();
            //Them Don hang
            DON_DAT_HANG ddh = new DON_DAT_HANG();
            KHACH_HANG kh = (KHACH_HANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MA_KH = kh.MA_KH;
            ddh.NGAY_DAT = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NGAY_GIAO = DateTime.Parse(ngaygiao);
            ddh.TINH_TRANG_GH = false;
            ddh.DA_THANH_TOAN = false;
            data.DON_DAT_HANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Them chi tiet don hang
            ViewBag.Tongtien2 = TongTien();
            foreach (var item in gh)
            {
                CT_DON_HANG ctdh = new CT_DON_HANG();
                ctdh.MA_DON_HANG = ddh.MA_DON_HANG;
                ctdh.MA_SP= item.iMasp;
                ctdh.SL = item.iSoluong;
                ctdh.DIA_CHI = Session["diachi"].ToString();
                ctdh.DON_GIA= (decimal)item.dDongia;
                ctdh.TONG_TIEN = (decimal)item.dDongia * item.iSoluong;
               
                data.CT_DON_HANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            // gui mail
            string content = System.IO.File.ReadAllText(Server.MapPath("~/content/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", kh.TEN_KH);
            content = content.Replace("{{Phone}}", Session["SDTKH"].ToString());
            content = content.Replace("{{Email}}", Session["gmailNH"].ToString());
            content = content.Replace("{{Address}}", Session["diachi"].ToString());
            content = content.Replace("{{Total}}", String.Format("{0:0,0}", ViewBag.Tongtien2));
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendMail(Session["gmailNH"].ToString(), "Đơn hàng mới từ Web Bán Quần áo", content);
            new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Web Bán Quần áo", content);
            //gu mail
            Session["gmailNH"] = null;
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");    
        }

        public ActionResult Xacnhandonhang()
        {
            return View();

        }
    }
}