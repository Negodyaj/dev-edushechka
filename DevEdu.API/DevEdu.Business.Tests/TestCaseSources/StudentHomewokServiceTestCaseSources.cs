using DevEdu.Business.Constants;
using DevEdu.DAL.Enums;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.TestCaseSources
{
    public static class StudentHomewokServiceTestCaseSources
    {
        public static IEnumerable<TestCaseData> GetTestCaseDataForWrongStatusPassedConflictException()
        {
            yield return new TestCaseData(StudentHomeworkStatus.NotDone, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.NotDone, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.NotDone, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.NotDone, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.DoneWithLate, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.DoneWithLate, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.NotDone, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneWithLate, StudentHomeworkStatus.DoneWithLate, ServiceMessages.HomeworkStatusCantBeChanged);
        }

        public static IEnumerable<TestCaseData> GetTestCaseDataForWrongStatusPassedAuthorizationException()
        {
            //First two test cases is for roles who doesn't have right on any chaning status
            yield return new TestCaseData(Role.Methodist, StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Manager, StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Tutor, StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Teacher, StudentHomeworkStatus.NotDone, StudentHomeworkStatus.OnCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheck, StudentHomeworkStatus.DoneWithLate, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Tutor, StudentHomeworkStatus.ToFix, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Teacher, StudentHomeworkStatus.ToFix, StudentHomeworkStatus.OnCheckRepeat, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.OnCheckRepeat, StudentHomeworkStatus.DoneWithLate, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
        }
    }
}
