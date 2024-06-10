using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stock.Models;

namespace Stock.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ConnectionPostgres c = new();
        Articles article = new();
        Magasins magasin = new();
        Type_mouvement type_mouvement = new();
        // Mouvements mouvement = new();
        // Validation validation = new();
        List<Articles> Articles = article.SelectAllArticles(c);
        List<Magasins> Magasins = magasin.SelectAllMagasin(c);
        List<Type_mouvement> types_mouvement = type_mouvement.SelectAllTypeMouvement(c);
        // DateTime d = new(2023,01,01);
        // DateTime d1 = new(2023,12,31);
        // ViewBag.valiny = mouvement.ExceptionDestockage(c,d,"MGS1","VAR001");
        ViewData["Articles"] = Articles;
        ViewData["Magasins"] = Magasins;
        ViewData["Types_mouvement"] = types_mouvement;
        c.Close();
        return View();
    }
    public IActionResult Sortie_stock(){
        ConnectionPostgres c = new();
        Articles article = new();
        Magasins magasin = new();
        List<Articles> Articles = article.SelectAllArticles(c);
        List<Magasins> Magasins = magasin.SelectAllMagasin(c);
        ViewData["Articles"] = Articles;
        ViewData["Magasins"] = Magasins;
        c.Close();
        return View();
    }
    public IActionResult Mouvement_stock(DateTime date_debut, DateTime date_fin,string article_id,string magasin_id){
        ConnectionPostgres c = new();
        Validation validation = new();
        Mouvements mouvement = new();
        int Etat = 0;
        string type_sortie_id = mouvement.getTypeSortie(c,article_id);
        List<Validation> mo = validation.CalculerQuantiteDestockerSelonType(c,date_debut,date_fin.Date,article_id,magasin_id,type_sortie_id);
        List<Mouvements> mouvements = mouvement.SelectAllMouvementbyEtat(c,Etat);
        ViewData["mo"] = mo;
        ViewData["mouvements"] = mouvements;
        int mouvement_id = HttpContext.Session.GetInt32("Vali") ?? 0;
        ViewBag.m = mouvement_id;

        c.Close();
        return View();
    }
    public IActionResult Voir_Sortie_stock(DateTime date_debut, DateTime date_fin,string article_id,string magasin_id){
        ConnectionPostgres c = new();
        Mouvements mouvement = new();
        Validation validation = new();
        string type_sortie_id = mouvement.getTypeSortie(c,article_id);
        List<Validation> mo = validation.CalculerQuantiteDestockerSelonType(c,date_debut,date_fin.Date,article_id,magasin_id,type_sortie_id);
        List<Validation> validations = validation.SelectAllValidation(c);
        ViewData["mo"] = mo;
        ViewData["validations"] = validations;
        c.Close();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Etat_stock(){
        ConnectionPostgres c = new();
        Articles article = new();
        Magasins magasin = new();
        List<Articles> Articles = article.SelectAllArticles(c);
        List<Magasins> Magasins = magasin.SelectAllMagasin(c);
        ViewData["Articles"] = Articles;
        ViewData["Magasins"] = Magasins;
        c.Close();
        return View();
    }
    public IActionResult Result_etat_stock(DateTime date_debut, DateTime date_fin,string article_id,string magasin_id){
        ConnectionPostgres c = new();
        Mouvements mouv = new();
        Etat_stock etat = new();
        List<Etat_stock> etats = etat.Etats(date_debut,date_fin,article_id,magasin_id);
        ViewData["etat"] = etats;
        ViewBag.mess = mouv.getMessage(c,date_debut,date_fin,article_id,magasin_id);
        c.Close();
        return View();
    }
    public IActionResult InsertMouvement(string mouvement_id, DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id,int etat){
        ConnectionPostgres c = new();
        Mouvements mouvement = new(mouvement_id,daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id,etat);

        if(mouvement.getSommeQuantiteSortie(c,article_id,magasin_id) + quantite > mouvement.getSommeQuantiteEntre(c,article_id,magasin_id) && type_mouvement_id.Equals("TM2")){
                ViewBag.exceptionDestockage = mouvement.exceptionDestockage();
                return View();
        }
        else if(prix_unitaire < 0 || quantite <0){
            ViewBag.prixException = mouvement.prixException();
                return View();
        }
        else if(mouvement.ExceptionDestockage(c,daty,magasin_id,article_id) == 405){
            ViewBag.message = mouvement.MessageExceptionDestockage();
            return View();
        }
        

        mouvement.Insert(c);
        c.Close();
        return RedirectToAction("Index");
    }
    public IActionResult GoTo_Modif(string mouvement_id){
        HttpContext.Session.SetString("Mouvement_id", mouvement_id);
        ConnectionPostgres c = new();
        Articles article = new();
        Magasins magasin = new();
        Type_mouvement type_mouvement = new();
        List<Articles> Articles = article.SelectAllArticles(c);
        List<Magasins> Magasins = magasin.SelectAllMagasin(c);
        List<Type_mouvement> types_mouvement = type_mouvement.SelectAllTypeMouvement(c);
        ViewData["Articles"] = Articles;
        ViewData["Magasins"] = Magasins;
        ViewData["Types_mouvement"] = types_mouvement;
        c.Close();
        return View();
    }
    public IActionResult Modifier(DateTime daty,string article_id, double quantite,  double prix_unitaire, string type_mouvement_id, string magasin_id){
        ConnectionPostgres c = new();
        c.Open();
        Mouvements mouvement = new();
        if(mouvement.getSommeQuantiteSortie(c,article_id,magasin_id) + quantite > mouvement.getSommeQuantiteEntre(c,article_id,magasin_id) && type_mouvement_id.Equals("TM2")){
                ViewBag.exceptionDestockage = mouvement.exceptionDestockage();
                return View("InsertMouvement");
        }
        else if(prix_unitaire < 0 || quantite <0){
            ViewBag.prixException = mouvement.prixException();
                return View("InsertMouvement");
        }
        string mouvement_id = HttpContext.Session.GetString("Mouvement_id");
        int etat = HttpContext.Session.GetInt32("Etat") ?? 0;
        mouvement.UpdateMouvementbyId(c,mouvement_id,daty,article_id,quantite,prix_unitaire,magasin_id,type_mouvement_id);
        c.Close();
        return RedirectToAction("Deconnect");
    }
    public IActionResult DeleteMouvement(string mouvement_id){
        ConnectionPostgres c = new();
        c.Open();
        Mouvements mouvement = new();
        mouvement.DeleteMouvementbyId(c,mouvement_id);
        c.Close();
        return RedirectToAction("Mouvement_stock");
    }
    public IActionResult Deconnect(){
        HttpContext.Session.Clear();
        return Redirect("Mouvement_stock");
    }
    public IActionResult Validation(){
        ConnectionPostgres c = new();
        c.Open();
        Validation valid = new();
        valid.InsertValidation(c);
        valid.DeleteMouvement(c);
        c.Close();
        return RedirectToAction("Mouvement_stock");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
