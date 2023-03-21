using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQuanAoDoAn.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebQuanAoDoAn.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        DbWebQuanAoDataContext data = new DbWebQuanAoDataContext();
        public ActionResult IndexAdmin()
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult DangNhapadm()
        {
            return View();
        }
        public ActionResult DangNhapadm(FormCollection collection)
        {
            var tendn = collection["txt-signin"];
            var matkhau = collection["pwd"];
            ADMIN admin = data.ADMINs.SingleOrDefault(n => n.TEN_DN_AD == tendn && n.MK_DN_AD== matkhau);
            if (admin != null)
            {

                Session["TaiKhoanADM"] = admin;
                ADMIN adm = (ADMIN)Session["TaiKhoanADM"];
                if (Session["TaiKhoanADM"] != null)
                {
                    Session["ten-adm"] = adm.HO_TEN_AD;
                    Session["avatar-admin"] = adm.HINH_AD;
                    Session["tendn-ADM"] = adm.TEN_DN_AD;
                }
                return RedirectToAction("IndexAdmin", "Admin");
            }
            else
            {
                ViewBag.Thongbao = "Thông Tin Tài Khoản Không Đúng !";
            }
            return View();
        }

        public ActionResult QLSP(int? page)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.SAN_PHAMs.ToList().OrderByDescending(n => n.MA_SP).ToPagedList(pagenum, pagesize));
            }
        }

        [HttpGet]
        public ActionResult Themmoisp()
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                ViewBag.MA_LOAI = new SelectList(data.LOAI_SPs.ToList().OrderBy(n => n.TEN_LOAI), "MA_LOAI", "TEN_LOAI");
                ViewBag.MA_MAU = new SelectList(data.MAUs.ToList().OrderBy(n => n.TEN_MAU), "MA_MAU", "TEN_MAU");
                return View();
            }
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Themmoisp(SAN_PHAM sp, HttpPostedFileBase fileUpload )
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                ViewBag.MA_LOAI = new SelectList(data.LOAI_SPs.ToList().OrderBy(n => n.TEN_LOAI), "MA_LOAI", "TEN_LOAI");
                ViewBag.MA_MAU = new SelectList(data.MAUs.ToList().OrderBy(n => n.TEN_MAU), "MA_MAU", "TEN_MAU");
                //Kiem tra duong dan file
                if (fileUpload == null )
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //var file2 = Path.GetFileName(f2.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("/img/products"), fileName);
                        //var path2 = Path.Combine(Server.MapPath("/img/products"), );
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                            //fileUpload.SaveAs(path2);
                        }
                        sp.HINH_ANH = fileName;
                        //sp.HINH_ANH1 = fileName;
                        //Luu vao CSDL
                        data.SAN_PHAMs.InsertOnSubmit(sp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("QLSP", "Admin");
                }
            }
        }

        public ActionResult Chitietsp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var sach = from s in data.SAN_PHAMs where s.MA_SP == id select s;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpGet]
        public ActionResult editsp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                 SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MA_LOAI = new SelectList(data.LOAI_SPs.ToList().OrderBy(n => n.TEN_LOAI), "MA_LOAI", "TEN_LOAI", sp.MA_LOAI);
                ViewBag.MA_MAU = new SelectList(data.MAUs.ToList().OrderBy(n => n.TEN_MAU), "MA_MAU", "TEN_MAU", sp.MA_MAU);
                return View(sp);
            }
        }

        [HttpPost]
        public ActionResult editsp(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);
                ViewBag.MA_LOAI = new SelectList(data.LOAI_SPs.ToList().OrderBy(n => n.TEN_LOAI), "MA_LOAI", "TEN_LOAI");
                ViewBag.MA_MAU = new SelectList(data.MAUs.ToList().OrderBy(n => n.TEN_MAU), "MA_MAU", "TEN_MAU");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao1 = "Vui lòng chọn ảnh";
                    return View(sp);
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);

                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("/img/products"), fileName);

                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);

                        }
                        sp.HINH_ANH = fileName;
                        //Luu vao CSDL
                        UpdateModel(sp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("QLSP", "Admin");
                }
            }
        }


        [HttpGet]
        public ActionResult Xoasp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var sach = from s in data.SAN_PHAMs where s.MA_SP== id select s;
                return View(sach.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoasp")]
        public ActionResult Xacnhanxoa(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
               SAN_PHAM sp = data.SAN_PHAMs.SingleOrDefault(n => n.MA_SP == id);
                data.SAN_PHAMs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLSP", "Admin");
            }
        }

        public ActionResult QLIMAGE(int? page)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 4;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.HINH_ANHs.ToList().OrderByDescending(n => n.MA_HINH).ToPagedList(pagenum, pagesize));
            }
        }

        public ActionResult ThemmoiIMAGESP()
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");
                
                return View();
            }
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult ThemmoiIMAGESP(HINH_ANH imgsp, HttpPostedFileBase fileUpload)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //var file2 = Path.GetFileName(f2.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("/img/products"), fileName);
                        //var path2 = Path.Combine(Server.MapPath("/img/products"), );
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                            //fileUpload.SaveAs(path2);
                        }
                        imgsp.HINH_ANH1= fileName;
                        //sp.HINH_ANH1 = fileName;
                        //Luu vao CSDL
                        data.HINH_ANHs.InsertOnSubmit(imgsp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("QLIMAGE", "Admin");
                }
            }
        }

        public ActionResult editImageSp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                HINH_ANH sp = data.HINH_ANHs.SingleOrDefault(n => n.MA_HINH == id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP", sp.MA_SP);
                return View(sp);
            }
        }

        [HttpPost]
        public ActionResult editImageSp(int id, HttpPostedFileBase fileUpload)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                HINH_ANH sp = data.HINH_ANHs.SingleOrDefault(n => n.MA_HINH == id);

                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");
                //Kiem tra duong dan file
                if (fileUpload == null)
                {
                    ViewBag.Thongbao1 = "Vui lòng chọn ảnh";
                    return View(sp);
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);

                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("/img/products"), fileName);

                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);

                        }
                        sp.HINH_ANH1 = fileName;
                        //Luu vao CSDL
                        UpdateModel(sp);
                        data.SubmitChanges();
                    }
                    return RedirectToAction("QLIMAGE", "Admin");
                }
            }
        }

        [HttpGet]
        public ActionResult XoaImage(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var imgsp = from s in data.HINH_ANHs where s.MA_HINH == id select s;
                return View(imgsp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("XoaImage")]
        public ActionResult XacnhanxoaIMAGE(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                HINH_ANH imgsp = data.HINH_ANHs.SingleOrDefault(n => n.MA_HINH == id);
                data.HINH_ANHs.DeleteOnSubmit(imgsp);
                data.SubmitChanges();
                return RedirectToAction("QLIMAGE", "Admin");
            }
        }

        public ActionResult QLloaisp()
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
               
                return View(data.LOAI_SPs.ToList().OrderByDescending(n => n.MA_LOAI) );
            }
        }
        public ActionResult themloaisp()
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {

                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult themloaisp(LOAI_SP LSP)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                
                        //Luu vao CSDL
                        data.LOAI_SPs.InsertOnSubmit(LSP);
                        data.SubmitChanges();
                    return RedirectToAction("QLloaisp", "Admin");
                
            }
        }

        public ActionResult editloaisp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                LOAI_SP sp = data.LOAI_SPs.SingleOrDefault(n => n.MA_LOAI == id);
                return View(sp);
            }
        }

        [HttpPost , ActionName("editloaisp")]
        public ActionResult xacnhanEditloaisp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                LOAI_SP sp = data.LOAI_SPs.SingleOrDefault(n => n.MA_LOAI == id);

                        //Luu vao CSDL
                        UpdateModel(sp);
                        data.SubmitChanges();
                    
                    return RedirectToAction("QLloaisp", "Admin");
                
            }
            
        }
        [HttpGet]
        public ActionResult DeleteLoaisp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var imgsp = from s in data.LOAI_SPs where s.MA_LOAI == id select s;
                return View(imgsp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("DeleteLoaisp")]
        public ActionResult Xacnhanxoalsp(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                LOAI_SP sp = data.LOAI_SPs.SingleOrDefault(n => n.MA_LOAI== id);
                data.LOAI_SPs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLloaisp", "Admin");
            }
        }

        public ActionResult QLsize(int? page)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 4;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.KICH_THUOCs.ToList().OrderByDescending(n => n.MA_KICH_THUOC).ToPagedList(pagenum, pagesize));
            }
        }

        public ActionResult Themmoisize()
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");

                return View();
            }
        }
        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Themmoisize(KICH_THUOC imgsp)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");
                //Kiem tra duong dan file
                
                        data.KICH_THUOCs.InsertOnSubmit(imgsp);
                        data.SubmitChanges();
                    
                    return RedirectToAction("QLsize", "Admin");
                
            }
        }

        public ActionResult editsize(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                KICH_THUOC sp = data.KICH_THUOCs.SingleOrDefault(n => n.MA_KICH_THUOC == id);
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP", sp.MA_SP);
                return View(sp);
            }
        }

        [HttpPost, ActionName("editsize")]
        public ActionResult xacnhanEditsize(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                KICH_THUOC sp = data.KICH_THUOCs.SingleOrDefault(n => n.MA_KICH_THUOC == id);
                ViewBag.MA_SP = new SelectList(data.SAN_PHAMs.ToList().OrderBy(n => n.TEN_SP), "MA_SP", "TEN_SP");
                //Luu vao CSDL
                UpdateModel(sp);
                data.SubmitChanges();

                return RedirectToAction("QLsize", "Admin");

            }

        }


        [HttpGet]
        public ActionResult DeleteSize(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var imgsp = from s in data.KICH_THUOCs where s.MA_KICH_THUOC== id select s;
                return View(imgsp.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("DeleteSize")]
        public ActionResult XacnhanxoaSize(int id)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                KICH_THUOC sp = data.KICH_THUOCs.SingleOrDefault(n => n.MA_KICH_THUOC == id);
                data.KICH_THUOCs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLsize", "Admin");
            }
        }


        public ActionResult QLDONHANG(int? page)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 5;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.DON_DAT_HANGs.ToList().OrderByDescending(n => n.MA_DON_HANG).ToPagedList(pagenum, pagesize));
            }
        }

        public ActionResult editDONHANG(int id)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                DON_DAT_HANG sp = data.DON_DAT_HANGs.SingleOrDefault(n => n.MA_DON_HANG == id);
              
                return View(sp);
            }
        }

        [HttpPost, ActionName("editDONHANG")]
        public ActionResult xacnhaneditDONHANG(int id , FormCollection collection)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                DON_DAT_HANG sp = data.DON_DAT_HANGs.SingleOrDefault(n => n.MA_DON_HANG == id);
                var tt = collection["tinhtrang"];
                var ttG = collection["tinhtrangG"];
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["ngaygiao"]);
                var ngaydat = String.Format("{0:MM/dd/yyyy}", collection["ngaydat"]);
                //Luu vao CSDL
                sp.NGAY_GIAO = DateTime.Parse(ngaygiao);
                sp.NGAY_DAT = DateTime.Parse(ngaydat);
                sp.DA_THANH_TOAN= Boolean.Parse(tt);
                sp.TINH_TRANG_GH = Boolean.Parse(ttG);
                UpdateModel(sp);
                data.SubmitChanges();
                return RedirectToAction("QLDONHANG", "Admin");

            }

        }

        


        public ActionResult DeleteDONHANG(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                DON_DAT_HANG sp = data.DON_DAT_HANGs.SingleOrDefault(n => n.MA_DON_HANG == id);
                return View(sp);
            }
        }

        [HttpPost, ActionName("DeleteDONHANG")]
        public ActionResult xacnhandeleDH(int id)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                DON_DAT_HANG sp = data.DON_DAT_HANGs.SingleOrDefault(n => n.MA_DON_HANG == id);
                
                //Luu vao CSDL
                data.DON_DAT_HANGs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLDONHANG", "Admin");

            }

        }

        public ActionResult QLCTDH(int id)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                var ctdh= from ctd in data.CT_DON_HANGs where ctd.MA_DON_HANG == id select ctd;
                return View(ctdh.OrderByDescending(n=>n.SL));
            }
        }
       
        public ActionResult QLTV(int? page, string tk = "")
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else if(tk !="")
            {
                    int pagesize = 4;
                   int pagenum = (page ?? 1);
                   var tkSDT = from ctd in data.KHACH_HANGs where ctd.SDT.ToUpper().Contains(tk.ToUpper()) select ctd;
                   return View(tkSDT.OrderByDescending(n=> n.MA_KH).ToPagedList(pagenum, pagesize));
               
            }
            else
            {
                //kích thước trang = số mẫu tin cho 1 trang
                int pagesize = 4;
                //Số thứ tự trang: nêu page là null thì pagenum =1, ngược lại pagenum=page
                int pagenum = (page ?? 1);
                return View(data.KHACH_HANGs.ToList().OrderByDescending(n => n.MA_KH).ToPagedList(pagenum, pagesize));
            }    
        }

        public ActionResult DeleteTV(int id)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                KHACH_HANG sp = data.KHACH_HANGs.SingleOrDefault(n => n.MA_KH == id);
                return View(sp);
            }
        }

        [HttpPost, ActionName("DeleteTV")]
        public ActionResult xacnhandeleTV(int id)
        {
            //if (Session["TaiKhoanADM"] == null)
            //{
            //    return RedirectToAction("DangNhapadm", "Admin");
            //}
            //else
            {
                KHACH_HANG sp = data.KHACH_HANGs.SingleOrDefault(n => n.MA_KH == id);

                //Luu vao CSDL
                data.KHACH_HANGs.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("QLTV", "Admin");

            }

        }

        


        // view SL partialView
        public ActionResult SLThanhViênpartiaview()
        {
            var QSCount = (from num in data.KHACH_HANGs select num).Count();
            return PartialView(Session["SLTHANHVIEN"] = QSCount);
        }
        public ActionResult SLDHpartiaview()
        {
            var QSCount = (from num in data.DON_DAT_HANGs select num).Count();
            return PartialView(Session["SLDH"] = QSCount);
        }

        public ActionResult Logout()
        {
            Session["TaiKhoanADM"] = null;//remove session
            return RedirectToAction("DangNhapadm", "Admin");
        }
        [HttpGet]
        public ActionResult Thongtinadmin(string id)
        {
            var ttADM = from sp in data.ADMINs where sp.TEN_DN_AD == id select sp;

            return View(ttADM.SingleOrDefault());
        }
        [HttpPost, ActionName("Thongtinadmin")]
        public ActionResult Thongtinadmin(FormCollection f, HttpPostedFileBase fileUpload)
        {
            if (Session["TaiKhoanADM"] == null)
            {
                return RedirectToAction("DangNhapadm", "Admin");
            }
            else
            {
                ADMIN sp = data.ADMINs.SingleOrDefault(n => n.TEN_DN_AD == Session["tendn-ADM"].ToString());


                ////Kiem tra duong dan file
                if (fileUpload== null)
                {

                    return View(sp);
                }
                ////Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileUpload.FileName);

                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("/img/avata"), fileName);

                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);

                        }
                           sp.HINH_AD= fileName;
                           sp.HO_TEN_AD = f["tenADM"];
                        //Luu vao CSDL
                        UpdateModel(sp);
                        data.SubmitChanges();
                        Session["ten-adm"] = sp.HO_TEN_AD;
                        Session["avatar-admin"] = sp.HINH_AD;
                        
                    }
                    return RedirectToAction("IndexAdmin", "Admin");
                   
                }
            }
        }
    }


}