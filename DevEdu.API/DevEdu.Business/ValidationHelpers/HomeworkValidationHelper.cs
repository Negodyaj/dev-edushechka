﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class HomeworkValidationHelper : IHomeworkValidationHelper
    {
        private readonly IHomeworkRepository _homeworkRepository;

        public HomeworkValidationHelper(IHomeworkRepository homeworkRepository)
        {
            _homeworkRepository = homeworkRepository;
        }

        public HomeworkDto GetHomeworkByIdAndThrowIfNotFound(int homeworkId)
        {
            var homework = _homeworkRepository.GetHomework(homeworkId);
            if (homework == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(homework), homeworkId));
            return homework;
        }
    }
}