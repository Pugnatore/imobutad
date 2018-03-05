using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Projeto_Lab4.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Projeto_Lab4.Controllers
{
    public class ProprietarioController : Controller
    {

        // GET: Proprietario
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OsmeusImoveis(string id)
        {
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Inquilino inq = new Inquilino();
            Imovel im = new Imovel();
            List<Imovel> imo = new List<Imovel>();
            List<int> ids = new List<int>();
            inq = db.Inquilinos.Where(X => X.ID_Inquilino == id).FirstOrDefault();
            Proprietario prop = new Proprietario();
            prop = db.Proprietarios.Where(X => X.ID_Proprietario == User.Identity.GetUserId()).FirstOrDefault();
            if (inq != null)
            {
                ViewBag.foto = inq.Foto_perfil;
                TempData["tipo"] = "inq";
            }
            if (prop != null)
            {
                ViewBag.foto = prop.Foto_perfil;
                TempData["tipo"] = "Prop";
            }
            if (inq != null)
            {
                foreach (var item in db.Alugas)
                {
                    if (item.ID_Inquilino == User.Identity.GetUserId())
                    {
                        ids.Add(item.ID_Imovel);
                    }
                }

            }
            else
            {
                foreach (var item in db.PossuiImovels)
                {
                    if (item.ID_Proprietario == id)
                    {
                        ids.Add(item.ID_Imovel);
                    }
                }
            }

            foreach (var item in ids)
            {
                im= db.Imovels.Where(x => x.ID_Imovel == item).FirstOrDefault();
                imo.Add(im);
            }

            return View("OsmeusImoveis",imo);
        }
        public ActionResult Verperfilinquilino(int id)
        {
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Inquilino inq = new Inquilino();
            Aluga al = new Aluga();
            Compra cp = new Compra();
            string idinquilno;
            al = db.Alugas.Where(x => x.ID_Imovel == id).FirstOrDefault();
            if (al == null)
            {
                cp = db.Compras.Where(x => x.ID_Imovel == id).FirstOrDefault();
                if (cp!= null)
                {
                    idinquilno = al.ID_Inquilino;
                    inq = db.Inquilinos.Where(x => x.ID_Inquilino == idinquilno).FirstOrDefault();
                    return View("PerfilParaProp", inq);
                }
                else
                {
                    TempData["aaa"] = "O perfil não existe porque o imovel não se encontra arrendado nem vendido";
                    return View("PerfilParaProp", inq);
                }

            }
            else
            {
                idinquilno = al.ID_Inquilino;
                inq = db.Inquilinos.Where(x => x.ID_Inquilino == idinquilno).FirstOrDefault();
                return View("PerfilParaProp", inq);
            }
            
        }

        public ActionResult Verperfilproprietario(int id)
        {
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Proprietario prop = new Proprietario();
            Aluga al = new Aluga();
            string idprop;
            al = db.Alugas.Where(x => x.ID_Imovel == id).FirstOrDefault();
            if (al == null)
            {
               
                    TempData["aaa"] = "O perfil não existe porque o imovel não se encontra arrendado nem vendido";
                    return View("PerfilParaInq",prop);
                
           
            }
            else
            {
                idprop = al.ID_Proprietario;
                prop = db.Proprietarios.Where(x => x.ID_Proprietario == idprop).FirstOrDefault();
                return View("PerfilParaInq", prop);
            }

        }
        public ActionResult RegistarAlguer(int id)
        {
            int idimovel = id;
            TempData["idimovel"] = idimovel;
            return View("RegistarAlguer");
        }
        [HttpPost]
        public ActionResult RegistarAlguer(FormCollection form)
        {
            int idimovel = Convert.ToInt32(TempData["idimovel"]);
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Imovel imo = new Imovel();
            Inquilino inq = new Inquilino();
            imo = db.Imovels.Where(x => x.ID_Imovel == idimovel).FirstOrDefault();
            inq = db.Inquilinos.Where(x => x.Email == form["Email"]).FirstOrDefault();
            if (inq == null)
            {
                ModelState.AddModelError("Email", "Nao foi encontrado nenhum utilizador com e email que introduziu");
                return View("RegistarAlguer");
            }
            else
            {
                imo.Estado = 2;
                db.SubmitChanges();
                Aluga al = new Aluga();
                al.Data_Aluguer_Inicio = DateTime.Now;
                al.ID_Imovel = idimovel;
                al.ID_Inquilino = inq.ID_Inquilino;
                al.Data_Aluguer_Final = Convert.ToDateTime(form["Data_Aluguer_Final"]);
                al.Valor = Convert.ToInt32(form["Valor"]);
               al.ID_Proprietario = User.Identity.GetUserId();
              
                db.Alugas.InsertOnSubmit(al);
                db.SubmitChanges();
                TempData["Sucesso"] = "O aluguer foi registado com sucesso!!";
                return RedirectToAction("OsmeusImoveis",new {id=User.Identity.GetUserId() });
            }
            
        }
        
        public ActionResult RegistarVenda(int id)
        {
            int idimovel = id;
            TempData["idimovelV"] = idimovel;
            return View("RegistarVenda");
            
        }

        [HttpPost]
        public ActionResult RegistarVenda(FormCollection form)
        {

            int idimovel = Convert.ToInt32(TempData["idimovelV"]);
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Imovel imo = new Imovel();
            PossuiImovel pi = new PossuiImovel();
            Inquilino inq = new Inquilino();
            imo = db.Imovels.Where(x => x.ID_Imovel == idimovel).FirstOrDefault();
            inq = db.Inquilinos.Where(x => x.Email == form["Email"]).FirstOrDefault();
            
            
            if (inq == null)
            {
                ModelState.AddModelError("Email", "Nao foi encontrado nenhum utilizador com e email que introduziu");
                return View("RegistarAlguer");
            }
            else
            {

                Compra compra = new Compra();
                compra.Valor = Convert.ToInt32(form["Valor"]);
                compra.ID_Imovel = idimovel;
                compra.ID_Inquilino = inq.ID_Inquilino;
                compra.Data_de_compra = DateTime.Now;
                compra.ID_Proprietario = User.Identity.GetUserId();
                db.Compras.InsertOnSubmit(compra);
                db.SubmitChanges();
                imo = db.Imovels.Where(x => x.ID_Imovel == idimovel).FirstOrDefault();
                imo.Estado = 0;
                db.SubmitChanges();
                inq = db.Inquilinos.Where(x => x.Email == form["Email"]).FirstOrDefault();
                var i = db.PossuiImovels.Where(x => x.ID_Imovel == imo.ID_Imovel).FirstOrDefault();
                db.PossuiImovels.DeleteOnSubmit(i);
                db.SubmitChanges();
                
                TempData["SucessoCompra"] = "A venda foi registada com sucesso!!";
                return RedirectToAction("OsmeusImoveis", new { id = User.Identity.GetUserId() });
            }
        }

        public ActionResult DenunciarInquilino(string id)
        {
            TempData["idinquilino"] = id;
            return View("DenunciarInquilino");
         
        }
        [HttpPost]
        public ActionResult DenunciarInquilino(FormCollection form)
        {
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            string idinquilino = (string)TempData["idinquilino"];
            Inquilino inq = new Inquilino();
            inq = db.Inquilinos.Where(x => x.ID_Inquilino == idinquilino).FirstOrDefault();
            inq.Estado = 1;
            db.SubmitChanges();
            Denuncia den = new Denuncia();
            den.ID_Inquilino = (string)TempData["idinquilino"];
            den.ID_Proprietario = User.Identity.GetUserId();
            den.Estado = "Em processamento";
            den.Texto = form["Texto"];
            den.Data_denuncia = DateTime.Now;
            db.Denuncias.InsertOnSubmit(den);
            db.SubmitChanges();
            TempData["denuncia"] = "A sua denuncia foi efetuada com sucesso! Iremos proceder a uma analise do seu motivo, tentaremos ser breves";
            return RedirectToAction("OsmeusImoveis", "Proprietario", new { id = User.Identity.GetUserId() });

        }
        public ActionResult DenunciarProprietario(string id)
        {
            TempData["idprop"] = id;
            return View("DenunciarProprietario");

        }
        [HttpPost]
        public ActionResult DenunciarProprietario(FormCollection form)
        {
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            string idprop = (string)TempData["idprop"];
            Proprietario prop = new Proprietario();
            prop = db.Proprietarios.Where(x => x.ID_Proprietario == idprop).FirstOrDefault();
            prop.Estado = 1;
            db.SubmitChanges();
            DenunciaProp den = new DenunciaProp();
            den.ID_Inquilino = User.Identity.GetUserId();
            den.ID_Proprietario = (string)TempData["idprop"];
            den.Estado = "Em processamento";
            den.Texto = form["Texto"];
            den.Data_denuncia = DateTime.Now;
            db.DenunciaProps.InsertOnSubmit(den);
            db.SubmitChanges();
            TempData["denuncia"] = "A sua denuncia foi efetuada com sucesso! Iremos proceder a uma analise do seu motivo, tentaremos ser breves";
            return RedirectToAction("OsmeusImoveis", "Proprietario", new { id = User.Identity.GetUserId() });

        }

        public ActionResult VerPerfilProp(string id)
        {

            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account");
            }
            DBImobiliariaDataContext db = new DBImobiliariaDataContext();
            Proprietario prop = new Proprietario();
            prop = db.Proprietarios.Where(x => x.ID_Proprietario == id).FirstOrDefault();
            
            return View("ProfileProp",prop);
        }

    }
}