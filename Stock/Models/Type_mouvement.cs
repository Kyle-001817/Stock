using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Stock
{
    public class Type_mouvement
    {
        public Type_mouvement()
        {
        }

        public Type_mouvement(string type_mouvement_id, string nom)
        {
            this.type_mouvement_id = type_mouvement_id;
            this.nom = nom;
        }

        public string type_mouvement_id {get; set;}
        public string nom {get; set;}

        public List<Type_mouvement> SelectAllTypeMouvement(ConnectionPostgres c){
            c.Open();
            NpgsqlDataReader datareader = c.Execute("select *from type_mouvement");
            List<Type_mouvement> list = new();
            while(datareader.Read()){
                string type_mouvement_id = (string)datareader.GetString(0);
                string nom = (string)datareader.GetString(1);
                list.Add(new Type_mouvement(type_mouvement_id,nom));
            }
            datareader.Close();
            c.Close();
            return list;
        }
    }
}