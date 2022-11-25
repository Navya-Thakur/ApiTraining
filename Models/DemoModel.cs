namespace backend.Models
{
    public class FindDemoData
    {
         public string count { get; set; }
        // public object msgData { get; set; }
         public QuestionData[] question { get; set; }


    }

    public class QuestionData{

         public string test { get; set; }
        // public object msgData { get; set; }
         public string no { get; set; }

    }
}