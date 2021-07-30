using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.Extensions
{
    public static class ServicesExtensions
    {
        public static bool CheckListValuesAreUnique(this List<int> list)
        {
            return list.Distinct().Count() == list.Count();
        }
    }
}
