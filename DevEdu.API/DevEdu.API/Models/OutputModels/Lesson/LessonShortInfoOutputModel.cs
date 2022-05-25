namespace DevEdu.API.Models
{
    public class LessonShortInfoOutputModel
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Name { get; set; }
        public string AdditionalMaterials { get; set; }
        public string LinkToRecord { get; set; }
        public int Number { get; set; }
    }
}