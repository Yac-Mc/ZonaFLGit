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

namespace ZonaFl.Persistence.Repository
{
    public class ProjectRepository<T>: RepositoryGeneric<Project> 
    {

        public virtual IEnumerable<Entities.Project> GetListPaged<Project, Category>(int pagenumber, int itemsperpage, string conditions, string order)
        {
            SqlConnection _connection;
            IEnumerable<Entities.Project> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "SELECT P.Id, P.IdSkill, P.IdOffer,P.Comments,P.[Status],P.Qualification,P.Postulantes," +
                   "O.Id,O.PrecioFijo,O.Sale,O.ValueFixedProject,O.NumberPhases,O.AutomaticValuePhases,O.IdUser,O.CategoryId,O.TitleOffer,O.[Description],O.RangoValor,O.DateOffer "+
        " FROM Project P INNER JOIN Offer O ON O.Id = P.IdOffer INNER JOIN OfferUser OU ON OU.IdOffer=O.id  and OU.IdUser=O.IdUser " + conditions;
                CategoryRepository cater = new CategoryRepository();
                result = _connection.Query<Entities.Project, Entities.Offer, Entities.Project>(
                sql,
                (project, offer) =>
                {
                    project.Offer = offer;
                    project.Offer.Category = cater.FindCategoryById(offer.CategoryId);
                    return project;
                }
                ).AsEnumerable();
                
                return result;
            }






        }

        public List<Project> GetProjectsEndedByUser(string idUser)
        {
            SqlConnection _connection;
            List<Project> result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "select PROJECT.Id, PROJECT.IdCategory, PROJECT.IdOffer, PROJECT.Comments, PROJECT.Qualification, PROJECT.Status, PROJECT.Postulantes" +
                   " FROM PROJECT inner join OfferUser on Project.Idoffer=OfferUser.IdOffer WHERE OfferUser.IdUser='" + idUser+ "' and PROJECT.Status=3";
                CategoryRepository cater = new CategoryRepository();
                result = _connection.Query<Project>(
                sql
                ).ToList();

                return result;
            }
        }

        public int GetNoPostuladosByOffer(int idOffer)
        {
            SqlConnection _connection;
            int result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "SELECT COUNT(ID) " +
                   " FROM OFFERUSER WHERE OFFERUSER.IDOFFER="+idOffer;
                CategoryRepository cater = new CategoryRepository();
                result = _connection.ExecuteScalar<Int32>(
                sql
                );

                return result;
            }
        }

        public Project GetByOffer(int idoffer)
        {
            SqlConnection _connection;
            Project result;


            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);



                var sql = "select Id, IdCategory, IdOffer, Comments, Qualification, Status, Postulantes" +
                   " FROM PROJECT WHERE IDOFFER="+ idoffer;
                CategoryRepository cater = new CategoryRepository();
                result = _connection.Query<Project>(
                sql
                ).FirstOrDefault();

                return result;
            }
        }

        public int? Insert(Project project, string IdUser)
        {

            int? IdProject = this.Insert(project);
            if (IdProject != null)
            {
                int idp = (int)IdProject;
                SqlConnection _connection;
                using (_connection = Utilities.GetOpenConnection())
                {
                    string processQuery = "INSERT INTO projectuser VALUES (@IdProject, @IdUser)";
                    return _connection.Execute(processQuery,new { IdProject, IdUser });
                }
                return IdProject;
            }
            else
            {
                return null;
            }
        }

        public bool GetStatusEndedProjectByOffer(int idoffer)
        {
            Project project=  this.GetByOffer(idoffer);
            
           if(project.Status== StatusProject.Finalizada)
            {
                return true;
            }
           else
            {
                return false;
            }
           
        }

        public bool IsProjectForFinally(int idoffer)
        {
            SqlConnection _connection;
           List< OfferPhases> result;

            int cantfases = 0;
            int cantfasesencurso = 0;
            using (_connection = Utilities.GetOpenConnection())
            {
                //result = _connection.GetListPaged<TEntity>(pagenumber, itemsperpage, conditions, order);

                var sql = "select project.Id, project.IdOffer, Name, InitPhase, FinishPhase, Value, StatusPhase from offerPhases inner join project on offerPhases.IdOffer = project.IdOffer where project.IdOffer = " + idoffer;

              
               
                result = _connection.Query<OfferPhases>(
                sql
                ).ToList();
                cantfases = result.Count();
                
                foreach (var offerp in result)
                {
                    if(offerp.StatusPhase==StatusPhase.EnCurso)
                    {
                        cantfasesencurso += 1;
                    }
                }

                if(cantfases- cantfasesencurso==0 || cantfases - cantfasesencurso == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


               
            }
        }
    }
}


