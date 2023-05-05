namespace AspControler.Models;

    public class QuestionModel
    {
        public int Id { get; set; }
        public string? Question { get; set; }
        public List<Choise>? Choices { get; set; }
        public MediaModel? Media { get; set; }
        public string? Description { get; set; }
    }


