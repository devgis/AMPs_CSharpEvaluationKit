using System;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSConsolePublisher
//
// This is a minimalist way of publishing messages to a topic in AMPS.
// The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Publish a message the "messages" topic 
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.


namespace AMPSConsoleSubscriber
{
    class AMPSConsolePublisher
    {

        private static string uri_ = "tcp://127.0.0.1:9027/amps/json";

        static void Main(string[] args)
        {
            using(Client client = new Client("examplePublisher"))
            {
                try
                {
                    // connect and logon
                    client.connect(uri_);
                    client.logon();

                    // publish a simple JSON message to the "messages" topic
                    client.publish("messages", "{ \"hi\" : \"Hello, World!\" }");
                    Console.WriteLine("Published a message.");
                }
                catch (AMPSException e)
                {
                    Console.Error.WriteLine(e.Message);
                }
            }
 
        }
    }
}
