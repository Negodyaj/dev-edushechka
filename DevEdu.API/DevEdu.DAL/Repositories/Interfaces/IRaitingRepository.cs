﻿using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IRaitingRepository
    {
        int AddStudentRaiting(StudentRaitingDto studentRaitingDto);
        void DeleteStudentRaiting(int id);
        List<StudentRaitingDto> SelectAllStudentRaitings();
        StudentRaitingDto SelectStudentRaitingById(int id);
        List<StudentRaitingDto> SelectStudentRaitingByUserId(int userId);
        public List<StudentRaitingDto> SelectStudentRaitingByGroupId(int groupId);
        void UpdateStudentRaiting(StudentRaitingDto studentRaitingDto);
    }
}