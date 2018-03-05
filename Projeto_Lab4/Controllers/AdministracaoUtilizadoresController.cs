using Microsoft.AspNet.Identity;
using Projeto_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Projeto_Lab4.Controllers
{
    public class AdministracaoUtilizadoresController : Controller
    {
        // GET: AdministracaoUtilizadores
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult GerirInquiilinosMostrar()
        {
            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult GerirProprietariosMostrar()
        {
            
            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult GerirInquiilinos()
        {
            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult GerirInquiilinos(FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            List<Inquilino> listaauxiliarInq = new List<Inquilino>();
            listaauxiliarInq = (List<Inquilino>)db.Inquilinos.ToList();
            List<int> pos = new List<int>();
            int i = 0;
            pos.Clear();

            if (form["Nome"] == "" && form["Email"] == "" && form["Username"] == "")
            {
               
            }
            else {
                if (form["Nome"] != "")
                {
                    foreach (Inquilino _inq in db.Inquilinos)
                    {
                        if (_inq.Nome != form["Nome"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();
                if (form["Email"] != "")
                {
                    i = 0;
                    foreach (Inquilino _inq in listaauxiliarInq)
                    {
                        if ((_inq.Email).ToString() != form["Email"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();
                if (form["Username"] != "")
                {
                    i = 0;
                    foreach (Inquilino _inq in listaauxiliarInq)
                    {
                        if (_inq.Username != form["Username"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarInq.Remove(listaauxiliarInq[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();


            }
            if (listaauxiliarInq.Count() == 0)
            {
                ModelState.AddModelError("Username", "Não foram encontrados utilizadores com os criterios procurados");
                return View();
                
            }
            
            return View("GerirInquiilinosMostrar", listaauxiliarInq);
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult GerirProprietarios()
        {
            return View();
        }

        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult GerirProprietarios(FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            List<Proprietario> listaauxiliarprop = new List<Proprietario>();
            listaauxiliarprop = (List<Proprietario>)db.Proprietarios.ToList();
            List<int> pos = new List<int>();
            int i = 0;

            if (form["Nome"] == "" && form["Email"] == "" && form["Username"] == "")
            {
                ;

            }

            else
            {
                pos.Clear();
                if (form["Nome"] != "")
                {
                    foreach (Proprietario _prop in db.Proprietarios)
                    {
                        if (_prop.Nome != form["Nome"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();
                if (form["Email"] != "")
                {
                    i = 0;
                    foreach (Proprietario _prop in listaauxiliarprop)
                    {
                        if ((_prop.Email).ToString() != form["Email"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();
                if (form["Username"] != "")
                {
                    i = 0;
                    foreach (Proprietario _prop in listaauxiliarprop)
                    {
                        if (_prop.Username != form["Username"])
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    int h = 0;
                    for (int j = 0; j < pos.Count(); j++)
                    {
                        if (h == 0)
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarprop.Remove(listaauxiliarprop[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();
            }
           

            if (listaauxiliarprop.Count() == 0)
            {
                ModelState.AddModelError("Username", "Não foram encontrados utilizadores com os criterios procurados");
                return View();
            }
            return View("GerirProprietariosMostrar",listaauxiliarprop);
        }
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult DeleteProprietario(string id)
        {

            var db = new DBImobiliariaDataContext();
            var user = db.Proprietarios.Where(X => X.ID_Proprietario== id).FirstOrDefault();
            db.Proprietarios.DeleteOnSubmit(user);
            db.SubmitChanges();
            return RedirectToAction ("IndexAdmin","Home");
        }
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult DeleteInquilino(string id)
        {

            var db = new DBImobiliariaDataContext();
            var user = db.Inquilinos.Where(X => X.ID_Inquilino == id).FirstOrDefault();
            db.Inquilinos.DeleteOnSubmit(user);
            db.SubmitChanges();
            return RedirectToAction("IndexAdmin", "Home");
        }


        [Authorize(Users = "Admin@admin.com")]
        public ActionResult EditInquilino(string id)
        {
            return View();
        }

        //depois tenho de ver melhor isto por causa dos HIDDEN na view
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult EditInquilino(string id,FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            
            var inq = db.Inquilinos
               .Where(X => X.ID_Inquilino.Equals(id)
                      ).FirstOrDefault();
            inq.ID_Inquilino = form["ID_Inquilino"];
            inq.Email = form["Email"];
            inq.Nome = form["Nome"];
            inq.Data_de_Registo = Convert.ToDateTime(form["Data_de_Registo"]);
            inq.Data_Nascimento = Convert.ToDateTime(form["Data_Nascimento"]);
            inq.Numero_de_Telemovel = form["Numero_de_Telemovel"];
            inq.Cartao_de_Cidadao = Convert.ToInt32(form["Cartao_de_Cidadao"]);
            inq.Username = form["Username"];
            inq.Password = form["Password"];
            inq.Estado = null;
            db.Inquilinos.InsertOnSubmit(inq);
            db.SubmitChanges();


            return RedirectToAction("IndexAdmin", "Home");
        }


        [Authorize(Users = "Admin@admin.com")]
        public ActionResult EditProprietario(string id)
        {
            return View();
        }

        //depois tenho de ver melhor isto por causa dos HIDDEN na view
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult EditProprietario(string id, FormCollection form)
        {
            var db = new DBImobiliariaDataContext();

            var prop = db.Proprietarios
               .Where(X => X.ID_Proprietario.Equals(id)
                      ).FirstOrDefault();
            prop.ID_Proprietario = form["ID_Inquilino"];
            prop.Email = form["Email"];
            prop.Nome = form["Nome"];
            prop.Data_de_Registo = Convert.ToDateTime(form["Data_de_Registo"]);
            prop.Data_Nascimento = Convert.ToDateTime(form["Data_Nascimento"]);
            prop.Numero_de_Telemovel = form["Numero_de_Telemovel"];
            prop.Cartao_de_Cidadao = Convert.ToInt32(form["Cartao_de_Cidadao"]);
            prop.Username = form["Username"];
            prop.Password = form["Password"];
            prop.Estado = null;
            db.Proprietarios.InsertOnSubmit(prop);
            db.SubmitChanges();


            return RedirectToAction("IndexAdmin", "Home");
        }




        [Authorize(Users = "Admin@admin.com")]
        public ActionResult VerDenuncias()
        {   //considerando  que apenas os inquilinos podem ser denunciados tal como o professor disse

            var db = new DBImobiliariaDataContext();
            List<Inquilino> listaauxiliarinq = new List<Inquilino>();
            List<Denuncia> listaauxiliarden = new List<Denuncia>();
            listaauxiliarinq.Clear();
            foreach(Denuncia _den in db.Denuncias)
            {
                listaauxiliarinq.Add(_den.Inquilino);
            }

            foreach(Inquilino _inq in db.Inquilinos)
            {
                // se o utilizador ja estiver bloquado, nao apresenta nada
                //0- livre
                //1- em processamento
                //2-bloqueado
                //3-pendente


               if (_inq.Estado == 0)
                {
                    listaauxiliarinq.Remove(_inq);
                }
            }

            //vao ficar apenas o que tem o estado a 1 porque os que tem o estado a 0 nao aparecem nas denuncias acima


            return View(listaauxiliarinq);
        }

        //depois de carregar nmo botao de detalhes
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult VerDetalhesDenuncias(string id)
        {
            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _den = db.Denuncias
                    .Where(X => X.ID_Inquilino.Equals(id)
                           ).FirstOrDefault();

            var _util = db.Inquilinos
                  .Where(X => X.ID_Inquilino.Equals(id)
                         ).FirstOrDefault();

            TempData["UtilizadorBloqueado"] = _util.Nome;


            return View(_den);
        }

        [Authorize(Users = "Admin@admin.com")]
        public ActionResult VerDenunciasProp()
        {   

            var db = new DBImobiliariaDataContext();
            List<Proprietario> listaauxiliarprop = new List<Proprietario>();
            List<DenunciaProp> listaauxiliarden = new List<DenunciaProp>();
            listaauxiliarprop.Clear();
            foreach (DenunciaProp _den in db.DenunciaProps)
            {
                listaauxiliarprop.Add(_den.Proprietario);
            }

            foreach (Proprietario _prop in db.Proprietarios)
            {
                // se o utilizador ja estiver bloquado, nao apresenta nada
                //0- livre
                //1- em processamento
                //2-bloqueado
                //3-pendente


                if (_prop.Estado == 0)
                {
                    listaauxiliarprop.Remove(_prop);
                }
            }

            //vao ficar apenas o que tem o estado a 1 porque os que tem o estado a 0 nao aparecem nas denuncias acima


            return View(listaauxiliarprop);
        }
        //depois de carregar nmo botao de detalhes
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult VerDetalhesDenunciasprop(string id)
        {
            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _den = db.DenunciaProps
                    .Where(X => X.ID_Proprietario.Equals(id)
                           ).FirstOrDefault();

            var _util = db.Proprietarios.Where(X => X.ID_Proprietario.Equals(id)
                         ).FirstOrDefault();

            TempData["UtilizadorBloqueado"] = _util.Nome;


            return View(_den);
        }

        [Authorize(Users = "Admin@admin.com")]
        public ActionResult BloquearUtilizador(string id)
        {

            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _inq = db.Inquilinos
                    .Where(X => X.ID_Inquilino==id
                           ).FirstOrDefault();
            if (_inq == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }

            TempData["UtilizadorBloqueado"] = _inq.Nome;
            TempData["id_inquilino"] = id;

            return View();
        }


    
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult BloquearUtilizador(FormCollection form)
        {
            string id = (string)TempData["id_inquilino"];

            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _inq = db.Inquilinos
                    .Where(X => X.ID_Inquilino==id
                           ).FirstOrDefault();
            if (_inq == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }

            _inq.Estado = 2;
            db.SubmitChanges();

            BloqueiaInqulino bloc = new BloqueiaInqulino();
            bloc.ID_Inquilino = id;
            bloc.Data_de_bloqueio = DateTime.Now;
            bloc.Motivo = form["Motivo"];
            bloc.ID_Administrador = User.Identity.GetUserId();
            db.BloqueiaInqulinos.InsertOnSubmit(bloc);
            db.SubmitChanges();

            TempData["Bloqueio"] = "ATENÇÃO!! UTILIZADOR BLOQUEADO COM SUCESSO";


            //falta por mensagem de sucesso e atribuir uma view em baixo


            return View();
        }

        [Authorize(Users = "Admin@admin.com")]
        public ActionResult BloquearUtilizadorprop(string id)
        {

            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _prop = db.Proprietarios
                    .Where(X => X.ID_Proprietario == id
                           ).FirstOrDefault();
            if (_prop == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }

            TempData["UtilizadorBloqueado"] = _prop.Nome;
            TempData["id_prop"] = id;

            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        [HttpPost]
        public ActionResult BloquearUtilizadorprop(FormCollection form)
        {
            string id = (string)TempData["id_prop"];

            var db = new DBImobiliariaDataContext();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _prop = db.Proprietarios
                    .Where(X => X.ID_Proprietario == id
                           ).FirstOrDefault();
            if (_prop == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }

            _prop.Estado = 2;
            db.SubmitChanges();

            BloqueiaProprietario bloc = new BloqueiaProprietario();
            bloc.ID_Proprietario = id;
            bloc.Data_de_bloqueio = DateTime.Now;
            bloc.Motivo = form["Motivo"];
            bloc.ID_Administrador = User.Identity.GetUserId();
            db.BloqueiaProprietarios.InsertOnSubmit(bloc);
            db.SubmitChanges();

            TempData["Bloqueio"] = "ATENÇÃO!! UTILIZADOR BLOQUEADO COM SUCESSO";


            //falta por mensagem de sucesso e atribuir uma view em baixo


            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult DesloquearUtilizador(string id)
        {

            var db = new DBImobiliariaDataContext();
            Denuncia den = new Denuncia();
            BloqueiaInqulino bi = new BloqueiaInqulino();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _inq = db.Inquilinos
                    .Where(X => X.ID_Inquilino == id
                           ).FirstOrDefault();
            if (_inq == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }
            den = db.Denuncias.Where(x => x.ID_Inquilino == id).FirstOrDefault();
            if (den != null)
            {
                db.Denuncias.DeleteOnSubmit(den);
                db.SubmitChanges();
            }
                bi = db.BloqueiaInqulinos.Where(x => x.ID_Inquilino == id).FirstOrDefault();
                db.BloqueiaInqulinos.DeleteOnSubmit(bi);
                db.SubmitChanges();


                _inq.Estado = 0;
                db.SubmitChanges();
            
            
            TempData["Desbloqueio"] = "ATENÇÃO!! UTILIZADOR DESBLOQUEADO COM SUCESSO";
            return RedirectToAction("VerDenuncias");
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult DesloquearUtilizadorprop(string id)
        {

            var db = new DBImobiliariaDataContext();
            DenunciaProp den = new DenunciaProp();
            BloqueiaProprietario bi = new BloqueiaProprietario();
            if (id == null) //caso não seja preenchido o id retorna erro http 400 - Bad Request
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _prop = db.Proprietarios
                    .Where(X => X.ID_Proprietario == id
                           ).FirstOrDefault();
            if (_prop == null)//caso o user não exista retornamos o erro http 404 - Recurso não encontrado
            {
                return HttpNotFound("User Not Found");
            }
            den = db.DenunciaProps.Where(x => x.ID_Proprietario == id).FirstOrDefault();
            if (den != null)
            {
                db.DenunciaProps.DeleteOnSubmit(den);
                db.SubmitChanges();
            }
            bi = db.BloqueiaProprietarios.Where(x => x.ID_Proprietario == id).FirstOrDefault();
            db.BloqueiaProprietarios.DeleteOnSubmit(bi);
            db.SubmitChanges();


            _prop.Estado = 0;
            db.SubmitChanges();


            TempData["Desbloqueio"] = "ATENÇÃO!! UTILIZADOR DESBLOQUEADO COM SUCESSO";
            return RedirectToAction("VerDenunciasProp");
        }
    }
}