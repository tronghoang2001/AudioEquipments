using NguyễnTrọngHoàng_WebThiếtBịÂmThanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyễnTrọngHoàng_WebThiếtBịÂmThanh.Controllers
{
    public class NguoiDungController : Controller
    {
        dbThietBiAmThanhDataContext db = new dbThietBiAmThanhDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        // POST: Hàm DangKy(post) nhận dữ liệu từ trang DangKy và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var hoten = collection["hoTenKH"];
            var tendn = collection["taiKhoanKH"];
            var matkhau = collection["matKhauKH"];
            var nhaplaimatkhau = collection["nhapLaiMK"];
            var sodienthoai = collection["soDienThoaiKH"];
            var diachi = collection["diaChiKH"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên không được trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Tên đăng nhập không được trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được trống";
            }
            if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(sodienthoai))
            {
                ViewData["Loi5"] = "Số điện không được trống";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được trống";
            }
            else
            {
                kh.hoTenKH = hoten;
                kh.taiKhoanKH = tendn;
                kh.matKhauKH = matkhau;
                kh.soDienThoaiKH = sodienthoai;
                kh.diaChiKH = diachi;
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            //Gán giá trị người dùng nhập liệu cho các biến
            var tendn = collection["taiKhoanKH"];
            var matkhau = collection["matKhauKH"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Bạn phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Bạn phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.taiKhoanKH == tendn && n.matKhauKH == matkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công!";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("GioHang", "GioHang");
                }
                else ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            }
            return View();
        }
    }
}