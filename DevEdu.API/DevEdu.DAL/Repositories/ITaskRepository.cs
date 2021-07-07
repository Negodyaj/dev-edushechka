﻿using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto task);
        void UpdateTask(TaskDto task);
        void DeleteTask(int id);
    }
}