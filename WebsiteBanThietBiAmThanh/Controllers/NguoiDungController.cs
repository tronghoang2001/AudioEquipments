using WebsiteBanThietBiAmThanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanThietBiAmThanh.Controllers
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
            var matkhau = MaHoa.GetMD5(collection["matKhauKH"]);
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
                kh.matKhauKH = MaHoa.GetMD5(matkhau);
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
            var matkhau = MaHoa.GetMD5(collection["matKhauKH"]);
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
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.taiKhoanKH == tendn && n.matKhauKH == MaHoa.GetMD5(matkhau));
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

        public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            Session.Clear();
            return RedirectToAction("Index", "ThietBiAmThanh");
        }
        public ActionResult KhachHang()
        {
            var khachhang = from kh in db.KhachHangs select kh;
            return PartialView(khachhang);
        }
        //GET
        public ActionResult DoiMatKhau(int id)
        {
            var khachhang = db.KhachHangs.FirstOrDefault(n => n.idKhachHang == id);
            return View(khachhang);
        }
        //POST
        [HttpPost]
        public ActionResult DoiMatKhau(int id, FormCollection collection)
        {
            //Tạo 1 biến khachhang với đối tương id = id truyền vào
            var khachhang = db.KhachHangs.First(n => n.idKhachHang == id);
            var matkhaumoi = MaHoa.GetMD5(collection["matKhauMoi"]);
            var nhaplaimatkhaumoi = collection["nhapLaiMKMoi"];
            khachhang.idKhachHang = id;
            //Nếu người dùng không nhập mk mới và nhập lại mk mới
            if (string.IsNullOrEmpty(matkhaumoi))
            {
                ViewData["Loi1"] = "Chưa nhập mật khẩu mới!";
            }
            if (string.IsNullOrEmpty(nhaplaimatkhaumoi))
            {
                ViewData["Loi2"] = "Chưa nhập lại mật khẩu mới!";
            }
            else
            {
                khachhang.matKhauKH = MaHoa.GetMD5(matkhaumoi);
                //Update trong CSDL
                UpdateModel(khachhang);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DoiMatKhau(id);
        }

        //GET
        public ActionResult SuaThongTin(int id)
        {
            var khachhang = db.KhachHangs.FirstOrDefault(n => n.idKhachHang == id);
            return View(khachhang);
        }
        //POST
        [HttpPost]
        public ActionResult SuaThongTin(int id, FormCollection collection)
        {
            //Tạo 1 biến khachhang với đối tương id = id truyền vào
            var khachhang = db.KhachHangs.First(n => n.idKhachHang == id);
            var hoten = collection["hoTenKH"];
            var sdt = collection["sdtKH"];
            var diachi = collection["diaChi"];
            var taikhoan = collection["taikhoan"];
            khachhang.idKhachHang = id;
            //Nếu người dùng không nhập mk mới và nhập lại mk mới
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Chưa nhập họ tên!";
            }
            if (string.IsNullOrEmpty(sdt))
            {
                ViewData["Loi2"] = "Chưa nhập số điện thoại!";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi3"] = "Chưa nhập địa chỉ!";
            }
            if (string.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi4"] = "Chưa nhập tài khoản!";
            }
            else
            {
                khachhang.hoTenKH = hoten;
                khachhang.soDienThoaiKH = sdt;
                khachhang.diaChiKH = diachi;
                khachhang.taiKhoanKH = taikhoan;
                //Update trong CSDL
                UpdateModel(khachhang);
                db.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.SuaThongTin(id);
        }
        public ActionResult ThongTinKhachHang()
        {
            //Kiểm tra đăng nhập
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return PartialView();
        }
    }
}