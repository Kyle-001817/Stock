using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Articles
    {
        public Articles()
        {
        }

        public Articles(string article_id, string nom, string type_sortie_id, string id_unite)
        {
            this.article_id = article_id;
            this.nom = nom;
            this.type_sortie_id = type_sortie_id;
            this.id_unite = id_unite;
        }

        public string article_id {get; set;}
        public string nom {get; set;}
        public string type_sortie_id {get; set;}
        public string id_unite {get; set;}

        public List<Articles> SelectAllArticles(ConnectionPostgres c){
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from articles");
            List<Articles> list = new();
            while(datareader.Read()){
                string article_id = (string)datareader.GetString(0);
                string nom = (string)datareader.GetString(1);
                string type_sortie_id = (string)datareader.GetString(2);
                string id_unite = (string)datareader.GetString(3);
                list.Add(new Articles(article_id,nom, type_sortie_id,id_unite));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}