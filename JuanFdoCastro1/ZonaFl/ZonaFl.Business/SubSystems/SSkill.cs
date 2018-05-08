using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Repository;

namespace ZonaFl.Business.SubSystems
{
   public  class SSkill
    {
        public ZonaFl.Persistence.Entities.Skill FindSkillByName(string Name)
        {

            SkillRepository skillrepo = new SkillRepository();
            return skillrepo.FindSkillByName(Name);
           

        }
        public List<ZonaFl.Persistence.Entities.Skill> FindSkillByCategory(int idcategory)
        {

            SkillRepository skillrepo = new SkillRepository();
            return skillrepo.FindSkillByCategory(idcategory);


        }

        public List<ZonaFl.Persistence.Entities.Skill> FindSkillByCategoryEdit(int categoryid,string iduser)
        {
            SkillRepository skillrepo = new SkillRepository();
            return skillrepo.FindSkillByCategoryEdit(categoryid, iduser);
        }
    }
}
