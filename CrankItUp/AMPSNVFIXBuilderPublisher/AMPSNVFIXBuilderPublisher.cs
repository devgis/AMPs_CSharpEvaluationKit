using System;

using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSNVFIXBuilderPublisher
//
// This is a minimalist way of publishing NVFIX messages to a topic in AMPS.
// The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Create a NVFIX builder and append data
// * Publish a message the "messages" topic 
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.


namespace AMPSNVFIXBuilderPublisher
{
    class AMPSNVFIXBuidlerPublisher
    {

        private static string uri_ = "tcp://127.0.0.1:9007/amps/nvfix";

        static void Main(string[] args)
        {
            using (Client client = new Client("exampleNVFIXPublisher"))
            {
                try
                {
                    // connect and logon
                    client.connect(uri_);
                    client.logon();

                    // create a builder with 1024 bytes of initial capacity
                    // using the default 0x01 delimiter
                    NVFIXBuilder builder = new NVFIXBuilder(1024, (byte)1);

                    // add fields to the builder
                    builder.append("test", "data");
                    builder.append("more", "test data");

                    // create a string for the topic
                    string topic = "messages";

                    // publish the message to the "messages" topic
                    client.publish(topic, builder.ToString());

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
