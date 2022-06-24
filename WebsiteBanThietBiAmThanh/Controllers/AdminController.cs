using WebsiteBanThietBiAmThanh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebsiteBanThietBiAmThanh.Controllers
{
    public class AdminController : Controller
    {
        dbThietBiAmThanhDataContext db = new dbThietBiAmThanhDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult  Sanpham(int ? page)
         {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SanPhams.ToList());
           return View(db.SanPhams.ToList().OrderBy(n=>n.idSanPham).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {

            //dua du lieu vao dropdownlist
            //lay ds tu table chu de, sap xep tang dan theo ten chu de, chon lay gia tri ma CD, hien thi ten CD
            ViewBag.idLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.tenLoai), "idLoai", "tenLoai");
            ViewBag.idThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.hinhAnhTH), "idThuongHieu", "hinhAnhTH");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisanpham(SanPham sanPham,HttpPostedFileBase fileUpload)
        { 
            ViewBag.idLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.tenLoai), "idLoai", "tenLoai");
            ViewBag.idThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.hinhAnhTH), "idThuongHieu", "hinhAnhTH");

            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh ";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                //luu ten file
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        // luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanPham.hinhAnhSP = fileName;
                    //luu vao CSDL
                    db.SanPhams.InsertOnSubmit(sanPham);
                    db.SubmitChanges();
                }


                return RedirectToAction("Sanpham");

            }
        }
        [HttpGet]
        public  ActionResult Login ()
        {
            return View();
        }
        [HttpPost]
         public ActionResult Login(FormCollection collection)
        {
            //gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";

            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }   
            else
            {
                //gán  giá trị cho đối tượng được tạo mới(ad)
                Admin nv = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (nv != null)
                {
                    //ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = nv;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
            
        }

        //Hiển thị sản phẩm

        public ActionResult Chitietsanpham(int id)
        {
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.idSanPham == id);
            ViewBag.idSanPham = sanPham.idSanPham;
            if(sanPham==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);
        }
       //Xoa san pham
       [HttpGet]
       public ActionResult Xoasanpham(int id)
        {
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.idSanPham == id);
            ViewBag.idSanPham = sanPham.idSanPham;
            if(sanPham==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanPham);

        }
        [HttpPost,ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong can xoa theo ma
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.idSanPham == id);
            ViewBag.idSanPham = sanPham.idSanPham;
            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SanPhams.DeleteOnSubmit(sanPham);
            db.SubmitChanges();
            return RedirectToAction("SanPham");
        }


        //chinh sua san pham
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            //lay ra doi tuong san pham theo ma
            SanPham sanPham = db.SanPhams.SingleOrDefault(n => n.idSanPham == id);
            if (sanPham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.idLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.tenLoai), "idLoai", "tenLoai");
            ViewBag.idThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.hinhAnhTH), "idThuongHieu", "hinhAnhTH");
            return View(sanPham);
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(int id, FormCollection collection,HttpPostedFileBase fileUpload)
        {
            var sanpham = db.SanPhams.FirstOrDefault(n => n.idSanPham == id);
            sanpham.idSanPham = id;
            //Dua du lieu vao dropdownload
            ViewBag.idLoai = new SelectList(db.LoaiSanPhams.ToList().OrderBy(n => n.tenLoai), "idLoai", "tenLoai");
            ViewBag.idThuongHieu = new SelectList(db.ThuongHieus.ToList().OrderBy(n => n.hinhAnhTH), "idThuongHieu", "hinhAnhTH");
            //kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(sanpham);
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //luu ten file
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))                  
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";            
                    else
                    {
                        // luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    sanpham.hinhAnhSP = fileName;
                    sanpham.idSanPham = id;
                    //luu vao CSDL
                    UpdateModel(sanpham);
                    db.SubmitChanges();
                    return RedirectToAction("SanPham");
                }
                return this.Suasanpham(id);

            }
        }    

        }
    }
