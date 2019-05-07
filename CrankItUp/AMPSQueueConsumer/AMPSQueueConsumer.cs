
/** AMPSQueueConsumer
*
* This is a minimalist way of subscribing to an at-least-once queue
* in AMPS. The program flow is simple:
*
* * Connect to AMPS
* * Logon
* * Subscribe to all messages published on the "sample-queue" topic 
* * Output the messages to the console
* 
* This sample uses automatic acknowledgement of the messages in the
* queue. With this setting, the client will handle acknowledgement
* for the previous message each time the MessageStream returns
* a new message. (See the Developer Guide for full details.)
*
* (c) 2015-2016 60East Technologies, Inc.  All rights reserved.
* This file is a part of the AMPS Evaluation Kit.
*/

////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2015-2016 60East Technologies Inc., All Rights Reserved.
//
// This computer software is owned by 60East Technologies Inc. and is
// protected by U.S. copyright laws and other laws and by international
// treaties.  This computer software is furnished by 60East Technologies
// Inc. pursuant to a written license agreement and may be used, copied,
// transmitted, and stored only in accordance with the terms of such
// license agreement and with the inclusion of the above copyright notice.
// This computer software or any other copies thereof may not be provided
// or otherwise made available to any other person.
//
// U.S. Government Restricted Rights.  This computer software: (a) was
// developed at private expense and is in all respects the proprietary
// information of 60East Technologies Inc.; (b) was not developed with
// government funds; (c) is a trade secret of 60East Technologies Inc.
// for all purposes of the Freedom of Information Act; and (d) is a
// commercial item and thus, pursuant to Section 12.212 of the Federal
// Acquisition Regulations (FAR) and DFAR Supplement Section 227.7202,
// Government’s use, duplication or disclosure of the computer software
// is subject to the restrictions set forth by 60East Technologies Inc..
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

using AMPS.Client;
using AMPS.Client.Exceptions;


namespace AMPSQueueConsumer
{
    class AMPSQueueConsumer
    {

        private static string uri_ = "tcp://127.0.0.1:9007/amps/json";

        static void Main(string[] args)
        {
            String id = "";

            if (args.Length > 0 )                
            {
                id = args[0];
            }
            else
            {
                id = "QueueSubscriber-" + ((Int32)((new Random().NextDouble()) * 10000.0)).ToString();      
            } 

            using (Client client = new Client(id))
            {

                try
                {
                    // Turn on automatic acknowledgements.
                    // Let the client choose the acknowledgement batch size
                    // and frequency.
                    client.setAutoAck(true);

                    // connect to the AMPS server and logon
                    client.connect(uri_);
                    client.logon();

                    System.Console.WriteLine(id + " connected.");

                    // Subscribe to the messages topic. When a message arrives,
                    // write the message data to the console.

                    foreach (var message in client.execute(
                                     new Command(Message.Commands.Subscribe)
                                           .setTopic("sample-queue")))
                    {
                        Console.WriteLine("[" + id + "] : " + message.Data);
                    } // when the loop retrieves the next message, the previous message
                      // is marked for acknowledgement


                }
                catch (AMPSException e)
                {
                    System.Console.Error.WriteLine(e.Message);
                    System.Console.Error.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
