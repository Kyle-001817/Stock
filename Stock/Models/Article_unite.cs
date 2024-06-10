using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Article_unite
    {
        public string id_unite_article;
        public string id_articles;
        public string id_unite;
         public double quantite;

        public Article_unite()
        {
        }

        public Article_unite(string id_unite_article, string id_articles, string id_unite, double quantite)
        {
            this.id_unite_article = id_unite_article;
            this.id_articles = id_articles;
            this.id_unite = id_unite;
            this.quantite = quantite;
        }

        public List<Article_unite> SelectAllArticle_unite(ConnectionPostgres c){
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from unite_article");
            List<Article_unite> list = new();
            while(datareader.Read()){
                string id_unite_article = (string)datareader.GetString(0);
                string id_articles = (string)datareader.GetString(1);
                string id_unite = (string)datareader.GetString(2);
                double quantite = (double)datareader.GetDouble(3);
                list.Add(new Article_unite(id_unite_article,id_articles,id_unite,quantite));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}