using System;
using System.Threading;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSConsoleSubscriber
//
// This is a minimalist way of subscribing to a topic in AMPS. The
// program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Subscribe to all messages published on the "messages" topic 
// * Output the messages to the console
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.

namespace AMPSConsoleSubscriber
{
    class AMPSConsoleSubscriber
    {

        private static string uri_ = "tcp://127.0.0.1:9027/amps/json";

        static void Main(string[] args)
        {

            using (Client client = new Client("exampleSubscriber"))
            {
                CommandId subscriptionId = new CommandId();

                try
                {
                    // connect to the AMPS server and logon
                    client.connect(uri_);
                    client.logon();

                    // Subscribe to the messages topic. When a message arrives,
                    // write the message data to the console.

                    foreach (var message in client.execute(
                                     new Command(Message.Commands.Subscribe)
                                           .setTopic("messages")))
                    {
                        Console.WriteLine(message.Data);
                    }
                     

                }
                catch (AMPSException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
        }
    }
}
