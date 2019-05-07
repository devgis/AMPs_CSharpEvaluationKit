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

namespace AMPSSOWUpdateForOOF
{
    class AMPSSOWUpdateForOOF
    {
        private static string uri_ = "tcp://127.0.0.1:9027/amps/json";

        static void Main(string[] args)
        {

             Client client = new Client("SOWConsolePublisher");

    try
    {
      System.Console.WriteLine("... starting ...");
      // connect and logon
      client.connect(uri_);
      client.logon();

      // publish a message with expiration set

      client.publish("messages-sow",
                     "{ \"messageNumber\":50000, \"message\":\"Here and then gone...\"}",
                      3);
      
      // publish a message to be deleted later on
      

      client.publish("messages-sow",
                     "{ \"messageNumber\":50000, " +
                        "\"message\":\"I've got a bad feeling about this...\"}");


      // publish two sets of messages, the first one to match the
      // subscriber filter, the next one to make messages no longer
      // match the subscriber filter.
       
      // the first set of messages is designed so that the
      // sample that uses OOF tracking receives an updated message.

      for (int number = 0 ; number < 10000; number += 1250)
      {
        client.publish("messages-sow",
            "{ \"messageNumber\":" + number + ", " +
                        "\"message\":\"Hello, World!\"}");
          //  "message=Hello, World!\u0001messageNumber=" + number );
      }

      

      for (int number = 0 ; number < 10000; number += 1250)
      {
        client.publish("messages-sow",

               "{ \"messageNumber\":" + number + ", " +
                  "\"OptionalField\":\"ignore_me\", " +
                        "\"message\":\"Updated, world!\"}");
      }


      client.sowDelete(
                          new DefaultMessageHandler()
                          , "messages-sow", "/messageNumber = 500", 0);

      // wait up to 2 seconds for all messages to be published

      client.flush(2000);
      client.disconnect();
      System.Console.WriteLine("... done ...");

    }
    catch (AMPSException e)
    {
      System.Console.WriteLine(e.Message);
      System.Console.WriteLine(e.StackTrace);
    }

  }

    }
}
