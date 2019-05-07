using System;
using System.Threading;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSSOWandSubscribeConsoleSubscriber
//
// This sample retrieves messages from a state-of-the-world database and subscribes to
// updates to those messages. The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Get the state-of-the-world for the "messages-sow" topic, filtered
//   messages with a message number less than 10.
// * Stay subscribed to any messages on the topic that meet the filter criteria. 
// * Output all messages received to the console
//
// This sample also sets simple connection retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.

namespace AMPSSOWandSubscribeConsoleSubscriber
{
    class AMPSSOWandSubscribeConsoleSubscriber
    {
    
        private static string uri_ = "tcp://127.0.0.1:9027/amps/json";

        // create a method to be called if the client is disconnected.
        public static void AttemptReconnection(Client client)
        {
            // sleep for 250 milliseconds, then try to reconnect.
            Thread.Sleep(250);
            client.connect(uri_);
        }


        static void Main(string[] args)
        {

            using (Client client = new Client("sowAndSubscribeClient"))
            {
                CommandId subscriptionId = new CommandId();

                try
                {
                    // set the client to use the disconnect handler defined above
                    client.setDisconnectHandler(AttemptReconnection);

                    // connect to the AMPS server and logon
                    client.connect(uri_);
                    client.logon();

                    // Subscribe to the messages topic, with a timeout of 5000.
                    // When a message arrives, invoke a lambda function that writes the message data
                    // to the console.

                    foreach (var message in
                                 client.execute(new Command(Message.Commands.SOWAndSubscribe)
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

                }
                catch (AMPSException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }

        }

    }
}
