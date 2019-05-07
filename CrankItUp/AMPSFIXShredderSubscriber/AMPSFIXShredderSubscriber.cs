using System;
using System.Threading;
using System.Collections.Generic;
using AMPS.Client;
using AMPS.Client.Exceptions;

// AMPSFIXShredderSubscriber
//
// This sample retrieves fix messages from a state-of-the-world database. 
// The program flow is simple:
//
// * Connect to AMPS
// * Logon
// * Get the state-of-the-world for the "messages" topic
// * Output all fields of the messages received to the console
//
// This sample doesn't include error handling or connection
// retry logic.
//
// (c) 2013 60East Technologies, Inc.  All rights reserved.
// This file is a part of the AMPS Evaluation Kit.

namespace AMPSFIXShredderSubscriber
{
    class AMPSFIXShredderSubscriber
    {

        private static string uri_ = "tcp://127.0.0.1:9007/amps/fix";

        static void Main(string[] args)
        {
            using (Client client = new Client("exampleNVFIXClient"))
            {

                try
                {
                    // connect to the AMPS server and logon
                    client.connect(uri_);
                    client.logon();

                    // subscribe to the messages topic
                    MessageStream ms = client.subscribe("messages");

                    try
                    {
                        // create a shredder -- since this just returns
                        // the Map, we can reuse the same shredder.
                        FIXShredder shredder = new FIXShredder((byte)1);

                        // iterate through each message and write data to console
                        foreach (Message msg in ms)
                        {
                            System.Console.Write("Got a message");

                            // shred the message to a map
                            Dictionary<int, string> fields = shredder.toMap(msg.getData());

                            // iterate over the keys in the map and display the key and dataa
                            foreach (KeyValuePair<int, string> key in fields)
                            {
                                System.Console.Write("  " + key + " " + key.Value);
                            }
                        }
                    }
                    finally // close the message stream to release the subscription
                    { ms.close(); }
                }
                catch (AMPSException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
    }
}
