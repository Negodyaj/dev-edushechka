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
            yield return new TestCaseData(StudentHomeworkStatus.Undone, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Undone, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Undone, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToFix, StudentHomeworkStatus.DoneAfterDeadline, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedOnThisStatus);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.Done, StudentHomeworkStatus.DoneAfterDeadline, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.Undone, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChanged);
            yield return new TestCaseData(StudentHomeworkStatus.DoneAfterDeadline, StudentHomeworkStatus.DoneAfterDeadline, ServiceMessages.HomeworkStatusCantBeChanged);
        }

        public static IEnumerable<TestCaseData> GetTestCaseDataForWrongStatusPassedAuthorizationException()
        {
            //First two test cases is for roles who doesn't have right on any chaning status
            yield return new TestCaseData(Role.Methodist, StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Manager, StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Tutor, StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Teacher, StudentHomeworkStatus.Undone, StudentHomeworkStatus.ToCheck, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToCheck, StudentHomeworkStatus.DoneAfterDeadline, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Tutor, StudentHomeworkStatus.ToFix, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Teacher, StudentHomeworkStatus.ToFix, StudentHomeworkStatus.ToVerifyFixes, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.ToFix, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.Done, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
            yield return new TestCaseData(Role.Student, StudentHomeworkStatus.ToVerifyFixes, StudentHomeworkStatus.DoneAfterDeadline, ServiceMessages.HomeworkStatusCantBeChangedByThisUser);
        }
    }
}
