using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteBanThietBiAmThanh.Models
{
    public class GioHang
    {
        //Tạo đối tượng data chứa dữ liệu từ model dbThietBiAmThanh đã tạo
        dbThietBiAmThanhDataContext data = new dbThietBiAmThanhDataContext();
        public int iIdSanPham { set; get; }
        public string sTenSanPham { set; get; }
        public string sHinhAnhSP { set; get; }
        public int iDonGia { set; get; }
        public int iSoLuong { set; get; }
        public int iThanhTien
        {
            get { return iSoLuong * iDonGia; }
        }
        //Khởi tạo giỏ hàng theo mã sản phẩm được truyền vào với soLuong mặc định là 1
        public GioHang(int idSanPham)
        {
            iIdSanPham = idSanPham;
            SanPham sp = data.SanPhams.Single(n => n.idSanPham == iIdSanPham);
            sTenSanPham = sp.tenSanPham;
            sHinhAnhSP = sp.hinhAnhSP;
            iDonGia = int.Parse(sp.giaSanPham.ToString());
            iSoLuong = 1;
        }
    }
}