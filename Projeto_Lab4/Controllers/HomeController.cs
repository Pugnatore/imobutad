using Microsoft.AspNet.Identity;
using Projeto_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Projeto_Lab4.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (TempData["Erro"] != null)
            {
                ViewBag.erro = TempData["Erro"];
            }
            User.Identity.GetUserName();
            var db = new DBImobiliariaDataContext();
            List<Imovel> listaimo = new List<Imovel>();
            listaimo = db.Imovels.OrderByDescending(X => X.N_vizualizacoes).ToList();
            List<int> pos = new List<int>();
            int t = 0;
            foreach (Imovel _imo in listaimo)
            {
                if (_imo.Estado == 0)
                {
                    pos.Add(t);
                }
                t++;
            }
            int h = 0;
            for (int j = 0; j < pos.Count(); j++)
            {
                if (h == 0)
                {
                    listaimo.Remove(listaimo[pos[j]]);
                }
                else
                {
                    listaimo.Remove(listaimo[pos[j] - h]);
                }
                h++;
            }
            if (listaimo.Count() <= 6)
            {

                return View(listaimo);
            }
            else
            {
                for (int i = 0; i < listaimo.Count(); i++)
                {
                    listaimo.Remove(listaimo[6]);
                    if (listaimo.Count() == 6)
                    {
                        return View(listaimo);
                    }
                }
                return View(listaimo); ;
            }
        }


        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            List<Imovel> listaauxiliarImo = new List<Imovel>();
            listaauxiliarImo = (List<Imovel>)db.Imovels.ToList();

            List<int> pos = new List<int>();
            int i = 0;
            int cidade = 0;
            int z = 0;

            foreach (Imovel _imo in listaauxiliarImo)
            {
                if (_imo.Estado == 0)
                {
                    pos.Add(z);
                }
                z++;
            }
            int y = 0;
            for (int j = 0; j < pos.Count(); j++)
            {
                if (y == 0)
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                }
                else
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - y]);
                }
                y++;
            }
            pos.Clear();

            if (form["Procurar"] == "" && form["Finalidade"] == "" && form["Tipo_de_imovel"] == "")
            {

            }
            else
            {
                int tipologia = 0;
                if (form["Procurar"] != "")
                {
                    int t;
                    t = -1;
                    if (form["Procurar"].ToLower() == "t0")
                    {
                        tipologia = 1;
                        t = 0;
                    }
                    if (form["Procurar"].ToLower() == "t1")
                    {
                        tipologia = 1;
                        t = 1;
                    }
                    if (form["Procurar"].ToLower() == "t2")
                    {
                        tipologia = 1;
                        t = 2;
                    }
                    if (form["Procurar"].ToLower() == "t3")
                    {
                        tipologia = 1;
                        t = 3;
                    }
                    if (form["Procurar"].ToLower() == "t4")
                    {
                        tipologia = 1;
                        t = 4;
                    }
                    if (form["Procurar"].ToLower() == "t5")
                    {
                        tipologia = 1;
                        t = 5;
                    }
                    if (form["Procurar"].ToLower() == "t6")
                    {
                        tipologia = 1;
                        t = 6;
                    }
                    if (form["Procurar"].ToLower() == "t7")
                    {
                        tipologia = 1;
                        t = 7;
                    }
                    if (t != -1)
                    {
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Quantidade_de_Quartos != t)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }
                    }
                }
                pos.Clear();
                if (tipologia == 0)
                {
                    if (form["Procurar"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Encontra_se.Concelho.Situa_se.Distrito.Nome_distrito.ToLower() != form["Procurar"].ToLower())
                            {
                                pos.Add(i);
                            }
                            i++;
                        }
                        if (pos.Count() != listaauxiliarImo.Count())
                        {
                            int h = 0;
                            for (int j = 0; j < pos.Count(); j++)
                            {
                                if (h == 0)
                                {
                                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                                }
                                else
                                {
                                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                                }
                                h++;
                            }
                            cidade = 1;
                        }

                    }
                    pos.Clear();
                    if (form["Procurar"] != "")
                    {
                        if (cidade != 1)
                        {
                            i = 0;
                            foreach (Imovel _imo in listaauxiliarImo)
                            {
                                if (_imo.Encontra_se.Concelho.Nome_concelho.ToLower() != form["Procurar"].ToLower())
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
                                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                                }
                                else
                                {
                                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                                }
                                h++;
                            }
                        }
                    }
                }
                pos.Clear();
                if (form["Finalidade"] != "")
                {
                    if (form["Finalidade"] != "Ambos")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Finalidade != "Ambos")
                            {
                                if (_imo.Finalidade != form["Finalidade"])
                                {
                                    pos.Add(i);
                                }
                            }
                            i++;
                        }
                        int h = 0;
                        for (int j = 0; j < pos.Count(); j++)
                        {
                            if (h == 0)
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }
                    }
                }
                pos.Clear();
                if (form["Tipo_de_imovel"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (_imo.Tipo_de_Imovel.ToLower() != form["Tipo_de_imovel"].ToLower())
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
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();


            }
            if (listaauxiliarImo.Count() == 0)
            {
                TempData["Erro"] = "Não foram encontrados imóveis com os critérios procurados";
                return RedirectToAction("Index", "Home");

            }
            TempData["imoveis"] = listaauxiliarImo;
            return RedirectToAction("ListarImoveis", "Imoveis");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Sobre nos";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contactos";

            return View();
        }
        [Authorize(Users = "Admin@admin.com")]
        public ActionResult IndexAdmin()
        {
            return View();
        }
        public ActionResult PesquisaAvancada()
        {
            var db = new DBImobiliariaDataContext();
            List<Imovel> listaauxiliarImo = new List<Imovel>();
            if (TempData["Imoveis"] == null)
            {
                listaauxiliarImo = db.Imovels.ToList();
            }
            else
            {
                listaauxiliarImo = (List<Imovel>)TempData["Imoveis"];
            }
            List<int> pos = new List<int>();

            int t = 0;
            foreach (Imovel _imo in listaauxiliarImo)
            {
                if (_imo.Estado == 0)
                {
                    pos.Add(t);
                }
                t++;
            }
            int z = 0;
            for (int j = 0; j < pos.Count(); j++)
            {
                if (z == 0)
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                }
                else
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - z]);
                }
                z++;
            }

            return View(listaauxiliarImo);


        }
        [HttpPost]
        public ActionResult PesquisaAvancada(FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            List<Imovel> listaauxiliarImo = new List<Imovel>();
            listaauxiliarImo = (List<Imovel>)db.Imovels.ToList();
            List<int> pos = new List<int>();
            int i = 0;
            int cidade = 0;
            int t = 0;
            foreach (Imovel _imo in listaauxiliarImo)
            {
                if (_imo.Estado == 0)
                {
                    pos.Add(t);
                }
                t++;
            }
            int z = 0;
            for (int j = 0; j < pos.Count(); j++)
            {
                if (z == 0)
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                }
                else
                {
                    listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - z]);
                }
                z++;
            }

            pos.Clear();

            if (form["Localizacao"] == "" && form["Finalidade"] == "" && form["Tipo_de_imovel"] == "" && form["Tipologia"] == "" && form["Precomin"] == "" && form["Precomax"] == "" && form["Areamin"] == "" && form["Areamax"] == "")
            {

            }
            else
            {
                if (form["Localizacao"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (_imo.Encontra_se.Concelho.Situa_se.Distrito.Nome_distrito.ToLower() != form["Localizacao"].ToLower())
                        {
                            pos.Add(i);
                        }
                        i++;
                    }
                    if (pos.Count() != listaauxiliarImo.Count())
                    {
                        int h = 0;
                        for (int j = 0; j < pos.Count(); j++)
                        {
                            if (h == 0)
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }
                        cidade = 1;
                    }

                }
                pos.Clear();
                if (form["Localizacao"] != "")
                {
                    if (cidade != 1)
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Encontra_se.Concelho.Nome_concelho.ToLower() != form["Localizacao"].ToLower())
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }
                    }
                }


                pos.Clear();
                if (form["Finalidade"] != "")
                {
                    if (form["Finalidade"] != "Ambos")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Finalidade != "Ambos")
                            {
                                if (_imo.Finalidade != form["Finalidade"])
                                {
                                    pos.Add(i);
                                }
                            }
                            i++;
                        }
                        int h = 0;
                        for (int j = 0; j < pos.Count(); j++)
                        {
                            if (h == 0)
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }
                    }
                }


                pos.Clear();
                if (form["Tipo_de_imovel"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (_imo.Tipo_de_Imovel.ToLower() != form["Tipo_de_imovel"].ToLower())
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
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                        }
                        h++;
                    }
                }
                pos.Clear();

                if (form["Tipologia"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (_imo.Quantidade_de_Quartos != Convert.ToInt32(form["Tipologia"]))
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
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                        }
                        h++;
                    }

                }
                pos.Clear();
                if (form["Finalidade"] == "Comprar")
                {
                    if (form["Precomin"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomin"]) > _imo.Valor_venda)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                    pos.Clear();
                    if (form["Precomax"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomax"]) < _imo.Valor_venda)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                }
                pos.Clear();
                if (form["Finalidade"] == "Alugar")
                {
                    if (form["Precomin"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomin"]) > _imo.Valor_aluguer)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                    pos.Clear();
                    if (form["Precomax"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomax"]) < _imo.Valor_aluguer)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                }
                pos.Clear();

                if (form["Finalidade"] == "Ambos")
                {
                    if (form["Precomin"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomin"]) > _imo.Valor_aluguer && Convert.ToInt32(form["Precomin"]) > _imo.Valor_venda)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                    pos.Clear();
                    if (form["Precomax"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (Convert.ToInt32(form["Precomax"]) < _imo.Valor_aluguer && Convert.ToInt32(form["Precomax"]) < _imo.Valor_venda)
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                }
                pos.Clear();

                if (form["Finalidade"] == "")
                {
                    if (form["Precomin"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Valor_aluguer < Convert.ToInt32(form["Precomin"]) || _imo.Valor_venda < Convert.ToInt32(form["Precomin"]))
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                    pos.Clear();
                    if (form["Precomax"] != "")
                    {
                        i = 0;
                        foreach (Imovel _imo in listaauxiliarImo)
                        {
                            if (_imo.Valor_aluguer > Convert.ToInt32(form["Precomax"]) || _imo.Valor_venda > Convert.ToInt32(form["Precomax"]))
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
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                            }
                            else
                            {
                                listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                            }
                            h++;
                        }

                    }
                }
                pos.Clear();

                if (form["Areamin"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (Convert.ToInt32(form["Areamin"]) > _imo.Metros)
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
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                        }
                        h++;
                    }

                }
                pos.Clear();
                if (form["Areamax"] != "")
                {
                    i = 0;
                    foreach (Imovel _imo in listaauxiliarImo)
                    {
                        if (Convert.ToInt32(form["Areamax"]) < _imo.Metros)
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
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j]]);
                        }
                        else
                        {
                            listaauxiliarImo.Remove(listaauxiliarImo[pos[j] - h]);
                        }
                        h++;
                    }

                }



            }

            TempData["imoveis"] = listaauxiliarImo;
            if (listaauxiliarImo.Count() == 0)
            {
                ViewBag.erro = "Não foram encontrados imóveis com os critérios procurados";
                return View(listaauxiliarImo);

            }

            return View(listaauxiliarImo);


        }

        public ActionResult ConfirmarEmail(string id)
        {
            var db = new DBImobiliariaDataContext();
            Inquilino inq = new Inquilino();
            Proprietario prop = new Proprietario();

            inq = db.Inquilinos.Where(X => X.ID_Inquilino == id).FirstOrDefault();
            prop = db.Proprietarios.Where(X => X.ID_Proprietario == id).FirstOrDefault();

            if (inq != null)
            {
                inq.Estado = 0;
                db.SubmitChanges();
            }
            if (prop != null)
            {
                prop.Estado = 0;
                db.SubmitChanges();
            }
            return View();
        }

        public ActionResult historicocompras(string id)
        {
            var db = new DBImobiliariaDataContext();
            
            List<Compra> compras = new List<Compra>();
            
            foreach(var item in db.Compras)
            {
                if (item.ID_Inquilino ==  id)
                {
                    compras.Add(item);
                }
            }
            
            foreach (var item in db.Compras)
            {
                if (item.ID_Proprietario ==  id)
                {
                    compras.Add(item);
                }
            }

            return View(compras);
        }
        public ActionResult historicoalugas(string id)
        {
            var db = new DBImobiliariaDataContext();

         
            List<Aluga> alugas = new List<Aluga>();
            foreach (var item in db.Alugas)
            {
            if (item.ID_Inquilino == id)
            {
                alugas.Add(item);
            }
            }
            foreach (var item in db.Alugas)
            {
            if (item.ID_Proprietario == id)
            {
                alugas.Add(item);
            }
            }

            return View(alugas);
        }
        }
}
