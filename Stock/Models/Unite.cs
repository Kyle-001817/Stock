using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Unite
    {
        public Unite()
        {
        }

        public Unite(string id_unite, string nom_unite)
        {
            this.id_unite = id_unite;
            this.nom_unite = nom_unite;
        }

        public string id_unite {get; set;}
        public string nom_unite {get; set;}

        public List<Unite> SelectAllUnite(ConnectionPostgres c){
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from unite");
            List<Unite> list = new();
            while(datareader.Read()){
                string id_unite = (string)datareader.GetString(0);
                string nom_unite = (string)datareader.GetString(1);
                list.Add(new Unite(id_unite,nom_unite));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}