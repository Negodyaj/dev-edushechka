﻿namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        void CheckGroupExistence(int groupId);
        void CheckAccessGetGroupMembers(int groupId, int userId);
        void TmpAccess(int id , int id2, int id3 = 0);
    }
}