namespace DevEdu.DAL.Repositories
{
    public interface IUser_GroupRepository
    {
        void AddTag(int groupId, int userId, int roleId);
        void DeleteTag(int userId, int groupId);
    }
}