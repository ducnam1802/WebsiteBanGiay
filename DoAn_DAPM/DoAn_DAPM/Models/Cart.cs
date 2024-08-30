using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn_DAPM.Models
{
    public class Cart
    {
        QlyGiayEntities db = new QlyGiayEntities();
        public String Hinh { get; set; }
        public int MaSp { get; set; }
        public String TenSp { get; set; }
        public double GiaSP { get; set; }
        public int Soluong { get; set; }
        public int soLuongTrongKho { get; set; }
        public double ThanhTien
        {
            get
            {
                return Soluong * GiaSP;
            }
        }
        public Cart(int MaSp)
        {
            this.MaSp = MaSp;
            var sanpham = db.SanPhams.Single(s => s.MaSP == this.MaSp);
            this.Hinh = sanpham.SanPham_Anh.FirstOrDefault()?.Anh;
            this.TenSp = sanpham.TenSP;
            this.GiaSP = double.Parse(sanpham.GiaSP.ToString());
            this.soLuongTrongKho = sanpham.SoLuong;
            this.Soluong = 1;


        }

    }
}