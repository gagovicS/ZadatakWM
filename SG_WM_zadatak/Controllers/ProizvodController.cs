using Newtonsoft.Json;
using SG_WM_zadatak.Config;
using SG_WM_zadatak.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SG_WM_zadatak.Controllers
{
    
    public class ProizvodController : Controller
    {
        // GET: Proizvod
        
        //Baza
        public ActionResult PrikaziIzBaze()
        {
            List<Proizvod> proizvodi = new List<Proizvod>();

            using (SqlConnection con = new SqlConnection(Konekcija.GetKonekcija()))
            {
                using (SqlCommand cmd = new SqlCommand("select* from proizvodi", con))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    { con.Open(); }
                    SqlDataReader sdr = cmd.ExecuteReader();

                    DataTable dtProizvodi = new DataTable();
                    dtProizvodi.Load(sdr);
                    foreach (DataRow row in dtProizvodi.Rows)
                        proizvodi.Add(
                            new Proizvod
                            {
                                proizvodID = Convert.ToInt32(row["proizvodID"]),
                                naziv = row["naziv"].ToString(),
                                opis = row["opis"].ToString(),
                                kategorija = row["kategorija"].ToString(),
                                proizvodjac = row["proizvodjac"].ToString(),
                                dobavljac = row["dobavljac"].ToString(),
                                cena = Convert.ToDouble(row["cena"])
                            }
                            );
                    }
                }
            return View(proizvodi);           
        }
        public ActionResult Create()
        {

            return View(new Proizvod { proizvodID = 0 });
        }
        [HttpPost]
        public ActionResult Create(Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {
                string insertSQL = "INSERT INTO Proizvodi(naziv,opis,kategorija,proizvodjac,dobavljac,cena) VALUES('" + proizvod.naziv + "', '" + proizvod.opis + "','" + proizvod.kategorija + "','" + proizvod.proizvodjac + "','" + proizvod.dobavljac + "','" + proizvod.cena + "')";
                string updateSQL = "UPDATE Proizvodi SET naziv = '" + proizvod.naziv + "',opis= '" + proizvod.opis + "',kategorija= '" + proizvod.kategorija + "',proizvodjac= '" + proizvod.proizvodjac + "',dobavljac= '" + proizvod.dobavljac + "',cena= '" + proizvod.cena + "' WHERE proizvodID=  '" + proizvod.proizvodID + "' ";


                using (SqlConnection con = new SqlConnection(Konekcija.GetKonekcija()))
                {
                    using (SqlCommand cmd = new SqlCommand
                   ((proizvod.proizvodID > 0) ? updateSQL : insertSQL, con))
                    {
                        if (con.State != System.Data.ConnectionState.Open)
                        { con.Open(); }
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("PrikaziIzBaze");
            }
            
            return View("Create", proizvod);
        }
        public ActionResult Delete(int id)
        {
            if (id < 1) { return HttpNotFound(); }
            using (SqlConnection con = new SqlConnection(Konekcija.GetKonekcija()))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE  FROM Proizvodi WHERE proizvodID = '" + id + "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("PrikaziIzBaze");
            }
        }
        public ActionResult Edit(int id)
        {
            if (id < 1) { return HttpNotFound(); }
            var _proizvod = new Proizvod();
            using (SqlConnection con = new SqlConnection(Konekcija.GetKonekcija()))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Proizvodi WHERE proizvodID = '" + id + "' ", con))
                {
                    if (con.State != ConnectionState.Open) { con.Open(); }

                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    if (sdr.HasRows)
                    {
                        dt.Load(sdr);
                        DataRow row = dt.Rows[0];

                        _proizvod.proizvodID = Convert.ToInt32(row["proizvodID"]);
                        _proizvod.naziv = row["naziv"].ToString();
                        _proizvod.opis = row["opis"].ToString();
                        _proizvod.kategorija = row["kategorija"].ToString();
                        _proizvod.proizvodjac = row["proizvodjac"].ToString();
                        _proizvod.dobavljac = row["dobavljac"].ToString();
                        _proizvod.cena = Convert.ToDouble(row["cena"]);
                        return View("Create", _proizvod);
                    }
                    else { return HttpNotFound(); }
                }
            }
        }

        //JSON      
        public ActionResult PrikaziJson() {

            var WebClient = new WebClient();
            string json = WebClient.DownloadString(@"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json");
            ListaProizvoda proizvodi = new ListaProizvoda();
            proizvodi= JsonConvert.DeserializeObject<ListaProizvoda>(json);
           
            return View(proizvodi.listaj);
        }
        public ActionResult CreateJson()
        {

            return View(new Proizvod { proizvodID = 0 });
        }
        [HttpPost]
        public ActionResult CreateJson(Proizvod proizvod)
        {
            if (ModelState.IsValid)
            {
                var WebClient = new WebClient();
                string json = WebClient.DownloadString(@"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json");
                ListaProizvoda proizvodi = new ListaProizvoda();
                proizvodi = JsonConvert.DeserializeObject<ListaProizvoda>(json);
                
                if (proizvod.proizvodID == 0)
                {
                    Proizvod pr = proizvodi.listaj.Last();
                    int id = pr.proizvodID + 1;
                    proizvod.proizvodID = id;
                    proizvodi.listaj.Add(proizvod);
                    var pisiJson = JsonConvert.SerializeObject(proizvodi);
                    string fileName = @"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json";
                    System.IO.File.WriteAllText(fileName, pisiJson);
                }
                else{
                    foreach (Proizvod pr in proizvodi.listaj) {
                        if (proizvod.proizvodID == pr.proizvodID) {
                            pr.naziv = proizvod.naziv;
                            pr.opis = proizvod.opis;
                            pr.kategorija = proizvod.kategorija;
                            pr.proizvodjac = proizvod.proizvodjac;
                            pr.dobavljac = proizvod.dobavljac;
                            pr.cena = proizvod.cena;
                        }
                    }
                    var pisiJson = JsonConvert.SerializeObject(proizvodi);
                    string fileName = @"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json";
                    System.IO.File.WriteAllText(fileName, pisiJson);
                }

                return RedirectToAction("PrikaziJson");
            }
             return View("CreateJson", proizvod);
        }
        public ActionResult EditJson(int id) {
            var WebClient = new WebClient();
            string json = WebClient.DownloadString(@"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json");
            ListaProizvoda proizvodi = new ListaProizvoda();
            proizvodi = JsonConvert.DeserializeObject<ListaProizvoda>(json);
            Proizvod vrati = new Proizvod();
            foreach (Proizvod pr in proizvodi.listaj)
            {
                if (id == pr.proizvodID)
                {
                    vrati.proizvodID = pr.proizvodID;
                    vrati.naziv = pr.naziv; 
                    vrati.opis = pr.opis;
                    vrati.kategorija= pr.kategorija;
                    vrati.proizvodjac= pr.proizvodjac;
                    vrati.dobavljac= pr.dobavljac;
                    vrati.cena= pr.cena;
                }
            }


            return View("CreateJson",vrati);
        }
        public ActionResult DeleteJson(int id) {
            var WebClient = new WebClient();
            string json = WebClient.DownloadString(@"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json");
            ListaProizvoda proizvodi = new ListaProizvoda();
            proizvodi = JsonConvert.DeserializeObject<ListaProizvoda>(json);

            foreach (Proizvod pr in proizvodi.listaj)
            {
                if (id == pr.proizvodID)
                {
                    proizvodi.listaj.Remove(pr);
                    break;
                }
            }
            var pisiJson = JsonConvert.SerializeObject(proizvodi);
            string fileName = @"C:\Users\gagov\source\repos\SG_WM_zadatak\SG_WM_zadatak\JsonFile\rezultat.json";
            System.IO.File.WriteAllText(fileName, pisiJson);

            return RedirectToAction("PrikaziJson");
        }
    }
}