using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Generic;
using System.Data.Common;
using Dapper;
using System.Data.SqlClient;
using Z.Dapper.Plus;
using ZonaFl.Business.SubSystems;
using System.Globalization;

namespace ZonaFl.Persistence.Repository
{
    public class OfferRepository<T> : RepositoryGeneric<Offer>
    {

        public virtual IEnumerable<Entities.Offer> GetListPaged<Offer, Category>(int pagenumber, int itemsperpage, string conditions, string order, int statusproject=0)
        {
            SqlConnection _connection;
            IEnumerable<Entities.Offer> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);

                string sql="";
                if (statusproject == 0 )
                {
                   sql = "SELECT O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer," +
        "C.Id, C.Name, C.NameLarge " +
        "FROM Offer O INNER JOIN Category C ON C.Id = O.CategoryId  " + conditions;
                }
                else if(statusproject>=1 && statusproject < 3)
                {
                    sql = "select O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer,C.Id, C.Name, C.NameLarge,project.Comments, project.Qualification from offer O inner join project on O.id = project.idoffer  INNER JOIN Category C ON C.Id = O.CategoryId " + conditions;
                }
                else if(statusproject==3)
                {
                    sql = "select O.Id,O.Status,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer,C.Id, C.Name, C.NameLarge,project.Comments, project.Qualification from offer O left join project on O.id = project.idoffer  INNER JOIN Category C ON C.Id = O.CategoryId " + conditions;
                }
                result = _connection.Query<Entities.Offer, Entities.Category, Entities.Offer>(
                sql,
                (offer, category) =>
                {
                    offer.Category = category;
                    return offer;
                }
                ).AsEnumerable();
               


                return result;
            }






        }


        public virtual IEnumerable<Entities.Offer> GetListPaged<Offer, Category>(int pagenumber, int itemsperpage, string conditions, string order)
        {
            SqlConnection _connection;
            IEnumerable<Entities.Offer> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);

                string sql = "";
             
                    sql = "SELECT O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer," +
         "C.Id, C.Name, C.NameLarge " +
         "FROM Offer O INNER JOIN Category C ON C.Id = O.CategoryId INNER JOIN OfferPhases OP ON OP.IdOffer=O.id  " + conditions;
               
                result = _connection.Query<Entities.Offer, Entities.Category, Entities.Offer>(
                sql,
                (offer, category) =>
                {
                    offer.Category = category;
                    return offer;
                }
                ).AsEnumerable();
                return result;
            }






        }

        public virtual Entities.Offer GetOffer<Offer, Category>(int idoffer)
        {
            SqlConnection _connection;
         Entities.Offer result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "SELECT O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer," +
    "C.Id, C.Name, C.NameLarge " +
    "FROM Offer O INNER JOIN Category C ON C.Id = O.CategoryId INNER JOIN OfferPhases OP ON OP.IdOffer=O.id  where O.Id= " + idoffer;

                result = _connection.Query<Entities.Offer, Entities.Category, Entities.Offer>(
                sql,
                (offer, category) =>
                {
                    offer.Category = category;
                    return offer;
                }
                ).FirstOrDefault();
                return result;
            }






        }

       

        public OfferPhases GetPhaseFinal(int idoffer)
        {
            SqlConnection _connection;
            OfferPhasRepository<OfferPhases> OfferPhas = new OfferPhasRepository<OfferPhases>();
            Entities.OfferPhases result;

            using (_connection = Utilities.GetOpenConnection())
            {
                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM OfferPhases WHERE IdOffer=@IdOffer";
                result = _connection.Query<OfferPhases>(sqlQuery, new { idoffer }).LastOrDefault();
                _connection.Close();

            }
            return result;
        }

        public virtual Entities.OfferUser GetOfferUser<OfferUser>(int idofferUser)
        {

            SqlConnection _connection;
            Entities.OfferUser result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);
                var sql = "";


                sql = "Select *  from OfferUser where id=" + idofferUser;


                result = _connection.Query<Entities.OfferUser>(sql).FirstOrDefault();


                return result;
            }
        }

        public virtual Entities.Offer GetOfferUser<Offer, OfferUser>(int idoffer)
        {
            SqlConnection _connection;
            Entities.Offer result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);
                var sql = "";

                if (idoffer != -1)
                {
                    sql = "Select Offer.Id, Offer.PrecioFijo, Offer.Sale, Offer.ValueFixedProject, Offer.NumberPhases, Offer.AutomaticValuePhases, Offer.IdUser, Offer.CategoryId, Offer.TitleOffer, Offer.[Description], Offer.RangoValor, Offer.DateOffer" +
                       ",OfferUser.Id, OfferUser.IdOffer, OfferUser.IdUser " +
                       " FROM Offer inner join OfferUser  on Offer.Id=OfferUser.IdOffer inner join AspnetUsers on AspnetUsers.id=OfferUser.IdUser   where  Offer.Id=" + idoffer +  " and Offer.Id NOT IN(SELECT Project.idoffer  from project)";
                }
                else
                {
                    sql = "Select Offer.Id, Offer.PrecioFijo, Offer.Sale, Offer.ValueFixedProject, Offer.NumberPhases, Offer.AutomaticValuePhases, Offer.IdUser, Offer.CategoryId, Offer.TitleOffer, Offer.[Description], Offer.RangoValor, Offer.DateOffer" +
                                          ",OfferUser.Id, OfferUser.IdOffer, OfferUser.IdUser " +
                                          " FROM Offer inner join OfferUser  on Offer.Id=OfferUser.IdOffer inner join AspnetUsers on AspnetUsers.id=OfferUser.IdUser where Offer.Id NOT IN(SELECT Project.idoffer  from project)";
                }

                result = _connection.Query<Entities.Offer, List<Entities.OfferUser>, Entities.Offer>(
                sql,
                (offer, offerusers) =>
                {
                    offer.OfferUsers = offerusers;
                    return offer;
                }
                ).FirstOrDefault();
              

                // result = _connection.Query<Entities.Offer, Entities.OfferUser, Entities.Offer>(
                //sql,
                //(offer, offerusers) =>
                //{
                //    offer.OfferUser = offerusers;
                //    return offer;
                //}
                //).FirstOrDefault();


                OfferUserRepository<OfferUser> offerUrepo = new OfferUserRepository<OfferUser>();
              var offerusersre=  offerUrepo.GetList(new { IdOffer = idoffer });
                if(offerusersre.Count()>0 && result!=null)
                result.OfferUsers = offerusersre.ToList();
                return result;
            }






        }
        public virtual IEnumerable<Entities.Offer> GetAppliedOfferByUserListPaged<Offer, Category>(int pagenumber, int itemsperpage, string conditions, string order)
        {
            SqlConnection _connection;
            IEnumerable<Entities.Offer> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "SELECT O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer," +
    "C.Id, C.Name, C.NameLarge " +
    "FROM Offer O INNER JOIN Category C ON C.Id = O.CategoryId INNER JOIN OfferPhases OP ON OP.IdOffer=O.id INNER JOIN  OfferUser OU ON OU.IdOffer=O.Id   " + conditions;

                result = _connection.Query<Entities.Offer, Entities.Category, Entities.Offer>(
                sql,
                (offer, category) =>
                {
                    offer.Category = category;
                    return offer;
                }
                ).AsEnumerable();
                return result;
            }






        }

        public Offer GetById(int IdOffer)
        {
            SqlConnection _connection;
            Entities.Offer result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "SELECT O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer," +
    "C.Id, C.Name, C.NameLarge,OFP.Id,OFP.IdOffer,OFP.Name,OFP.InitPhase,OFP.FinishPhase,OFP.Value,OFP.StatusPhase " +
    "FROM Offer O INNER JOIN Category C  ON C.Id = O.CategoryId INNER JOIN offerPhases OFP ON OFP.IdOffer=O.Id  where O.Id=" + IdOffer;

                result = _connection.Query<Entities.Offer, Entities.Category,List<Entities.OfferPhases>, Entities.Offer>(
                sql,
                (offer, category, offerphases) =>
                {
                    offer.OfferPhases = offerphases;
                    offer.Category = category;
                    return offer;
                }
                ).FirstOrDefault();


                sql = "select [OfferPhases].Id, [OfferPhases].IdOffer, Name, InitPhase, FinishPhase, cast(Value as integer) AS Value,CONVERT(varchar(20), Value, 0) AS Value2, StatusPhase, SUBSTRING(master.dbo.fn_varbintohexstr(HashBytes('MD5', '85moF22S2ZGBU3NsM5054aqXK1~568308~'+CONVERT(varchar(20), [OfferPhases].Id, 0)+'~'+CONVERT(varchar(20), Value, 0)+'~COP')), 3, 64) AS Signature,aspnetusers.Email as buyerEmail from [OfferPhases] inner join offer on [OfferPhases].idoffer=offer.id inner join aspnetusers on offer.iduser=aspnetusers.id WHERE OfferPhases.IDOFFER=" + IdOffer;
                List<OfferPhases> result2 = new List<OfferPhases>();
                result2 = _connection.Query<OfferPhases>(sql).ToList();
                if (result2.Count > 0)
                {
                    var pagos = new ZonaFl.Entities.CustomEntities.Pagos();
                   
                    result.OfferPhases = result2;
                    result.OfferPhases.ToList().ForEach(e => e.pagos = pagos);
                    result.OfferPhases.ToList().ForEach(e => e.pagos.referenceCode = result.OfferPhases.Where(t => t.IdOffer == e.IdOffer).First().Id.ToString());
                    result.OfferPhases.ToList().ForEach(e => e.pagos.amount =result.OfferPhases.Where(t => t.IdOffer == e.IdOffer).First().Value2);
                    result.OfferPhases.ToList().ForEach(e => e.pagos.buyerEmail = result2.Where(t => t.IdOffer == e.IdOffer).First().buyerEmail);
                 result.OfferPhases.ToList().ForEach(e => e.pagos.description = "Pago por:"+result.OfferPhases.Where(t => t.IdOffer == e.IdOffer).First().Name+ " del proyecto:"+ result.Description);
                }
                
                return result;
            }

        }

        public int InsertPhases(List<OfferPhases> listOfferPhases)
        {
            SqlConnection _connection;
            using (_connection = Utilities.GetOpenConnection())
            {
                string processQuery = "INSERT INTO OFFERPHASES VALUES (@IdOffer, @Name,@InitPhase,@FinishPhase,@Value,@StatusPhase)";
                return _connection.Execute(processQuery, listOfferPhases);
            }
        }

        public int DeletePhases(int IdOffer)
        {
            SqlConnection _connection;
            OfferPhasRepository<OfferPhases> OfferPhas = new OfferPhasRepository<OfferPhases>();
            Entities.Offer result;
            int rta = 0;
            using (_connection = Utilities.GetOpenConnection())
            {
                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "DELETE FROM OfferPhases WHERE IdOffer=@IdOffer";
                rta=_connection.Execute(sqlQuery, new { IdOffer });
                _connection.Close();

            }
            return rta;

         
        }
        public OfferPhases GetPhaseInitial(int IdOffer)
        {
            SqlConnection _connection;
            OfferPhasRepository<OfferPhases> OfferPhas = new OfferPhasRepository<OfferPhases>();
            Entities.OfferPhases result;
           
            using (_connection = Utilities.GetOpenConnection())
            {
                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT * FROM OfferPhases WHERE IdOffer=@IdOffer";
                result = _connection.Query<OfferPhases>(sqlQuery, new { IdOffer }).FirstOrDefault();
                _connection.Close();

            }
            return result;
        }

        public int UpdatePhases(List<OfferPhases> listOfferPhases, int IdOffer)
        {
            SqlConnection _connection;
            OfferPhasRepository<OfferPhases> ofre = new OfferPhasRepository<OfferPhases>();
            using (_connection = Utilities.GetOpenConnection())
            {
               
               
                foreach(OfferPhases fase in listOfferPhases)
                {
                    ofre.Update(fase);
                   
                }

                return 1;
            }
        }
        public OfferUser GetOfferUser(int idoffer, string idUser)
        {
            OfferUserRepository<OfferUser> offeruserrepo = new OfferUserRepository<OfferUser>();
            OfferUser offeruser= offeruserrepo.GetOfferUser(idoffer, idUser);
            return offeruser;
        }

        public List<Offer> GetOffertsByContractorEmail(string emailcontratante)
        {
            SqlConnection _connection;

            List<Persistence.Entities.Offer> result;
            using (_connection = Utilities.GetOpenConnection())
            {

                //string processQuery = "DELETE FROM OFFERPHASES WHERE IDOFFER IN )";
                string sqlQuery = "SELECT Offer.Id, Offer.IdUser FROM Offer inner join AspNetUsers on  AspNetUsers.id=Offer.IdUser WHERE  AspNetUsers.Email=@emailcontratante";
                result = _connection.Query<Offer>(sqlQuery, new { emailcontratante }).ToList();
                _connection.Close();

            }
            return result;
        }

        //private static Offer MapUser(dynamic result)
        //{
        //    Offer item = new Offer();
        //    item.Id = result.Id;
        //    item.UserName = result.UserName;
        //    item.City = result.City;
        //    item.Country = result.Country;
        //    item.DescUser = result.DescUser;
        //    item.Email = result.Email;
        //    item.Empresa = result.Empresa;
        //    item.Freelance = result.Freelance;
        //    item.PhoneNumber = result.PhoneNumber;
        //    item.Skills = result.Skills;
        //    item.Image = result.Image;
        //    item.FirstMiddleName = result.FirstMiddleName;
        //    item.Status = result.Status;
        //    item.Rol = result.Rol;
        //    item.TipoUser = result.TipoUser;
        //    item.Accion = result.Accion;
        //    item.Estilo = result.Estilo;
        //    item.Url = result.Url;
        //    item.SecurityStamp = result.SecurityStamp;
        //    /* The custom mapping */


        //    return item;
        //}


    }


    public class OfferPhasRepository<T> : RepositoryGeneric<OfferPhases>
    {
    }
}
