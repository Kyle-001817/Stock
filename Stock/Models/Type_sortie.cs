using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Type_sortie
    {
        public string type_sortie_id {get; set;}
        public string nom {get; set;}

        public Type_sortie(){}
        public Type_sortie(string type_sortie_id, string nom)
        {
            this.type_sortie_id = type_sortie_id;
            this.nom = nom;
        }

        public List<Type_sortie> SelectAllType_Sortie(ConnectionPostgres c)
        {
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from type_sortie");
            List<Type_sortie> list = new();
            while (datareader.Read())
            {
                string type_sortie_id = (string)datareader.GetString(0);
                string nom = (string)datareader.GetString(1);
                list.Add(new Type_sortie(type_sortie_id, nom));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}