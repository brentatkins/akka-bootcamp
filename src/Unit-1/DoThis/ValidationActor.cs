using System;
using Akka.Actor;

namespace WinTail
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriterActor.Tell(new Messages.NullInputError("no input"));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Message was valid"));
                }
                else
                {
                    _consoleWriterActor.Tell(new Messages.ValidationError("Odd number of characters error"));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());
        }

        private static bool IsValid(string msg)
        {
            return msg.Length%2 == 0;
        }
    }
}