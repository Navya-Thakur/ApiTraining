using System;

namespace backend.Models
{
    public class FindQuestion
    {
         public string lmsData { get; set; }
        // public object msgData { get; set; }
         public MsgData msgData { get; set; }


    }

    public class MsgData{

         public string config { get; set; }
        // public object msgData { get; set; }
         public string assignment { get; set; }

    }
}