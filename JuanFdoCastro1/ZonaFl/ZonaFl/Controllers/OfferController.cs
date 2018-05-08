using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZonaFl.Business.SubSystems;
using ZonaFl.Persistence.Entities;
using Omu.ValueInjecter;
using ZonaFl.Models;
using ZonaFl.Controllers.Filters;
using System.Globalization;
using ZonaFl.Business;

namespace ZonaFl.Controllers
{
    [HandleError(View = "~/Views/Error/Index")]
    public class OfferController : Controller
    {
        // GET: Offer
       
        public ActionResult Index(string id)
        {
            try
            {
                SOffer soffer = new SOffer();
                int pagenumber = 1;//int.Parse( Request.QueryString.Get("pagenumber"));
                int itemsperpage = 50;// int.Parse(Request.QueryString.Get("itemsperpage"));
                string conditions = "";

                ///user session bag
                ZonaFl.Persistence.Entities.AspNetUsers useru = null;
                ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
                useru = usern.GetUserById(new Guid(id));
                if (SessionBag.Current.User == null)
                {
                    SessionBag.Current.User = useru;
                }
                if (!useru.Freelance)
                {
                    conditions = " where iduser='" + id + "'and(O.Status=0 )"; //Request.QueryString.Get("conditions");
                }
                else
                {
                    conditions = " where OP.StatusPhase<>2 and OP.StatusPhase<>3 and OP.StatusPhase<>4 AND  O.Id NOT IN(SELECT Project.idoffer  from project)"; //Request.QueryString.Get("conditions");
                }

                var skills = useru.Skills.Where(e => e.CategoryId != null).ToList();
                var categories = skills.GroupBy(n => n.CategoryId).ToList();
                int i = 0;
                foreach (var cat in categories)
                {
                    if (i == 0)
                    {
                        conditions += " and (O.CategoryId=" + cat.Key;
                    }
                    else
                    {
                        conditions += " or O.CategoryId=" + cat.Key;

                    }
                    i += 1;
                }
                conditions += ")";

                RegisterBindingModel regbm = new RegisterBindingModel();
                regbm.InjectFrom(useru);
                regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
                SessionBag.Current.User = regbm;
                ViewBag.IdUser = id;
                ViewBag.User = useru.UserName;
                ViewBag.ImageUser = SessionBag.Current.User.Image;
                /////

                string order = Request.QueryString.Get("order");
                SCategory scat = new SCategory();
                List<ZonaFl.Persistence.Entities.Category> listcat = scat.FindAll();
                ViewBag.Categories = listcat;
                List<Persistence.Entities.Offer> lista = soffer.GetListPaged(pagenumber, itemsperpage, conditions, order);
                List<Models.Offer> listoffers = lista.Select(e => new Models.Offer().InjectFrom(e)).Cast<Models.Offer>().ToList();
                List<Persistence.Entities.Category> listcategories = lista.Select(e => e.Category).ToList();

                Dictionary<int, string> dicofferexiste = new Dictionary<int, string>();
                int offeruserexist = -1;
                if (TempData["OfferMessage"] != null)
                {

                    dicofferexiste = (Dictionary<int, string>)TempData["OfferMessage"];
                    offeruserexist = dicofferexiste.FirstOrDefault().Key;
                }

                foreach (var offer in lista)
                {

                    var offerget = soffer.GetById(offer.Id);
                    if (offerget != null)
                        offer.OfferPhases = offerget.OfferPhases.Select(e => new OfferPhases().InjectFrom(e)).Cast<OfferPhases>().ToList();
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).OfferPhases = offer.OfferPhases.Select(e => new OfferPhase().InjectFrom(e)).Cast<OfferPhase>().ToList();
                    if (listoffers.FirstOrDefault(e => e.Id == offer.Id) != null)
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).Category = new Models.Category();//.InjectFrom(offer.Category);
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).Category.InjectFrom(offer.Category);
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCity = ((RegisterBindingModel)SessionBag.Current.User).City;
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCountry = ((RegisterBindingModel)SessionBag.Current.User).Country;
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).NameContractor = ((RegisterBindingModel)SessionBag.Current.User).FirstMiddleName;
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).NoPostulados = soffer.GetNoPostuladosByOffer(offer.Id);
                    if (offer.Id == offeruserexist)
                    {
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).Applicada = true;
                    }

                    var dateoferfase1 = soffer.GetPhaseInitial(offer.Id);
                    if (dateoferfase1 == null)
                    {
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = DateTime.Parse("01/01/1900");
                    }
                    else
                    {
                        listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = dateoferfase1.InitPhase;
                    }
                }
                //listoffers.ForEach(e => e.Category.InjectFrom(listcategories.Where(i => i.Id == e.Id).FirstOrDefault()));
                if (((RegisterBindingModel)SessionBag.Current.User).Freelance)
                {
                    return View("DetailsForFreelance", listoffers);
                }
                else
                {
                    return RedirectToAction("Index", "Projects", new { id = SessionBag.Current.User.Id });
                }
            }
            catch (Exception er)
            {
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                   
                }
                else
                {
                    logger2.Error(er);
                }
            }

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public ActionResult AplicarOferta(int Id)
        {
            SOffer soffer = new SOffer();
            int? rta = 0;
            try
            {
                rta = soffer.InsertUserOffer(Id, ((RegisterBindingModel)SessionBag.Current.User).Id, true);

                if (rta == -1)
                {
                    Dictionary<int, string> dicofferexiste = new Dictionary<int, string>();
                    dicofferexiste.Add(Id, "Ya aplicaste a esta oferta anteriormente");
                    TempData["OfferMessage"] = dicofferexiste;// "Ya aplicaste a esta oferta anteriormente";
                }
            }
            catch (Exception er)
            {
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);

                }
                else
                {
                    logger2.Error(er);
                }
            }
            return Json(rta, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Filter(string filter)
        {
            if(!string.IsNullOrEmpty(filter) && filter.EndsWith("or "))
            {
                filter= " and " + ReplaceLastOccurrence(filter, "or", "");
               
            }

            if (!string.IsNullOrEmpty(filter) && filter.EndsWith("and "))
            {
                filter = " and " + ReplaceLastOccurrence(filter, "and", "");

            }

            SOffer soffer = new SOffer();
            int pagenumber = 1;//int.Parse( Request.QueryString.Get("pagenumber"));
            int itemsperpage = 50;// int.Parse(Request.QueryString.Get("itemsperpage"));
            string conditions = "";

            ///user session bag
            ZonaFl.Persistence.Entities.AspNetUsers useru = null;
            ZonaFl.Business.SubSystems.SUser usern = new Business.SubSystems.SUser();
            string id = ((RegisterBindingModel)SessionBag.Current.User).Id;
            useru = usern.GetUserById(new Guid(id));
            if (!useru.Freelance)
            {
               
                conditions = " where iduser='" + id+ "' " + filter + "' and OP.StatusPhase<>2 and OP.StatusPhase<>3; "; //Request.QueryString.Get("conditions");
            }
            else
            {
                conditions = " where OP.StatusPhase<>2 and OP.StatusPhase<>3 AND  O.Id NOT IN(SELECT Project.idoffer  from project) " + filter ; //Request.QueryString.Get("conditions");
            }
             //se  quitan los skill porque aqui se puede haber filtrado por skills diferentes
            //var skills = useru.Skills.Where(e => e.CategoryId != null).ToList();
            //var categories = skills.GroupBy(n => n.CategoryId).ToList();
            //int i = 0;
            //foreach (var cat in categories)
            //{
            //    if (i == 0)
            //    {
            //        conditions += " and (O.CategoryId=" + cat.Key;
            //    }
            //    else
            //    {
            //        conditions += " or O.CategoryId=" + cat.Key;

            //    }
            //    i += 1;
            //}
            //conditions += ")";

            RegisterBindingModel regbm = new RegisterBindingModel();
            regbm.InjectFrom(useru);
            regbm.Skills = useru.Skills.Select(e => new ZonaFl.Models.Skill().InjectFrom(e)).Cast<ZonaFl.Models.Skill>().ToList();
            //SessionBag.Current.User = regbm;
            ViewBag.IdUser = id;
            /////

            string order = Request.QueryString.Get("order");
            SCategory scat = new SCategory();
            List<ZonaFl.Persistence.Entities.Category> listcat = scat.FindAll();
            ViewBag.Categories = listcat;
            List<Persistence.Entities.Offer> lista = soffer.GetListPaged(pagenumber, itemsperpage, conditions, order);
            List<Models.Offer> listoffers = lista.Select(e => new Models.Offer().InjectFrom(e)).Cast<Models.Offer>().ToList();
            List<Persistence.Entities.Category> listcategories = lista.Select(e => e.Category).ToList();

            Dictionary<int, string> dicofferexiste = new Dictionary<int, string>();
            int offeruserexist = -1;
            if (TempData["OfferMessage"] != null)
            {

                dicofferexiste = (Dictionary<int, string>)TempData["OfferMessage"];
                offeruserexist = dicofferexiste.FirstOrDefault().Key;
            }

            foreach (var offer in lista)
            {

                var offerget = soffer.GetById(offer.Id);
                if (offerget != null)
                    offer.OfferPhases = offerget.OfferPhases.Select(e => new OfferPhases().InjectFrom(e)).Cast<OfferPhases>().ToList();
                listoffers.FirstOrDefault(e => e.Id == offer.Id).OfferPhases = offer.OfferPhases.Select(e => new OfferPhase().InjectFrom(e)).Cast<OfferPhase>().ToList();
                if (listoffers.FirstOrDefault(e => e.Id == offer.Id) != null)
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).Category = new Models.Category();//.InjectFrom(offer.Category);
                listoffers.FirstOrDefault(e => e.Id == offer.Id).Category.InjectFrom(offer.Category);
                listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCity = ((RegisterBindingModel)SessionBag.Current.User).City;
                listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCountry = ((RegisterBindingModel)SessionBag.Current.User).Country;
                listoffers.FirstOrDefault(e => e.Id == offer.Id).NameContractor = ((RegisterBindingModel)SessionBag.Current.User).FirstMiddleName;
                if (offer.Id == offeruserexist)
                {
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).Applicada = true;
                }

                var dateoferfase1 = soffer.GetPhaseInitial(offer.Id);
                if (dateoferfase1 == null)
                {
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = DateTime.Parse("01/01/1900");
                }
                else
                {
                    listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = dateoferfase1.InitPhase;
                }
            }
            //listoffers.ForEach(e => e.Category.InjectFrom(listcategories.Where(i => i.Id == e.Id).FirstOrDefault()));
            if (((RegisterBindingModel)SessionBag.Current.User).Freelance)
            {
                return PartialView("PartialFilterOffer", listoffers);
            }
            else
            {
                return RedirectToAction("Index", "Projects", new { id = SessionBag.Current.User.Id });
            }

            //SOffer soffer = new SOffer();
            //int pagenumber = 1;//int.Parse( Request.QueryString.Get("pagenumber"));
            //int itemsperpage = 50;// int.Parse(Request.QueryString.Get("itemsperpage"));

            //string conditions = " where iduser='" + ((RegisterBindingModel)SessionBag.Current.User).Id + "' " + filter + "; "; //Request.QueryString.Get("conditions");
            //string order = Request.QueryString.Get("order");
            //SCategory scat = new SCategory();
            //List<ZonaFl.Persistence.Entities.Category> listcat = scat.FindAll();
            //ViewBag.Categories = listcat;
            //List<Persistence.Entities.Offer> lista = soffer.GetListPaged(pagenumber, itemsperpage, conditions, order);
            //List<Models.Offer> listoffers = lista.Select(e => new Models.Offer().InjectFrom(e)).Cast<Models.Offer>().ToList();
            //List<Persistence.Entities.Category> listcategories = lista.Select(e => e.Category).ToList();
            //foreach (var offer in lista)
            //{
            //    if (listoffers.FirstOrDefault(e => e.Id == offer.Id) != null)
            //        listoffers.FirstOrDefault(e => e.Id == offer.Id).Category = new Models.Category();//.InjectFrom(offer.Category);
            //    listoffers.FirstOrDefault(e => e.Id == offer.Id).Category.InjectFrom(offer.Category);
            //    listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCity = ((RegisterBindingModel)SessionBag.Current.User).City;
            //    listoffers.FirstOrDefault(e => e.Id == offer.Id).ContractorCountry = ((RegisterBindingModel)SessionBag.Current.User).Country;
            //    listoffers.FirstOrDefault(e => e.Id == offer.Id).NameContractor = ((RegisterBindingModel)SessionBag.Current.User).FirstMiddleName;
            //   var phase= soffer.GetPhaseInitial(offer.Id);
            //    if (phase != null)
            //    {
            //        ViewBag.InicioEst = phase.InitPhase;
            //        if(offer.OfferPhases.Count>0)
            //        ViewBag.FinEst = offer.OfferPhases.LastOrDefault(e => e.FinishPhase != null).FinishPhase;
            //        listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = soffer.GetPhaseInitial(offer.Id).InitPhase;

            //    }
            //    else
            //    {
            //        listoffers.FirstOrDefault(e => e.Id == offer.Id).DateIniPhase1 = DateTime.Parse("01/01/1900");
            //    }
            //}
            ////listoffers.ForEach(e => e.Category.InjectFrom(listcategories.Where(i => i.Id == e.Id).FirstOrDefault()));
            //if (((RegisterBindingModel)SessionBag.Current.User).Freelance)
            //{
            //    return PartialView("PartialFilterOffer", listoffers);
            //}
            //else
            //{
            //    return PartialView("PartialFilterOffer", listoffers);
            //}



        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        // GET: Offer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Offer/Create
        public ActionResult Create()
        {
            try
            {
                if (SessionBag.Current.User != null)
                {
                    SCategory scate = new SCategory();
                    Models.Offer offer = new Models.Offer();
                    offer.Categories = scate.FindAll();
                    //Request.Referrer.ToString()
                    ViewBag.UrlRef = Request.UrlReferrer.LocalPath;
                    offer.DateIniPhase1 = DateTime.Now.Date;
                    offer.DateEndPhase1 = DateTime.Now.Date;
                    offer.DateIniPhase2 = DateTime.Now.Date;
                    offer.DateEndPhase2 = DateTime.Now.Date;

                    offer.DateIniPhase3 = DateTime.Now.Date;
                    offer.DateEndPhase3 = DateTime.Now.Date;
                    offer.DateIniPhase4 = DateTime.Now.Date;
                    offer.DateEndPhase4 = DateTime.Now.Date;


                    ViewBag.IdUser = SessionBag.Current.User.Id;
                    return View(offer);
                }
                else
                {
                    //offerp.IdUser = "e43b03c5-9b9a-4f03-aa81-46e592d5b358";
                    return RedirectToAction("Index", "Home");
                    //throw new Exception("Favor iniciar sesion con usuario y password ");
                }
            }
            catch(Exception er)
            {
                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                    throw new Exception(er.Message);
                }
                else
                {
                    logger2.Error(er);
                }
            }
            return RedirectToAction("Index", "Home");

        }

        // POST: Offer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection,Models.Offer offer)
        {
            
            string[] formatdate=Helper.ReadSetting("FormatDate").Split(',') ;
            DateTime dateini, datefin;
            DateTime.TryParseExact(collection["DateIniPhase1"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateini);//DateTime.ParseExact(offer.DateIniPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
            DateTime.TryParseExact(collection["DateEndPhase1"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out datefin);//DateTime.ParseExact(offer.DateEndPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
          
            Persistence.Entities.Offer offerp = new Persistence.Entities.Offer();
          
            var category = collection["Categories"];

            try
            {

                // TODO: Add insert logic here
                SOffer soffer = new SOffer();
               
                
               
                offerp.InjectFrom(offer);
               
                if (SessionBag.Current.User != null)
                {
                    offerp.IdUser = SessionBag.Current.User.Id;
                    ViewBag.IdUser = offerp.IdUser;
                }
                else
                {
                    //offerp.IdUser = "e43b03c5-9b9a-4f03-aa81-46e592d5b358";
                    RedirectToAction("Index", "Home");
                    //throw new Exception("Favor iniciar sesion con usuario y password ");
                }
                offerp.CategoryId = int.Parse(category);
                if (offer.ValueFixedProject > 0 && offer.RangoValor == 0)
                {
                    offerp.PrecioFijo = true;
                    offerp.RangoValor = GetRangoValor(offerp.ValueFixedProject);
                }
                else
                {
                    offerp.PrecioFijo = false;
                }
                offerp=soffer.Insert(offerp);

                CreateOfferPhases(offerp, offer, formatdate, collection);
                
                if(SessionBag.Current.User.Freelance)
                    {
                    return RedirectToAction("Index", new { id = SessionBag.Current.User.Id });
                }
                else
                {
                    return RedirectToAction("Index", new { id = SessionBag.Current.User.Id });
                }
               
            }
            catch (Exception er)
            {

                Log4NetLogger logger2 = new Log4NetLogger();
                logger2.CurrentUser = SessionBag.Current.User.Id;
                if (Request != null)
                {
                    logger2.Error(er, Request.Path, Request.RawUrl);
                    //throw new Exception(er.Message);
                }
                else
                {
                    logger2.Error(er);
                }
            }

            return RedirectToAction("Index", new { id = SessionBag.Current.User.Id } );
        }

        private int GetRangoValor(int valueFixedProject)
        {
           if(valueFixedProject<500000)
            {
                return 1;
            }
           else if(valueFixedProject>=500000 && valueFixedProject<1000000)
            {
                return 2;
            }
            else if (valueFixedProject >= 1000000 && valueFixedProject<2000000)
            {
                return 3;
            }
            else if (valueFixedProject >= 2000000 && valueFixedProject < 3000000)
            {
                return 4;
            }
            else if (valueFixedProject >= 3000000 && valueFixedProject < 4000000)
            {
                return 5;
            }
           else
            {

                return 6;
            }
        }

        private void CreateOfferPhases(Persistence.Entities.Offer offerp, Models.Offer offer, string[] formatdate, FormCollection collection)
        {
            
            List<Persistence.Entities.OfferPhases> ListOfferPhases = new List<Persistence.Entities.OfferPhases>();
            DateTime dateini, datefin;
            if (!string.IsNullOrEmpty(offer.Phase1Name))
            {
                if(offer.PercentValuePhase1!=0 && offer.ValuePhase1==0)
                {
                    offer.ValuePhase1 = offer.PercentValuePhase1;
                }
               
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                offerphas.IdOffer = offerp.Id;
                offerphas.Name = offer.Phase1Name;
                DateTime.TryParseExact(collection["DateIniPhase1"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateini);//DateTime.ParseExact(offer.DateIniPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                 DateTime.TryParseExact(collection["DateEndPhase1"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out datefin);//DateTime.ParseExact(offer.DateEndPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                offerphas.InitPhase = dateini;
                offerphas.FinishPhase = datefin;
                offerphas.Value = offer.ValuePhase1;
                offerphas.StatusPhase = Persistence.Entities.StatusPhase.SinIniciar;
                ListOfferPhases.Add(offerphas);
            }

            if (!string.IsNullOrEmpty(offer.Phase2Name))
            {

                if (offer.PercentValuePhase2 != 0 && offer.ValuePhase2 == 0)
                {
                    offer.ValuePhase2 = offer.PercentValuePhase2;
                }
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                offerphas.IdOffer = offerp.Id;
                offerphas.Name = offer.Phase2Name;
                DateTime.TryParseExact(collection["DateIniPhase2"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateini);//DateTime.ParseExact(offer.DateIniPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                DateTime.TryParseExact(collection["DateEndPhase2"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out datefin);//DateTime.ParseExact(offer.DateEndPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                offerphas.InitPhase = dateini;
                offerphas.FinishPhase = datefin;
                offerphas.Value = offer.ValuePhase2;
                offerphas.StatusPhase = Persistence.Entities.StatusPhase.SinIniciar;
                ListOfferPhases.Add(offerphas);
            }

            if (!string.IsNullOrEmpty(offer.Phase3Name))
            {
                if (offer.PercentValuePhase3 != 0 && offer.ValuePhase3 == 0)
                {
                    offer.ValuePhase3 = offer.PercentValuePhase3;
                }
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                offerphas.IdOffer = offerp.Id;
                offerphas.Name = offer.Phase3Name;
                DateTime.TryParseExact(collection["DateIniPhase3"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateini);//DateTime.ParseExact(offer.DateIniPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                DateTime.TryParseExact(collection["DateEndPhase3"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out datefin);//DateTime.ParseExact(offer.DateEndPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                offerphas.InitPhase = dateini;
                offerphas.FinishPhase = datefin;
                offerphas.Value = offer.ValuePhase3;
                offerphas.StatusPhase = Persistence.Entities.StatusPhase.SinIniciar;
                ListOfferPhases.Add(offerphas);
            }

            if (!string.IsNullOrEmpty(offer.Phase4Name))
            {
                if (offer.PercentValuePhase4 != 0 && offer.ValuePhase4 == 0)
                {
                    offer.ValuePhase4 = offer.PercentValuePhase4;
                }
                
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                offerphas.IdOffer = offerp.Id;
                offerphas.Name = offer.Phase4Name;
                DateTime.TryParseExact(collection["DateIniPhase4"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateini);//DateTime.ParseExact(offer.DateIniPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                DateTime.TryParseExact(collection["DateEndPhase4"], formatdate, CultureInfo.InvariantCulture, DateTimeStyles.None, out datefin);//DateTime.ParseExact(offer.DateEndPhase1.ToShortDateString(), formatdate, CultureInfo.InvariantCulture);
                offerphas.InitPhase = dateini;
                offerphas.FinishPhase = datefin;
                offerphas.Value = offer.ValuePhase4;
                offerphas.StatusPhase = Persistence.Entities.StatusPhase.SinIniciar;
                ListOfferPhases.Add(offerphas);
            }

            SOffer soffer = new SOffer();
            soffer.InsertPhases(ListOfferPhases);
        }

        private Models.Offer AddOfferPhasesToOffer(Persistence.Entities.Offer offerp, Models.Offer offer)
        {

           // List<Persistence.Entities.OfferPhases> ListOfferPhases = new List<Persistence.Entities.OfferPhases>();
            if(offerp.OfferPhases.Count>0)
            {
                int i = 1;
                foreach(var fases in offerp.OfferPhases)
                {
                    if (i == 1)
                    {
                        offer.Phase1Name = fases.Name;
                        offer.DateIniPhase1 = fases.InitPhase;
                        offer.DateEndPhase1 = fases.FinishPhase;
                        if (offerp.ValueFixedProject>0)
                        {
                            offer.ValuePhase1 = fases.Value;
                        }
                        else
                        {
                            offer.PercentValuePhase1 = fases.Value;
                        }


                    }
                    else if (i == 2)
                    {
                        offer.Phase2Name = fases.Name;
                        offer.DateIniPhase2 = fases.InitPhase;
                        offer.DateEndPhase2 = fases.FinishPhase;
                        if (offerp.ValueFixedProject > 0)
                        {
                            offer.ValuePhase2 = fases.Value;
                        }
                        else
                        {
                            offer.PercentValuePhase2 = fases.Value;
                        }

                    }
                    else if (i == 3)
                    {
                        offer.Phase3Name = fases.Name;
                        offer.DateIniPhase3 = fases.InitPhase;
                        offer.DateEndPhase3 = fases.FinishPhase;
                        if (offerp.ValueFixedProject > 0)
                        {
                            offer.ValuePhase3 = fases.Value;
                        }
                        else
                        {
                            offer.PercentValuePhase3 = fases.Value;
                        }

                    }
                    else if (i == 4)
                    {
                        offer.Phase4Name = fases.Name;
                        offer.DateIniPhase4 = fases.InitPhase;
                        offer.DateEndPhase4 = fases.FinishPhase;
                        if (offerp.ValueFixedProject > 0)
                        {
                            offer.ValuePhase4 = fases.Value;
                        }
                        else
                        {
                            offer.PercentValuePhase4 = fases.Value;
                        }

                    }
                    i += 1;
                }
            }

            return offer;
            //if (!string.IsNullOrEmpty(offer.Phase1Name))
            //{
            //    Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
            //    offerphas.IdOffer = offerp.Id;
            //    offerphas.Name = offer.Phase1Name;
            //    offerphas.InitPhase = offer.DateIniPhase1;
            //    offerphas.FinishPhase = offer.DateEndPhase1;
            //    offerphas.Value = offer.ValuePhase1;
            //    offerphas.StatusPhase = (short)StatusPhase.Aplicada;
            //    ListOfferPhases.Add(offerphas);
            //}

            //if (!string.IsNullOrEmpty(offer.Phase2Name))
            //{
            //    Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
            //    offerphas.IdOffer = offerp.Id;
            //    offerphas.Name = offer.Phase2Name;
            //    offerphas.InitPhase = offer.DateIniPhase2;
            //    offerphas.FinishPhase = offer.DateEndPhase2;
            //    offerphas.Value = offer.ValuePhase2;
            //    offerphas.StatusPhase = (short)StatusPhase.Aplicada;
            //    ListOfferPhases.Add(offerphas);
            //}

            //if (!string.IsNullOrEmpty(offer.Phase3Name))
            //{
            //    Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
            //    offerphas.IdOffer = offerp.Id;
            //    offerphas.Name = offer.Phase3Name;
            //    offerphas.InitPhase = offer.DateIniPhase3;
            //    offerphas.FinishPhase = offer.DateEndPhase3;
            //    offerphas.Value = offer.ValuePhase3;
            //    offerphas.StatusPhase = (short)StatusPhase.Aplicada;
            //    ListOfferPhases.Add(offerphas);
            //}

            //if (!string.IsNullOrEmpty(offer.Phase4Name))
            //{
            //    Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
            //    offerphas.IdOffer = offerp.Id;
            //    offerphas.Name = offer.Phase4Name;
            //    offerphas.InitPhase = offer.DateIniPhase4;
            //    offerphas.FinishPhase = offer.DateEndPhase4;
            //    offerphas.Value = offer.ValuePhase4;
            //    offerphas.StatusPhase = (short)StatusPhase.Aplicada;
            //    ListOfferPhases.Add(offerphas);
            //}

            
        }

        // GET: Offer/Edit/5
        public ActionResult Edit(int id)
        {


            SOffer soffer = new SOffer();
            Persistence.Entities.Offer offer = soffer.GetById(id);
            
            Models.Offer offerm = new Models.Offer();
            SCategory scate = new SCategory();

            offerm.Categories = scate.FindAll();
            offerm.InjectFrom(offer);
            offerm.Category = new Models.Category();
            offerm.Category.InjectFrom(offer.Category);
            AddOfferPhasesToOffer(offer, offerm);
            ViewBag.UrlRef = Request.UrlReferrer.LocalPath;
            ViewBag.IdUser = SessionBag.Current.User.Id;

            return View(offerm);
        }



        // POST: Offer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, Models.Offer offer)
        {
            try
            {
                // TODO: Add update logic here
                SOffer soffer = new SOffer();
                Persistence.Entities.Offer offerp = soffer.GetById(id);
                var category = collection["Categories"];
                offerp.CategoryId = int.Parse(category);
                foreach(var prop in offerp.GetType().GetProperties())
                {
                    if (offerp.GetType().GetProperty(prop.Name) != null)
                    {
                        var value = offerp.GetType().GetProperty(prop.Name).GetValue(offerp);
                        if (offer.GetType().GetProperty(prop.Name) != null)
                        {
                            if (offer.GetType().GetProperty(prop.Name).GetValue(offer)!=null && value.ToString() != offer.GetType().GetProperty(prop.Name).GetValue(offer).ToString())
                            {
                                offerp.GetType().GetProperty(prop.Name).SetValue(offerp, offer.GetType().GetProperty(prop.Name).GetValue(offer));
                            }
                        }
                    }
                }
                //offerp.InjectFrom(offer);
                Persistence.Entities.Offer updaoofer = soffer.Update(offerp);
                UpdateOfferPhases(offerp, offer);
                return RedirectToAction("Index","Projects", new { id = SessionBag.Current.User.Id });
            }
            catch (Exception er)
            {

                //offer.MessageError = er.Message;

                return View("Error");
            }
        }


        private void UpdateOfferPhases(Persistence.Entities.Offer offerp, Models.Offer offer)
        {

            List<Persistence.Entities.OfferPhases> ListOfferPhases = new List<Persistence.Entities.OfferPhases>();
            List<Persistence.Entities.OfferPhases> ListOfferPhasesAdd = new List<Persistence.Entities.OfferPhases>();

            if (!string.IsNullOrEmpty(offer.Phase1Name))
            {
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                offerphas.Id = offerp.OfferPhases.FirstOrDefault().Id;
                offerphas.IdOffer = offerp.Id;
                offerphas.Name = offer.Phase1Name;
                offerphas.InitPhase = offer.DateIniPhase1;
                offerphas.FinishPhase = offer.DateEndPhase1;
                offerphas.Value = offer.ValuePhase1;
                offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                ListOfferPhases.Add(offerphas);
            }

            if (!string.IsNullOrEmpty(offer.Phase2Name))
            {
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                if (offerp.OfferPhases.ToArray().Length >= 2)
                {
                    offerphas.Id = offerp.OfferPhases.ToArray()[1].Id;
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase2Name;
                    offerphas.InitPhase = offer.DateIniPhase2;
                    offerphas.FinishPhase = offer.DateEndPhase2;
                    offerphas.Value = offer.ValuePhase2;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhases.Add(offerphas);
                }
                else
                {
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase2Name;
                    offerphas.InitPhase = offer.DateIniPhase2;
                    offerphas.FinishPhase = offer.DateEndPhase2;
                    offerphas.Value = offer.ValuePhase2;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhasesAdd.Add(offerphas);
                }
            }

            if (!string.IsNullOrEmpty(offer.Phase3Name))
            {
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                if (offerp.OfferPhases.ToArray().Length >= 3)
                {
                  
                    offerphas.Id = offerp.OfferPhases.ToArray()[2].Id;
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase3Name;
                    offerphas.InitPhase = offer.DateIniPhase3;
                    offerphas.FinishPhase = offer.DateEndPhase3;
                    offerphas.Value = offer.ValuePhase3;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhases.Add(offerphas);
                }
                else
                {
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase3Name;
                    offerphas.InitPhase = offer.DateIniPhase3;
                    offerphas.FinishPhase = offer.DateEndPhase3;
                    offerphas.Value = offer.ValuePhase3;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhasesAdd.Add(offerphas);
                }

            }

            if (!string.IsNullOrEmpty(offer.Phase4Name))
            {
                Persistence.Entities.OfferPhases offerphas = new Persistence.Entities.OfferPhases();
                if (offerp.OfferPhases.ToArray().Length >= 4)
                {
                    offerphas.Id = offerp.OfferPhases.ToArray()[3].Id;
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase4Name;
                    offerphas.InitPhase = offer.DateIniPhase4;
                    offerphas.FinishPhase = offer.DateEndPhase4;
                    offerphas.Value = offer.ValuePhase4;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhases.Add(offerphas);
                }
                else
                {
                    offerphas.IdOffer = offerp.Id;
                    offerphas.Name = offer.Phase4Name;
                    offerphas.InitPhase = offer.DateIniPhase4;
                    offerphas.FinishPhase = offer.DateEndPhase4;
                    offerphas.Value = offer.ValuePhase4;
                    offerphas.StatusPhase = Persistence.Entities.StatusPhase.EnCurso;
                    ListOfferPhasesAdd.Add(offerphas);
                }
            }

            SOffer soffer = new SOffer();
            soffer.UpdatePhases(ListOfferPhases, offer.Id);
            //Por si acaso hay pahases que en formulario de edicón se adicionan masfases
            soffer.InsertPhases(ListOfferPhasesAdd);
        }

        // GET: Offer/Delete/5
        public ActionResult Delete(int id)
        {

            SOffer soffer = new SOffer();
            Persistence.Entities.Offer offer = soffer.Get(id);
            int? rta = 0;
            if (offer.OfferPhases != null && offer.OfferPhases.Count > 0)
            {
               
            rta = soffer.ChangeStatusPhases(offer.OfferPhases, Persistence.Entities.StatusPhase.Eliminada, offer);
            }


            return Json(rta, JsonRequestBehavior.AllowGet);
          
        }

        public ActionResult Declinar(int id)
        {

            SOffer soffer = new SOffer();
            Persistence.Entities.Offer offer = soffer.Get(id);
            int? rta = 0;
            if (offer.OfferPhases != null && offer.OfferPhases.Count > 0)
            {

                rta = soffer.ChangeStatusPhases(offer.OfferPhases, Persistence.Entities.StatusPhase.Finalizada, offer);
            }


            return Json(rta, JsonRequestBehavior.AllowGet);

        }
        // POST: Offer/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here
        //        SOffer soffer = new SOffer();

        //        soffer.Delete(id);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
