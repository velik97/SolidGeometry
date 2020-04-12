using Lesson;

namespace Editor.Lesson
{
    public interface ILessonDataCarrier
    {
        LessonData GetLessonData();

        void SetLessonData(LessonData lessonData, string lessonName);

        void CreateNewLesson(string lessonName);
    }
}