using System;
using System.Threading;

using AMPS.Client;
using AMPS.Client.Exceptions;
using AMPS.Client.Fields;

// AMPSSOWAndSubscribeWithOOF
//
// This sample retrieves messages from a state-of-the-world database. The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Get the state-of-the-world for the "messages-sow" topic, filtered
// messages with a message number less than 10.
// * Output all messages received to the console
//
// For demonstration purposes, this sample uses the asynchronous method
// of executing a command. In this sample, there's no advantage to using
// this method.
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013-2016 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.


namespace AMPSSOWSubscribeWithOOF
{
    class AMPSSOWSubscribeWithOOF
    {

    private static string uri_ = "tcp://127.0.0.1:9027/amps/json";


    static void Main(string[] args)
    {
    Client client = new Client("SOWandSubscribeConsoleSubscribeWithOOF");

     try {
      client.connect(uri_);
      client.logon();

      // create an object to process the messages.

      Action<Message> mh = (message) => {

                               if (message.getCommand() == Message.Commands.OOF)
                               {
                                 Console.WriteLine("Message no longer in focus because : " +
                                   ReasonField.encodeReason(message.getReason()) +
                                   " : " + message.getData() );
                                   return;
                                }

                                if (message.Command == Message.Commands.GroupBegin)
                                {
                                    Console.WriteLine("Receiving messages from the SOW.");
                                }
                                Console.WriteLine(message.Data);
                                // when the GroupEnd message arrives, the SOW
                                // query is complete.
                                if (message.Command == Message.Commands.GroupEnd) {
                                    Console.WriteLine("Done receiving messages from the SOW.");
                                } 
      };


      // request messages from the messages-sow topic where
      // the message number is less than 10. Retrieve in batches
      // of 5 messages at a time.

      // for sample purposes, demonstrate executeAsync

      System.Console.WriteLine("... entering subscription ...");

      client.executeAsync(
          new Command(Message.Commands.SOWAndSubscribe)
             .setTopic("messages-sow")
             .setFilter("/messageNumber % 10 = 0 AND " +
                        "(/OptionalField IS NULL OR /OptionalField <> 'ignore_me')")
            .setBatchSize(5)
            .setOptions(Message.Options.OOF)
            .setTimeout(5000),
         new ActionMessageHandler(mh));

      System.Console.WriteLine("Requested SOW messages.");

      // the results of the sow query arrive asynchronously,
      // so the sample sleeps to let the messages arrive.

      // this program uses this construct for sample purposes.
      // generally speaking, the program would use the results
      // of the query as they arrive.

      while (true)
      {
        Thread.Sleep(100);
      }



    }
    catch (AMPSException e) {
      System.Console.WriteLine(e.Message);
      System.Console.WriteLine(e.StackTrace);
    }
    catch (Exception e) {
      System.Console.WriteLine(e.Message);
      System.Console.WriteLine(e.StackTrace);
    }


  }

 }
}
