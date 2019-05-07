/** CompositeMessagePublisher
 *
 * Simple example demonstrating publishing a composite message to a topic
 * in AMPS.
 *
 * The program flow is simple:
 *
 * * Connect to AMPS, using the transport configured for composite json-binary
 *   messages
 * * Logon
 * * Construct binary data for the message. For demonstration purposes,
 *   the sample uses the same binary data for each message.
 * * Publish a set of messages to AMPS. For each message:
 *   - Construct a json part that the subscriber can filter on.
 *   - Construct a composite message payload.
 *   - Publish the message.
 *
 * This sample doesn't include error handling or connection
 * retry logic.
 *
 * This file is a part of the AMPS Evaluation Kit.
 */

////////////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2013-2016 60East Technologies Inc., All Rights Reserved.
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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using AMPS.Client;
using AMPS.Client.Fields;

namespace _09_AMPSCompositeMessagePublisher
{
    class AMPSCompositeMessagePublisher
    {

        static void Main(string[] args)
        {
            HAClient client = new HAClient("CompositePubber");
            try
            {
                DefaultServerChooser sc = new DefaultServerChooser();
                sc.add("tcp://127.0.0.1:9027/amps/composite-json-binary");
                client.setServerChooser(sc);

                // Construct binary data

                List<double> theData = new List<double>();
                theData.Add(1.0);

                for (double d = 1.0; d < 50.0; ++d)
                {
                    if (d <= 1.0)
                    {
                        theData.Add(1.0);
                        continue;
                    }
                    theData.Add(d + theData[(int)d - 2]);
                }

                client.connectAndLogon();

                // Publish the message
                string topic = "messages";

                for (int count = 1; count < 10; ++count)
                {
                    // Construct a JSON part
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"binary_type\": \"double\"")
                      .Append(", \"size\" : ").Append(theData.Count)
                      .Append(", \"number\" : ").Append(count)
                      .Append(", \"message\" : \"Hi, world!\"")
                      .Append("}");


                    // Create a byte array from the data: this is
                    // what the program will send.
                    byte[] outBytes = null;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BinaryFormatter format = new BinaryFormatter();
                        format.Serialize(stream, theData);
                        outBytes = stream.ToArray();
                    }

                    // Construct the composite
                    CompositeMessageBuilder builder = new CompositeMessageBuilder();
                    builder.append(sb.ToString());               
                    builder.append(outBytes, 0, outBytes.Length);

                    Field outMessage = new Field();
                    builder.setField(outMessage);

                    byte[] topicBytes = System.Text.Encoding.UTF8.GetBytes(topic.ToCharArray());

                    client.publish(topicBytes, 0, topicBytes.Length, outMessage.buffer, 0, outMessage.length);

                }


            }
            finally
            {

                client.close();
            }

        }
    }
}
