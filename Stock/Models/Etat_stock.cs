using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Etat_stock
    {
#pragma warning disable CS8618
        public Etat_stock(){

        }
#pragma warning restore CS8618
        public Etat_stock(string etat_stock_id, DateTime date_debut, DateTime date_fin, string article_id, double quantite_restante, double prix_unitaire, string magasin_id)
        {
            this.etat_stock_id = etat_stock_id;
            this.date_debut = date_debut;
            this.date_fin = date_fin;
            this.article_id = article_id;
            this.quantite_restante = quantite_restante;
            this.prix_unitaire = prix_unitaire;
            this.magasin_id = magasin_id;
        }

        public string etat_stock_id {get; set;}
        public DateTime date_debut {get;set;}
        public DateTime date_fin {get;set;}
        public string article_id {get; set;}
        public double quantite_restante{get; set;}
        public double prix_unitaire{get; set;}
        public string magasin_id{get; set;}

        public List<Etat_stock> SelectAllEtatStock(ConnectionPostgres c)
        {
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from etat_stock");
            List<Etat_stock> list = new();
            while (datareader.Read())
            {
                string etat_stock_id = (string)datareader.GetString(0);
                DateTime date_debut = (DateTime)datareader.GetDateTime(1);
                DateTime date_fin = (DateTime)datareader.GetDateTime(2);
                string article_id = (string)datareader.GetString(3);
                double quantite_restante = (double)datareader.GetDouble(4);
                double prix_unitaire = (double)datareader.GetDouble(5);
                string magasin_id = (string)datareader.GetString(6);
                list.Add(new Etat_stock(etat_stock_id, date_debut, date_fin, article_id, quantite_restante, prix_unitaire, magasin_id));
            }
            datareader.Close();
            c.Close();
            return list;
        }
        public void Insert(ConnectionPostgres co)
        {
            co.Open();
            string query = "insert into etat_stock(date_debut,date_fin,article_id,quantite_restante,prix_unitaire,magasin_id) values ('" + this.date_debut + "','" + this.date_fin + "','"+this.article_id + "',"+this.quantite_restante+","+this.prix_unitaire+",'"+this.magasin_id+"')";
            co.ExecuteInsert(query);
            co.Close();
        }

        public List<Etat_stock> Etats(DateTime date_debut, DateTime date_fin,string article_id,string magasin_id)
        {
            
            ConnectionPostgres c = new();
            Mouvements mouvement = new();
            List<Etat_stock> Etats = new();
            if(mouvement.GestionErreur(c,date_debut,date_fin,article_id,magasin_id) == 200)
            {
                quantite_restante = mouvement.getQuantite(c,date_debut,date_fin,article_id,magasin_id);
                prix_unitaire = mouvement.getPrixUnitaire(c,date_debut,date_fin,article_id,magasin_id);
                Etat_stock etat_stock = new(etat_stock_id,date_debut,date_fin,article_id,quantite_restante,prix_unitaire,magasin_id);
                Etats.Add(etat_stock);
            }
            else{
                mouvement.getMessage(c,date_debut,date_fin,article_id,magasin_id);
            }

            c.Close();
            return Etats;
        }
    }
}