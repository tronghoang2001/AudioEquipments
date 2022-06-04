using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyễnTrọngHoàng_WebThiếtBịÂmThanh.Models;

namespace NguyễnTrọngHoàng_WebThiếtBịÂmThanh.Controllers
{
    public class GioHangController : Controller
    {
        //Tạo đối tượng data chứa dữ liệu từ model dbThietBiAmThanh
        dbThietBiAmThanhDataContext data = new dbThietBiAmThanhDataContext();
        //Lấy giỏ hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì khởi tạo listGioHang
                lstGioHang = new List<GioHang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iIDSanPham, string strURL)
        {
            //Lấy ra Session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm này có tồn tại trong Session["Giohang"] chưa?
            GioHang sanpham = lstGioHang.Find(n => n.iIdSanPham == iIDSanPham);
            if (sanpham == null)
            {
                sanpham = new GioHang(iIDSanPham);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        //Tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        //Tổng tiền
        private int TongTien()
        {
            int iTongTien = 0;
            List<GioHang> lstGioHang = Session["Giohang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.iThanhTien);
            }
            return iTongTien;
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if(lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "ThietBiAmThanh");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }
        //Tạo PartialView để hiển thị thông tin giỏ hàng
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Lấy giỏ hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sản phẩm đã có trong giỏ hàng Session["Giohang"]?
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iIdSanPham == iMaSP);
            //Nếu tồn tại thì sửa soLuong
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iIdSanPham == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "ThietBiAmThanh");
            }
            return RedirectToAction("GioHang");
        }
        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang (int iMaSP, FormCollection f)
        {
            //Lấy giỏ hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sách đã có trong Session["Giohang"]?
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iIdSanPham == iMaSP);
            //Nếu tồn tại thì cho sửa soLuong
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        //Xóa tất cả giỏ hàng
        public ActionResult XoaTatCaGioHang()
        {
            //Lấy giỏ hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "ThietBiAmThanh");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra đã đăng nhập chưa?
            if(Session["Taikhoan"]==null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "ThietBiAmThanh");
            }
            //Lấy giỏ hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            //Thêm đơn hàng
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<GioHang> gh = LayGioHang();
            var tennguoinhan = collection["tenNguoiNhan"];
            var diachinguoinhan = collection["diaChiNguoiNhan"];
            var sđtnguoinhan = collection["sđtNguoiNhan"];
            var ghichudonhang = collection["ghiChuDH"];

            dh.idKhachHang = kh.idKhachHang;
            dh.ngayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            dh.ngayGiao = DateTime.Parse(ngaygiao);
            dh.tenNguoiNhan = tennguoinhan;
            dh.diaChiNguoiNhan = diachinguoinhan;
            dh.sdtNguoiNhan = sđtnguoinhan;
            dh.ghiChuDH = ghichudonhang;
            dh.ThanhTien = TongTien();
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            //thêm chi tiết đơn hàng
            foreach(var item in gh)
            {
                CTDH ctdh = new CTDH();
                ctdh.idDonHang = dh.idDonHang;
                ctdh.idSanPham = item.iIdSanPham;
                ctdh.soLuong = item.iSoLuong;
                ctdh.donGia = item.iDonGia;            
                data.CTDHs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}