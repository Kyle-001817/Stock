using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Magasins
    {
        public Magasins()
        {
        }

        public Magasins(string magasin_id, string nom)
        {
            this.magasin_id = magasin_id;
            this.nom = nom;
        }

        public string magasin_id {get; set;}
        public string nom {get; set;}

        public List<Magasins> SelectAllMagasin(ConnectionPostgres c){
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from magasins");
            List<Magasins> list = new();
            while(datareader.Read()){
                string magasin_id = (string)datareader.GetString(0);
                string nom = (string)datareader.GetString(1);
                list.Add(new Magasins(magasin_id,nom));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}