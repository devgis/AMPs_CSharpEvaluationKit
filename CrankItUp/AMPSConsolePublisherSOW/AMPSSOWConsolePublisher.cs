using System;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSSOWConsolePublisher
//
// This sample publishes messages to a topic in AMPS that
// maintains a state-of-the-world (SOW) database.
// The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Publish 100 messages to the "messages-sow" topic 
// * Publish another message with a duplicate messageNumber to the topic,
//   effectively updating that message. 
//
// The "messages-sow" topic is configured in config/sample.xml to
// maintain a SOW database, where each messageNumber is a unique
// message.
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.

namespace AMPSConsoleSOWPublisher
{
    class AMPSSOWConsolePublisher
    {
        private static string uri_ = "tcp://3.95.158.49:9007/amps/json";
        
        static void Main(string[] args)
        {
            using(Client client = new Client("exampleSOWPublisher"))
            {
                //try
                //{
                //    // connect and logon
                //    client.connect(uri_);
                //    client.logon();

                //    // send 100 messages with unique message numbers
                //    // to fill the SOW database
                //    for (int number = 0; number < 100; ++number)
                //    {
                //        client.publish("messages-sow",
                //                       "{ \"text\" : \"Hello, world!\", " +
                //                       " \"messageNumber\" : " + number + "}");
                //    }

                //    // Now make a change to message 5.

                //    client.publish("messages-sow", "{ \"text\":\"This is new information\"" +
                //                                      "\"messageNumber\" : 5 }" );
                //}
                //catch (AMPSException e)
                //{
                //    Console.Error.WriteLine(e.Message);
                //}

                try
                {
                    // connect and logon
                    client.connect(uri_);
                    client.logon();

                    // send 100 messages with unique message numbers
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
                    Console.Error.WriteLine(e.Message);
                }
            }
            Console.Read();
        }
    }
    
}
