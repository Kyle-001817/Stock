using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Mouvements
    {
        public Mouvements()
        {
        }

        public Mouvements(string mouvement_id, DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id, int etat)
        {
            this.mouvement_id = mouvement_id;
            this.daty = daty;
            this.quantite = quantite;
            this.article_id = article_id;
            this.prix_unitaire = prix_unitaire;
            this.type_mouvement_id = type_mouvement_id;
            this.magasin_id = magasin_id;
            this.etat = etat;
        }
        public Mouvements(DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id, int etat)
        {
            this.daty = daty;
            this.quantite = quantite;
            this.article_id = article_id;
            this.prix_unitaire = prix_unitaire;
            this.type_mouvement_id = type_mouvement_id;
            this.magasin_id = magasin_id;
            this.etat = etat;
        }

        public Mouvements(string mouvement_id, DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id, int etat, string nom_article, string nom_type_mouvement, string nom_magasin) : this(mouvement_id, daty, quantite, article_id, prix_unitaire, type_mouvement_id, magasin_id, etat)
        {
            this.nom_article = nom_article;
            this.nom_type_mouvement = nom_type_mouvement;
            this.nom_magasin = nom_magasin;
        }

        public string mouvement_id { get; set; }
        public DateTime daty { get; set; }
        public double quantite { get; set; }
        public string article_id { get; set; }
        public double prix_unitaire { get; set; }
        public string type_mouvement_id { get; set; }
        public string magasin_id { get; set; }
        public int etat { get; set; }
        public string nom_article {get; set;}
        public string nom_type_mouvement {get; set;}
        public string nom_magasin {get; set;}

        public List<Mouvements> SelectAllMouvement(ConnectionPostgres c)
        {
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from v_mouvements");
            List<Mouvements> list = new();
            while (datareader.Read())
            {
                string mouvement_id = (string)datareader.GetString(0);
                DateTime daty = (DateTime)datareader.GetDateTime(1);
                double quantite = (double)datareader.GetDouble(2);
                string article_id = (string)datareader.GetString(3);
                double prix_unitaire = (double)datareader.GetDouble(4);
                string type_mouvement_id = (string)datareader.GetString(5);
                string magasin_id = (string)datareader.GetString(6);
                int etat = (int)datareader.GetInt32(7);
                string nom_article = (string)datareader.GetString(8);
                string nom_type_mouvement = (string)datareader.GetString(9);
                string nom_magasin = (string)datareader.GetString(10);
                list.Add(new Mouvements(mouvement_id, daty.Date, quantite, article_id, prix_unitaire, type_mouvement_id, magasin_id,etat,nom_article,nom_type_mouvement,nom_magasin));
            }
            datareader.Close();
            c.Close();
            return list;
        }
        public List<Mouvements> SelectAllMouvementbyEtat(ConnectionPostgres c,int etats)
        {
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from v_mouvements where etat = "+etats);
            List<Mouvements> list = new();
            while (datareader.Read())
            {
                string mouvement_id = (string)datareader.GetString(0);
                DateTime daty = (DateTime)datareader.GetDateTime(1);
                double quantite = (double)datareader.GetDouble(2);
                string article_id = (string)datareader.GetString(3);
                double prix_unitaire = (double)datareader.GetDouble(4);
                string type_mouvement_id = (string)datareader.GetString(5);
                string magasin_id = (string)datareader.GetString(6);
                int etat = (int)datareader.GetInt32(7);
                string nom_article = (string)datareader.GetString(8);
                string nom_type_mouvement = (string)datareader.GetString(9);
                string nom_magasin = (string)datareader.GetString(10);
                list.Add(new Mouvements(mouvement_id, daty.Date, quantite, article_id, prix_unitaire, type_mouvement_id, magasin_id,etat,nom_article,nom_type_mouvement,nom_magasin));
            }
            datareader.Close();
            c.Close();
            return list;
        }
        // public List<Mouvements> SelectAllMouvementby(ConnectionPostgres c, string articles_id,string types_mouvement_id,string magasins_id)
        // {
        //     c.Open();
        //     NpgsqlDataReader datareader = c.Execute("SELECT *FROM mouvements where lower(concat(article_id)) like lower('"+articles_id+"%') and lower(concat(type_mouvement_id)) like lower('"+types_mouvement_id+"%') and lower(concat(magasin_id)) like lower('"+magasins_id+"%');");
        //     List<Mouvements> list = new();
        //     while (datareader.Read())
        //     {
        //         string mouvement_id = (string)datareader.GetString(0);
        //         DateTime daty = (DateTime)datareader.GetDateTime(1);
        //         double quantite = (double)datareader.GetDouble(2);
        //         string article_id = (string)datareader.GetString(3);
        //         double prix_unitaire = (double)datareader.GetDouble(4);
        //         string type_mouvement_id = (string)datareader.GetString(5);
        //         string magasin_id = (string)datareader.GetString(6);
        //         list.Add(new Mouvements(mouvement_id, daty, quantite, article_id, prix_unitaire, type_mouvement_id, magasin_id,etat));
        //     }
        //     datareader.Close();
        //     c.Close();
        //     return list;
        // }

        public void Insert(ConnectionPostgres co)
        {
            co.Open();
            string query = "insert into mouvements(daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id) values ('" + this.daty + "'," + this.quantite + ",'"+this.article_id + "',"+this.prix_unitaire+",'"+this.type_mouvement_id+"','"+this.magasin_id+"')";
            co.ExecuteInsert(query);
            co.Close();
        }

        public List<Validation> getDates(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin){
            Validation mouvement = new();
            List<Validation> mouvements = mouvement.SelectAllValidation(c);
            List<Validation> valiny = mouvements.Where(mouvement => mouvement.daty >= dateDebut && mouvement.daty <= dateFin).ToList();
            valiny = valiny.OrderBy(mouvement => mouvement.daty).ToList();
            c.Close();
            return valiny;
        }
        public List<Validation> getListMouvementStockagebyDateFIFO(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin){
            string type_mouvement_id = "TM1";
            Validation mouvement = new();
            List<Validation> mouvements = mouvement.SelectAllValidation(c);
            List<Validation> valiny = mouvements.Where(mouvement => mouvement.daty >= dateDebut && mouvement.daty <= dateFin && mouvement.type_mouvement_id.Equals(type_mouvement_id)).ToList();
            valiny = valiny.OrderBy(mouvement => mouvement.daty).ToList();
            c.Close();
            return valiny;
        }
        public List<Validation> getListMouvementDestockagebyDate(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin){
            string type_mouvement_id = "TM2";
            Validation mouvement = new();
            List<Validation> mouvements = mouvement.SelectAllValidation(c);
            List<Validation> valiny = mouvements.Where(mouvement => mouvement.daty >= dateDebut && mouvement.daty <= dateFin && mouvement.type_mouvement_id.Equals(type_mouvement_id)).ToList();
            valiny = valiny.OrderBy(mouvement => mouvement.daty).ToList();
            c.Close();
            return valiny;
        }
        public List<Validation> getListMouvementStockagebyDateLIFO(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin){
            string type_mouvement_id = "TM1";
            Validation mouvement = new();
            List<Validation> mouvements = mouvement.SelectAllValidation(c);
            List<Validation> valiny = mouvements.Where(mouvement => mouvement.daty >= dateDebut && mouvement.daty <= dateFin && mouvement.type_mouvement_id.Equals(type_mouvement_id)).ToList();
            valiny = valiny.OrderByDescending(mouvement => mouvement.daty).ToList();
            c.Close();
            return valiny;
        }
        public double getSommeQuantiteDestocker(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = 0;
            string type_mouvement_id = "TM2";
            for(int i=0; i<this.getDates(c, dateDebut, dateFin).Count; i++){
                if(this.getDates(c, dateDebut, dateFin)[i].type_mouvement_id.Equals(type_mouvement_id) && this.getDates(c, dateDebut, dateFin)[i].article_id.Equals(article_id) && this.getDates(c, dateDebut, dateFin)[i].magasin_id.Equals(magasin_id)){
                    valiny += this.getDates(c, dateDebut, dateFin)[i].quantite;
                }
            }
            c.Close();
            return valiny;
        }
        public double getSommeQuantiteSortie(ConnectionPostgres c,string article_id,string magasin_id)
        {
            string type_mouvement_id = "TM2";
            double valiny = 0;
            Validation mouvement = new();
            for(int i=0; i<mouvement.SelectAllValidation(c).Count(); i++){
                if(mouvement.SelectAllValidation(c)[i].type_mouvement_id.Equals(type_mouvement_id) && mouvement.SelectAllValidation(c)[i].article_id.Equals(article_id)&&mouvement.SelectAllValidation(c)[i].magasin_id.Equals(magasin_id)){
                    valiny += mouvement.SelectAllValidation(c)[i].quantite;
                }
            }
            c.Close();
            return valiny;
        }
        public double getSommeQuantiteEntre(ConnectionPostgres c,string article_id,string magasin_id)
        {
            string type_mouvement_id = "TM1";
            double valiny = 0;
            Validation mouvement = new();
            for(int i=0; i<mouvement.SelectAllValidation(c).Count(); i++){
                if(mouvement.SelectAllValidation(c)[i].type_mouvement_id.Equals(type_mouvement_id) && mouvement.SelectAllValidation(c)[i].article_id.Equals(article_id)&&mouvement.SelectAllValidation(c)[i].magasin_id.Equals(magasin_id)){
                    valiny += mouvement.SelectAllValidation(c)[i].quantite;
                }
            }
            c.Close();
            return valiny;
        }
        
        public double getSommePUDestocker(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = 0;
            string type_mouvement_id = "TM2";
            for(int i=0; i<this.getDates(c, dateDebut, dateFin).Count; i++){
                if(this.getDates(c, dateDebut, dateFin)[i].type_mouvement_id.Equals(type_mouvement_id) && this.getDates(c, dateDebut, dateFin)[i].article_id.Equals(article_id) && this.getDates(c, dateDebut, dateFin)[i].magasin_id.Equals(magasin_id)){
                    valiny += this.getDates(c, dateDebut, dateFin)[i].prix_unitaire;
                }
            }
            c.Close();
            return valiny;
        }
        public double getSommeQuantiteStocker(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = 0;
            string type_mouvement_id = "TM1";
            for(int i=0; i<this.getDates(c, dateDebut, dateFin).Count; i++){
                if(this.getDates(c, dateDebut, dateFin)[i].type_mouvement_id.Equals(type_mouvement_id) && this.getDates(c, dateDebut, dateFin)[i].article_id.Equals(article_id) && this.getDates(c, dateDebut, dateFin)[i].magasin_id.Equals(magasin_id)){
                    valiny += this.getDates(c, dateDebut, dateFin)[i].quantite;
                }
            }
            c.Close();
            return valiny;
        }
        public double getSommePUStocker(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = 0;
            string type_mouvement_id = "TM1";
            for(int i=0; i<this.getDates(c, dateDebut, dateFin).Count; i++){
                if(this.getDates(c, dateDebut, dateFin)[i].type_mouvement_id.Equals(type_mouvement_id) && this.getDates(c, dateDebut, dateFin)[i].article_id.Equals(article_id) && this.getDates(c, dateDebut, dateFin)[i].magasin_id.Equals(magasin_id)){
                    valiny += this.getDates(c, dateDebut, dateFin)[i].prix_unitaire * this.getDates(c, dateDebut, dateFin)[i].quantite;
                }
            }
            c.Close();
            return valiny;
        }
        public double getQuantite(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = this.getSommeQuantiteStocker(c,dateDebut,dateFin,article_id,magasin_id) - this.getSommeQuantiteDestocker(c,dateDebut,dateFin,article_id,magasin_id);
            c.Close();
            return valiny;
        }
        public double getMontant(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = this.getSommePUStocker(c,dateDebut,dateFin,article_id,magasin_id) - this.getSommePUDestocker(c,dateDebut,dateFin,article_id,magasin_id);
            c.Close();
            return valiny;
        }
        public double getPrixUnitaire(ConnectionPostgres c,DateTime dateDebut, DateTime dateFin,string article_id,string magasin_id){
            double valiny = getMontant(c,dateDebut,dateFin,article_id,magasin_id)/getQuantite(c,dateDebut,dateFin,article_id,magasin_id);
            c.Close();
            return valiny;
        }
        public string getTypeSortie(ConnectionPostgres c, string article_id)
        {
            string typeSortie = "";

            try
            {
                c.Open();

                string query = "SELECT articles.type_sortie_id " +
                            "FROM validation " +
                            "JOIN articles ON articles.article_id = validation.article_id " +
                            "WHERE validation.article_id = '" + article_id + "';"; // Méfiez-vous des injections SQL, préférez les paramètres

                NpgsqlDataReader reader = c.Execute(query);

                if (reader.Read())
                {
                    if (reader["type_sortie_id"] != DBNull.Value)
                    {
                        typeSortie = reader["type_sortie_id"].ToString();
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
            finally
            {
                c.Close();
            }

            return typeSortie;
        }
        public int GestionErreur(ConnectionPostgres c, DateTime dateDebut, DateTime dateFin, string article_id,string magasin_id){
            double sum_qt_stock = this.getSommeQuantiteStocker(c,dateDebut,dateFin,article_id,magasin_id);
            double sum_qt_Dstock = this.getSommeQuantiteDestocker(c,dateDebut,dateFin,article_id,magasin_id);
            if(sum_qt_stock < sum_qt_Dstock){
                return 0;
            }
            else if(this.getQuantite(c,dateDebut,dateFin,article_id,magasin_id) == 0){
                return 11;
            }
            else if(this.getQuantite(c,dateDebut,dateFin,article_id,magasin_id) < 0){
                return 22;
            }
            c.Close();
            return 200;
        }
        public string getMessage(ConnectionPostgres c, DateTime dateDebut, DateTime dateFin, string article_id,string magasin_id){
            if(GestionErreur(c,dateDebut,dateFin,article_id,magasin_id) == 0){
                return "Stock Insuffisant";
            }
            else if(GestionErreur(c,dateDebut,dateFin,article_id,magasin_id) == 11){
                return "Vous n'avez pas de stock sur ces dates";
            }
            else if(GestionErreur(c,dateDebut,dateFin,article_id,magasin_id) == 22){
                return "Veuillez Rajouter des Stocks";
            }
            c.Close();
            return "";
        }
        public string exceptionDestockage(){
            return "Desole, Votre Stock n'est pas suffisant";
        }
        public void insertMouvement(string mouvement_id, DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id){
            ConnectionPostgres c = new();
            Mouvements mouvement = new(mouvement_id,daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id,etat);
            if(mouvement.getSommeQuantiteSortie(c,article_id,magasin_id) + quantite > mouvement.getSommeQuantiteEntre(c,article_id,magasin_id)){
                mouvement.exceptionDestockage();
            }
                mouvement.Insert(c);
            c.Close();
        }
        
        public string gestionErreurDestockage()
        {
           return "Vous ne pouvez pas faire de destockage a cette date";
        }
        public string prixException()
        {
           return "Requette non acceptable";
        }
        public string MessageExceptionDestockage()
        {
           return "la Date du mouvement n'est plus valide";
        }
    
        public void UpdateMouvementbyId(ConnectionPostgres c,string mouvement_id, DateTime daty, string article_id, double quantite, double prix_unitaire, string magasin_id,string type_mouvement_id){
            c.Open();
            string updateQuery = "UPDATE mouvements SET daty = @daty, quantite = @quantite, article_id = @article_id, prix_unitaire = @prix_unitaire, type_mouvement_id = @type_mouvement_id,magasin_id = @magasin_id WHERE mouvement_id = @mouvement_id";
            using var command = new NpgsqlCommand(updateQuery, c.connection);
            command.Parameters.AddWithValue("@daty", daty);
            command.Parameters.AddWithValue("@quantite", quantite);
            command.Parameters.AddWithValue("@article_id", article_id);
            command.Parameters.AddWithValue("@prix_unitaire", prix_unitaire);
            command.Parameters.AddWithValue("@type_mouvement_id", type_mouvement_id);
            command.Parameters.AddWithValue("@magasin_id", magasin_id);
            command.Parameters.AddWithValue("@mouvement_id", mouvement_id);
            c.Open();
            int rowsAffected = command.ExecuteNonQuery();
            c.Close(); 
        }
        
        public void DeleteMouvementbyId(ConnectionPostgres c,string mouvement_id){
            c.Open();
            string updateQuery = "DELETE FROM mouvements WHERE mouvement_id = @mouvement_id";

            using var command = new NpgsqlCommand(updateQuery, c.connection);
            command.Parameters.AddWithValue("@mouvement_id", mouvement_id);
            c.Open();
            int rowsAffected = command.ExecuteNonQuery();
            c.Close(); 
        }
        public int ExceptionDestockage(ConnectionPostgres c,DateTime daty,string magasin_id,string article_id){
            int erreur = 0;
            Validation validation = new();
            for(int i=0; i<validation.SelectAllValidation(c).Count; i++){
                if(validation.getDateFinalDestockage(c,magasin_id,article_id) > daty){
                    erreur = 405;
                }
            }
            return erreur;
        }

    }

}