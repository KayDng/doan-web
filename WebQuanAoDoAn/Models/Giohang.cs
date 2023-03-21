using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQuanAoDoAn.Models;

namespace WebQuanAoDoAn.Models
{
    public class Giohang
    {
        DbWebQuanAoDataContext data = new DbWebQuanAoDataContext();
        public int iMasp { set; get; }
        public string sTensp { set; get; }
        public string sAnhsp { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
       
        public Giohang(int Masp)
        {
            iMasp = Masp;
            SAN_PHAM sp = data.SAN_PHAMs.Single(n => n.MA_SP == iMasp);
            sTensp = sp.TEN_SP;
            sAnhsp = sp.HINH_ANH;
            dDongia = double.Parse(sp.GIA_BAN.ToString());
            iSoluong = 1;
        }
    }
}