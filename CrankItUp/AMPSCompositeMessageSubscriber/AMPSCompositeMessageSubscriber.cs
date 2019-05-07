/** CompositeMessageSubscriber
 *
 * Simple example demonstrating receiving and parsing a composite message
 * from a topic in AMPS.
 *
 * The program flow is simple:
 *
 * * Connect to AMPS, using the transport configured for composite json-binary
 *   messages
 * * Logon
 * * Subscribe to the topic, using a filter that refers to the json part of the
 *   message
 * * For each message received:
 *   - Parse the message
 *   - Extract the json part of the message
 *   - Re-create the binary part of the message as a Java object
 *   - Print the contents of the message
 *
 * This sample doesn't include error handling or connection
 * retry logic.
 *
 * This file is a part of the AMPS Evaluation Kit.
 */

////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2010-2016 60East Technologies Inc., All Rights Reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using AMPS.Client;
using AMPS.Client.Fields;


namespace AMPSCompositeMessageSubscriber
{
    class AMPSCompositeMessageSubscriber
    {

        static void Main(string[] args)
        {
            // Create the client
            HAClient client = new HAClient("CompositeSubber");
            try
            {
                // Add URIs and connect the client
                DefaultServerChooser sc = new DefaultServerChooser();
                sc.add("tcp://127.0.0.1:9027/amps/composite-json-binary");
                client.setServerChooser(sc);

                client.connectAndLogon();

                // Construct the parser to use
                CompositeMessageParser parser = new CompositeMessageParser();

                System.Console.WriteLine("Subscribing to messages where message number is a multiple of 3.");

                // Subscribe and print messages
                foreach (Message message in client.subscribe("messages", "/0/number % 3 == 0"))
                {
                    // Parse the message and get the number of parts

                    int parts = parser.parse(message);
                    string json = parser.getString(0);
                    Field binary = new Field();
                    parser.getPart(1, binary);


                    // Recreate the List<double> from the binary part
                    // of the message.
                    List<double> theData = new List<double>();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BinaryFormatter format = new BinaryFormatter();
                        stream.Write(binary.buffer, binary.position, binary.length);
                        stream.Seek(0, SeekOrigin.Begin);
                        theData = (List<double>)format.Deserialize(stream);
                    }

                    // Print the message
                    System.Console.WriteLine("Received message with " + parts + " parts");
                    System.Console.WriteLine(json);
                    foreach (double d in theData)
                    {
                        System.Console.Write(d + " ");
                    }
                    System.Console.WriteLine();
                }

            }
            finally
            {

                client.close();
            }

        }

    }
}
