using System.Collections.Generic;

namespace Autorepair.Data
{
    public class Answer
    {
        public int Cod { get; private set; }
        public List<string> Messages { get; private set; }

        public Answer () {
            Cod = 1;
            Messages = new List<string> ();
        }

        public void AddErrorMessage (string message)
        {
            Messages.Add(message);
            Cod = -1;
        }
        
        public void AddReferredMessage (string message)
        {
            Messages.Add(message);
            Cod = 0;
        }

        public void AddApproveMessage (string message)
        {
            Messages.Add(message);
        }
    }
}