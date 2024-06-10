using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Validation
    {
        public Validation()
        {
        }

        public Validation(string validation_id, DateTime daty, double quantite, string article_id, double prix_unitaire, string type_mouvement_id, string magasin_id, int etat)
        {
            this.validation_id = validation_id;
            this.daty = daty;
            this.quantite = quantite;
            this.article_id = article_id;
            this.prix_unitaire = prix_unitaire;
            this.type_mouvement_id = type_mouvement_id;
            this.magasin_id = magasin_id;
            this.etat = etat;
        }

        public string validation_id { get; set; }
        public DateTime daty { get; set; }
        public double quantite { get; set; }
        public string article_id { get; set; }
        public double prix_unitaire { get; set; }
        public string type_mouvement_id { get; set; }
        public string magasin_id { get; set; }
        public int etat { get; set; }

        public List<Validation> SelectAllValidation(ConnectionPostgres c)
        {
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from validation");
            List<Validation> list = new();
            while (datareader.Read())
            {
                string validation_id = (string)datareader.GetString(0);
                DateTime daty = (DateTime)datareader.GetDateTime(1);
                double quantite = (double)datareader.GetDouble(2);
                string article_id = (string)datareader.GetString(3);
                double prix_unitaire = (double)datareader.GetDouble(4);
                string type_mouvement_id = (string)datareader.GetString(5);
                string magasin_id = (string)datareader.GetString(6);
                int etat = (int)datareader.GetInt32(7);
                list.Add(new Validation(validation_id, daty.Date, quantite, article_id, prix_unitaire, type_mouvement_id, magasin_id,etat));
            }
            datareader.Close();
            c.Close();
            return list;
        }
        public void Insert(ConnectionPostgres co)
        {
            co.Open();
            string query = "insert into validation(daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id) values ('" + this.daty + "'," + this.quantite + ",'"+this.article_id + "',"+this.prix_unitaire+",'"+this.type_mouvement_id+"','"+this.magasin_id+"')";
            co.ExecuteInsert(query);
            co.Close();
        }
        public List<Validation> InsertValidation(ConnectionPostgres c){
            Mouvements mouvement = new();
            c.Open();
            List<Mouvements> mouvements = mouvement.SelectAllMouvementbyEtat(c,0);
            List<Validation> list_validation = new();
            for(int i = 0; i<mouvements.Count; i++){
                daty = mouvements[i].daty;
                quantite = mouvements[i].quantite;
                article_id = mouvements[i].article_id;
                prix_unitaire = mouvements[i].prix_unitaire;
                type_mouvement_id = mouvements[i].type_mouvement_id;
                magasin_id = mouvements[i].magasin_id;
                Validation valid = new(validation_id,daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id,etat);
                list_validation.Add(valid);
                valid.Insert(c);
            }
            c.Close();
            return list_validation;
        }
        public void DeleteMouvement(ConnectionPostgres c){
            c.Open();
            string updateQuery = "DELETE from mouvements where etat = 0";
            using var command = new NpgsqlCommand(updateQuery, c.connection);
            c.Open();
            int rowsAffected = command.ExecuteNonQuery();
            c.Close(); 
        }
        public List<Validation> CalculerQuantiteDestockerSelonType(ConnectionPostgres c, DateTime dateDebut, DateTime dateFin, string article_id, string magasin_id, string type_article)
        {
            Type_sortie ts = new();
            Mouvements m = new();
            List<Type_sortie> types_sortie = ts.SelectAllType_Sortie(c);
            List<Validation> mouvementsFIFO = m.getListMouvementStockagebyDateFIFO(c, dateDebut, dateFin);
            List<Validation> mouvementsLIFO = m.getListMouvementStockagebyDateLIFO(c, dateDebut, dateFin);
            List<Validation> resultats = new();
            double quantiteRestante = m.getSommeQuantiteDestocker(c, dateDebut, dateFin, article_id, magasin_id);
            double prixRestante = m.getSommePUDestocker(c, dateDebut, dateFin, article_id, magasin_id);
            if(types_sortie[0].type_sortie_id.Equals(type_article))
            {
                foreach (var mouvementFIFO in mouvementsFIFO)
                {
                    if (quantiteRestante == 0 && prixRestante == 0)
                        break;

                    double quantiteSoustraite = Math.Min(quantiteRestante, mouvementFIFO.quantite);
                    double prixSoustraite = Math.Min(prixRestante, mouvementFIFO.prix_unitaire);
                    Validation resultatPartiel = new()
                    {
                        daty = mouvementFIFO.daty,
                        article_id = mouvementFIFO.article_id,
                        magasin_id = mouvementFIFO.magasin_id,
                        quantite = quantiteSoustraite,
                        prix_unitaire = prixSoustraite
                    };

                    resultats.Add(resultatPartiel);
                    quantiteRestante -= quantiteSoustraite;
                    prixRestante -= prixSoustraite;
                }
            }
            else if(types_sortie[1].type_sortie_id.Equals(type_article))
            {
                foreach (var mouvementLIFO in mouvementsLIFO)
                {
                    if (quantiteRestante == 0 && prixRestante == 0)
                        break;

                    double quantiteSoustraite = Math.Min(quantiteRestante, mouvementLIFO.quantite);
                    double prixSoustraite = Math.Min(prixRestante, mouvementLIFO.prix_unitaire);
                    Validation resultatPartiel = new()
                    {
                        daty = mouvementLIFO.daty,
                        article_id = mouvementLIFO.article_id,
                        magasin_id = mouvementLIFO.magasin_id,
                        quantite = quantiteSoustraite,
                        prix_unitaire = prixSoustraite
                    };

                    resultats.Add(resultatPartiel);
                    quantiteRestante -= quantiteSoustraite;
                    prixRestante -= prixSoustraite;
                }
            }
            c.Close();
            return resultats;
        }
        public DateTime getDateFinalDestockage(ConnectionPostgres c, string magasin_id, string article_id)
        {
            
            DateTime dateFinale = DateTime.MinValue;
            string type_mouvement_id = "TM2";

            try
            {
                List<Validation> validation = SelectAllValidation(c);
                List<Validation> filteredMovements = validation.Where(m =>
                    m.magasin_id == magasin_id &&
                    m.article_id == article_id &&
                    m.type_mouvement_id == type_mouvement_id
                ).OrderByDescending(m => m.daty).ToList();

                if (filteredMovements.Count > 0)
                {
                    dateFinale = filteredMovements[0].daty;
                }

                c.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
            finally
            {
                c.Close();
            }

            return dateFinale;
        }
        // public Validation getDernierDestockagebyDate(ConnectionPostgres c, string magasin_id, string article_id){
        //     Mouvements mouvement = new();
        //     Validation valiny = new();
        //     DateTime daty = getDateFinalDestockage(c,magasin_id,article_id);
        //     Console.WriteLine(daty);
        //     for(int i=0; i< SelectAllValidation(c).Count; i++){
        //         if(SelectAllValidation(c)[i].daty == daty){
        //             valiny = new(validation_id,daty,quantite,article_id,prix_unitaire,type_mouvement_id,magasin_id,etat);
        //         }
        //     }
        //     return valiny;
        // }
    }
}