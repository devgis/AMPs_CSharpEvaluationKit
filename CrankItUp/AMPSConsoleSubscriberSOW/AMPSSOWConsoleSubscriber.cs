using System;
using System.Threading;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSSOWConsoleSubscriber
//
// This sample retrieves messages from a state-of-the-world database. The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Get the state-of-the-world for the "messages-sow" topic, filtered
//   messages with a message number less than 10.
// * Output all messages received to the console
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.

namespace AMPSConsoleSubscriberSOW
{
    class AMPSSOWConsoleSubscriber
    {

        private static string uri_ = "tcp://127.0.0.1:9027/amps/json";

        static void Main(string[] args)
        {
            using (Client client = new Client("exampleClient"))
            {
                
                try
                {
                    // connect to the AMPS server and logon
                    client.connect(uri_);
                    client.logon();

                    // request messages from the messages-sow topic where
                    // the message number is less than 10.
                    

                    foreach (var message in
                                 client.execute(new Command(Message.Commands.SOW)
                                                 .setTopic("messages-sow")
                                                 .setFilter("/messageNumber < 10")))
                    {
                        if (message.Command == Message.Commands.GroupBegin)
                        {
                            Console.WriteLine("Receiving messages from the SOW.");
                            continue;
                        }
                        
                        // when the GroupEnd message arrives, the SOW
                        // query is complete.
                        if (message.Command == Message.Commands.GroupEnd)
                        {
                            Console.WriteLine("Done receiving messages from the SOW.");
                            continue;
                        }

                        Console.WriteLine(message.Data);
                    }
                  

                    Console.Write("Press Enter to end the program.");
                    Console.ReadLine();

                }
                catch (AMPSException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
    }
}
