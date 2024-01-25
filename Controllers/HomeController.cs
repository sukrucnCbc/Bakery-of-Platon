using Merve.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections;
using Microsoft.AspNetCore.Http;

namespace Merve.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About() 
        {
            
            return View();
        }
        public IActionResult Login(Users user)
        {
           
             string BaglantiAdresi = "Server=tcp:firinci.database.windows.net,1433;Initial Catalog=firin;Persist Security Info=False;User ID=firinci;Password=Merve123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
             SqlConnection con = new SqlConnection(BaglantiAdresi);
             SqlCommand cmd = new SqlCommand();
             cmd.Connection = con;
             //okuma
             cmd.CommandText = "SELECT * FROM kullanici";
             con.Open();
             cmd.ExecuteNonQuery();
             SqlDataReader dr = cmd.ExecuteReader();
            
            //List<Users> kullanicilar = new List<Users>();
             while (dr.Read() )
             {
                string tempUserPass = dr["sifre"].ToString().Trim();
                string tempUserMail = dr["ePosta"].ToString().Trim();
                bool userMailCompare = string.Equals(user.Email, tempUserMail);
                bool userPassCompare = string.Equals(user.Password, tempUserPass);

                if (userMailCompare&&userPassCompare)
                {
                    //Kullanıcı bilgileri doğru ise çalışacak yer.
                    user.FullName = dr["adSoyad"].ToString().Trim();
                    Response.Cookies.Append("login", user.FullName);
                    return RedirectToAction("Index");
                }
             }           
             dr.Close();
             con.Close();
            //Kullanıcı bilgileri yanlış ise çalışacak yer.
            return RedirectToAction("Error");
        }

        public IActionResult Register(Users user)
        {
            string BaglantiAdresi = "Server=tcp:firinci.database.windows.net,1433;Initial Catalog=firin;Persist Security Info=False;User ID=firinci;Password=Merve123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection con = new SqlConnection(BaglantiAdresi);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO kullanici (adSoyad, ePosta, sifre) VALUES('" + user.FullName + "'  , '" + user.Email + "', '" + user.Password + "'); ";
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }
        public IActionResult OrderIt(Order order)
        {
            string BaglantiAdresi = "Server=tcp:firinci.database.windows.net,1433;Initial Catalog=firin;Persist Security Info=False;User ID=firinci;Password=Merve123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlConnection con = new SqlConnection(BaglantiAdresi);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO siparisler (ad_soyad, tarih, adres,odemeYontemi,siparisOzeti,siparisTutari) VALUES('" + order.fullName + "','" + DateTime.Now + "'  , '" + order.Adress + "', '" + order.paymentType + "', '" + order.summary + "',  '" + 5 + "'); ";
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }
        public IActionResult Order()
        {
            string logineduser = Request.Cookies["login"];
            if (logineduser == null)
            {
                return Redirect("/Home/About#login");
            }
            return View();
        }
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("login");
            return RedirectToAction("Index");
        }
        public IActionResult Admin()
        {
            string logineduser = Request.Cookies["login"];
            if (logineduser == "Admin")
            {
                string BaglantiAdresi = "Server=tcp:firinci.database.windows.net,1433;Initial Catalog=firin;Persist Security Info=False;User ID=firinci;Password=Merve123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                SqlConnection con = new SqlConnection(BaglantiAdresi);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //okuma
                cmd.CommandText = "SELECT * FROM siparisler";
                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();

                List<Order> orders = new List<Order>();
                while (dr.Read())
                {
                    Order tempOrder = new Order();
                    tempOrder.Adress = dr["adres"].ToString().Trim();
                    tempOrder.amount = dr["siparisTutari"].ToString().Trim();
                    tempOrder.Date = dr["tarih"].ToString().Trim();
                    tempOrder.fullName = dr["ad_soyad"].ToString().Trim();
                    tempOrder.paymentType = dr["odemeYontemi"].ToString().Trim();
                    tempOrder.summary = dr["siparisOzeti"].ToString().Trim();
                    orders.Add(tempOrder);
                }
                dr.Close();
                con.Close();
                ViewData["orders"] = orders;
                return View();
            }
            return Redirect("/Home/About#login");
        }
    }
}
