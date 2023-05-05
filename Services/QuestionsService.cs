
using AspControler.Models;
using Newtonsoft.Json;

namespace AspControler.Services;

public class QuestionsService
{
    public  List<QuestionModel> _questions;
    private static QuestionsService Salom;
    public static QuestionsService _Salom => Salom ??= new QuestionsService();
    public QuestionsService()
    {
        Language("uz");

        _questions ??= new List<QuestionModel>(); 
       
    }

    public void Language(string language)
    {
        var til = "uzlotin.json";

        switch (language)
        {
            case "uz": til = "uzlotin.json"; break;
            case "krl": til = "uzkiril.json"; break;
            case "ru": til = "rus.json"; break;
        }

        var yul = Path.Combine("JsonData", $"{til}");
        var json = System.IO.File.ReadAllText(yul);

        _questions = JsonConvert.DeserializeObject<List<QuestionModel>>(json);
    }
}



