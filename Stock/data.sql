drop database stock;
CREATE DATABASE stock;
\c stock;

CREATE SEQUENCE move_seq;
CREATE SEQUENCE mgs_seq;
CREATE SEQUENCE estk_seq;


CREATE TABLE type_sortie(
    type_sortie_id VARCHAR(20) PRIMARY KEY,
    nom VARCHAR(20)
);
    
CREATE TABLE type_mouvement(
    type_mouvement_id VARCHAR(20) PRIMARY KEY,
    nom VARCHAR(20)
);

CREATE TABLE magasins(
    magasin_id VARCHAR(20) DEFAULT 'MGS' || nextval('mgs_seq') PRIMARY KEY,
    nom VARCHAR(20)
);
CREATE SEQUENCE unit_seq;
CREATE TABLE unite(
    id_unite VARCHAR(20) DEFAULT 'UNIT' || nextval('unit_seq') PRIMARY KEY,
    nom VARCHAR(20)
);

INSERT INTO unite(nom) VALUES('gony');
INSERT INTO unite(nom) VALUES('conteneur');
INSERT INTO unite(nom) VALUES('bouteille');
INSERT INTO unite(nom) VALUES('paque');


CREATE TABLE articles(
    article_id VARCHAR(20) PRIMARY KEY,
    nom VARCHAR(50),
    type_sortie_id VARCHAR(20),
    id_unite VARCHAR(20),
    FOREIGN KEY (type_sortie_id) REFERENCES type_sortie(type_sortie_id),
    FOREIGN KEY (id_unite) REFERENCES unite(id_unite)
);
CREATE TABLE unite_article(
    id_unite_article VARCHAR(20) PRIMARY KEY,
    article_id VARCHAR(20),
    id_unite VARCHAR(20),
    quantite FLOAT,
    FOREIGN KEY (article_id) REFERENCES articles(article_id),
    FOREIGN KEY (id_unite) REFERENCES unite(id_unite)
);


CREATE TABLE mouvements(
    mouvement_id VARCHAR(20) DEFAULT 'MOVE' || nextval('move_seq') PRIMARY KEY,
    daty TIMESTAMP,
    quantite FLOAT,
    article_id VARCHAR(20),
    prix_unitaire FLOAT,
    type_mouvement_id VARCHAR(20),
    magasin_id VARCHAR(20),
    etat int DEFAULT 0,
    FOREIGN KEY (article_id) REFERENCES articles(article_id),
    FOREIGN KEY (type_mouvement_id) REFERENCES type_mouvement(type_mouvement_id),
    FOREIGN KEY (magasin_id) REFERENCES magasins(magasin_id)
);
CREATE SEQUENCE valid_seq;

CREATE TABLE validation(
    validation_id VARCHAR(20) DEFAULT 'VALID' || nextval('valid_seq') PRIMARY KEY,
    daty TIMESTAMP,
    quantite FLOAT,
    article_id VARCHAR(20),
    prix_unitaire FLOAT,
    type_mouvement_id VARCHAR(20),
    magasin_id VARCHAR(20),
    etat int DEFAULT 10,
    FOREIGN KEY (article_id) REFERENCES articles(article_id),
    FOREIGN KEY (type_mouvement_id) REFERENCES type_mouvement(type_mouvement_id),
    FOREIGN KEY (magasin_id) REFERENCES magasins(magasin_id)
);

CREATE TABLE etat_stock(
    etat_stock_id VARCHAR(20) DEFAULT 'ESTK' || nextval('estk_seq') PRIMARY KEY,
    date_debut TIMESTAMP,
    date_fin TIMESTAMP,
    article_id VARCHAR(20),
    quantite_restante FLOAT,
    prix_unitaire FLOAT,
    magasin_id VARCHAR(20),
    FOREIGN KEY (article_id) REFERENCES articles(article_id),
    FOREIGN KEY (magasin_id) REFERENCES magasins(magasin_id)
);

INSERT INTO type_sortie(type_sortie_id,nom) VALUES('TS1','FIFO');
INSERT INTO type_sortie(type_sortie_id,nom) VALUES('TS2','LIFO');

INSERT INTO type_mouvement(type_mouvement_id,nom) VALUES('TM1','Stockage');
INSERT INTO type_mouvement(type_mouvement_id,nom) VALUES('TM2','Destockage');

INSERT INTO magasins(nom) VALUES('Magasin M');
INSERT INTO magasins(nom) VALUES('Magasin U');
INSERT INTO magasins(nom) VALUES('TRI-A');
INSERT INTO magasins(nom) VALUES('Soamanatombo');

INSERT INTO articles(article_id,nom,type_sortie_id,id_unite) VALUES('VAR001','Biscuit','TS1','UNIT1');
INSERT INTO articles(article_id,nom,type_sortie_id,id_unite) VALUES('VAR002','THB','TS2','UNIT2');
INSERT INTO articles(article_id,nom,type_sortie_id,id_unite) VALUES('VAR003','Fromage','TS2','UNIT3');

INSERT INTO unite_article(id_unite_article,article_id,id_unite,quantite) VALUES('UA1','VAR001','UNIT1',50);
INSERT INTO unite_article(id_unite_article,article_id,id_unite,quantite) VALUES('UA2','VAR002','UNIT2',8);
INSERT INTO unite_article(id_unite_article,article_id,id_unite,quantite) VALUES('UA3','VAR003','UNIT3',10);

SELECT *FROM mouvements where lower(concat(article_id)) like lower('VAR%') and lower(concat(type_mouvement_id)) like lower('TM1%') and lower(concat(magasin_id)) like lower('MGS1%');

CREATE or REPLACE VIEW v_mouvements as select mouvements.*,articles.nom as nom_article,type_mouvement.nom as nom_type_mouvement,magasins.nom as nom_magasin from mouvements join articles on mouvements.article_id = articles.article_id join type_mouvement on type_mouvement.type_mouvement_id = mouvements.type_mouvement_id join magasins on mouvements.magasin_id = magasins.magasin_id;