using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BAI3.Models;
using BAI3.Context;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Collections.ObjectModel;
using System.Text;

namespace BAI3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaiContext baiContext;
        public int attempts = 0;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            var optionsBuilder = new DbContextOptionsBuilder<BaiContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BaiConnection"));
            baiContext = new BaiContext(optionsBuilder.Options);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Rejestracja()
        {
            return View();
        }
        public int ChangeIntTabToInt(int[] tab)
        {
            int value = 0;

            for (int i = tab.Length - 1; i >= 0; i--)
            {
                if (tab[i] == 1)
                {
                    value += (int)Math.Pow(2, tab.Length - i - 1);
                }
            }
            return value;
        }
        public string ChangeIntListToString(List<int> list)
        {
            string value = "";
            foreach (var a in list)
            {
                value += a + ";";
            }
            return value;
        }
        public void GeneratePasswordSchema(User user, int userId)
        {
            string schema = "";
            Random random = new Random();
            string password = user.haslo;
            int halfPasswordSize = password.Length / 2;
            int schemaSize = 5;
            int[] schemaTabRepresentation = new int[password.Length];
            int repeatCount = 0;
            int place;
            int value;
            List<int> schemaList = new List<int>();
            if (halfPasswordSize > 5)
                schemaSize = random.Next(5, halfPasswordSize);

            while (schemaList.Count != 10)
            {
                while (repeatCount != schemaSize)
                {
                    place = random.Next(0, password.Length - 1);
                    if (schemaTabRepresentation[place] != 1)
                    {
                        schemaTabRepresentation[place] = 1;
                        repeatCount++;
                    }
                }
                value = ChangeIntTabToInt(schemaTabRepresentation);
                if (!schemaList.Contains(value))
                {
                    schemaList.Add(value);
                }
                repeatCount = 0;
                schemaTabRepresentation = new int[password.Length];
            }

            schema = ChangeIntListToString(schemaList);
            FragmentalPasswordSchema fragmentalPasswordSchema = new FragmentalPasswordSchema(userId, schema, password.Length);
            baiContext.FragmentalPasswordSchemas.Add(fragmentalPasswordSchema);
        }

        [HttpPost]
        public IActionResult Rejestracja(User user)
        {
            User newUser = new User(user.login, GenerateHash(user.haslo), user.imie, user.nazwisko);
            baiContext.Users.Add(newUser);
            baiContext.SaveChanges();
            User userWithSetId = baiContext.Users.Where(x => x.login == user.login).FirstOrDefault();
            GeneratePasswordSchema(user, userWithSetId.userId);
            baiContext.SaveChanges();
            return RedirectToAction("Logowanie");
        }
        public IActionResult Logowanie()
        {



            return View();
        }
        [HttpPost]
        [DelayFilter]
        public IActionResult Logowanie(User user)
        {
            User userWithGivenLogin = baiContext.Users.Where(x => x.login == user.login).FirstOrDefault();
            if (userWithGivenLogin == null)
            {
                LogowanieZdarzen noweZdarzenie = null;
                try { noweZdarzenie = baiContext.LogowanieZdarzens.Where(x => x.Login == user.login && x.haslo == user.haslo).FirstOrDefault(); }
                catch (Exception e) { }
                if (noweZdarzenie == null)
                {
                    noweZdarzenie = new LogowanieZdarzen(user.login, user.haslo, 1, DateTime.Now);
                    baiContext.LogowanieZdarzens.Add(noweZdarzenie);

                }
                else
                {
                    noweZdarzenie.iloscNieudanychLogowan++;
                    noweZdarzenie.ostatniaProbaLogowania = DateTime.Now;
                    baiContext.LogowanieZdarzens.Update(noweZdarzenie);

                }
                baiContext.SaveChanges();
                ModelState.AddModelError("Error", "Nie udana próba logowania!");
                return View();

            }
            HttpContext.Session.SetString("_UserLogin", user.login);
            return RedirectToAction("ActualLogin");
        }
        public int[] ChangeIntToIntTab(int length, int value)
        {
            int[] tab = new int[length];
            int pom = value;
            int reszta;
            int i = 0;
            while (pom != 0)
            {
                reszta = pom % 2;
                pom = pom / 2;
                tab[i++] = reszta;
            }

            return tab;
        }
        public bool CheckPassword(FragmentalPasswordSchema schema, string password, User user)
        {
            string[] w = schema.schema.Split(';');
            string userPassword = Decrypt(user.haslo);
            int[] intTabSchema = ChangeIntToIntTab(schema.passwordSice, Int32.Parse(w[schema.actualPasswordSchema]));
            for (int i = 0; i < schema.passwordSice; i++)
            {
                if (intTabSchema[i] == 1)
                {
                    if (password[i] != userPassword[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public string ChangeIntTabToString(int[] tab)
        {
            string value = "";
            for (int i = 0; i < tab.Length; i++)
            {
                value += tab[i] + "";
            }
            return value;
        }
        public IActionResult ActualLogin()
        {
            User user = baiContext.Users.Where(x => x.login == HttpContext.Session.GetString("_UserLogin")).FirstOrDefault();
            FragmentalPasswordSchema fragmentalPasswordSchema = baiContext.FragmentalPasswordSchemas.Where(x => x.userId == user.userId).FirstOrDefault();
            string[] w = fragmentalPasswordSchema.schema.Split(';');
            ViewBag.schema = ChangeIntTabToString(ChangeIntToIntTab(fragmentalPasswordSchema.passwordSice, Int32.Parse(w[fragmentalPasswordSchema.actualPasswordSchema])));
            return View();
        }
        [HttpPost]
        [DelayFilter]
        public IActionResult ActualLogin([FromBody] string data)
        {

            LoginAttepmts newLoginAttempt;
            User user = baiContext.Users.Where(x => x.login == HttpContext.Session.GetString("_UserLogin")).FirstOrDefault();
            FragmentalPasswordSchema fragmentalPasswordSchema = baiContext.FragmentalPasswordSchemas.Where(x => x.userId == user.userId).FirstOrDefault();
            while (data.Length != fragmentalPasswordSchema.passwordSice)
            {
                data += " ";
            }
            if (CheckPassword(fragmentalPasswordSchema, data, user))
            {
                fragmentalPasswordSchema.actualPasswordSchema = fragmentalPasswordSchema.actualPasswordSchema == 9 ? 0 : ++fragmentalPasswordSchema.actualPasswordSchema;
                baiContext.FragmentalPasswordSchemas.Update(fragmentalPasswordSchema);
                baiContext.SaveChanges();
                if (baiContext.LoginAttempts.ToList().Count == 0)
                {
                    newLoginAttempt = new LoginAttepmts(0);
                }
                else
                {
                    newLoginAttempt = baiContext.LoginAttempts.First();
                }
                newLoginAttempt.attempt++;
                baiContext.LoginAttempts.Update(newLoginAttempt);
                if (baiContext.Users.Any())
                {
                    if (user.blokada == "false")
                    {
                        newLoginAttempt.attempt = 0;
                        baiContext.LoginAttempts.Update(newLoginAttempt);
                        user.dataOstatniegoUdanegoLogowania = DateTime.Now;
                        HttpContext.Session.SetString("_UserImie", user.imie);
                        HttpContext.Session.SetString("_UserNazwisko", user.nazwisko);
                        HttpContext.Session.SetString("_UserLogin", user.login);
                        HttpContext.Session.SetString("_UserId", user.userId.ToString());
                        baiContext.Users.Update(user);
                        baiContext.SaveChanges();


                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Konto zablokowane!");
                        return View();
                    }
                }
            }
            else
            {
                user.iloscNieUdanychProbLogowania++;
                user.dataOstatniegoNieudanegoLogowania = DateTime.Now;
                user.blokada = (user.wlaczenieBlokadyKonta == "true") && (user.iloscNieUdanychProbLogowania >= 5) ? "true" : "false";
                ModelState.AddModelError("Error", "Nie udana próba logowania!");
                baiContext.Users.Update(user);
                baiContext.SaveChanges();
                return View();

            }
            return RedirectToAction("Index");
        }
        public IActionResult Panel()
        {
            var lista = baiContext.LogowanieZdarzens.ToList();
            return View(lista);
        }
        public IActionResult Wyloguj(int id)
        {
            var user = baiContext.Users.Where(x => x.userId == id).FirstOrDefault();
            user.iloscNieUdanychProbLogowania = 0;
            baiContext.Users.Update(user);
            baiContext.SaveChanges();
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Konto()
        {
            var user = baiContext.Users.Where(x => x.userId.ToString() == HttpContext.Session.GetString("_UserId")).FirstOrDefault();
            user.haslo = Decrypt(user.haslo);
            return View(user);
        }
        public IActionResult BlokadaKonta(int id)
        {
            ViewBag.id = id;
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public IActionResult ChangeUserPassword(ChangePassword changePassword)
        {
            var user = baiContext.Users.Where(x => x.userId == Int32.Parse(HttpContext.Session.GetString("_UserId"))).FirstOrDefault();
            if (user != null)
            {
                if (changePassword.haslo != Decrypt(user.haslo))
                {
                    ModelState.AddModelError("Error", "Nie udana próba logowania!");
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    user.haslo = GenerateHash(changePassword.Nowehaslo);
                    baiContext.Users.Update(user);
                    FragmentalPasswordSchema fragmentalPasswordSchema = baiContext.FragmentalPasswordSchemas.Where(x => x.userId == user.userId).FirstOrDefault();
                    baiContext.FragmentalPasswordSchemas.Remove(fragmentalPasswordSchema);
                    User newUser = new User(user.login, Decrypt(user.haslo), user.imie, user.nazwisko);
                    GeneratePasswordSchema(newUser, user.userId);
                    baiContext.SaveChanges();
                }

            }
            return RedirectToAction("Konto");
        }
        public IActionResult OdbKonta(int id)
        {
            ViewBag.id = id;
            return View();
        }
        public IActionResult BlokowanieKonta(int id)
        {
            var user = baiContext.Users.Where(x => x.userId == id).First();
            user.wlaczenieBlokadyKonta = "true";
            baiContext.Users.Update(user);
            baiContext.SaveChanges();
            return RedirectToAction("Konto");
        }
        public IActionResult OdblokowanieKonta(int id)
        {
            var user = baiContext.Users.Where(x => x.userId == id).FirstOrDefault();
            user.wlaczenieBlokadyKonta = "false";
            baiContext.Users.Update(user);
            baiContext.SaveChanges();
            return RedirectToAction("Konto");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private static string GenerateHash(string value)
        {
            using (var md5 = MD5.Create())
            {
                using (var tdes = TripleDES.Create())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes("MarekKacper"));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(value);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }
        private static string Decrypt(string cypher)
        {
            using (var md5 = MD5.Create())
            {
                using (var tdes = TripleDES.Create())
                {
                    tdes.Key = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes("MarekKacper"));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cypher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return System.Text.Encoding.ASCII.GetString(bytes);
                    }
                }
            }
        }
        private string generatePartialPasswordSchema(string password)
        {
            string partialPasswordSchema = "";



            return partialPasswordSchema;
        }

    }
}
