
// AMPSQueuePublisher
//
//This is a minimalist way of publishing messages to a queue in AMPS.
//The program flow is simple:
//
//* Connect to AMPS
//* Logon
//* Publish messages the "sample-queue" topic 
//
//This sample doesn't include error handling or connection
//retry logic.
//
//(c) 2015-2016 60East Technologies, Inc.  All rights reserved.
//This file is a part of the AMPS Evaluation Kit.
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


namespace AMPSQueuePublisher
{
    class AMPSQueuePublisher
    {

         private static String uri_ = "tcp://127.0.0.1:9007/amps/json";


        static void Main(string[] args)
        {

            String id = "";

            if (args.Length > 0)
            {
                id = args[0];
            }
            else
            {
                id = "QueuePublisher-" + ((Int32)((new Random().NextDouble()) * 10000.0)).ToString();
            }

            using (Client client = new Client(id))
            {
                try
                {
                    client.connect(uri_);
                    client.logon();
                    System.Console.WriteLine(id + " connected..");

                    // To enqueue a message, you simply publish it to a topic
                    // that is captured in a queue. The publisher does not need
                    // to do any special work.

                    for (int i = 0; i < 1000; ++i)
                    {
                        client.publish("sample-queue",
                           "{\"message\" : \"Hello, World! This is message " + i + " \"}");
                        Thread.Sleep(250);
                    }

                    Environment.Exit(0);
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
