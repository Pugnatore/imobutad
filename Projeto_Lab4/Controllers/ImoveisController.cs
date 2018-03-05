using Projeto_Lab4.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Projeto_Lab4.Controllers
{
    public class ImoveisController : Controller
    {    
        // GET: Imoveis
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult CriarImovel()
        {
            var db = new DBImobiliariaDataContext();
            Proprietario pro = db.Proprietarios.Where(X => X.ID_Proprietario == User.Identity.GetUserId()).FirstOrDefault();
            if(pro != default(Proprietario))
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");

                return View();
            }
            return RedirectToAction("Login", "Account");


            
        }
        
        [HttpPost]
        public ActionResult CriarImovel(Imovel model, FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            if (model.Finalidade == null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Finalidade", "Campo de preenchimento obrigatório!");
                return View();
            }
            if (model.Rua == null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Rua", "Campo de preenchimento obrigatório!");
                return View();
            }
            if (model.Tipo_de_Imovel == null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Tipo_de_Imovel", "Campo de preenchimento obrigatório!");
                return View();
            }
           
            if (form["Concelho"] == null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Encontra_se.Concelho", "Campo de preenchimento obrigatório!");
                return View();
            }
            if (model.Finalidade == "Alugar" && model.Valor_venda != null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Valor_aluguer", "Selecionou a finalidade alugar por isso deve preencher apenas o valor de aluguer!!!");
                return View();
            }
            if (model.Finalidade == "Comprar" && model.Valor_aluguer != null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Valor_venda", "Selecionou a finalidade vender por isso deve preencher apenas o valor de venda!!!");
                return View();
            }
            if (model.Finalidade == "Comprar" && model.Valor_venda==null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Valor_venda", "Selecionou a finalidade vender por isso deve preencher o valor de venda!!!");
                return View();
            }
            if (model.Finalidade == "Alugar" && model.Valor_aluguer == null)
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                ModelState.AddModelError("Valor_aluguer", "Selecionou a finalidade alugar por isso deve preencher o valor de aluguer!!!");
                return View();
            }
            if (model.Finalidade == "Ambos")
            {
                if(model.Valor_aluguer == null || model.Valor_venda == null)
                {
                    ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                    ModelState.AddModelError("Valor_venda", "Selecionou a finalidade alugar e vender(Ambos) por isso deve preencher o valor de aluguer e de venda!!!!!!");
                    return View();
                }
            }
            PossuiImovel pi = new PossuiImovel();
            
            
          
        
            Imovel imo = new Imovel();
            
            imo.Rua = model.Rua;
            imo.Codigo_Postal = model.Codigo_Postal;
            imo.Data_de_Inscricao = DateTime.Now;
            imo.Garagem = model.Garagem;
            imo.Metros = model.Metros;
            imo.Quantidade_de_Quartos = model.Quantidade_de_Quartos;
            imo.N_Porta = model.N_Porta;
            imo.Quantidade_de_Suits = model.Quantidade_de_Suits;
            imo.Quantidade_de_Casas_de_Banho = model.Quantidade_de_Casas_de_Banho;
            imo.Valor_aluguer = model.Valor_aluguer;
            imo.Valor_venda = model.Valor_venda;
            imo.Tipo_de_Imovel = model.Tipo_de_Imovel;
            imo.Finalidade = model.Finalidade;
            imo.Latitude = model.Latitude;
            imo.Longitude = model.Longitude;
            imo.Estado = 1;
            imo.Foto_principal = "nada"; //vai ser alterado abaixo
            imo.Descricao = model.Descricao;
            
            db.Imovels.InsertOnSubmit(imo);
            db.SubmitChanges();
            Concelho con = db.Concelhos.Where(X => X.Nome_concelho == form["Concelho"]).FirstOrDefault();
            Encontra_se enc = new Encontra_se();
            enc.ID_Concelho = con.ID_Concelho;
            enc.ID_Imovel = db.Imovels.Count();
            db.Encontra_ses.InsertOnSubmit(enc);

            TempData["Id_Imovel"] = db.Imovels.Count();

            pi.ID_Imovel = db.Imovels.Count();

            pi.ID_Proprietario= User.Identity.GetUserId();
            db.PossuiImovels.InsertOnSubmit(pi);
            db.SubmitChanges();

            return View("CriarImovel2");
        }
        
        public ActionResult CriarImovel2()
        {
            return View();
        }
     
        [HttpPost]
        public ActionResult CriarImovel2(PhotoInfo model, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            int id_imovel = Convert.ToInt32(TempData["Id_Imovel"]);
                    var db = new DBImobiliariaDataContext();

                    
                Foto photo = new Foto();
                Foto photo2 = new Foto();
                Foto photo3 = new Foto();
                Foto photo4 = new Foto();
                Foto photo5 = new Foto();
                PossuiFoto pf = new PossuiFoto();
                PossuiFoto pf2 = new PossuiFoto();
                PossuiFoto pf3 = new PossuiFoto();
                PossuiFoto pf4 = new PossuiFoto(); 
                PossuiFoto pf5 = new PossuiFoto();


            //obter o caminho fisico da raiz do servidor
            var caminho = Server.MapPath("/");

                if (file1 != null)
                    {
                    //vereficar se o ficheiro é suportado 
                    if (!file1.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                    && !file1.ContentType.Equals("image/png")
                    && !file1.ContentType.Equals(MediaTypeNames.Image.Gif)
                    )
                    {
                        ModelState.AddModelError("Nome_foto1", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                        return View();
                    }
                    //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                    //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                    //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                    string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file1.FileName;
                    photo.Nome_Foto = "Content//" + caminho_restante;

                    //guardar o ficheiro
                    file1.SaveAs(caminho + photo.Nome_Foto);
                    //guardar o caminho para a bd
                    photo.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo);
                     //id = photo.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                     db.SubmitChanges();
                       photo = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                        pf.ID_Foto = photo.ID_Foto;
                        pf.ID_Imovel = id_imovel;
                        db.PossuiFotos.InsertOnSubmit(pf);
                        db.SubmitChanges();
                        var imo = db.Imovels.Where(x => x.ID_Imovel ==id_imovel).FirstOrDefault(); //consideremos que a primeira foto vai ser sempre a principal
                        imo.Foto_principal = photo.Nome_Foto;
                        db.SubmitChanges();
                        

                    }
                    else
                    {

                    //guardar o caminho para a bd
                    photo.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo);
                    
                    db.SubmitChanges();
                    List<Foto> ultima = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                    pf.ID_Foto = ultima[0].ID_Foto;
           
                    pf.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf);
                    db.SubmitChanges();
                 


                    }

            

                if (file2 != null)
                {
                    if (!file2.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                        && !file2.ContentType.Equals("image/png")
                        && !file2.ContentType.Equals(MediaTypeNames.Image.Gif)
                       )
                    {
                        ModelState.AddModelError("Nome_foto2", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                        return View();
                    }
                    //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                    //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                    //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                    string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file2.FileName;
                    photo2.Nome_Foto = "Content//" + caminho_restante;

                    //guardar o ficheiro
                    file2.SaveAs(caminho + photo2.Nome_Foto);

                    //guardar o caminho para a bd
                    photo2.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo2);
                    //id = photo2.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                    db.SubmitChanges();
                    photo2 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                    pf2.ID_Foto = photo2.ID_Foto;
                    pf2.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf2);
                    db.SubmitChanges();

                }
                else
                {

                    //guardar o caminho para a bd
                    photo2.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo2);
                    
                    db.SubmitChanges();
                List<Foto> ultima1 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf2.ID_Foto = ultima1[0].ID_Foto;
             
                pf2.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf2);
                    db.SubmitChanges();
                 


                }

            

                if (file3 != null)
                {
                    if (!file3.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                     && !file3.ContentType.Equals("image/png")
                     && !file3.ContentType.Equals(MediaTypeNames.Image.Gif)
                    )
                    {
                        ModelState.AddModelError("Nome_foto3", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                        return View();
                    }

                    //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                    //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                    //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                    string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file3.FileName;
                    photo3.Nome_Foto = "Content//" + caminho_restante;

                    //guardar o ficheiro
                    file3.SaveAs(caminho + photo3.Nome_Foto);

                    //guardar o caminho para a bd
                    photo3.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo3);
                    //id = photo3.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                    db.SubmitChanges();
                    photo3 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                    pf3.ID_Foto = photo3.ID_Foto;
                    pf3.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf3);
                    db.SubmitChanges();
                }
                else
                {

                    //guardar o caminho para a bd
                    photo3.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo3);
                    
                    db.SubmitChanges();
                List<Foto> ultima2 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf3.ID_Foto = ultima2[0].ID_Foto;
               
                pf3.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf3);
                    db.SubmitChanges();
                


                }

                

                if (file4 != null)
                {
                    if (!file4.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                         && !file4.ContentType.Equals("image/png")
                         && !file4.ContentType.Equals(MediaTypeNames.Image.Gif)
                         )
                    {
                        ModelState.AddModelError("Nome_foto4", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                        return View();
                    }
                    //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                    //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                    //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                    string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file4.FileName;
                    photo4.Nome_Foto = "Content//" + caminho_restante;

                    //guardar o ficheiro
                    file4.SaveAs(caminho + photo4.Nome_Foto);

                    //guardar o caminho para a bd
                    photo4.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo4);
                    //id = photo4.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                    db.SubmitChanges();
                    photo4 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                    pf4.ID_Foto = photo4.ID_Foto;
                    pf4.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf4);
                    db.SubmitChanges();


                }
                else
                {

                    //guardar o caminho para a bd
                    photo4.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo4);
                    
                    db.SubmitChanges();
                List<Foto> ultima3 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf4.ID_Foto = ultima3[0].ID_Foto;
             
                pf4.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf4);
                    db.SubmitChanges();
          


                }
               

                if (file5 != null)
                {
                    if (!file5.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                              && !file5.ContentType.Equals("image/png")
                              && !file5.ContentType.Equals(MediaTypeNames.Image.Gif)
                              )
                    {
                        ModelState.AddModelError("Nome_foto5", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                        return View();
                    }
                    //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                    //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                    //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                    string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file5.FileName;
                    photo5.Nome_Foto = "Content//" + caminho_restante;

                    //guardar o ficheiro
                    file5.SaveAs(caminho + photo5.Nome_Foto);

                    //guardar o caminho para a bd
                    photo5.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo5);
                    //id = photo5.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                    db.SubmitChanges();
                    photo5 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                    pf5.ID_Foto = photo5.ID_Foto;
                    pf5.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf5);
                    db.SubmitChanges();


                }
                else
                {

                    //guardar o caminho para a bd
                    photo5.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                    db.Fotos.InsertOnSubmit(photo5);
                    
                    db.SubmitChanges();
                List<Foto> ultima4 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf5.ID_Foto = ultima4[0].ID_Foto;
                
                pf5.ID_Imovel = id_imovel;
                    db.PossuiFotos.InsertOnSubmit(pf5);
                    db.SubmitChanges();
             


                }
                ViewBag.sucessoRI = "Imóvel registado com sucesso!!!";
                return RedirectToAction ("Index", "Home"); //nao retorna nada
        }

        public ActionResult ListarImoveis()
        {
            List<Imovel> listaauxiliarimo = (List<Imovel>)TempData["imoveis"];
            return View(listaauxiliarimo);
        }

        public ActionResult DetalhesImoveis(int imovelid)
        {

            var db = new DBImobiliariaDataContext();
            Proprietario prop = new Proprietario();
            prop = db.Proprietarios.Where(X => X.ID_Proprietario == User.Identity.GetUserId()).FirstOrDefault();
            Imovel imo = db.Imovels.Where(X => X.ID_Imovel == imovelid).FirstOrDefault();
            imo.N_vizualizacoes = imo.N_vizualizacoes + 1;
            db.SubmitChanges();

            List<Foto> TodasFotos = db.Fotos.ToList();
            List<Foto> _fotosdoimovel = new List<Foto>();
            if (prop != null)
            {
                TempData["prop"] = "prop";
            }
            if (User.Identity.GetUserId() == null)
            {
                TempData["registado"] = "nao";
            }
            else
            {
                if (prop != null)
                {
                    if (prop.ID_Proprietario == imo.PossuiImovel.ID_Proprietario)
                    {
                        TempData["donodoimovel"] = "sim";
                    }
                }
            }
            
            foreach(var _f in TodasFotos)
            {
                if (_f.PossuiFoto.ID_Imovel == imovelid)
                {
                    _fotosdoimovel.Add(_f);
                }
            }

            ViewBag.foto1 = _fotosdoimovel[0].Nome_Foto;
            ViewBag.foto2 = _fotosdoimovel[1].Nome_Foto;
            ViewBag.foto3 = _fotosdoimovel[2].Nome_Foto;
            ViewBag.foto4 = _fotosdoimovel[3].Nome_Foto;
            ViewBag.foto5 = _fotosdoimovel[4].Nome_Foto;

            return View(imo);
        }

        public ActionResult VerComentarios(int idimovel)
        {
            
            var db = new DBImobiliariaDataContext();
          
            List<Comenta> listcom = new List<Comenta>();
            foreach(var item in db.Comentas)
            {
                if (item.ID_Imovel == idimovel)
                {
                    listcom.Add(item);
                }
            }
            return View(listcom);
        }

        public ActionResult Comentar(int idimovel)
        {
          
            // var db = new DBImobiliariaDataContext();
            TempData["idimovel"] = idimovel;
           
            return View();
        }
        [HttpPost]
        public ActionResult Comentar(FormCollection form)
        {
            
            int imovelid = Convert.ToInt32(TempData["idimovel"]);
            var db = new DBImobiliariaDataContext();
            Comenta com = new Comenta
            {
                ID_Imovel = imovelid,
                ID_Inquilino = User.Identity.GetUserId(),
                Texto = form["Texto"],
                Data_de_comentario = DateTime.Now
            };
            db.Comentas.InsertOnSubmit(com);
            db.SubmitChanges();
            return RedirectToAction("DetalhesImoveis","Imoveis", new { imovelid = imovelid });
        }

        public ActionResult editarimovel(int id)
        {
            var db = new DBImobiliariaDataContext();
            Imovel imo = new Imovel();
            imo = db.Imovels.Where(X => X.ID_Imovel == id).FirstOrDefault();
            ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
            TempData["finalidade"] = imo.Finalidade;
            TempData["finalidade1"] = imo.Finalidade;
            TempData["finalidade2"] = imo.Finalidade;
            return View(imo);
        }

        [HttpPost]
        public ActionResult editarimovel(Imovel model,FormCollection form)
        {
            var db = new DBImobiliariaDataContext();
            Imovel imo = db.Imovels.Where(X => X.ID_Imovel == model.ID_Imovel).FirstOrDefault();
            if (form["Concelho"] == "")
            {
                ViewBag.ListaConcelhos = new SelectList(db.Concelhos, "Nome_concelho", "Nome_concelho");
                TempData["finalidade"] = imo.Finalidade;
                TempData["finalidade1"] = imo.Finalidade;
                TempData["finalidade2"] = imo.Finalidade;
                ModelState.AddModelError("Encontra_se.Concelho", "Campo de preenchimento obrigatório!");
                return View(imo);
            }
            
            imo.Rua = model.Rua;
            imo.Codigo_Postal = model.Codigo_Postal;
            imo.N_Porta = model.N_Porta;
            if (imo.Finalidade == "Vender")
            {
                imo.Valor_venda = model.Valor_venda;
            }
            if (imo.Finalidade == "Alugar")
            {
                imo.Valor_aluguer = model.Valor_aluguer;
            }
            if (imo.Finalidade == "Ambos")
            {
                imo.Valor_aluguer = model.Valor_aluguer;
                imo.Valor_venda = model.Valor_venda;
            }
            imo.Metros = model.Metros;
            imo.Latitude = model.Latitude;
            imo.Longitude = model.Longitude;
            imo.Garagem = model.Garagem;
            imo.Quantidade_de_Suits = model.Quantidade_de_Suits;
            imo.Quantidade_de_Quartos = model.Quantidade_de_Quartos;
            imo.Quantidade_de_Casas_de_Banho = model.Quantidade_de_Casas_de_Banho;
            imo.Descricao = model.Descricao;
            db.SubmitChanges();

            Concelho con = db.Concelhos.Where(X => X.Nome_concelho == form["Concelho"]).FirstOrDefault();
            Encontra_se enc = db.Encontra_ses.Where(X => X.ID_Imovel == imo.ID_Imovel ).FirstOrDefault();
            enc.ID_Concelho = con.ID_Concelho;
            db.SubmitChanges();
          

            return RedirectToAction("DetalhesImoveis", "Imoveis", new { imovelid = model.ID_Imovel });
        }
        public ActionResult editarfotosimovel(int id)
        {
            TempData["idimo"]= id;
            return View();
        }
        [HttpPost]
        public ActionResult editarfotosimovel(HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            int id_imovel = Convert.ToInt32(TempData["idimo"]);
            var db = new DBImobiliariaDataContext();

            
            Foto photo = new Foto();
            Foto photo2 = new Foto();
            Foto photo3 = new Foto();
            Foto photo4 = new Foto();
            Foto photo5 = new Foto();
            List<Foto> fotos = db.Fotos.Where(X => X.PossuiFoto.ID_Imovel == id_imovel).ToList();
            List<PossuiFoto> pf = db.PossuiFotos.Where(X => X.ID_Imovel == id_imovel).ToList();
            PossuiFoto pf1 = new PossuiFoto();
            PossuiFoto pf2 = new PossuiFoto();
            PossuiFoto pf3 = new PossuiFoto();
            PossuiFoto pf4 = new PossuiFoto();
            PossuiFoto pf5 = new PossuiFoto();

            //obter o caminho fisico da raiz do servidor
            var caminho = Server.MapPath("/");

            if (file1 != null)
            {
                
                //vereficar se o ficheiro é suportado 
                if (!file1.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                && !file1.ContentType.Equals("image/png")
                && !file1.ContentType.Equals(MediaTypeNames.Image.Gif)
                )
                {
                    ModelState.AddModelError("Nome_foto1", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }
                
                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file1.FileName;
                photo.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file1.SaveAs(caminho + photo.Nome_Foto);
                //guardar o caminho para a bd
                photo.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo);
                //id = photo.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                db.SubmitChanges();
                photo = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                List<Foto> ultima = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf1.ID_Foto =ultima[0].ID_Foto;
                pf1.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf1);
                db.SubmitChanges();

                var imo = db.Imovels.Where(x => x.ID_Imovel == id_imovel).FirstOrDefault(); //consideremos que a primeira foto vai ser sempre a principal
                imo.Foto_principal = photo.Nome_Foto;
                db.SubmitChanges();


            }
            else
            {

                //guardar o caminho para a bd
                photo.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo);

                db.SubmitChanges();
                List<Foto> ultima1 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf1.ID_Foto = ultima1[0].ID_Foto;
                pf1.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf1);
                db.SubmitChanges();
              



            }



            if (file2 != null)
            {
                if (!file2.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                    && !file2.ContentType.Equals("image/png")
                    && !file2.ContentType.Equals(MediaTypeNames.Image.Gif)
                   )
                {
                    ModelState.AddModelError("Nome_foto2", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }
                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file2.FileName;
                photo2.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file2.SaveAs(caminho + photo2.Nome_Foto);

                //guardar o caminho para a bd
                photo2.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo2);
                //id = photo2.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                db.SubmitChanges();
                photo2 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                List<Foto> ultima2 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf2.ID_Foto = ultima2[0].ID_Foto;
                pf2.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf2);
                db.SubmitChanges();

            }
            else
            {

                //guardar o caminho para a bd
                photo2.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo2);

                db.SubmitChanges();
                List<Foto> ultima3 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf2.ID_Foto = ultima3[0].ID_Foto;
                pf2.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf2);
                db.SubmitChanges();



            }



            if (file3 != null)
            {
                if (!file3.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                 && !file3.ContentType.Equals("image/png")
                 && !file3.ContentType.Equals(MediaTypeNames.Image.Gif)
                )
                {
                    ModelState.AddModelError("Nome_foto3", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }

                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file3.FileName;
                photo3.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file3.SaveAs(caminho + photo3.Nome_Foto);

                //guardar o caminho para a bd
                photo3.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo3);
                //id = photo3.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                db.SubmitChanges();
                photo3 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                List<Foto> ultima4 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf3.ID_Foto =ultima4[0].ID_Foto;
                pf3.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf3);
                db.SubmitChanges();
            }
            else
            {

                //guardar o caminho para a bd
                photo3.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo3);

                db.SubmitChanges();
                List<Foto> ultima5 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf3.ID_Foto = ultima5[0].ID_Foto;
                pf3.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf3);
                db.SubmitChanges();



            }



            if (file4 != null)
            {
                if (!file4.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                     && !file4.ContentType.Equals("image/png")
                     && !file4.ContentType.Equals(MediaTypeNames.Image.Gif)
                     )
                {
                    ModelState.AddModelError("Nome_foto4", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }
                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file4.FileName;
                photo4.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file4.SaveAs(caminho + photo4.Nome_Foto);

                //guardar o caminho para a bd
                photo4.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo4);
                //id = photo4.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                db.SubmitChanges();
                photo4 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                List<Foto> ultima6 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf4.ID_Foto = ultima6[0].ID_Foto;
                pf4.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf4);
                db.SubmitChanges();


            }
            else
            {

                //guardar o caminho para a bd
                photo4.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo4);

                db.SubmitChanges();
                List<Foto> ultima7 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf4.ID_Foto = ultima7[0].ID_Foto;
                pf4.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf4);
                db.SubmitChanges();



            }


            if (file5 != null)
            {
                if (!file5.ContentType.Equals(MediaTypeNames.Image.Jpeg)
                          && !file5.ContentType.Equals("image/png")
                          && !file5.ContentType.Equals(MediaTypeNames.Image.Gif)
                          )
                {
                    ModelState.AddModelError("Nome_foto5", "Formato de Imagem Inválida, Por favor submete uma imagem jpeg, png, gif");
                    return View();
                }
                //apenas guardamos o caminho do ficheiro relativo à raiz do servidor
                //assim é possivel usar este caminho na view para apresentar a imagem usando apenas o texto da variavel
                //o datetime serve para garantir que o nome do ficheiro na pasta é unico, e assim não á substituição do mesmo.
                string caminho_restante = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + file5.FileName;
                photo5.Nome_Foto = "Content//" + caminho_restante;

                //guardar o ficheiro
                file5.SaveAs(caminho + photo5.Nome_Foto);

                //guardar o caminho para a bd
                photo5.Nome_Foto = "../../Content/" + caminho_restante;//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo5);
                //id = photo5.ID_Foto; acho que só se pode ir buscar o id depois de guardar na base de dados
                db.SubmitChanges();
                photo5 = db.Fotos.Where(X => X.Nome_Foto == "../../Content/" + caminho_restante).FirstOrDefault();
                List<Foto> ultima8 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf5.ID_Foto = ultima8[0].ID_Foto;
                pf5.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf5);
                db.SubmitChanges();


            }
            else
            {

                //guardar o caminho para a bd
                photo5.Nome_Foto = "../../Content/branca.png";//meter o nome para usar na view
                db.Fotos.InsertOnSubmit(photo5);

                db.SubmitChanges();
                List<Foto> ultima9 = db.Fotos.OrderByDescending(X => X.ID_Foto).ToList();
                pf5.ID_Foto = ultima9[0].ID_Foto;
                pf5.ID_Imovel = id_imovel;
                db.PossuiFotos.InsertOnSubmit(pf5);
                db.SubmitChanges();



            }
            db.PossuiFotos.DeleteAllOnSubmit(pf);
            db.Fotos.DeleteAllOnSubmit(fotos);
            db.SubmitChanges();
            ViewBag.sucessoRI = "Imóvel registado com sucesso!!!";
            
            return RedirectToAction("DetalhesImoveis", "Imoveis", new { imovelid = id_imovel }); //nao retorna nada
            
        }

    }
}