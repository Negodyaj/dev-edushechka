using System.Threading.Tasks;

namespace DevEdu.API.Models
{
    public class MaterialInfoWithGroupsOutputModel : MaterialInfoOutputModel
    {
        public List<GroupInfoOutputModel> Groups { get; set; }
    }
}
