using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPS.Client;

/**
  * Simple server chooser implementation that returns
  * a random server from the list of servers provided.
  * This implements a simple form of load balancing for
  * clients that place similar demands on the servers.
  *
  */
namespace _12_AMPSSampleServerChooser
{
    class AMPSSampleServerChooser : AMPS.Client.ServerChooser
    {
        // list to hold the current list of URIs
        private List<string> _uris;

        // list to hold the failure count of each URI
        private List<int> _failures;        

        // Index of the current URI
        private int _currentURI;

        public AMPSSampleServerChooser()
        {
            _uris = new List<string>();
            _failures = new List<int>();
            _currentURI = 0; 
        }

       /**
        * Adds a URI to self and adds its failure count
        * 
        */
        public AMPSSampleServerChooser add(string uri)
        {
            _uris.Add(uri);
            _failures.Add(0);
            chooseNext();    
            return this;
        }

       /**
        * Return the current URI. This method does not change the
        * current URI or advance to the next URI.
        * 
        */
        public string getCurrentURI()
        {
            return _uris[_currentURI];
        }

       /**
        * Always returns null. This implementation doesn't provide an
        * authenticator.
        */
        public Authenticator getCurrentAuthenticator()
        {
            return null;
        }

       /**
        * Handle connection failures. As with DefaultServerChooser, if
        * the client is simply disconnected, retry the same URI. If there is
        * a different error (for example, connection refused), choose another
        * URI.
        */
        public void reportFailure(Exception exception, ConnectionInfo info)
        {
            // In either case we'll just move on to the next
            // server whenever we have a failure connecting.
            // If we just got disconnected, though, we'll retry.
            if (!(exception is AMPS.Client.Exceptions.DisconnectedException))
            {
                // increment failure count for the current URI
                ++_failures[_currentURI];
                chooseNext();
            }
        }

       /**
        * Advance to the next URI. For this server chooser,
        * the next URI is whichever URI has a lowest fail count.
        * If the lowest fail count is the current URI, then the 
        * adjacent URI will be selected.
        */
        public void chooseNext()
        {
            int minFailsIndex = _failures.IndexOf(_failures.Min());
            if (minFailsIndex != _currentURI)
            {
                _currentURI = minFailsIndex;
            }
            else
                _currentURI = (++_currentURI % _uris.Count());
        }
       /**
        * Not used in this server chooser.
        *
        */
        public string getError()
        {
            return null;
        }

       /**
        * Not used in this server chooser.
        *
        */
        public void reportSuccess(ConnectionInfo info)
        {
            return;
        }
    }
}
