using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanThietBiAmThanh.Models;

using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace WebsiteBanThietBiAmThanh.Controllers
{
    public class ThietBiAmThanhController : Controller
    {
        //Tạo 1 đối tượng chứa toàn bộ CSDL từ dbBanThietBiAmThanh
        dbThietBiAmThanhDataContext data = new dbThietBiAmThanhDataContext();

        private List<SanPham> LaySanPham(int count)
        {
            return data.SanPhams.OrderByDescending(a => a.idSanPham).Take(count).ToList();
        }
        // GET: ThietBiAmThanh
        public ActionResult Index(int ? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang 
            int pageSize = 4;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            //lấy 12 sản phẩm theo id
            var sanphammoi = LaySanPham(12);
            return View(sanphammoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult LoaiSanPham()
        {
            var loaisanpham = from lsp in data.LoaiSanPhams select lsp;
            return PartialView(loaisanpham);
        }
        public ActionResult ThuongHieu()
        {
            var thuonghieu = from th in data.ThuongHieus select th;
            return PartialView(thuonghieu);
        }
        public ActionResult SPTheoLoai(int id, string searchString)
        {
            var sanpham = from sp in data.SanPhams where sp.idLoai == id select sp;
            if (!String.IsNullOrEmpty(searchString))
            {
                sanpham = sanpham.Where(s => s.tenSanPham.Contains(searchString));
            }
            return View(sanpham);
        }
        public ActionResult SPTheoThuongHieu(int id, string searchString)
        {
            var sanpham = from sp in data.SanPhams where sp.idThuongHieu == id select sp;
            if (!String.IsNullOrEmpty(searchString))
            {
                sanpham = sanpham.Where(s => s.tenSanPham.Contains(searchString));
            }
            return View(sanpham);
        }
        public ActionResult Details(int id)
        {
            var sanpham = from sp in data.SanPhams where sp.idSanPham == id select sp;
            return View(sanpham.Single());
        }
        [HttpGet]
        public ActionResult BinhLuan()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult BinhLuan(FormCollection collection)
        {
            BinhLuan binhluan = new BinhLuan();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            var noidungbl = collection["noiDungBL"];

            binhluan.idKhachHang = kh.idKhachHang;
            binhluan.noiDungBL = noidungbl;
            binhluan.hoTenNguoiBL = kh.hoTenKH;
            if (ModelState.IsValid)
            {
                data.BinhLuans.InsertOnSubmit(binhluan);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult LienHe()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult LienHe(FormCollection collection)
        {
            LienHe lienhe = new LienHe();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            var noidunglh = collection["noiDungLH"];

            lienhe.idKhachHang = kh.idKhachHang;
            lienhe.noiDungLH = noidunglh;
            if (ModelState.IsValid)
            {
                data.LienHes.InsertOnSubmit(lienhe);
                data.SubmitChanges();
            }
            return RedirectToAction("Index");
        }
        private List<BinhLuan> DSBinhLuan(int count)
        {
            return data.BinhLuans.OrderByDescending(a => a.idBinhLuan).Take(count).ToList();
        }
        public ActionResult HienThiBinhLuan()
        {
            var binhluanmoi = DSBinhLuan(3);
            return View(binhluanmoi);
        }

        public List<TinTuc> DSTinTuc(int count)
        {
            return data.TinTucs.OrderByDescending(a => a.idTinTuc).Take(count).ToList();
        }

        public ActionResult HienThiTinTuc()
        {
            var tintucmoi = DSTinTuc(4);
            return View(tintucmoi);
        }
        public ActionResult ChiTietTinTuc(int id)
        {
            var tintuc = from tt in data.TinTucs where tt.idTinTuc == id select tt;
            return View(tintuc.Single());
        }
    }
}